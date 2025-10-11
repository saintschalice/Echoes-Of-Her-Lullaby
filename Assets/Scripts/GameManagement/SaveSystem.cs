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
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        currentScene = "Room01_Foyer";
        playerPosition = Vector3.zero;
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
            if (HasSaveFile(1))
                LoadGame(1);
            else
                CreateNewGame();
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

        if (HasSaveFile(1))
        {
            LoadGame(1);
        }
        else
        {
            CreateNewGame();
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

        string roomDisplayName = SaveUIManager.GetRoomDisplayName(currentSaveData.currentScene);
        string autoSaveName = roomDisplayName;

        currentSaveData.saveSlot = slot;
        currentSaveData.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        currentSaveData.saveName = autoSaveName;

        UpdatePlayerData();
        UpdateSettingsData();

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

        currentSaveData.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
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
}