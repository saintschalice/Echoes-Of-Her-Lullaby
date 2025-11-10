using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Helper script to automatically set Emily's patrol bounds based on the actual NavMesh
/// Attach this to Emily and click "Auto-Set Patrol Bounds" button in Inspector
/// </summary>
[RequireComponent(typeof(EmilyMovement))]
public class EmilyPatrolBoundsSetter : MonoBehaviour
{
    [Header("Auto-Detection")]
    public bool autoSetOnStart = true;

    [Header("Manual Override")]
    public GameObject floorObject; // Drag your Floor GameObject here
    public float boundsPadding = 2f; // Padding from edges

    private EmilyMovement movement;

    private void Start()
    {
        movement = GetComponent<EmilyMovement>();

        if (autoSetOnStart)
        {
            SetPatrolBoundsAutomatically();
        }
    }

    [ContextMenu("Auto-Set Patrol Bounds")]
    public void SetPatrolBoundsAutomatically()
    {
        movement = GetComponent<EmilyMovement>();

        if (movement == null)
        {
            Debug.LogError("[PatrolBounds] EmilyMovement component not found!");
            return;
        }

        Debug.Log("========== SETTING PATROL BOUNDS ==========");

        // Method 1: Try to find bounds from NavMesh
        bool success = TrySetBoundsFromNavMesh();

        // Method 2: If NavMesh method failed, try to use Floor object
        if (!success && floorObject != null)
        {
            success = TrySetBoundsFromFloor();
        }

        // Method 3: If both failed, use scene bounds
        if (!success)
        {
            SetDefaultBounds();
        }

        LogCurrentBounds();
        Debug.Log("========== PATROL BOUNDS SET ==========");
    }

    bool TrySetBoundsFromNavMesh()
    {
        // Try to find NavMesh bounds by sampling
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        if (triangulation.vertices.Length == 0)
        {
            Debug.LogWarning("[PatrolBounds] No NavMesh found in scene!");
            return false;
        }

        // Calculate bounds from NavMesh vertices
        Vector3 min = triangulation.vertices[0];
        Vector3 max = triangulation.vertices[0];

        foreach (Vector3 vertex in triangulation.vertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        // Add padding
        min -= new Vector3(boundsPadding, boundsPadding, 0);
        max += new Vector3(boundsPadding, boundsPadding, 0);

        // Set the bounds
        movement.patrolAreaMin = new Vector2(min.x, min.y);
        movement.patrolAreaMax = new Vector2(max.x, max.y);

        Debug.Log($"[PatrolBounds] Set from NavMesh: Min({min.x:F2}, {min.y:F2}) Max({max.x:F2}, {max.y:F2})");
        return true;
    }

    bool TrySetBoundsFromFloor()
    {
        if (floorObject == null)
        {
            Debug.LogWarning("[PatrolBounds] Floor object not assigned!");
            return false;
        }

        // Try to get bounds from Renderer
        Renderer renderer = floorObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;

            movement.patrolAreaMin = new Vector2(
                bounds.min.x + boundsPadding,
                bounds.min.y + boundsPadding
            );
            movement.patrolAreaMax = new Vector2(
                bounds.max.x - boundsPadding,
                bounds.max.y - boundsPadding
            );

            Debug.Log($"[PatrolBounds] Set from Floor Renderer: Min({movement.patrolAreaMin}) Max({movement.patrolAreaMax})");
            return true;
        }

        // Try to get bounds from Collider
        Collider2D collider = floorObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            Bounds bounds = collider.bounds;

            movement.patrolAreaMin = new Vector2(
                bounds.min.x + boundsPadding,
                bounds.min.y + boundsPadding
            );
            movement.patrolAreaMax = new Vector2(
                bounds.max.x - boundsPadding,
                bounds.max.y - boundsPadding
            );

            Debug.Log($"[PatrolBounds] Set from Floor Collider: Min({movement.patrolAreaMin}) Max({movement.patrolAreaMax})");
            return true;
        }

        Debug.LogWarning("[PatrolBounds] Floor object has no Renderer or Collider!");
        return false;
    }

    void SetDefaultBounds()
    {
        // Set reasonable default bounds around Emily's current position
        Vector3 pos = transform.position;

        movement.patrolAreaMin = new Vector2(pos.x - 5, pos.y - 5);
        movement.patrolAreaMax = new Vector2(pos.x + 5, pos.y + 5);

        Debug.LogWarning($"[PatrolBounds] Using default bounds around Emily's position: Min({movement.patrolAreaMin}) Max({movement.patrolAreaMax})");
    }

    void LogCurrentBounds()
    {
        Debug.Log($"✓ Current Patrol Bounds:");
        Debug.Log($"  Min: X={movement.patrolAreaMin.x:F2}, Y={movement.patrolAreaMin.y:F2}");
        Debug.Log($"  Max: X={movement.patrolAreaMax.x:F2}, Y={movement.patrolAreaMax.y:F2}");
        Debug.Log($"  Area Size: {(movement.patrolAreaMax - movement.patrolAreaMin)}");
    }

    private void OnDrawGizmosSelected()
    {
        if (movement == null) return;

        // Draw patrol bounds
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            (movement.patrolAreaMin.x + movement.patrolAreaMax.x) / 2,
            (movement.patrolAreaMin.y + movement.patrolAreaMax.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            movement.patrolAreaMax.x - movement.patrolAreaMin.x,
            movement.patrolAreaMax.y - movement.patrolAreaMin.y,
            1
        );
        Gizmos.DrawWireCube(center, size);

        // Draw corners
        Gizmos.color = Color.yellow;
        Vector3 bottomLeft = new Vector3(movement.patrolAreaMin.x, movement.patrolAreaMin.y, 0);
        Vector3 bottomRight = new Vector3(movement.patrolAreaMax.x, movement.patrolAreaMin.y, 0);
        Vector3 topLeft = new Vector3(movement.patrolAreaMin.x, movement.patrolAreaMax.y, 0);
        Vector3 topRight = new Vector3(movement.patrolAreaMax.x, movement.patrolAreaMax.y, 0);

        Gizmos.DrawSphere(bottomLeft, 0.3f);
        Gizmos.DrawSphere(bottomRight, 0.3f);
        Gizmos.DrawSphere(topLeft, 0.3f);
        Gizmos.DrawSphere(topRight, 0.3f);
    }
}