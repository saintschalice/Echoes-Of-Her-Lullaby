using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SaveUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject saveLoadPanel;

    [Header("Save Slot Prefab")]
    public GameObject saveSlotPrefab;
    public Transform slotParent; // Single parent for all slots

    [Header("Buttons")]
    public Button closePanelButton;
    public Button newGameButton;

    private List<SaveSlotUI> saveSlots = new List<SaveSlotUI>();
    private bool wasOpenedFromPauseMenu = false;

    void Start()
    {
        SetupUI();
        CreateSaveSlots();

        if (saveLoadPanel != null)
            saveLoadPanel.SetActive(false);
    }

    void SetupUI()
    {
        if (closePanelButton != null)
            closePanelButton.onClick.AddListener(CloseSaveLoadPanel);

        if (newGameButton != null)
            newGameButton.onClick.AddListener(StartNewGame);
    }

    void CreateSaveSlots()
    {
        if (SaveSystem.Instance == null) return;

        // Create AutoSave slot (slot 0)
        CreateSaveSlot(0, false); // AutoSave is load-only

        // Create regular save slots (1-3)
        int maxSlots = SaveSystem.Instance.maxSaveSlots;
        for (int i = 1; i <= maxSlots; i++)
        {
            CreateSaveSlot(i, true); // Regular slots can save/load
        }

        RefreshSlots();
    }

    void CreateSaveSlot(int slotIndex, bool canSave)
    {
        if (saveSlotPrefab == null || slotParent == null) return;

        GameObject slotObj = Instantiate(saveSlotPrefab, slotParent);
        SaveSlotUI slotUI = slotObj.GetComponent<SaveSlotUI>();

        if (slotUI == null)
        {
            slotUI = slotObj.AddComponent<SaveSlotUI>();
        }

        slotUI.Initialize(slotIndex, canSave, this);
        saveSlots.Add(slotUI);
    }

    public void RefreshSlots()
    {
        foreach (SaveSlotUI slot in saveSlots)
        {
            if (SaveSystem.Instance != null)
            {
                GameSaveData saveData = SaveSystem.Instance.GetSaveInfo(slot.SlotIndex);
                slot.UpdateSlotInfo(saveData);
            }
        }
    }

    public void OpenSaveLoadPanel()
    {
        if (saveLoadPanel != null)
        {
            saveLoadPanel.SetActive(true);
        }

        // Check if opened from pause menu
        wasOpenedFromPauseMenu = PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused();

        // Only pause game if not already paused by pause menu
        if (!wasOpenedFromPauseMenu)
        {
            Time.timeScale = 0f;
        }

        RefreshSlots();
    }

    public void CloseSaveLoadPanel()
    {
        if (saveLoadPanel != null)
            saveLoadPanel.SetActive(false);

        // Only resume game if we paused it (not if pause menu is handling it)
        if (!wasOpenedFromPauseMenu)
        {
            Time.timeScale = 1f;
        }
        else
        {
            // Notify pause menu that save menu was closed
            if (PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.OnSaveMenuClosed();
            }
        }

        wasOpenedFromPauseMenu = false;
    }

    public void OnSlotClicked(int slotIndex)
    {
        if (SaveSystem.Instance == null) return;

        // AutoSave slot (0) is load-only
        if (slotIndex == 0)
        {
            if (SaveSystem.Instance.HasSaveFile(slotIndex))
            {
                SaveSystem.Instance.LoadGame(slotIndex);
                CloseSaveLoadPanel();

                // If we loaded from pause menu, also close pause menu
                if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
                {
                    PauseMenuManager.Instance.ResumeGame();
                }

                Debug.Log("AutoSave loaded");
            }
            return;
        }

        // Regular slots: Save if empty, Load if filled
        if (SaveSystem.Instance.HasSaveFile(slotIndex))
        {
            // Load existing save
            SaveSystem.Instance.LoadGame(slotIndex);
            CloseSaveLoadPanel();

            // If we loaded from pause menu, also close pause menu
            if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.ResumeGame();
            }

            Debug.Log($"Game loaded from slot {slotIndex}");
        }
        else
        {
            // Save to empty slot with auto-generated name
            SaveSystem.Instance.SaveGame(slotIndex);
            RefreshSlots();
            Debug.Log($"Game saved to slot {slotIndex}");

            // If saving from pause menu, just close save panel (keep pause menu open)
            if (wasOpenedFromPauseMenu)
            {
                CloseSaveLoadPanel();
            }
        }
    }

    public void OnDeleteSlotClicked(int slotIndex)
    {
        if (SaveSystem.Instance == null || slotIndex == 0) return; // Can't delete AutoSave

        SaveSystem.Instance.DeleteSave(slotIndex);
        RefreshSlots();
        Debug.Log($"Deleted save slot {slotIndex}");
    }

    public void StartNewGame()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
            CloseSaveLoadPanel();

            // Close pause menu if it was open
            if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.ResumeGame();
            }

            // Load first scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Room01_Foyer");
        }
    }

    // Get display name for room scenes
    public static string GetRoomDisplayName(string sceneName)
    {
        switch (sceneName)
        {
            case "SplashScreen": return "Game Start";
            case "MainMenu": return "Main Menu";
            case "Room01_Foyer": return "Foyer";
            case "Room02_LivingRoom": return "Living Room";
            case "Room03_Hallway": return "Hallway";
            case "Room04_Kitchen": return "Kitchen";
            case "Room05_DiningRoom": return "Dining Room";
            case "Room06_ReturnHallway": return "Return Hallway";
            case "Room07_LisaBedroom": return "Lisa's Bedroom";
            case "Room08_LisaBathroom": return "Lisa's Bathroom";
            case "Room09_MasterBathroom": return "Master Bathroom";
            case "Room10_MasterBedroom": return "Master Bedroom";
            case "LoadingScreen": return "Loading";
            case "PauseMenu": return "Paused";
            case "InventoryScreen": return "Inventory";
            case "GameOver": return "Game Over";
            case "Credits": return "Credits";
            case "Tutorial": return "Tutorial";
            default: return "Unknown Location";
        }
    }

    void Update()
    {
        // ESC to toggle save menu (only if pause menu is not handling it)
        if (Input.GetKeyDown(KeyCode.S) && PauseMenuManager.Instance == null)
        {
            if (saveLoadPanel != null && saveLoadPanel.activeSelf)
            {
                CloseSaveLoadPanel();
            }
            else
            {
                OpenSaveLoadPanel();
            }
        }

        // F5 for quick save to slot 1
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveGame(1);
                Debug.Log("Quick saved to slot 1");
            }
        }

        // F9 for quick load from slot 1
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (SaveSystem.Instance != null && SaveSystem.Instance.HasSaveFile(1))
            {
                SaveSystem.Instance.LoadGame(1);
                Debug.Log("Quick loaded from slot 1");
            }
        }
    }
}