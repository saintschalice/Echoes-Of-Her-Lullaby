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
    public int lullabySongProgress; // How many fragments collected

    // Inventory
    public List<string> inventoryItems = new List<string>();
    public List<string> examinedObjects = new List<string>();

    // Room States
    public Dictionary<string, RoomState> roomStates = new Dictionary<string, RoomState>();

    // Game Settings
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public bool subtitlesEnabled = true;

    // Metadata
    public string saveDate;
    public string saveName;
    public int saveSlot;

    public GameSaveData()
    {
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        currentScene = "Room01_Foyer";
        playerPosition = Vector3.zero;
        currentChapter = 1;
        playtimeSeconds = 0f;
        lullabySongProgress = 0;
        saveSlot = 1;
        saveName = "Save Game";
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
    public float autoSaveInterval = 120f; // 2 minutes

    [Header("References")]
    public Transform player;

    private GameSaveData currentSaveData;
    private float autoSaveTimer;
    private float sessionStartTime;

    public static SaveSystem Instance { get; private set; }

    // Events
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
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sessionStartTime = Time.time;

        // Try to load existing save or create new one
        if (HasSaveFile(1))
        {
            LoadGame(1);
        }
        else
        {
            CreateNewGame(); // ADD THIS LINE - creates currentSaveData
        }

        // Add this debug line
        Debug.Log($"SaveSystem initialized - currentSaveData: {(currentSaveData != null ? "CHECK" : "NULL")}");
    }

    void Update()
    {
        UpdatePlaytime();
        HandleAutoSave();

        // Debug save/load keys
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
        currentSaveData = new GameSaveData();
        currentSaveData.saveName = "New Game";

        Debug.Log("Created new game save data");
    }

    public void SaveGame(int slot, string saveName = "")
    {
        if (currentSaveData == null)
        {
            Debug.LogWarning("No save data to save!");
            return;
        }

        // Auto-generate save name based on current room
        string roomDisplayName = SaveUIManager.GetRoomDisplayName(currentSaveData.currentScene);
        string autoSaveName = roomDisplayName;

        // Update save metadata
        currentSaveData.saveSlot = slot;
        currentSaveData.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        currentSaveData.saveName = autoSaveName;

        // Update current player position and scene
        UpdatePlayerData();

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
        SaveGame(0); // Use slot 0 for AutoSave
        Debug.Log("AutoSave completed");
    }

    // Call this when player enters a new room to autosave progress
    public void OnRoomEntered(string roomName)
    {
        if (currentSaveData != null)
        {
            currentSaveData.currentScene = roomName;
            AutoSave(); // Automatically save when entering new rooms
        }
    }

    // Call this when player completes significant story beats
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

            // Apply loaded data to game
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

        // Update player position
        if (player != null)
        {
            currentSaveData.playerPosition = player.position;
        }

        // Update current scene
        currentSaveData.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    void ApplyLoadedData()
    {
        if (currentSaveData == null) return;

        // Load scene if different
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene != currentSaveData.currentScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSaveData.currentScene);
        }

        // Apply player position (after scene loads)
        StartCoroutine(ApplyPlayerPositionDelayed());

        // Apply audio settings
        ApplyAudioSettings();
    }

    System.Collections.IEnumerator ApplyPlayerPositionDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (player != null && currentSaveData != null)
        {
            player.position = currentSaveData.playerPosition;
        }
    }

    void ApplyAudioSettings()
    {
        if (currentSaveData == null) return;

        // Apply audio settings (you'll need to connect these to your audio manager)
        AudioListener.volume = currentSaveData.masterVolume;
    }

    // Public methods for game progression
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
        }
    }

    public void RemoveInventoryItem(string itemId)
    {
        if (currentSaveData != null)
        {
            currentSaveData.inventoryItems.Remove(itemId);
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

    // Room state management
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

    // Quick save/load
    public void QuickSave()
    {
        SaveGame(1, "Quick Save");
    }

    public void QuickLoad()
    {
        LoadGame(1);
    }

    // Getters for current save data
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
}