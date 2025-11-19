using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages game over state when Emily catches Lisa.
/// Handles intelligent restarting logic: Restarts the current room while keeping inventory.
/// Fixed: Prevents character freezing and correctly spawns at default room spawn.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Audio")]
    public AudioClip deathSound;

    // Flag to track if we are doing a "Soft Restart"
    private bool isSoftRestarting = false;
    private string targetRestartRoom = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SetupButtons();
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
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void TriggerGameOver(string message = "You were caught...")
    {
        if (gameOverPanel == null) return;

        // Freeze game
        Time.timeScale = 0f;

        // Show UI
        gameOverPanel.SetActive(true);
        if (gameOverText != null)
            gameOverText.text = message;

        // Play death sound
        if (deathSound != null)
            AudioManager.Instance?.PlaySFX(deathSound);

        // Disable player controls immediately
        DisablePlayerControls();

        Debug.Log("[GameOver] Player caught by Emily");
    }

    private void DisablePlayerControls()
    {
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = false;
            // Also stop any movement physics to prevent sliding while dead
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    public void RestartLevel()
    {
        // Ensure time is running immediately when button is clicked
        Time.timeScale = 1f;

        string currentRoomName = SceneManager.GetActiveScene().name;
        StartCoroutine(RestartRoutine(currentRoomName));
    }

    private IEnumerator RestartRoutine(string roomName)
    {
        Time.timeScale = 1f;

        // --- Reset Audio State ---
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(0f);
            AudioManager.Instance.StopAllSFX();
            AudioManager.Instance.StopMusic(0f);
        }

        // Set flags for OnSceneLoaded to handle unfreezing
        isSoftRestarting = true;
        targetRestartRoom = roomName;

        // --- SOFT RESTART LOGIC ---
        // Goal: Keep Inventory, Reset Room State, Spawn at Default Point

        if (SaveSystem.Instance != null)
        {
            GameSaveData data = SaveSystem.Instance.GetCurrentSaveData();
            if (data != null)
            {
                // 1. FIND DEFAULT SPAWN (In current scene before reload)
                Vector3 spawnPos = Vector3.zero;
                bool foundSpawn = false;

                // Find all spawn points in the current scene
                RoomSpawnPoint[] spawns = FindObjectsByType<RoomSpawnPoint>(FindObjectsSortMode.None);

                // Look for the default one
                foreach (var sp in spawns)
                {
                    if (sp.isDefaultSpawnPoint)
                    {
                        spawnPos = sp.transform.position;
                        foundSpawn = true;
                        Debug.Log($"[GameOver] Found default spawn at {spawnPos}");
                        break;
                    }
                }

                // 2. UPDATE SAVE SYSTEM PLAYER POSITION
                // This ensures when the scene reloads, SaveSystem places us here
                // instead of where we died or where the last save was.
                if (foundSpawn)
                {
                    data.playerPosition = spawnPos;
                }

                // 3. RESET ROOM PROGRESS
                // Clear the state for this specific room so puzzles/doors reset
                if (data.roomStates.ContainsKey(roomName))
                {
                    Debug.Log($"[GameOver] Resetting state for room: {roomName}");
                    data.roomStates[roomName] = new RoomState(); // Wipe state (resets isCompleted, openedDoors, etc.)
                }

                // Optional: Remove from completed rooms list if you track that globally
                if (data.completedRooms.Contains(roomName))
                {
                    data.completedRooms.Remove(roomName);
                }

                // Ensure SaveSystem knows we are staying in this scene
                data.currentScene = roomName;
            }
        }

        // 4. RELOAD SCENE
        // We do NOT use SaveSystem.LoadGame() because that loads from disk (old data).
        // We reload the scene directly, relying on the modified SaveSystem data in memory.
        SceneManager.LoadScene(roomName);

        // Hide Game Over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        yield return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        // Only finalize if this was a restart initiated by us
        if (isSoftRestarting && scene.name == targetRestartRoom)
        {
            StartCoroutine(FinalizeSoftRestart());
        }
    }

    private IEnumerator FinalizeSoftRestart()
    {
        Debug.Log($"[GameOver] Finalizing Soft Restart for {targetRestartRoom}...");

        // Wait for other scripts (SaveSystem, SpawnManager) to initialize
        // Two frames is safer to ensure Start() methods have run
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // 1. Force Player Controls Enabled
        // We use FindFirstObjectByType because the old reference might be destroyed
        var player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = true;
            Debug.Log("[GameOver] Player controls script re-enabled.");
        }
        else
        {
            // Fallback: Try finding by tag if Type lookup fails
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                var ctrl = playerObj.GetComponent<JoystickPlayerController>();
                if (ctrl != null) ctrl.enabled = true;
            }
            else
            {
                Debug.LogWarning("[GameOver] Player controller not found to re-enable!");
            }
        }

        // 2. Force Joystick UI Active
        EnsureJoystickActive();

        // 3. Reset Flags
        isSoftRestarting = false;
        targetRestartRoom = "";
    }

    private void EnsureJoystickActive()
    {
        // Try standard lookup
        GameObject joystickUI = GameObject.Find("Joystick");

        // Try finding inside PersistentUI
        if (joystickUI == null)
        {
            GameObject persistentUI = GameObject.Find("PersistentUI");
            if (persistentUI != null)
            {
                Transform joystickTransform = persistentUI.transform.Find("Joystick");
                if (joystickTransform != null)
                    joystickUI = joystickTransform.gameObject;
            }
        }

        // Try finding by Tag
        if (joystickUI == null)
        {
            joystickUI = GameObject.FindGameObjectWithTag("Joystick");
        }

        if (joystickUI != null)
        {
            joystickUI.SetActive(true);

            // Also ensure the canvas group isn't blocking/hidden if you use one
            CanvasGroup cg = joystickUI.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            Debug.Log("[GameOver] Joystick UI force-activated.");
        }
        else
        {
            Debug.LogWarning("[GameOver] Could not find Joystick UI to activate!");
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}