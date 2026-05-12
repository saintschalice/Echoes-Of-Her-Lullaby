using UnityEngine;
using System.Collections;

/// <summary>
/// Trigger for Final Chase in Room 05 - Dining Room
/// Configurable Emily stats for easy balancing
/// </summary>
public class TriggerFinalChase : MonoBehaviour
{
    [Header("Emily Configuration")]
    [Tooltip("Emily's movement speed during final chase")]
    [Range(1f, 10f)]
    public float emilyChaseSpeed = 5.5f;
    
    [Tooltip("Distance at which Emily catches player (Game Over)")]
    [Range(0.5f, 3f)]
    public float catchDistance = 1.0f;
    
    [Header("Knockback Settings")]
    [Tooltip("Force of knockback applied to player")]
    [Range(0f, 20f)]
    public float knockbackForce = 10f;
    
    [Tooltip("Direction of knockback (normalized automatically)")]
    public Vector2 knockbackDirection = new Vector2(-1f, 0.5f);
    
    [Header("Timing Settings")]
    [Tooltip("Delay before Emily starts chasing (seconds)")]
    [Range(0f, 2f)]
    public float chaseStartDelay = 0.2f;
    
    [Header("Audio Settings")]
    [Tooltip("Play jumpscare sound when triggered")]
    public bool playJumpscareSound = true;
    
    [Tooltip("Jumpscare sound effect (optional)")]
    public AudioClip jumpscareClip;
    
    [Header("Dialogue Settings")]
    [Tooltip("Show dialogue when chase starts")]
    public bool showDialogue = true;
    
    [Tooltip("Dialogue to show (leave empty for default)")]
    [TextArea(1, 3)]
    public string chaseDialogue = "";
    
    [Header("Debug")]
    [Tooltip("Show debug messages in console")]
    public bool debugMode = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (hasTriggered) return;
        
        if (col.CompareTag("Player") && Room05_DiningRoomController.Instance != null)
        {
            bool isPuzzleDone = Room05_DiningRoomController.Instance.puzzleCompleted;
            bool isEmilyGone = !Room05_DiningRoomController.Instance.isEmilyHunting;

            // Kapag tapos na ang puzzle, saka lang talaga aatake si Emily
            if (isPuzzleDone && isEmilyGone)
            {
                if (debugMode) Debug.Log("[FinalChase] Starting Final Chase sequence with knockback.");
                hasTriggered = true;
                
                // Apply knockback to player
                Rigidbody2D playerRb = col.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = knockbackDirection.normalized * knockbackForce;
                    if (debugMode) Debug.Log($"[FinalChase] Knockback applied: {knockbackDirection.normalized * knockbackForce}");
                }
                
                // Apply configured stats to Room Controller
                ApplyEmilyStats();
                
                // Start final chase (fast and intense)
                Room05_DiningRoomController.Instance.OnTriggerExitRoom();
                
                gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Apply configured Emily stats to Room Controller
    /// </summary>
    private void ApplyEmilyStats()
    {
        if (Room05_DiningRoomController.Instance == null) return;
        
        // Override Emily speed
        Room05_DiningRoomController.Instance.finalChaseSpeed = emilyChaseSpeed;
        
        if (debugMode) Debug.Log($"[FinalChase] Emily stats applied - Speed: {emilyChaseSpeed}, Catch Distance: {catchDistance}");
    }
    
    // Visualize trigger area and knockback direction in editor
    private void OnDrawGizmos()
    {
        // Draw trigger area
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange
            Gizmos.DrawCube(transform.position, col.bounds.size);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw knockback direction
        Gizmos.color = Color.yellow;
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(knockbackDirection.normalized * 2f);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.2f);
        
        // Draw catch distance preview
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
}