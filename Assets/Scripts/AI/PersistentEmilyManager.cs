using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// ZERO LAG VERSION - Uses object pooling instead of Instantiate/Destroy
/// Emily is created once and reused, never destroyed
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
    public float navMeshSearchRadius = 5f;

    [Header("Auto-Spawn Control")]
    public bool allowAutoSpawn = false;

    // Cached components for performance
    private NavMeshAgent emilyAgent;
    private bool emilyPreInstantiated = false;

    // ============================================================
    // INITIALIZATION
    // ============================================================
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
                transform.SetParent(null, true);
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PersistentEmilyManager] Created and set to persist");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // PRE-INSTANTIATE Emily once at startup to avoid lag later
        if (emilyPrefab != null && !emilyPreInstantiated)
        {
            StartCoroutine(PreInstantiateEmily());
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
        // Don't destroy Emily, just deactivate her
        if (currentEmily != null)
        {
            currentEmily.DeactivateEmily();
        }
    }

    // ============================================================
    // PRE-INSTANTIATION (Runs once at game start)
    // ============================================================
    IEnumerator PreInstantiateEmily()
    {
        yield return new WaitForSeconds(0.5f); // Wait for game to fully load

        if (currentEmily != null) yield break; // Already exists

        Debug.Log("[PersistentEmilyManager] Pre-instantiating Emily...");

        if (!TryInstantiateEmily(out EmilyAIController emily))
        {
            Debug.LogError("[PersistentEmilyManager] Failed to pre-instantiate Emily!");
            yield break;
        }

        PromoteEmilyToPersistentRoot(emily);
        currentEmily = emily;
        emilyAgent = emily.GetComponent<NavMeshAgent>();
        emilyPreInstantiated = true;

        // Keep her deactivated until needed
        currentEmily.gameObject.SetActive(false);

        Debug.Log("[PersistentEmilyManager] ✓ Emily pre-instantiated and pooled");
    }

    // ============================================================
    // SPAWN (Now just activates pre-existing Emily)
    // ============================================================
    public void SpawnEmily(string sceneName)
    {
        // If Emily doesn't exist yet, create her immediately
        if (currentEmily == null || currentEmily.Equals(null))
        {
            Debug.Log("[PersistentEmilyManager] Emily not pre-instantiated, creating now...");

            if (!TryInstantiateEmily(out EmilyAIController emily))
                return;

            PromoteEmilyToPersistentRoot(emily);
            currentEmily = emily;
            emilyAgent = emily.GetComponent<NavMeshAgent>();
        }

        // Activate and configure (no instantiation = no lag!)
        emilyIsActive = true;

        if (!currentEmily.gameObject.activeSelf)
            currentEmily.gameObject.SetActive(true);

        ConfigureEmilyForScene(sceneName);
        currentEmily.ActivateEmily();

        Debug.Log($"[PersistentEmilyManager] Emily activated in {sceneName}");
    }

    // ============================================================
    // INSTANTIATION (Only runs once)
    // ============================================================
    private bool TryInstantiateEmily(out EmilyAIController emily)
    {
        emily = null;

        if (emilyPrefab == null)
        {
            Debug.LogError("[PersistentEmilyManager] Emily prefab missing!");
            return false;
        }

        GameObject obj = Instantiate(emilyPrefab);
        emily = obj.GetComponent<EmilyAIController>();

        if (emily == null)
        {
            Debug.LogError("[PersistentEmilyManager] Prefab missing EmilyAIController!");
            Destroy(obj);
            return false;
        }

        Debug.Log("[PersistentEmilyManager] Instantiated Emily");
        return true;
    }

    private void PromoteEmilyToPersistentRoot(EmilyAIController emily)
    {
        if (emily == null) return;

        if (emily.transform.parent != null)
            emily.transform.SetParent(null, true);

        if (emily.gameObject.scene.name != "DontDestroyOnLoad")
            DontDestroyOnLoad(emily.gameObject);
    }

    // ============================================================
    // CONFIGURATION
    // ============================================================
    void ConfigureEmilyForScene(string sceneName)
    {
        if (currentEmily == null) return;

        EmilySceneConfig config = GetSceneConfig(sceneName);

        if (config != null)
        {
            currentEmily.TeleportTo(config.spawnPosition);

            if (currentEmily.movement != null)
            {
                currentEmily.movement.patrolAreaMin = config.patrolAreaMin;
                currentEmily.movement.patrolAreaMax = config.patrolAreaMax;
            }

            currentEmily.ForceState(config.initialState);

            Debug.Log($"[PersistentEmilyManager] Configured for {sceneName} at {config.spawnPosition}");

            // Only snap to NavMesh if config allows it
            if (autoPlaceOnNavMesh && config.snapToNavMesh)
            {
                StartCoroutine(PlaceEmilyOnNavMeshAsync());
            }
        }
        else
        {
            if (autoPlaceOnNavMesh)
                StartCoroutine(PlaceEmilyOnNavMeshAsync());

            Debug.LogWarning($"[PersistentEmilyManager] No config for {sceneName}");
        }
    }

    // ============================================================
    // NAVMESH (OPTIMIZED)
    // ============================================================
    IEnumerator PlaceEmilyOnNavMeshAsync()
    {
        yield return new WaitForSeconds(0.2f);

        if (currentEmily == null) yield break;

        Vector3 pos = currentEmily.transform.position;
        bool placed = false;

        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
        {
            currentEmily.transform.position = hit.position;
            placed = true;
            Debug.Log($"[PersistentEmilyManager] ✓ Placed on NavMesh at {hit.position}");
        }
        else
        {
            // Fallback positions
            Vector3[] testPositions = {
                Vector3.zero,
                new Vector3(5, 0, 0),
                new Vector3(-5, 0, 0)
            };

            foreach (var testPos in testPositions)
            {
                if (NavMesh.SamplePosition(testPos, out hit, 20f, NavMesh.AllAreas))
                {
                    currentEmily.transform.position = hit.position;
                    placed = true;
                    Debug.Log($"[PersistentEmilyManager] ✓ Fallback placement at {hit.position}");
                    break;
                }
                yield return null;
            }
        }

        if (!placed)
        {
            Debug.LogError($"[PersistentEmilyManager] ✗ No NavMesh found near {pos}");
        }
    }

    // ============================================================
    // LOOKUP
    // ============================================================
    EmilySceneConfig GetSceneConfig(string sceneName)
    {
        if (sceneConfigs == null) return null;

        foreach (var config in sceneConfigs)
        {
            if (config.sceneName == sceneName)
                return config;
        }
        return null;
    }

    // ============================================================
    // GAMEPLAY EVENTS
    // ============================================================
    public void OnEmilyCatchPlayer()
    {
        Debug.Log("[PersistentEmilyManager] Emily caught player");
        currentEmily?.audioController?.PlayCatchSound();
        GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
    }

    // ============================================================
    // EXTERNAL CONTROL
    // ============================================================
    public void ActivateEmily()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SpawnEmily(sceneName);
    }

    public void DeactivateEmily()
    {
        if (currentEmily != null)
        {
            currentEmily.DeactivateEmily();
            // Don't destroy, just deactivate
            currentEmily.gameObject.SetActive(false);
        }
        emilyIsActive = false;
    }

    public void RemoveEmily()
    {
        // Only use this if you need to completely remove Emily
        // (e.g., game over, returning to menu)
        if (currentEmily != null)
            Destroy(currentEmily.gameObject);

        currentEmily = null;
        emilyAgent = null;
        emilyIsActive = false;
        emilyPreInstantiated = false;

        Debug.Log("[PersistentEmilyManager] Emily removed from memory");
    }
}

// ============================================================
// SCENE CONFIG
// ============================================================
[System.Serializable]
public class EmilySceneConfig
{
    public string sceneName;
    public Vector3 spawnPosition;
    public Vector2 patrolAreaMin;
    public Vector2 patrolAreaMax;
    public bool autoSpawnOnLoad;
    public EmilyState initialState = EmilyState.PATROL;

    [Tooltip("If false, Emily will spawn at EXACT position without NavMesh snapping")]
    public bool snapToNavMesh = false; // Default to false for precise positioning
}