using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;   // Keep this one; remove UnityEditor.AI if it appears

/// <summary>
/// Collects 2D colliders and builds a 2D NavMesh at runtime.
/// Works with Unity 6.2 + AI Navigation package.
/// </summary>
[DefaultExecutionOrder(-102)]
[RequireComponent(typeof(NavMeshSurface))]
public class NavMesh2DCollector : MonoBehaviour
{
    [Header("Collection Settings")]
    [SerializeField] private LayerMask collectionLayerMask = -1;
    [SerializeField] private int navMeshArea = 0;

    [Header("Baking Area")]
    [SerializeField] private Vector3 bakeSize = new Vector3(100f, 100f, 10f);
    [SerializeField] private Vector3 bakeCenter = Vector3.zero;
    [SerializeField] private bool autoCalculateBounds = true;
    [Tooltip("Extra padding around auto-calculated bounds")]
    [SerializeField] private float boundsPadding = 2f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private NavMeshSurface navMeshSurface;
    private NavMeshDataInstance navMeshDataInstance;
    public bool IsBakeComplete { get; private set; } = false;
    private static readonly Matrix4x4 XY_TO_XZ = Matrix4x4.Rotate(Quaternion.Euler(90f, 0f, 0f));


    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
            Debug.LogError("[NavMesh2D] No NavMeshSurface component found!");
    }

    /// <summary>
    /// Call this to bake the NavMesh from 2D colliders
    /// </summary>
    public void Bake2DNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("[NavMesh2D] No NavMeshSurface found!");
            return;
        }

        if (showDebugLogs)
            Debug.Log("[NavMesh2D] Starting bake process...");

        // Step 1: Collect all 2D colliders
        List<NavMeshBuildSource> sources = CollectSources();

        if (sources.Count == 0)
        {
            Debug.LogWarning("[NavMesh2D] No colliders found to bake! Check your layer mask and colliders.");
            return;
        }

        // Step 2: Calculate bounds
        Bounds bounds = CalculateBounds(sources);

        // Step 3: Get build settings from the NavMeshSurface
        NavMeshBuildSettings buildSettings = navMeshSurface.GetBuildSettings();

        // Step 4: Build the NavMesh
        NavMeshData navData = UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(
            buildSettings,
            sources,
            bounds,
            transform.position,
            transform.rotation
        );

        if (navData == null)
        {
            Debug.LogError("[NavMesh2D] Failed to build NavMesh data!");
            return;
        }

        // Step 5: Register the NavMesh globally (this was missing before)
        if (navMeshDataInstance.valid)
            NavMesh.RemoveNavMeshData(navMeshDataInstance);

        navMeshDataInstance = NavMesh.AddNavMeshData(navData);
        navMeshSurface.navMeshData = navData;

        if (showDebugLogs)
        {
            Debug.Log("[NavMesh2D] NavMesh data added to Unity navigation system.");
            Debug.Log($"[NavMesh2D] Bake successful! Created NavMesh with {sources.Count} sources.");
        }
    }

    /// <summary>
    /// Collect all 2D colliders and convert them to NavMesh build sources
    /// </summary>
    private List<NavMeshBuildSource> CollectSources()
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

        // Use modern API
        Collider2D[] colliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);

        if (showDebugLogs)
            Debug.Log($"[NavMesh2D] Found {colliders.Length} Collider2D objects in scene");

        int collected = 0;

        foreach (Collider2D col in colliders)
        {
            if (!col.gameObject.activeInHierarchy || !col.enabled)
                continue;

            if (((1 << col.gameObject.layer) & collectionLayerMask) == 0)
                continue;

            if (col.isTrigger)
                continue;

            Mesh mesh = col.CreateMesh(false, false);
            if (mesh == null || mesh.vertexCount == 0)
            {
                if (showDebugLogs)
                    Debug.LogWarning($"[NavMesh2D] Could not create mesh for: {col.gameObject.name}");
                continue;
            }

            NavMeshBuildSource source = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Mesh,
                sourceObject = mesh,
                transform = col.transform.localToWorldMatrix,
                area = navMeshArea
            };

            sources.Add(source);
            collected++;

            if (showDebugLogs)
                Debug.Log($"[NavMesh2D] ✓ Collected: {col.gameObject.name} " +
                          $"(Layer: {LayerMask.LayerToName(col.gameObject.layer)}, " +
                          $"Type: {col.GetType().Name}, Vertices: {mesh.vertexCount})");
        }

        if (showDebugLogs)
            Debug.Log($"[NavMesh2D] Collection complete: {collected} sources collected");

        return sources;
    }

    /// <summary>
    /// Calculate bounds that encompass all collected sources
    /// </summary>
    private Bounds CalculateBounds(List<NavMeshBuildSource> sources)
    {
        if (!autoCalculateBounds)
        {
            Bounds manualBounds = new Bounds(transform.position + bakeCenter, bakeSize);

            if (showDebugLogs)
                Debug.Log($"[NavMesh2D] Using manual bounds: Center={manualBounds.center}, Size={manualBounds.size}");

            return manualBounds;
        }

        if (sources.Count == 0)
            return new Bounds(transform.position, bakeSize);

        Bounds bounds = new Bounds();
        bool initialized = false;

        foreach (NavMeshBuildSource source in sources)
        {
            if (source.sourceObject is Mesh mesh)
            {
                Bounds meshBounds = mesh.bounds;
                Vector3 center = source.transform.MultiplyPoint3x4(meshBounds.center);
                Vector3 size = source.transform.MultiplyVector(meshBounds.size);
                size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));

                Bounds worldBounds = new Bounds(center, size);

                if (!initialized)
                {
                    bounds = worldBounds;
                    initialized = true;
                }
                else
                {
                    bounds.Encapsulate(worldBounds);
                }
            }
        }

        bounds.Expand(boundsPadding);

        if (showDebugLogs)
            Debug.Log($"[NavMesh2D] Auto-calculated bounds: Center={bounds.center}, Size={bounds.size}");

        return bounds;
    }

    public IEnumerator BakeAndWait()
    {
        IsBakeComplete = false;
        Debug.Log("[NavMesh2DCollector] BakeAndWait() called by trigger.");

        yield return new WaitForEndOfFrame();

        Bake2DNavMesh();

        // Wait until NavMesh triangulation returns data
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        int safety = 0;

        while (triangulation.vertices.Length == 0 && safety < 100)
        {
            yield return new WaitForEndOfFrame();
            triangulation = NavMesh.CalculateTriangulation();
            safety++;
        }

        IsBakeComplete = true;
        Debug.Log($"[NavMesh2DCollector] BakeAndWait() complete! NavMesh has {triangulation.vertices.Length} vertices.");
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(bakeCenter, bakeSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}
