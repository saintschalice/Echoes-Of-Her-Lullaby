using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

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
    public Button backToMainMenuButton;

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
        FindReferences();
        SetupUI();
        CreateSaveSlots();

        if (saveLoadPanel != null)
            saveLoadPanel.SetActive(false);
    }

    void FindReferences()
    {
        if (saveLoadPanel == null)
        {
            saveLoadPanel = GameObject.Find("SaveLoadPanel");

            if (saveLoadPanel == null)
            {
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

            // NEW: Also try finding SaveSlotSelectionPanel in MainMenu
            if (saveLoadPanel == null)
            {
                saveLoadPanel = GameObject.Find("SaveSlotSelectionPanel");
            }

            if (saveLoadPanel == null)
            {
                Debug.LogError("[SaveUI] SaveLoadPanel not found!");
            }
            else
            {
                Debug.Log("[SaveUI] SaveLoadPanel found: " + saveLoadPanel.name);
            }
        }

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

        if (closePanelButton == null && saveLoadPanel != null)
        {
            Button[] buttons = saveLoadPanel.GetComponentsInChildren<Button>(true);
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

        if (backToMainMenuButton == null && closePanelButton != null)
        {
            backToMainMenuButton = closePanelButton;
            Debug.Log("[SaveUI] Using closePanelButton as backToMainMenuButton");
        }

        if (newGameButton == null && saveLoadPanel != null)
        {
            Button[] buttons = saveLoadPanel.GetComponentsInChildren<Button>(true);
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
        bool isInMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";

        if (closePanelButton != null)
        {
            // In MainMenu, don't use closePanelButton (use backToMainMenuButton instead)
            if (!isInMainMenu)
            {
                closePanelButton.onClick.AddListener(CloseSaveLoadPanel);
            }
        }

        if (newGameButton != null)
            newGameButton.onClick.AddListener(StartNewGame);

        if (backToMainMenuButton != null)
        {
            // In MainMenu, back button should call OnBackToMainMenu
            if (isInMainMenu)
            {
                backToMainMenuButton.onClick.AddListener(OnBackToMainMenu);
                Debug.Log("[SaveUI] Back button setup for MainMenu");
            }
            else if (backToMainMenuButton != closePanelButton)
            {
                // In game, if it's a separate button
                backToMainMenuButton.onClick.AddListener(OnBackToMainMenu);
            }
        }
    }

    void CreateSaveSlots()
    {
        // Clear existing slots first
        foreach (SaveSlotUI slot in saveSlots)
        {
            if (slot != null && slot.gameObject != null)
            {
                Destroy(slot.gameObject);
            }
        }
        saveSlots.Clear();

        // Check current scene name
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        bool isInMainMenu = (currentSceneName == "MainMenu");

        Debug.Log($"[SaveUI] Current Scene: {currentSceneName}, IsInMainMenu: {isInMainMenu}");

        if (!isInMainMenu && SaveSystem.Instance == null)
        {
            Debug.LogWarning("[SaveUI] SaveSystem.Instance is null and not in MainMenu!");
            return;
        }

        // Determine max slots
        int maxSlots = 3; // Default
        if (!isInMainMenu && SaveSystem.Instance != null)
        {
            maxSlots = SaveSystem.Instance.maxSaveSlots;
        }

        Debug.Log($"[SaveUI] Creating save slots - MaxSlots: {maxSlots}, IsInMainMenu: {isInMainMenu}");

        // Create AutoSave slot (slot 0)
        CreateSaveSlot(0, false);

        // Create regular save slots (1 to maxSlots)
        for (int i = 1; i <= maxSlots; i++)
        {
            CreateSaveSlot(i, true);
            Debug.Log($"[SaveUI] Created slot {i}");
        }

        RefreshSlots();
        Debug.Log($"[SaveUI] Total slots created: {saveSlots.Count}");
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

    void OnBackToMainMenu()
    {
        if (saveLoadPanel != null)
            saveLoadPanel.SetActive(false);

        MainMenuManager mainMenu = FindFirstObjectByType<MainMenuManager>();
        if (mainMenu != null)
        {
            mainMenu.OnSaveSlotClosed();
        }
    }

    public void RefreshSlots()
    {
        bool isInMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";

        foreach (SaveSlotUI slot in saveSlots)
        {
            GameSaveData saveData = null;

            // NEW: Load save data directly from file if in MainMenu
            if (isInMainMenu)
            {
                saveData = LoadSaveDataFromFile(slot.SlotIndex);
            }
            else if (SaveSystem.Instance != null)
            {
                saveData = SaveSystem.Instance.GetSaveInfo(slot.SlotIndex);
            }

            slot.UpdateSlotInfo(saveData);
        }
    }

    // NEW: Load save data directly from file (for MainMenu)
    GameSaveData LoadSaveDataFromFile(int slot)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");
        string filePath = Path.Combine(savePath, $"save_slot_{slot}.json");

        if (!File.Exists(filePath))
            return null;

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveUI] Failed to load save data: {e.Message}");
            return null;
        }
    }

    public void OpenSaveLoadPanel()
    {
        if (saveLoadPanel != null)
        {
            saveLoadPanel.SetActive(true);
        }

        wasOpenedFromPauseMenu = PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused();

        if (!wasOpenedFromPauseMenu)
        {
            Time.timeScale = 0f;
        }

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

        if (!wasOpenedFromPauseMenu)
        {
            Time.timeScale = 1f;
        }
        else
        {
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
        bool isInMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";

        // Check if save file exists
        bool hasSaveFile = false;
        if (isInMainMenu)
        {
            hasSaveFile = CheckSaveFileExists(slotIndex);
        }
        else if (SaveSystem.Instance != null)
        {
            hasSaveFile = SaveSystem.Instance.HasSaveFile(slotIndex);
        }

        if (slotIndex == 0)
        {
            if (hasSaveFile)
            {
                if (isInMainMenu)
                {
                    // Load save data, then load PersistentScene
                    PlayerPrefs.SetInt("LoadSlotOnStart", slotIndex);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("PersistentScene");
                }
                else if (SaveSystem.Instance != null)
                {
                    SaveSystem.Instance.LoadGame(slotIndex);
                    CloseSaveLoadPanel();
                    if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
                    {
                        PauseMenuManager.Instance.ResumeGame();
                    }
                }
            }
            return;
        }

        if (hasSaveFile)
        {
            if (isInMainMenu)
            {
                // Load save data, then load PersistentScene
                PlayerPrefs.SetInt("LoadSlotOnStart", slotIndex);
                UnityEngine.SceneManagement.SceneManager.LoadScene("PersistentScene");
            }
            else if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.LoadGame(slotIndex);
                CloseSaveLoadPanel();
                if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
                {
                    PauseMenuManager.Instance.ResumeGame();
                }
            }
        }
        else if (!isInMainMenu && SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveGame(slotIndex);
            RefreshSlots();
        }
    }

    // NEW: Check if save file exists (for MainMenu)
    bool CheckSaveFileExists(int slot)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");
        string filePath = Path.Combine(savePath, $"save_slot_{slot}.json");
        return File.Exists(filePath);
    }

    public void OnDeleteSlotClicked(int slotIndex)
    {
        if (slotIndex == 0) return;

        bool isInMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";

        if (isInMainMenu)
        {
            // Delete file directly in MainMenu
            string savePath = Path.Combine(Application.persistentDataPath, "Saves");
            string filePath = Path.Combine(savePath, $"save_slot_{slotIndex}.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"[SaveUI] Deleted save slot {slotIndex}");
            }
        }
        else if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.DeleteSave(slotIndex);
        }

        RefreshSlots();
    }

    public void StartNewGame()
    {
        bool isInMainMenu = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";

        if (isInMainMenu)
        {
            // Signal that we want a new game
            PlayerPrefs.SetInt("LoadSlotOnStart", -1); // -1 = new game
            UnityEngine.SceneManagement.SceneManager.LoadScene("PersistentScene");
        }
        else if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
            CloseSaveLoadPanel();

            if (wasOpenedFromPauseMenu && PauseMenuManager.Instance != null)
            {
                PauseMenuManager.Instance.ResumeGame();
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("Room01_Foyer");
        }

        Debug.Log("[SaveUI] Starting new game");
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
            case "Room04_Kitchen_Dining": return "Kitchen & Dining";
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