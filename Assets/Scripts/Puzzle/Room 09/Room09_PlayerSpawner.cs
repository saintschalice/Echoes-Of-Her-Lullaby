using UnityEngine;

/// <summary>
/// Spawns player at specific position when Room 09 loads
/// Simple spawn system - just moves player to spawn point position
/// </summary>
public class Room09_PlayerSpawner : MonoBehaviour
{
    [Header("Spawn Point")]
    [Tooltip("GameObject na position kung saan dapat lumabas si Lisa")]
    public Transform spawnPoint;
    
    [Header("Settings")]
    public bool spawnOnStart = true;
    public bool faceRight = true; // Kung saan nakaharap si Lisa

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        // Find player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        
        if (player == null)
        {
            Debug.LogError("[Room09_PlayerSpawner] Player not found!");
            return;
        }
        
        // Get spawn position
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        
        // Move player to spawn position
        player.transform.position = spawnPos;
        
        // Set facing direction
        if (faceRight)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
        
        Debug.Log($"[Room09_PlayerSpawner] Player spawned at {spawnPos}");
    }

    // Visualization in Editor
    void OnDrawGizmos()
    {
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        
        // Draw spawn point
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pos, 0.5f);
        
        // Draw facing direction
        Gizmos.color = Color.blue;
        Vector3 direction = faceRight ? Vector3.right : Vector3.left;
        Gizmos.DrawRay(pos, direction * 1f);
    }
}
