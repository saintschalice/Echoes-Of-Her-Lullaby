using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// DEBUG ONLY: Resets game progress to start a fresh playthrough without deleting save files
/// Attach this to a button in your test scenes for quick testing
/// </summary>
public class DebugNewGameButton : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Scene to load after resetting (usually Room01_Foyer)")]
    public string startingScene = "Room01_Foyer";

    [Tooltip("Show confirmation dialog before resetting?")]
    public bool requireConfirmation = true;

    [Header("Optional References")]
    [Tooltip("Leave empty to auto-find button on this GameObject")]
    public Button debugButton;

    private void Start()
    {
        // Auto-find button if not assigned
        if (debugButton == null)
        {
            debugButton = GetComponent<Button>();
        }

        // Setup button listener
        if (debugButton != null)
        {
            debugButton.onClick.AddListener(OnDebugNewGameClicked);
            Debug.Log("[DebugNewGame] Button initialized - Click to start fresh game");
        }
        else
        {
            Debug.LogWarning("[DebugNewGame] No button found! Attach this script to a Button or assign one in inspector");
        }
    }

    void OnDebugNewGameClicked()
    {
        if (requireConfirmation)
        {
            // Simple confirmation using Unity's built-in dialog (Editor only)
#if UNITY_EDITOR
            bool confirmed = UnityEditor.EditorUtility.DisplayDialog(
                "Debug: Start New Game",
                "This will reset your current progress (but keep your save files).\n\nAre you sure?",
                "Yes, Reset Progress",
                "Cancel"
            );

            if (!confirmed)
            {
                Debug.Log("[DebugNewGame] User cancelled reset");
                return;
            }
#endif
        }

        StartNewGame();
    }

    public void StartNewGame()
    {
        Debug.Log("[DebugNewGame] Creating fresh game state...");

        // Close any open UI panels
        CloseAllUIPanels();

        // Create completely fresh save data
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
            Debug.Log("[DebugNewGame] Fresh game data created");
        }
        else
        {
            Debug.LogWarning("[DebugNewGame] SaveSystem not found!");
        }

        // Reset inventory
        ResetInventory();

        // Resume time in case it was paused
        Time.timeScale = 1f;

        // Load starting scene
        Debug.Log($"[DebugNewGame] Loading starting scene: {startingScene}");
        SceneManager.LoadScene(startingScene);
    }

    void CloseAllUIPanels()
    {
        // Close inventory
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null && inventoryUI.IsOpen)
        {
            inventoryUI.ForceCloseInventory();
        }

        // Close save/load panel
        if (SaveUIManager.Instance != null && SaveUIManager.Instance.saveLoadPanel != null)
        {
            SaveUIManager.Instance.saveLoadPanel.SetActive(false);
        }

        // Close pause menu
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused())
        {
            PauseMenuManager.Instance.ResumeGame();
        }

        // Close dialogue
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            DialogueSystemV2.Instance.EndDialogue();
        }

        Debug.Log("[DebugNewGame] All UI panels closed");
    }

    void ResetInventory()
    {
        // Clear inventory state
        if (InventoryManager.Instance != null)
        {
            Debug.Log("[DebugNewGame] Inventory will be reset with new game data");
        }
    }

    // PUBLIC METHOD: Call this from other scripts if needed
    public static void QuickStartNewGame()
    {
        Debug.Log("[DebugNewGame] Quick starting new game...");

        // Close all UI
        DebugNewGameButton instance = FindFirstObjectByType<DebugNewGameButton>();
        if (instance != null)
        {
            instance.CloseAllUIPanels();
        }

        // Create fresh save
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
        }

        // Resume time
        Time.timeScale = 1f;

        // Load starting scene
        SceneManager.LoadScene("Room01_Foyer");
    }

    // Keyboard shortcut for quick testing (Editor only)
    void Update()
    {
#if UNITY_EDITOR
        // Press Ctrl+N to quick restart
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("[DebugNewGame] Keyboard shortcut pressed (Ctrl+N)");
            StartNewGame();
        }
#endif
    }

    #region Context Menu Testing
    [ContextMenu("Start New Game (No Confirmation)")]
    void DebugStartNewGame()
    {
        StartNewGame();
    }

    [ContextMenu("Test Button Click")]
    void TestButtonClick()
    {
        OnDebugNewGameClicked();
    }
    #endregion
}