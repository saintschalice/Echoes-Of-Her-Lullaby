using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class GameSaveData
{
    // Player Progress
    public string currentScene;
    public Vector3 playerPosition;
    public int currentChapter;
    public float playtimeSeconds;

    // Story Progress
    public List<string> completedRooms = new List<string>();
    public List<string> collectedMemoryFragments = new List<string>();
    public List<string> completedPuzzles = new List<string>();
    public List<string> triggeredDialogues = new List<string>();
    public int lullabySongProgress;

    // Inventory
    public List<string> inventoryItems = new List<string>();
    public List<string> examinedObjects = new List<string>();

    // Room States
    public Dictionary<string, RoomState> roomStates = new Dictionary<string, RoomState>();

    // Audio Settings
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float dialogueVolume = 1f;
    public float musicVolume = 1f;
    public float ambientVolume = 1f;

    // Video Settings
    public float brightness = 0.5f;
    public float contrast = 0.5f;

    // Metadata
    public string saveDate;
    public string saveName;
    public int saveSlot;

    public GameSaveData()
    {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd");
        currentScene = "Room01_Foyer";

        // NEW: Set default spawn position for Room01_Foyer Main spawn
        playerPosition = new Vector3(-2.98f, -11.87f, 0f); // Your SpawnPoint_Main position

        currentChapter = 1;
        playtimeSeconds = 0f;
        lullabySongProgress = 0;
        saveSlot = 1;
        saveName = "Save Game";

        // Default settings
        masterVolume = 1f;
        sfxVolume = 1f;
        dialogueVolume = 1f;
        musicVolume = 1f;
        ambientVolume = 1f;
        brightness = 0.5f;
        contrast = 0.5f;
    }
}

[System.Serializable]
public class RoomState
{
    public List<string> interactedObjects = new List<string>();
    public List<string> solvedPuzzles = new List<string>();
    public List<string> openedDoors = new List<string>();
    public List<string> collectedItems = new List<string>();
    public bool isCompleted = false;
    public bool hasBeenVisited = false;
}

public class SaveSystem : MonoBehaviour
{
    [Header("Save Settings")]
    public int maxSaveSlots = 3;
    public bool autoSaveEnabled = true;
    public float autoSaveInterval = 120f;

    [Header("References")]
    public Transform player;

    private GameSaveData currentSaveData;
    private float autoSaveTimer;
    private float sessionStartTime;

    public static SaveSystem Instance { get; private set; }

    public System.Action<GameSaveData> OnGameLoaded;
    public System.Action<GameSaveData> OnGameSaved;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (currentSaveData == null)
        {
            // Don't auto-load or create game data yet
            // MainMenu will handle this
            Debug.Log("[SaveSystem] Initialized, waiting for MainMenu input");
        }
    }

    void Start()
    {
        sessionStartTime = Time.time;

        // Find player reference if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log($"[SaveSystem] Found player: {player.name}");
            }
            else
            {
                Debug.LogError("[SaveSystem] Player not found! Make sure Lisa has 'Player' tag");
            }
        }

        // NEW: Check if we should load a specific save slot (from MainMenu)
        if (PlayerPrefs.HasKey("LoadSlotOnStart"))
        {
            int slotToLoad = PlayerPrefs.GetInt("LoadSlotOnStart");
            PlayerPrefs.DeleteKey("LoadSlotOnStart"); // Clear the flag

            if (slotToLoad == -1)
            {
                // New game requested from MainMenu
                CreateNewGame();
                
                // Clear inventory for fresh start
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.ClearAllItems();
                }
                
                Debug.Log("[SaveSystem] New game started from MainMenu");
            }
            else if (HasSaveFile(slotToLoad))
            {
                // Load specific save slot
                LoadGame(slotToLoad);
                Debug.Log($"[SaveSystem] Loaded save slot {slotToLoad} from MainMenu");
            }
            else
            {
                // Fallback to new game if save doesn't exist
                CreateNewGame();
                Debug.Log("[SaveSystem] Save not found, starting new game");
            }
        }
        else
        {
            // Normal start - just create empty save data, don't load anything
            if (currentSaveData == null)
            {
                CreateNewGame();
            }
        }

        Debug.Log($"SaveSystem initialized - currentSaveData: {(currentSaveData != null ? "CHECK" : "NULL")}");
    }

    void Update()
    {
        UpdatePlaytime();
        HandleAutoSave();

        if (Input.GetKeyDown(KeyCode.F5))
        {
            QuickSave();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            QuickLoad();
        }
    }

    void InitializeSaveSystem()
    {
        string savePath = GetSavePath();
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "Saves");
    }

    string GetSaveFilePath(int slot)
    {
        return Path.Combine(GetSavePath(), $"save_slot_{slot}.json");
    }

    void UpdatePlaytime()
    {
        if (currentSaveData != null)
        {
            currentSaveData.playtimeSeconds += Time.deltaTime;
        }
    }

    void HandleAutoSave()
    {
        if (!autoSaveEnabled) return;

        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveInterval)
        {
            AutoSave();
            autoSaveTimer = 0f;
        }
    }

    public void CreateNewGame()
    {
        // CRITICAL: Clear ALL PlayerPrefs to reset room-specific progress
        ClearAllGameProgress();

        currentSaveData = new GameSaveData();
        currentSaveData.saveName = "New Game";
        currentSaveData.currentScene = "Room01_Foyer";

        // NEW: Find the default spawn point in Room01_Foyer
        RoomSpawnPoint[] spawnPoints = FindObjectsByType<RoomSpawnPoint>(FindObjectsSortMode.None);
        foreach (RoomSpawnPoint sp in spawnPoints)
        {
            if (sp.roomName == "Room01_Foyer" && sp.isDefaultSpawnPoint)
            {
                currentSaveData.playerPosition = sp.transform.position;
                Debug.Log($"[SaveSystem] New game spawn set to: {sp.spawnPointID} at {sp.transform.position}");
                break;
            }
        }

        // Fallback if no spawn point found
        if (currentSaveData.playerPosition == Vector3.zero)
        {
            currentSaveData.playerPosition = new Vector3(-2.98f, -11.87f, 0f); // Your spawn point position
            Debug.LogWarning("[SaveSystem] Using hardcoded spawn position");
        }

        Debug.Log("[SaveSystem] Created new game save data - ALL progress cleared");
    }

    /// <summary>
    /// Clears ALL game progress including PlayerPrefs for room-specific data
    /// Called when starting a new game to ensure fresh start
    /// </summary>
    private void ClearAllGameProgress()
    {
        Debug.Log("[SaveSystem] Clearing ALL game progress...");

        // Clear all room-specific PlayerPrefs
        // Room 01
        PlayerPrefs.DeleteKey("FoyerIntro_Played");
        PlayerPrefs.DeleteKey("Foyer_MailPickedUp");
        
        // Room 02
        PlayerPrefs.DeleteKey("R02_TVInteracted");
        PlayerPrefs.DeleteKey("R02_PianoInteracted");
        PlayerPrefs.DeleteKey("R02_LullabyPlayed");
        PlayerPrefs.DeleteKey("R02_MrSnugglesFixed");
        PlayerPrefs.DeleteKey("R02_SmallKeyObtained");
        
        // Room 03
        PlayerPrefs.DeleteKey("R03_ClosetUsed");
        
        // Room 04
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_bridge");
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_dough");
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_oven");
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_cookies");
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_recipe");
        PlayerPrefs.DeleteKey("kitchen_cookie_puzzle_floorboard");
        PlayerPrefs.DeleteKey("emily_kitchen_intro");
        PlayerPrefs.DeleteKey("Room04_Bridge_Completed");
        PlayerPrefs.DeleteKey("Room04_Bridge_Fixed");
        
        // Room 05
        PlayerPrefs.DeleteKey("R05_Calendar");
        PlayerPrefs.DeleteKey("R05_Cabinet");
        PlayerPrefs.DeleteKey("R05_HasSpoon");
        PlayerPrefs.DeleteKey("R05_SpoonPlaced");
        PlayerPrefs.DeleteKey("R05_FirstHide");
        PlayerPrefs.DeleteKey("R05_Chairs");
        PlayerPrefs.DeleteKey("R05_ChildChair");
        PlayerPrefs.DeleteKey("R05_MotherChair");
        PlayerPrefs.DeleteKey("R05_FatherChair");
        
        // Room 06
        PlayerPrefs.DeleteKey("R06_IntroPlayed");
        PlayerPrefs.DeleteKey("R06_PhotoInteracted");
        
        // Room 07
        PlayerPrefs.DeleteKey("R07_IntroPlayed");
        PlayerPrefs.DeleteKey("R07_ToyboxOpened");
        PlayerPrefs.DeleteKey("R07_SlidingPuzzleSolved");
        PlayerPrefs.DeleteKey("R07_TeaPartyComplete");
        PlayerPrefs.DeleteKey("R07_CabinetUnlocked");
        PlayerPrefs.DeleteKey("R07_AllPuzzlesComplete");
        
        // Room 08
        PlayerPrefs.DeleteKey("R08_IntroPlayed");
        PlayerPrefs.DeleteKey("R08_MedicineCabinetOpened");
        PlayerPrefs.DeleteKey("R08_BathtubInteracted");
        PlayerPrefs.DeleteKey("R08_AllEvidenceCollected");
        PlayerPrefs.DeleteKey("R08_MirrorQTEComplete");
        
        // General game progress
        PlayerPrefs.DeleteKey("LoadSlotOnStart");
        PlayerPrefs.DeleteKey("HasSeenIntro");
        PlayerPrefs.DeleteKey("CurrentChapter");
        
        // Clear any dialogue triggers
        for (int i = 0; i < 100; i++)
        {
            PlayerPrefs.DeleteKey($"Dialogue_Triggered_{i}");
        }
        
        PlayerPrefs.Save();
        Debug.Log("[SaveSystem] All PlayerPrefs cleared for new game");
    }

    public void SaveGame(int slot, string saveName = "")
    {
        if (currentSaveData == null)
        {
            Debug.LogWarning("No save data to save!");
            return;
        }

        // Important: UpdatePlayerData is called before we get the display name
        UpdatePlayerData();
        UpdateSettingsData();

        string roomDisplayName = SaveUIManager.GetRoomDisplayName(currentSaveData.currentScene);

        // If saveName argument is empty, use the room name
        if (string.IsNullOrEmpty(saveName))
        {
            currentSaveData.saveName = roomDisplayName;
        }
        else
        {
            currentSaveData.saveName = saveName;
        }

        currentSaveData.saveSlot = slot;
        currentSaveData.saveDate = DateTime.Now.ToString("yyyy-MM-dd");

        try
        {
            string json = JsonUtility.ToJson(currentSaveData, true);
            string filePath = GetSaveFilePath(slot);

            File.WriteAllText(filePath, json);

            string slotType = slot == 0 ? "AutoSave" : $"slot {slot}";
            Debug.Log($"Game saved to {slotType}: {filePath}");
            OnGameSaved?.Invoke(currentSaveData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public void AutoSave()
    {
        SaveGame(0);
        Debug.Log("AutoSave completed");
    }

    public void OnRoomEntered(string roomName)
    {
        if (currentSaveData != null)
        {
            currentSaveData.currentScene = roomName;

            // Mark room as visited
            RoomState roomState = GetRoomState(roomName);
            roomState.hasBeenVisited = true;
            UpdateRoomState(roomName, roomState);

            AutoSave();
        }
    }

    public void OnStoryProgressMade()
    {
        AutoSave();
    }

    public bool LoadGame(int slot)
    {
        string filePath = GetSaveFilePath(slot);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Save file not found: {filePath}");
            return false;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            currentSaveData = JsonUtility.FromJson<GameSaveData>(json);

            ApplyLoadedData();

            Debug.Log($"Game loaded from slot {slot}");
            OnGameLoaded?.Invoke(currentSaveData);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return false;
        }
    }

    public bool DeleteSave(int slot)
    {
        string filePath = GetSaveFilePath(slot);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Deleted save slot {slot}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save: {e.Message}");
                return false;
            }
        }

        return false;
    }

    public bool HasSaveFile(int slot)
    {
        return File.Exists(GetSaveFilePath(slot));
    }

    public GameSaveData GetSaveInfo(int slot)
    {
        if (!HasSaveFile(slot)) return null;

        try
        {
            string json = File.ReadAllText(GetSaveFilePath(slot));
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        catch
        {
            return null;
        }
    }

    void UpdatePlayerData()
    {
        if (currentSaveData == null) return;

        if (player != null)
        {
            currentSaveData.playerPosition = player.position;
        }

        // FIX: Don't overwrite currentScene with GetActiveScene().name blindly.
        // We want to trust OnRoomEntered to set the specific room name.
        // Only set it if it's currently empty.
        if (string.IsNullOrEmpty(currentSaveData.currentScene))
        {
            currentSaveData.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
    }

    void UpdateSettingsData()
    {
        if (currentSaveData == null) return;

        // Get settings from PauseMenuManager if available
        if (PauseMenuManager.Instance != null)
        {
            currentSaveData.masterVolume = PauseMenuManager.Instance.GetMasterVolume();
            currentSaveData.sfxVolume = PauseMenuManager.Instance.GetSFXVolume();
            currentSaveData.dialogueVolume = PauseMenuManager.Instance.GetDialogueVolume();
            currentSaveData.musicVolume = PauseMenuManager.Instance.GetMusicVolume();
            currentSaveData.ambientVolume = PauseMenuManager.Instance.GetAmbientVolume();
            currentSaveData.brightness = PauseMenuManager.Instance.GetBrightness();
            currentSaveData.contrast = PauseMenuManager.Instance.GetContrast();
        }
    }

    void ApplyLoadedData()
    {
        if (currentSaveData == null) return;

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene != currentSaveData.currentScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSaveData.currentScene);
        }

        StartCoroutine(ApplyPlayerPositionDelayed());
        ApplySettings();
    }

    System.Collections.IEnumerator ApplyPlayerPositionDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (player != null && currentSaveData != null)
        {
            player.position = currentSaveData.playerPosition;
            Debug.Log($"Player position loaded: {currentSaveData.playerPosition}");
        }
    }

    void ApplySettings()
    {
        if (currentSaveData == null) return;

        // Apply to AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(currentSaveData.masterVolume);
            AudioManager.Instance.SetSFXVolume(currentSaveData.sfxVolume);
            AudioManager.Instance.SetDialogueVolume(currentSaveData.dialogueVolume);
            AudioManager.Instance.SetMusicVolume(currentSaveData.musicVolume);
            AudioManager.Instance.SetAmbientVolume(currentSaveData.ambientVolume);
        }

        // Apply to PauseMenuManager
        if (PauseMenuManager.Instance != null)
        {
            PauseMenuManager.Instance.ApplyLoadedSettings(
                currentSaveData.masterVolume,
                currentSaveData.sfxVolume,
                currentSaveData.dialogueVolume,
                currentSaveData.musicVolume,
                currentSaveData.ambientVolume,
                currentSaveData.brightness,
                currentSaveData.contrast
            );
        }
    }

    // Public progression methods
    public void MarkRoomCompleted(string roomName)
    {
        if (currentSaveData != null && !currentSaveData.completedRooms.Contains(roomName))
        {
            currentSaveData.completedRooms.Add(roomName);
        }
    }

    public void AddMemoryFragment(string fragmentId)
    {
        if (currentSaveData != null && !currentSaveData.collectedMemoryFragments.Contains(fragmentId))
        {
            currentSaveData.collectedMemoryFragments.Add(fragmentId);
            currentSaveData.lullabySongProgress = currentSaveData.collectedMemoryFragments.Count;
        }
    }

    public void MarkPuzzleSolved(string puzzleId)
    {
        if (currentSaveData != null && !currentSaveData.completedPuzzles.Contains(puzzleId))
        {
            currentSaveData.completedPuzzles.Add(puzzleId);
        }
    }

    public void AddInventoryItem(string itemId)
    {
        if (currentSaveData != null && !currentSaveData.inventoryItems.Contains(itemId))
        {
            currentSaveData.inventoryItems.Add(itemId);
            Debug.Log("[SaveSystem] Added item: " + itemId);

            if (autoSaveEnabled)
            {
                AutoSave();
            }
        }
    }

    public void RemoveInventoryItem(string itemId)
    {
        if (currentSaveData != null && currentSaveData.inventoryItems.Contains(itemId))
        {
            currentSaveData.inventoryItems.Remove(itemId);
            Debug.Log("[SaveSystem] Removed item: " + itemId);

            if (autoSaveEnabled)
            {
                AutoSave();
            }
        }
    }

    public void MarkObjectExamined(string objectId)
    {
        if (currentSaveData != null && !currentSaveData.examinedObjects.Contains(objectId))
        {
            currentSaveData.examinedObjects.Add(objectId);
        }
    }

    public void TriggerDialogue(string dialogueId)
    {
        if (currentSaveData != null && !currentSaveData.triggeredDialogues.Contains(dialogueId))
        {
            currentSaveData.triggeredDialogues.Add(dialogueId);
        }
    }

    public void SetChapter(int chapter)
    {
        if (currentSaveData != null)
        {
            currentSaveData.currentChapter = chapter;
        }
    }

    public RoomState GetRoomState(string roomName)
    {
        if (currentSaveData == null) return new RoomState();

        if (!currentSaveData.roomStates.ContainsKey(roomName))
        {
            currentSaveData.roomStates[roomName] = new RoomState();
        }

        return currentSaveData.roomStates[roomName];
    }

    public void UpdateRoomState(string roomName, RoomState state)
    {
        if (currentSaveData != null)
        {
            currentSaveData.roomStates[roomName] = state;
        }
    }

    public void QuickSave()
    {
        SaveGame(1, "Quick Save");
    }

    public void QuickLoad()
    {
        LoadGame(1);
    }

    public void ClearObjectExamined(string objectId)
    {
        if (currentSaveData != null && currentSaveData.examinedObjects != null)
        {
            currentSaveData.examinedObjects.Remove(objectId);
            Debug.Log($"[SaveSystem] Cleared examined object: {objectId}");
        }
    }

    // Getters
    public GameSaveData GetCurrentSaveData()
    {
        return currentSaveData;
    }

    public bool HasItem(string itemId)
    {
        return currentSaveData != null && currentSaveData.inventoryItems.Contains(itemId);
    }

    public bool HasMemoryFragment(string fragmentId)
    {
        return currentSaveData != null && currentSaveData.collectedMemoryFragments.Contains(fragmentId);
    }

    public bool IsPuzzleSolved(string puzzleId)
    {
        return currentSaveData != null && currentSaveData.completedPuzzles.Contains(puzzleId);
    }

    public bool IsRoomCompleted(string roomName)
    {
        return currentSaveData != null && currentSaveData.completedRooms.Contains(roomName);
    }

    public bool WasObjectExamined(string objectId)
    {
        return currentSaveData != null && currentSaveData.examinedObjects.Contains(objectId);
    }

    public bool WasDialogueTriggered(string dialogueId)
    {
        return currentSaveData != null && currentSaveData.triggeredDialogues.Contains(dialogueId);
    }

    public int GetLullabySongProgress()
    {
        return currentSaveData?.lullabySongProgress ?? 0;
    }

    public float GetPlaytimeHours()
    {
        return currentSaveData != null ? currentSaveData.playtimeSeconds / 3600f : 0f;
    }

    public string GetPlaytimeFormatted()
    {
        if (currentSaveData == null) return "00:00:00";

        int hours = Mathf.FloorToInt(currentSaveData.playtimeSeconds / 3600f);
        int minutes = Mathf.FloorToInt((currentSaveData.playtimeSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(currentSaveData.playtimeSeconds % 60f);

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    public int GetMostRecentSaveSlot()
    {
        int mostRecentSlot = -1;
        System.DateTime mostRecentTime = System.DateTime.MinValue;

        for (int i = 0; i <= maxSaveSlots; i++)
        {
            if (HasSaveFile(i))
            {
                GameSaveData saveData = GetSaveInfo(i);

                if (saveData != null)
                {
                    System.DateTime saveTime;
                    if (System.DateTime.TryParse(saveData.saveDate, out saveTime))
                    {
                        if (saveTime > mostRecentTime)
                        {
                            mostRecentTime = saveTime;
                            mostRecentSlot = i;
                        }
                    }
                }
            }
        }

        return mostRecentSlot;
    }

    public bool HasAnySaveFile()
    {
        for (int i = 0; i <= maxSaveSlots; i++)
        {
            if (HasSaveFile(i))
                return true;
        }
        return false;
    }
}