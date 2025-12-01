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
/// </summary>
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

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
    public Button retryButton;    // Previously Restart
    public Button mainMenuButton;
    public Button exitButton;     // New Exit button

    [Header("Audio")]
    public AudioClip deathSound;

    [Header("Transition Settings")]
    public float gameOverFadeInDuration = 1.5f; // Time for "GAME OVER" to fade in
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
            // CRITICAL FIX: Update references when scene reloads
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

        // 2. Audio Management (Disable ALL except Death/Catch)
        if (deathSound != null)
            AudioManager.Instance?.PlaySFX(deathSound); // Play death sound

        // Cut environmental audio immediately
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(0f);
            AudioManager.Instance.StopMusic(0f);
        }

        // Stop Emily's looping audio (Hunt/Chase) manually
        var emilyAudio = FindFirstObjectByType<EmilyAudio>();
        if (emilyAudio != null)
        {
            var source = emilyAudio.GetComponent<AudioSource>();
            if (source != null) source.Stop();
        }

        // 3. Disable Player
        DisablePlayerControls();

        // 4. Visual Fade In
        if (gameOverMessagePanel != null)
        {
            gameOverMessagePanel.SetActive(true);

            // Ensure raycasts are off for text
            var allTexts = gameOverMessagePanel.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var t in allTexts) t.raycastTarget = false;

            if (gameOverText != null) gameOverText.text = message;

            // Fade in using CanvasGroup (adding one if needed)
            CanvasGroup cg = gameOverMessagePanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = gameOverMessagePanel.AddComponent<CanvasGroup>();

            cg.alpha = 0f; // Start invisible

            // Fade loop using unscaled time (since timeScale is 0)
            float timer = 0f;
            while (timer < gameOverFadeInDuration)
            {
                timer += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, timer / gameOverFadeInDuration);
                yield return null;
            }
            cg.alpha = 1f;
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
            optCg.interactable = false; // Disable buttons during fade

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
        // Must unfreeze time so ScreenFader can animate
        Time.timeScale = 1f;
        string currentRoomName = SceneManager.GetActiveScene().name;
        StartCoroutine(RestartRoutine(currentRoomName));
    }

    private IEnumerator RestartRoutine(string roomName)
    {
        // 1. Fade Out Screen (Black overlay)
        // We do NOT fade out the UI via transparency anymore, to prevent seeing the frozen scene.
        // ScreenFader must render ON TOP of the UI for this to work perfectly (Set Sort Order > UI).
        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;
            ScreenFader.Instance.FadeOut(sceneTransitionDuration, () => fadeComplete = true);
            while (!fadeComplete) yield return null;
        }
        else
        {
            yield return new WaitForSeconds(sceneTransitionDuration);
        }

        // 2. DELAY while screen is black (requested)
        // This ensures the user sees black, not the scene resetting.
        yield return new WaitForSeconds(1.0f);

        // 3. Hide UI now that screen is fully black
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);

        // 4. Setup Logic for soft restart
        isSoftRestarting = true;
        targetRestartRoom = roomName;

        if (SaveSystem.Instance != null)
        {
            GameSaveData data = SaveSystem.Instance.GetCurrentSaveData();
            if (data != null)
            {
                // Find default spawn
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
                data.currentScene = roomName;
            }
        }

        // 5. Load Scene
        SceneManager.LoadScene(roomName);

        // UI Reset
        ResetUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

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

    // -----------------------------------------------------------
    // MAIN MENU LOGIC
    // -----------------------------------------------------------
    public void ReturnToMainMenu()
    {
        StartCoroutine(ReturnToMainMenuRoutine());
    }

    private IEnumerator ReturnToMainMenuRoutine()
    {
        // 1. Unfreeze time so ScreenFader works
        Time.timeScale = 1f;

        // 2. Fade Out Audio
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(sceneTransitionDuration);
            AudioManager.Instance.StopMusic(sceneTransitionDuration);
        }

        // 3. Fade Out Screen (To Black)
        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;
            ScreenFader.Instance.FadeOut(sceneTransitionDuration, () => fadeComplete = true);
            while (!fadeComplete) yield return null;
        }
        else
        {
            yield return new WaitForSeconds(sceneTransitionDuration);
        }

        // 4. Hide Game Over UI (now hidden by black screen)
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);

        // 5. Load
        ResetUI();
        SceneManager.LoadScene("MainMenu");
    }

    // Helper to fade out a UI panel (used during scene transitions)
    // NOTE: Not currently used for transitions to avoid transparency issues, 
    // but kept helper just in case you want to fade purely UI elements later.
    private IEnumerator FadeOutUI(GameObject panel)
    {
        if (panel == null || !panel.activeSelf) yield break;

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        float timer = 0f;
        float startAlpha = cg.alpha;

        // Use standard deltaTime since timeScale is 1 during transitions
        while (timer < sceneTransitionDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, timer / sceneTransitionDuration);
            yield return null;
        }
        cg.alpha = 0f;
        panel.SetActive(false);
    }

    private void ResetUI()
    {
        if (gameOverMessagePanel != null) gameOverMessagePanel.SetActive(false);
        if (gameOverOptionsPanel != null) gameOverOptionsPanel.SetActive(false);
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