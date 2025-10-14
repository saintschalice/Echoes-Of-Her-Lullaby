using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    [Header("Spawn Point Info")]
    [Tooltip("Name of this room (must match scene name)")]
    public string roomName = "Room01_Foyer";

    [Tooltip("Is this the default spawn point for this room?")]
    public bool isDefaultSpawnPoint = true;

    [Tooltip("Optional: Specific spawn ID for multiple spawn points in one room")]
    public string spawnPointID = "Main";

    [Header("Rotation Settings")]
    [Tooltip("Should player face same direction as spawn point?")]
    public bool matchRotation = false;

    [Header("Visual Gizmo")]
    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.5f;

    void Start()
    {
        // Auto-detect room name from scene if not set
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        // Register with PERSISTENT spawn manager (works with persistent player)
        if (PersistentSpawnManager.Instance != null)
        {
            PersistentSpawnManager.Instance.RegisterSpawnPoint(this);
        }
        else
        {
            Debug.LogWarning("[RoomSpawnPoint] PersistentSpawnManager not found!");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}