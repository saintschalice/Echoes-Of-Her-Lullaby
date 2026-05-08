using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PersistentSpawnManager : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // Lisa from persistent scene
    
    [Header("New Game Settings")]
    [Tooltip("Hide player on first load (for intro cutscene). FoyerIntroController will show her after cutscene.")]
    public bool hidePlayerOnNewGame = true;

    [Header("Camera Reference")]
    public Camera playerCamera; // Main camera that follows Lisa
    public CameraFollow cameraFollowScript; // Reference to CameraFollow script

    [Header("Debug")]
    public bool debugMode = true;

    private Dictionary<string, RoomSpawnPoint> spawnPoints = new Dictionary<string, RoomSpawnPoint>();
    private string currentSceneName = "";
    private bool isFirstLoad = true;

    public static PersistentSpawnManager Instance { get; private set; }

    void Awake()
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("[PersistentSpawn] Found player: " + player.name);
            }
            else
            {
                Debug.LogError("[PersistentSpawn] Player not found! Make sure Lisa has 'Player' tag");
            }
        }

        // Find camera follow script if not assigned
        if (playerCamera != null && cameraFollowScript == null)
        {
            cameraFollowScript = playerCamera.GetComponent<CameraFollow>();
        }

        currentSceneName = SceneManager.GetActiveScene().name;
        
        // CRITICAL: Hide player on new game to prevent visibility before cutscene
        if (hidePlayerOnNewGame && player != null)
        {
            // Check if this is a new game (LoadSlotOnStart == -1 means new game)
            if (PlayerPrefs.HasKey("LoadSlotOnStart"))
            {
                int loadSlot = PlayerPrefs.GetInt("LoadSlotOnStart");
                if (loadSlot == -1)
                {
                    // This is a NEW GAME - hide Lisa until FoyerIntroController shows her
                    player.gameObject.SetActive(false);
                    Debug.Log("[PersistentSpawn] NEW GAME detected - Lisa hidden until cutscene ends");
                }
                else
                {
                    // This is a LOAD GAME - Lisa should be visible
                    player.gameObject.SetActive(true);
                    Debug.Log("[PersistentSpawn] LOAD GAME detected - Lisa visible immediately");
                }
            }
            else
            {
                // No flag set, assume normal gameplay (Lisa visible)
                player.gameObject.SetActive(true);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PersistentSpawn] Scene loaded: {scene.name}");

        // Clear old spawn points from previous scene
        spawnPoints.Clear();
        currentSceneName = scene.name;

        // Update camera boundaries for new scene
        UpdateCameraBoundaries();

        // Wait for spawn points to register
        StartCoroutine(PositionPlayerAfterLoad());
    }

    System.Collections.IEnumerator PositionPlayerAfterLoad()
    {
        // Wait for scene to fully load
        yield return new WaitForEndOfFrame();

        // Check if we're loading from a save
        string loadingFromSave = PlayerPrefs.GetString("LoadingFromSave", "");
        if (loadingFromSave == "true")
        {
            // Clear the flag
            PlayerPrefs.SetString("LoadingFromSave", "");
            PlayerPrefs.Save();

            // Use saved position from SaveSystem
            if (SaveSystem.Instance != null)
            {
                GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();
                if (saveData != null && player != null)
                {
                    player.position = saveData.playerPosition;
                    Debug.Log($"[PersistentSpawn] Loaded from save - Position: {saveData.playerPosition}");
                }
            }
        }
        else
        {
            // Normal door transition logic
            string targetSpawn = PlayerPrefs.GetString("TargetSpawnPoint", "");

            if (!string.IsNullOrEmpty(targetSpawn))
            {
                // Use specific spawn point
                SpawnPlayerAt(currentSceneName, targetSpawn);
                PlayerPrefs.SetString("TargetSpawnPoint", ""); // Clear it
            }
            else if (SaveSystem.Instance != null && !isFirstLoad)
            {
                // Use saved position (for loading saves in same scene)
                GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();
                if (saveData != null && saveData.currentScene == currentSceneName)
                {
                    if (player != null)
                    {
                        player.position = saveData.playerPosition;
                        Debug.Log($"[PersistentSpawn] Loaded saved position (same scene): {saveData.playerPosition}");
                    }
                }
                else
                {
                    // Scene mismatch, use default spawn
                    SpawnPlayerAtDefault();
                }
            }
            else
            {
                // First load or no save, use default spawn
                SpawnPlayerAtDefault();
            }
        }

        isFirstLoad = false;

        // SKIP FADE IN for Room01_Foyer (FoyerIntroController handles its own fade)
        if (currentSceneName == "Room01_Foyer")
        {
            Debug.Log("[PersistentSpawn] Skipping fade in for Room01_Foyer (cutscene handles it)");
            yield break;
        }

        // Fade in after positioning player (for all other rooms)
        if (ScreenFader.Instance != null && !ScreenFader.Instance.IsFading())
        {
            // Small delay before fading in
            yield return new WaitForSeconds(0.1f);
            ScreenFader.Instance.FadeIn(0.8f);
            Debug.Log("[PersistentSpawn] Triggered fade in after scene load");
        }
    }

    public void RegisterSpawnPoint(RoomSpawnPoint spawnPoint)
    {
        if (spawnPoint == null) return;

        string key = spawnPoint.roomName + "_" + spawnPoint.spawnPointID;

        if (!spawnPoints.ContainsKey(key))
        {
            spawnPoints.Add(key, spawnPoint);
            if (debugMode)
                Debug.Log($"[PersistentSpawn] Registered spawn point: {key}");
        }
    }

    public void SpawnPlayerAtDefault()
    {
        // Look for default spawn point in current scene
        foreach (var kvp in spawnPoints)
        {
            if (kvp.Value.roomName == currentSceneName && kvp.Value.isDefaultSpawnPoint)
            {
                PositionPlayer(kvp.Value);
                return;
            }
        }

        // Fallback: use any spawn point in current scene
        foreach (var kvp in spawnPoints)
        {
            if (kvp.Value.roomName == currentSceneName)
            {
                PositionPlayer(kvp.Value);
                return;
            }
        }

        Debug.LogWarning($"[PersistentSpawn] No spawn point found for scene: {currentSceneName}");
    }

    public void SpawnPlayerAt(string roomName, string spawnPointID)
    {
        string key = roomName + "_" + spawnPointID;

        if (spawnPoints.ContainsKey(key))
        {
            PositionPlayer(spawnPoints[key]);
        }
        else
        {
            Debug.LogWarning($"[PersistentSpawn] Spawn point not found: {key}, using default");
            SpawnPlayerAtDefault();
        }
    }

    void PositionPlayer(RoomSpawnPoint spawnPoint)
    {
        if (player == null || spawnPoint == null) return;

        player.position = spawnPoint.transform.position;

        // Optional: match rotation for 2D (only Z rotation matters)
        if (spawnPoint.matchRotation)
        {
            Vector3 currentRotation = player.eulerAngles;
            currentRotation.z = spawnPoint.transform.eulerAngles.z;
            player.eulerAngles = currentRotation;
        }

        Debug.Log($"[PersistentSpawn] Positioned player at: {spawnPoint.spawnPointID} in {spawnPoint.roomName}");
    }

    void UpdateCameraBoundaries()
    {
        if (cameraFollowScript != null)
        {
            // Refresh tilemap boundaries for the new scene
            cameraFollowScript.RefreshTilemapBoundaries();
            Debug.Log("[PersistentSpawn] Camera boundaries updated for new scene");
        }
    }

    // Manual spawn control
    public void TeleportPlayerTo(Vector3 position)
    {
        if (player != null)
        {
            player.position = position;
            Debug.Log($"[PersistentSpawn] ✅ Teleported player to: {position}");
        }
        else
        {
            Debug.LogError("[PersistentSpawn] ❌ Cannot teleport - player is null!");
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return player != null ? player.position : Vector3.zero;
    }
    
    /// <summary>
    /// Enable the player GameObject. Called by FoyerIntroController after cutscene ends.
    /// </summary>
    public void EnablePlayer()
    {
        if (player != null)
        {
            player.gameObject.SetActive(true);
            Debug.Log("[PersistentSpawn] Player enabled (called externally)");
        }
        else
        {
            Debug.LogError("[PersistentSpawn] Cannot enable player - player reference is null!");
        }
    }
    
    /// <summary>
    /// Disable the player GameObject. For special cases like cutscenes.
    /// </summary>
    public void DisablePlayer()
    {
        if (player != null)
        {
            player.gameObject.SetActive(false);
            Debug.Log("[PersistentSpawn] Player disabled (called externally)");
        }
    }

    // Debug helper
    void Update()
    {
        // Debug keys for testing
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log($"[DEBUG] Player position: {(player != null ? player.position.ToString() : "NULL")}");
                Debug.Log($"[DEBUG] Player reference: {(player != null ? player.name : "NULL")}");
                Debug.Log($"[DEBUG] Player active: {(player != null ? player.gameObject.activeSelf.ToString() : "NULL")}");
            }
        }
    }
}