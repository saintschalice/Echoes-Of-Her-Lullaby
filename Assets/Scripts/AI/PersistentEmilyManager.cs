using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

/// <summary>
/// Manages Emily's persistence across multiple scenes
/// UPDATED: Automatically places Emily on valid NavMesh positions
/// </summary>
public class PersistentEmilyManager : MonoBehaviour
{
    public static PersistentEmilyManager Instance { get; private set; }

    [Header("Emily Settings")]
    public GameObject emilyPrefab;
    public bool emilyIsActive = false;

    [Header("Scene Control")]
    public string firstEmilyScene = "Room03_Hallway";
    public string lastEmilyScene = "Room10_Final";

    [Header("Current Emily Reference")]
    public EmilyAIController currentEmily;

    [Header("Per-Scene Settings")]
    public EmilySceneConfig[] sceneConfigs;

    [Header("NavMesh Auto-Placement")]
    public bool autoPlaceOnNavMesh = true;
    public float navMeshSearchRadius = 10f;

    [Header("Auto-Spawn Control")]
    [Tooltip("When false, Emily will NOT be spawned automatically on scene load. Use PersistentEmilyManager.ActivateEmily() from triggers instead.")]
    //public bool allowAutoSpawn = true;
    public bool allowAutoSpawn = false;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PersistentEmilyManager] Created and set to persist");
        }
        else
        {
            Destroy(gameObject);
            return;
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PersistentEmilyManager] Scene loaded: {scene.name}");

        // Retrieve scene config, but do nothing unless player manually trigger Emily.
        var config = GetSceneConfig(scene.name);
        RemoveEmily();
    }


    bool ShouldEmilyBeActive(string sceneName)
    {
        if (sceneName == firstEmilyScene)
        {
            return true;
        }

        if (sceneName.Contains("Room03") ||
            sceneName.Contains("Room04") ||
            sceneName.Contains("Room05") ||
            sceneName.Contains("Room06") ||
            sceneName.Contains("Room07") ||
            sceneName.Contains("Room08") ||
            sceneName.Contains("Room09") ||
            sceneName.Contains("Room10"))
        {
            return true;
        }

        return false;
    }

    public void SpawnEmily(string sceneName)
    {
        if (currentEmily != null)
        {
            Debug.LogWarning("[PersistentEmilyManager] Emily already exists!");
            return;
        }

        if (emilyPrefab == null)
        {
            Debug.LogError("[PersistentEmilyManager] Emily prefab not assigned!");
            return;
        }

        GameObject emilyObj = Instantiate(emilyPrefab);
        emilyObj.name = "Emily (Persistent)";

        UnityEngine.SceneManagement.Scene targetScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (targetScene.isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(emilyObj, targetScene);
            Debug.Log($"[PersistentEmilyManager] ✓ Moved Emily to {sceneName} scene");
        }

        currentEmily = emilyObj.GetComponent<EmilyAIController>();

        if (currentEmily == null)
        {
            Debug.LogError("[PersistentEmilyManager] Emily prefab missing EmilyAIController!");
            Destroy(emilyObj);
            return;
        }

        ConfigureEmilyForScene(sceneName);

        emilyIsActive = true;
        //currentEmily.ActivateEmily();

        Debug.Log($"[PersistentEmilyManager] Emily spawned in {sceneName}  (idle/inactive)");
    }

    void ConfigureEmilyForScene(string sceneName)
    {
        if (currentEmily == null) return;

        EmilySceneConfig config = GetSceneConfig(sceneName);

        if (config != null)
        {
            // Set spawn position
            currentEmily.transform.position = config.spawnPosition;

            // AUTO-PLACE ON NAVMESH
            if (autoPlaceOnNavMesh)
            {
                StartCoroutine(PlaceEmilyOnNavMeshAfterDelay());
            }

            // Set patrol area
            if (currentEmily.movement != null)
            {
                currentEmily.movement.patrolAreaMin = config.patrolAreaMin;
                currentEmily.movement.patrolAreaMax = config.patrolAreaMax;
            }

            // Set initial state
            currentEmily.ForceState(config.initialState);

            Debug.Log($"[PersistentEmilyManager] Configured Emily for {sceneName}");
        }
        else
        {
            // Default configuration - find center of NavMesh
            if (autoPlaceOnNavMesh)
            {
                StartCoroutine(PlaceEmilyOnNavMeshAfterDelay());
            }

            Debug.LogWarning($"[PersistentEmilyManager] No config for {sceneName}, using defaults");
        }

        if (!currentEmily.gameObject.activeSelf)
        {
            currentEmily.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Wait a frame for NavMesh to be ready, then place Emily on it
    /// </summary>
    System.Collections.IEnumerator PlaceEmilyOnNavMeshAfterDelay()
    {
        // Wait for NavMesh to be fully loaded
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);

        if (currentEmily == null) yield break;

        Vector3 emilyPos = currentEmily.transform.position;
        NavMeshHit hit;

        // Try to find nearest valid NavMesh position
        if (NavMesh.SamplePosition(emilyPos, out hit, navMeshSearchRadius, NavMesh.AllAreas))
        {
            currentEmily.transform.position = hit.position;
            Debug.Log($"[PersistentEmilyManager] ✓ Placed Emily on NavMesh at {hit.position}");
        }
        else
        {
            Debug.LogError($"[PersistentEmilyManager] ✗ Could not find valid NavMesh position near {emilyPos}!");
            Debug.LogError($"[PersistentEmilyManager] Emily will not be able to move!");

            // Try to find ANY NavMesh position in the scene
            if (TryFindAnyNavMeshPosition(out Vector3 anyPosition))
            {
                currentEmily.transform.position = anyPosition;
                Debug.Log($"[PersistentEmilyManager] ✓ Placed Emily at fallback position: {anyPosition}");
            }
        }
    }

    /// <summary>
    /// Try to find ANY valid NavMesh position in the scene (fallback)
    /// </summary>
    bool TryFindAnyNavMeshPosition(out Vector3 position)
    {
        // Try common positions
        Vector3[] testPositions = new Vector3[]
        {
            Vector3.zero,
            new Vector3(0, 0, 0),
            new Vector3(5, 0, 0),
            new Vector3(-5, 0, 0),
            new Vector3(0, 5, 0),
            new Vector3(0, -5, 0)
        };

        foreach (Vector3 testPos in testPositions)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(testPos, out hit, 50f, NavMesh.AllAreas))
            {
                position = hit.position;
                return true;
            }
        }

        position = Vector3.zero;
        return false;
    }

    EmilySceneConfig GetSceneConfig(string sceneName)
    {
        if (sceneConfigs == null) return null;

        foreach (var config in sceneConfigs)
        {
            if (config.sceneName == sceneName)
            {
                return config;
            }
        }

        return null;
    }

    public void OnEmilyCatchPlayer()
    {
        Debug.Log("[PersistentEmilyManager] Emily caught player - triggering game over");

        if (currentEmily != null)
        {
            currentEmily.audioController?.PlayCatchSound();
        }

        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }

    public void ActivateEmily()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // If Emily does not exist yet, spawn her normally
        if (!emilyIsActive)
        {
            SpawnEmily(sceneName);
            return;
        }

        // Emily exists → use scene config spawn position
        var config = GetSceneConfig(sceneName);
        if (config != null)
        {
            currentEmily.transform.position = config.spawnPosition;
        }

        // Make sure she is placed on NavMesh
        Vector3 navPos;
        if (NavMeshHelper.GetNearestNavMeshPosition(currentEmily.transform.position, out navPos))
        {
            currentEmily.transform.position = navPos;
        }

        // Reactivate Emily and restart her systems
        currentEmily.ActivateEmily();
    }



    public void DeactivateEmily()
    {
        if (currentEmily != null)
        {
            currentEmily.DeactivateEmily();
        }
    }

    public void RemoveEmily()
    {
        if (currentEmily != null)
        {
            Destroy(currentEmily.gameObject);
            currentEmily = null;
        }   
        emilyIsActive = false;
        Debug.Log("[PersistentEmilyManager] Emily removed");
    }
}

[System.Serializable]
public class EmilySceneConfig
{
    public string sceneName;
    public Vector3 spawnPosition;
    public Vector2 patrolAreaMin;
    public Vector2 patrolAreaMax;
    public bool autoSpawnOnLoad;

    public EmilyState initialState = EmilyState.PATROL;
}