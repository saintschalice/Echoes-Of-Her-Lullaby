using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages game over state when Emily catches Lisa.
/// Handles two-stage UI with visual fading and proper audio isolation.
/// Ensures transitions are solid black to prevent seeing the frozen game world.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI Structure")]
    [Tooltip("A standalone black Image/Panel that sits behind ALL Game Over UI but in front of the game. This ensures no 'gaps' in visibility.")]
    public CanvasGroup blackBackgroundFader;

    [Header("UI Panels")]
    [Tooltip("Panel 1: Shows 'GAME OVER' text only")]
    public GameObject gameOverMessagePanel;
    [Tooltip("Panel 2: Shows Retry, Main Menu, Exit buttons")]
    public GameObject gameOverOptionsPanel;

    [Header("Message Panel Elements")]
    public TextMeshProUGUI gameOverText;
    [Tooltip("Transparent button covering the screen to detect tap")]
    public Button continueToOptionsButton;

    [Header("Options Panel Buttons")]
    public Button retryButton;
    public Button mainMenuButton;
    public Button exitButton;

    [Header("Audio")]
    public AudioClip deathSound;

    [Header("Transition Settings")]
    public float gameOverFadeInDuration = 1.5f; // Time for black BG + Text to fade in
    public float uiCrossFadeDuration = 0.5f;    // Time to switch from Message to Options
    public float sceneTransitionDuration = 1.0f; // Time for Retry/Menu fade out

    // Flag to track if we are doing a "Soft Restart"
    private bool isSoftRestarting = false;
    private string targetRestartRoom = "";

    // Timer to prevent accidental double-taps skipping the game over screen instantly
    private float ignoreInputUntil = 0f;

    // Track current transition to prevent overlaps
    private Coroutine currentTransition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.UpdateUIReferences(this);
            Destroy(gameObject);
            return;
        }

        InitializeManager();
    }

    private void InitializeManager()
    {
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);

        // Initialize background to transparent and hidden
        if (blackBackgroundFader != null)
        {
            blackBackgroundFader.alpha = 0f;
            blackBackgroundFader.gameObject.SetActive(false);
            blackBackgroundFader.blocksRaycasts = true; // Block input while active
        }

        // Fix text raycasts blocking buttons
        if (gameOverMessagePanel != null)
        {
            var allTexts = gameOverMessagePanel.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var t in allTexts) t.raycastTarget = false;
        }

        SetupButtons();
    }

    public void UpdateUIReferences(GameOverManager newManager)
    {
        this.gameOverMessagePanel = newManager.gameOverMessagePanel;
        this.gameOverOptionsPanel = newManager.gameOverOptionsPanel;
        this.gameOverText = newManager.gameOverText;
        this.continueToOptionsButton = newManager.continueToOptionsButton;
        this.retryButton = newManager.retryButton;
        this.mainMenuButton = newManager.mainMenuButton;
        this.exitButton = newManager.exitButton;
        this.blackBackgroundFader = newManager.blackBackgroundFader; // Update background ref

        InitializeManager();
    }

    private void Update()
    {
        // Detect tap to advance from Message Panel to Options Panel
        if (gameOverMessagePanel != null && gameOverMessagePanel.activeSelf)
        {
            if (Time.unscaledTime < ignoreInputUntil) return;

            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                ShowOptionsPanel();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SetupButtons()
    {
        if (continueToOptionsButton != null)
        {
            continueToOptionsButton.onClick.RemoveAllListeners();
            continueToOptionsButton.onClick.AddListener(ShowOptionsPanel);
        }

        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RestartLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    public void TriggerGameOver(string message = "GAME OVER")
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(GameOverSequence(message));
    }

    private IEnumerator GameOverSequence(string message)
    {
        // 1. Freeze Logic
        Time.timeScale = 0f;

        // 2. Audio Management
        if (deathSound != null)
            AudioManager.Instance?.PlaySFX(deathSound);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(0f);
            AudioManager.Instance.StopMusic(0f);
        }

        var emilyAudio = FindFirstObjectByType<EmilyAudio>();
        if (emilyAudio != null)
        {
            var source = emilyAudio.GetComponent<AudioSource>();
            if (source != null) source.Stop();
        }

        // 3. Disable Player
        DisablePlayerControls();

        // 4. Visual Fade In
        // FIRST: Activate the Black Background
        if (blackBackgroundFader != null)
        {
            blackBackgroundFader.gameObject.SetActive(true);
            blackBackgroundFader.alpha = 0f;
        }

        if (gameOverMessagePanel != null)
        {
            gameOverMessagePanel.SetActive(true);
            if (gameOverText != null) gameOverText.text = message;

            CanvasGroup msgCg = gameOverMessagePanel.GetComponent<CanvasGroup>();
            if (msgCg == null) msgCg = gameOverMessagePanel.AddComponent<CanvasGroup>();
            msgCg.alpha = 0f;

            // Fade in both Background and Message simultaneously
            float timer = 0f;
            while (timer < gameOverFadeInDuration)
            {
                timer += Time.unscaledDeltaTime;
                float progress = timer / gameOverFadeInDuration;

                if (blackBackgroundFader != null) blackBackgroundFader.alpha = Mathf.Lerp(0f, 1f, progress);
                msgCg.alpha = Mathf.Lerp(0f, 1f, progress);

                yield return null;
            }
            if (blackBackgroundFader != null) blackBackgroundFader.alpha = 1f;
            msgCg.alpha = 1f;
        }

        // 5. Input Delay
        ignoreInputUntil = Time.unscaledTime + 0.5f;

        Debug.Log("[GameOver] Sequence complete. Waiting for input.");
    }

    public void ShowOptionsPanel()
    {
        Debug.Log("[GameOver] Switching to Options Panel.");
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(SwitchToOptionsSequence());
    }

    private IEnumerator SwitchToOptionsSequence()
    {
        // IMPORTANT: We do NOT fade out the blackBackgroundFader here.
        // It must stay opaque so the game world remains hidden.

        // 1. Fade OUT Message Panel
        if (gameOverMessagePanel != null && gameOverMessagePanel.activeSelf)
        {
            CanvasGroup msgCg = gameOverMessagePanel.GetComponent<CanvasGroup>();
            if (msgCg == null) msgCg = gameOverMessagePanel.AddComponent<CanvasGroup>();

            float timer = 0f;
            float startAlpha = msgCg.alpha;
            while (timer < uiCrossFadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                msgCg.alpha = Mathf.Lerp(startAlpha, 0f, timer / uiCrossFadeDuration);
                yield return null;
            }
            msgCg.alpha = 0f;
            gameOverMessagePanel.SetActive(false);
        }

        // 2. Fade IN Options Panel
        if (gameOverOptionsPanel != null)
        {
            gameOverOptionsPanel.SetActive(true);
            CanvasGroup optCg = gameOverOptionsPanel.GetComponent<CanvasGroup>();
            if (optCg == null) optCg = gameOverOptionsPanel.AddComponent<CanvasGroup>();

            optCg.alpha = 0f;
            optCg.interactable = false;

            float timer = 0f;
            while (timer < uiCrossFadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                optCg.alpha = Mathf.Lerp(0f, 1f, timer / uiCrossFadeDuration);
                yield return null;
            }
            optCg.alpha = 1f;
            optCg.interactable = true;
        }
    }

    private void DisablePlayerControls()
    {
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = false;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    // -----------------------------------------------------------
    // RESTART LOGIC
    // -----------------------------------------------------------
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        string currentRoomName = SceneManager.GetActiveScene().name;
        StartCoroutine(RestartRoutine(currentRoomName));
    }

    private IEnumerator RestartRoutine(string roomName)
    {
        // Issue Fix: "Retry just makes game visible".
        // Cause: UI was hiding before screen was fully black, or ScreenFader was under UI.
        // Solution: We fade the UI buttons OUT, revealing the persistent blackBackgroundFader.
        // This ensures the screen remains black.

        // 1. Fade out the OPTIONS UI, but keep the BLACK BACKGROUND.
        if (gameOverOptionsPanel != null)
        {
            CanvasGroup optCg = gameOverOptionsPanel.GetComponent<CanvasGroup>();
            // If component missing, add it, but normally it's added in ShowOptionsPanel
            if (optCg != null)
            {
                optCg.interactable = false;
                float timer = 0f;
                float startAlpha = optCg.alpha;
                // Quick fade out of buttons
                while (timer < 0.5f)
                {
                    timer += Time.deltaTime;
                    optCg.alpha = Mathf.Lerp(startAlpha, 0f, timer / 0.5f);
                    yield return null;
                }
                optCg.alpha = 0f;
                gameOverOptionsPanel.SetActive(false);
            }
        }

        // 2. Attempt ScreenFader as backup/overlay
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(sceneTransitionDuration, null);
        }

        // 3. Wait duration to ensure "Black" feeling
        // At this point, blackBackgroundFader should still be active and at alpha 1.
        yield return new WaitForSeconds(sceneTransitionDuration);

        // 4. Setup Logic for soft restart
        isSoftRestarting = true;
        targetRestartRoom = roomName;

        if (SaveSystem.Instance != null)
        {
            GameSaveData data = SaveSystem.Instance.GetCurrentSaveData();
            if (data != null)
            {
                Vector3 spawnPos = Vector3.zero;
                bool foundSpawn = false;
                RoomSpawnPoint[] spawns = FindObjectsByType<RoomSpawnPoint>(FindObjectsSortMode.None);
                foreach (var sp in spawns)
                {
                    if (sp.isDefaultSpawnPoint)
                    {
                        spawnPos = sp.transform.position;
                        foundSpawn = true;
                        break;
                    }
                }

                if (foundSpawn) data.playerPosition = spawnPos;
                if (data.roomStates.ContainsKey(roomName))
                {
                    data.roomStates[roomName] = new RoomState();
                }

                // --- FIX: Reset Emily Intro Trigger ---
                // We remove the specific trigger ID from the save data so the cutscene logic in EmilySpawnTrigger.cs
                // sees it as "not triggered" and runs the sequence (push/panic/dialogue) again.
                if (data.triggeredDialogues.Contains("EmilySpawn_Intro"))
                {
                    data.triggeredDialogues.Remove("EmilySpawn_Intro");
                }

                data.currentScene = roomName;
            }
        }

        // 5. Load Scene
        SceneManager.LoadScene(roomName);

        // UI Reset happens in OnSceneLoaded or here? 
        // We reset UI here, but KEEP black background until scene load handles it?
        // Actually, ResetUI() hides everything. 
        // If we hide BlackBackground here, we might flash frame 0 of new scene.
        // The OnSceneLoaded logic will handle scene setup.
        // We'll let ResetUI run after load starts or in InitializeManager.
        ResetUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        // Reset UI visibility cleanly
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);
        if (blackBackgroundFader != null)
        {
            blackBackgroundFader.alpha = 0f;
            blackBackgroundFader.gameObject.SetActive(false);
        }

        if (isSoftRestarting && scene.name == targetRestartRoom)
        {
            StartCoroutine(FinalizeSoftRestart());
        }
    }

    private IEnumerator FinalizeSoftRestart()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        var player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = true;
        else
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                var ctrl = playerObj.GetComponent<JoystickPlayerController>();
                if (ctrl != null) ctrl.enabled = true;
            }
        }

        EnsureJoystickActive();
        isSoftRestarting = false;
        targetRestartRoom = "";
    }

    private void EnsureJoystickActive()
    {
        GameObject joystickUI = GameObject.Find("Joystick");
        if (joystickUI == null)
        {
            GameObject persistentUI = GameObject.Find("PersistentUI");
            if (persistentUI != null)
            {
                Transform joystickTransform = persistentUI.transform.Find("Joystick");
                if (joystickTransform != null) joystickUI = joystickTransform.gameObject;
            }
        }
        if (joystickUI == null) joystickUI = GameObject.FindGameObjectWithTag("Joystick");

        if (joystickUI != null)
        {
            joystickUI.SetActive(true);
            CanvasGroup cg = joystickUI.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(ReturnToMainMenuRoutine());
    }

    private IEnumerator ReturnToMainMenuRoutine()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(sceneTransitionDuration);
            AudioManager.Instance.StopMusic(sceneTransitionDuration);
        }

        // Similar to Restart: Fade options out, leave black background
        if (gameOverOptionsPanel != null)
        {
            CanvasGroup optCg = gameOverOptionsPanel.GetComponent<CanvasGroup>();
            if (optCg != null)
            {
                float timer = 0f;
                float startAlpha = optCg.alpha;
                while (timer < 0.5f)
                {
                    timer += Time.deltaTime;
                    optCg.alpha = Mathf.Lerp(startAlpha, 0f, timer / 0.5f);
                    yield return null;
                }
                optCg.alpha = 0f;
                gameOverOptionsPanel.SetActive(false);
            }
        }

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(sceneTransitionDuration, null);
        }

        yield return new WaitForSeconds(sceneTransitionDuration);

        ResetUI();
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetUI()
    {
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);
        if (blackBackgroundFader != null)
        {
            blackBackgroundFader.alpha = 0f;
            blackBackgroundFader.gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}