using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Add this to PersistentEmilyManager to automatically place Emily on NavMesh
/// </summary>
public static class NavMeshHelper
{
    /// <summary>
    /// Find the nearest valid NavMesh position to the target position
    /// </summary>
    public static bool GetNearestNavMeshPosition(Vector3 targetPosition, out Vector3 result, float searchRadius = 10f)
    {
        NavMeshHit hit;

        // Try to find a valid NavMesh position near the target
        if (NavMesh.SamplePosition(targetPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            result = hit.position;
            Debug.Log($"[NavMeshHelper] Found valid position at {result}");
            return true;
        }

        Debug.LogWarning($"[NavMeshHelper] No valid NavMesh found near {targetPosition}");
        result = targetPosition;
        return false;
    }

    /// <summary>
    /// Ensure a GameObject is placed on a valid NavMesh position
    /// </summary>
    public static void PlaceOnNavMesh(GameObject obj, float searchRadius = 10f)
    {
        Vector3 validPosition;
        if (GetNearestNavMeshPosition(obj.transform.position, out validPosition, searchRadius))
        {
            obj.transform.position = validPosition;
            Debug.Log($"[NavMeshHelper] Placed {obj.name} on NavMesh at {validPosition}");
        }
        else
        {
            Debug.LogError($"[NavMeshHelper] Could not find valid NavMesh position for {obj.name}!");
        }
    }
}