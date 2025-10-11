using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SaveUIManager : MonoBehaviour
{
    public static SaveUIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject saveLoadPanel;

    [Header("Save Slot Prefab")]
    public GameObject saveSlotPrefab;
    public Transform slotParent;

    [Header("Buttons")]
    public Button closePanelButton;
    public Button newGameButton;

    private List<SaveSlotUI> saveSlots = new List<SaveSlotUI>();
    private bool wasOpenedFromPauseMenu = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // FIND REFERENCES AT RUNTIME (if not set in inspector)
        FindReferences();

        SetupUI();
        CreateSaveSlots();

        if (saveLoadPanel != null)
            saveLoadPanel.SetActive(false);
    }

    // NEW: Find references if they're missing
    void FindReferences()
    {
        // Find Save/Load Panel if not set
        if (saveLoadPanel == null)
        {
            saveLoadPanel = GameObject.Find("SaveLoadPanel");

            if (saveLoadPanel == null)
            {
                // Try finding in ButtonCanvas
                GameObject buttonCanvas = GameObject.Find("ButtonCanvas");
                if (buttonCanvas != null)
                {
                    Transform panelTransform = buttonCanvas.transform.Find("SaveLoadPanel");
                    if (panelTransform != null)
                    {
                        saveLoadPanel = panelTransform.gameObject;
                    }
                }
            }

            if (saveLoadPanel == null)
            {
                Debug.LogError("[SaveUI] SaveLoadPanel not found!");
            }
            else
            {
                Debug.Log("[SaveUI] SaveLoadPanel found successfully!");
            }
        }

        // Find SlotParent if not set
        if (slotParent == null && saveLoadPanel != null)
        {
            Transform slotParentTransform = saveLoadPanel.transform.Find("SlotParent");
            if (slotParentTransform == null)
            {
                slotParentTransform = saveLoadPanel.transform.Find("Slots");
            }
            if (slotParentTransform == null)
            {
                slotParentTransform = saveLoadPanel.transform.Find("SaveSlotContainer");
            }

            if (slotParentTransform != null)
            {
                slotParent = slotParentTransform;
                Debug.Log("[SaveUI] SlotParent found successfully!");
            }
            else
            {
                Debug.LogWarning("[SaveUI] SlotParent not found in SaveLoadPanel!");
            }
        }

        // Find buttons if not set
        if (closePanelButton == null && saveLoadPanel != null)
        {
            Button[] buttons = saveLoadPanel.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                if (btn.name.Contains("Close") || btn.name.Contains("Back"))
                {
                    closePanelButton = btn;
                    Debug.Log("[SaveUI] Close button found: " + btn.name);
                    break;
                }
            }
        }

        if (newGameButton == null && saveLoadPanel != null)
        {
            Button[] buttons = saveLoadPanel.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                if (btn.name.Contains("NewGame") || btn.name.Contains("New"))
                {
                    newGameButton = btn;
                    Debug.Log("[SaveUI] New Game button found: " + btn.name);
                    break;
                }
            }
        }
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
        if (SaveSystem.Instance == null)
        {
            Debug.LogWarning("[SaveUI] SaveSystem.Instance is null!");
            return;
        }

        // Create AutoSave slot (slot 0)
        CreateSaveSlot(0, false);

        // Create regular save slots (1-3)
        int maxSlots = SaveSystem.Instance.maxSaveSlots;
        for (int i = 1; i <= maxSlots; i++)
        {
            CreateSaveSlot(i, true);
        }

        RefreshSlots();
        Debug.Log($"[SaveUI] Created {saveSlots.Count} save slots");
    }

    void CreateSaveSlot(int slotIndex, bool canSave)
    {
        if (saveSlotPrefab == null || slotParent == null)
        {
            Debug.LogWarning("[SaveUI] Missing saveSlotPrefab or slotParent!");
            return;
        }

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

        // NEW: Force close inventory when save/load panel opens
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.ForceCloseInventory();
        }

        RefreshSlots();
        Debug.Log("[SaveUI] Save/Load panel opened");
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
        Debug.Log("[SaveUI] Save/Load panel closed");
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

                if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
                {
                    PauseMenuManager.Instance.ResumeGame();
                }

                Debug.Log("[SaveUI] AutoSave loaded");
            }
            return;
        }

        // Regular slots: Save if empty, Load if filled
        if (SaveSystem.Instance.HasSaveFile(slotIndex))
        {
            SaveSystem.Instance.LoadGame(slotIndex);
            CloseSaveLoadPanel();

            if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.ResumeGame();
            }

            Debug.Log($"[SaveUI] Game loaded from slot {slotIndex}");
        }
        else
        {
            SaveSystem.Instance.SaveGame(slotIndex);
            RefreshSlots();
            Debug.Log($"[SaveUI] Game saved to slot {slotIndex}");

            if (wasOpenedFromPauseMenu)
            {
                CloseSaveLoadPanel();
            }
        }
    }

    public void OnDeleteSlotClicked(int slotIndex)
    {
        if (SaveSystem.Instance == null || slotIndex == 0) return;

        SaveSystem.Instance.DeleteSave(slotIndex);
        RefreshSlots();
        Debug.Log($"[SaveUI] Deleted save slot {slotIndex}");
    }

    public void StartNewGame()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
            CloseSaveLoadPanel();

            if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.ResumeGame();
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("Room01_Foyer");
            Debug.Log("[SaveUI] Starting new game");
        }
    }

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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveGame(1);
                Debug.Log("[SaveUI] Quick saved to slot 1");
            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (SaveSystem.Instance != null && SaveSystem.Instance.HasSaveFile(1))
            {
                SaveSystem.Instance.LoadGame(1);
                Debug.Log("[SaveUI] Quick loaded from slot 1");
            }
        }
    }
}