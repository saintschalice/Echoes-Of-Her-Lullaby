using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// REFERENCE SCRIPT: Cinematic Chase Trigger with Configurable Emily Settings
/// 
/// This script triggers a cinematic chase sequence when the player enters a trigger zone.
/// Features:
/// - Configurable Emily spawn position
/// - Adjustable Emily speed and aggression
/// - Knockback effect on trigger
/// - Game Over on contact with Emily
/// - Dialogue support
/// - Audio support
/// 
/// SETUP:
/// 1. Create empty GameObject with 2D Collider (Is Trigger = true)
/// 2. Add this script
/// 3. Configure all settings in Inspector
/// 4. Assign Emily GameObject reference
/// 5. Position spawn point where Emily should appear
/// </summary>
public class CinematicChaseTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Only trigger once per game session")]
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    [Header("Emily Configuration")]
    [Tooltip("Emily GameObject (must have NavMeshAgent and EmilyGhost components)")]
    public GameObject emilyGameObject;

    [Tooltip("Where Emily spawns when chase starts")]
    public Transform emilySpawnPoint;

    [Tooltip("Emily's movement speed during this chase (default: 5.5)")]
    [Range(1f, 10f)]
    public float emilyChaseSpeed = 5.5f;

    [Tooltip("Distance at which Emily catches player and triggers Game Over (default: 1.0)")]
    [Range(0.5f, 3f)]
    public float catchDistance = 1.0f;

    [Header("Knockback Settings")]
    [Tooltip("Enable knockback when player touches trigger")]
    public bool enableKnockback = true;

    [Tooltip("Force of knockback applied to player")]
    [Range(0f, 20f)]
    public float knockbackForce = 10f;

    [Tooltip("Direction of knockback (normalized automatically)")]
    public Vector2 knockbackDirection = new Vector2(-1f, 0.5f);

    [Header("Dialogue Settings")]
    [Tooltip("Show dialogue when chase starts")]
    public bool showDialogue = true;

    [Tooltip("Dialogue text to show (leave empty for no dialogue)")]
    [TextArea(2, 4)]
    public string chaseDialogue = "She's coming!";

    [Tooltip("Speaker name for dialogue")]
    public string speakerName = "Lisa";

    [Header("Audio Settings")]
    [Tooltip("Play sound effect when chase starts")]
    public bool playSoundEffect = true;

    [Tooltip("Sound effect to play (jumpscare/scream)")]
    public AudioClip chaseSoundEffect;

    [Tooltip("Audio source for playing sounds (optional, uses AudioManager if null)")]
    public AudioSource audioSource;

    [Tooltip("Looping chase music (footsteps/tension)")]
    public AudioClip chaseLoopMusic;

    [Header("Timing Settings")]
    [Tooltip("Delay before Emily starts chasing (seconds)")]
    [Range(0f, 2f)]
    public float chaseStartDelay = 0.2f;

    [Header("Game Over Settings")]
    [Tooltip("Enable Game Over when Emily catches player")]
    public bool enableGameOver = true;

    [Tooltip("Game Over message")]
    [TextArea(1, 2)]
    public string gameOverMessage = "Emily caught you...";

    [Header("Debug")]
    [Tooltip("Show debug messages in console")]
    public bool debugMode = true;

    private NavMeshAgent emilyAgent;
    private EmilyGhost emilyGhostScript;
    private Transform playerTransform;
    private bool isChasing = false;

    private void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("[CinematicChase] Player not found! Make sure player has 'Player' tag.");
        }

        // Get Emily components
        if (emilyGameObject != null)
        {
            emilyAgent = emilyGameObject.GetComponent<NavMeshAgent>();
            emilyGhostScript = emilyGameObject.GetComponent<EmilyGhost>();

            if (emilyAgent == null)
            {
                Debug.LogError("[CinematicChase] Emily GameObject missing NavMeshAgent component!");
            }

            if (emilyGhostScript == null)
            {
                Debug.LogWarning("[CinematicChase] Emily GameObject missing EmilyGhost component. Game Over on contact won't work.");
            }
        }
        else
        {
            Debug.LogError("[CinematicChase] Emily GameObject not assigned!");
        }
    }

    private void Update()
    {
        // Check for catch during chase
        if (isChasing && enableGameOver && playerTransform != null && emilyGameObject != null)
        {
            float distance = Vector2.Distance(emilyGameObject.transform.position, playerTransform.position);

            if (distance <= catchDistance)
            {
                TriggerGameOver();
            }
        }

        // Update Emily's target during chase
        if (isChasing && emilyAgent != null && playerTransform != null)
        {
            if (emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.SetDestination(playerTransform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player entered trigger
        if (collision.CompareTag("Player"))
        {
            // Check if already triggered
            if (triggerOnce && hasTriggered)
            {
                if (debugMode) Debug.Log("[CinematicChase] Already triggered, ignoring.");
                return;
            }

            if (debugMode) Debug.Log("[CinematicChase] Player entered trigger! Starting chase sequence.");

            hasTriggered = true;
            StartCoroutine(StartChaseSequence(collision));
        }
    }

    private IEnumerator StartChaseSequence(Collider2D playerCollider)
    {
        // Apply knockback
        if (enableKnockback)
        {
            Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockback = knockbackDirection.normalized * knockbackForce;
                playerRb.linearVelocity = knockback;

                if (debugMode) Debug.Log($"[CinematicChase] Knockback applied: {knockback}");
            }
        }

        // Play sound effect
        if (playSoundEffect && chaseSoundEffect != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(chaseSoundEffect);
            }
            else
            {
                AudioManager.Instance?.PlaySFX(chaseSoundEffect);
            }

            if (debugMode) Debug.Log("[CinematicChase] Chase sound effect played.");
        }

        // Show dialogue
        if (showDialogue && !string.IsNullOrEmpty(chaseDialogue))
        {
            DialogueSystemV2.Instance?.StartDialogue(chaseDialogue, speakerName);

            if (debugMode) Debug.Log($"[CinematicChase] Dialogue shown: {chaseDialogue}");
        }

        // Wait for delay
        yield return new WaitForSeconds(chaseStartDelay);

        // Spawn and configure Emily
        if (emilyGameObject != null)
        {
            // Position Emily at spawn point
            if (emilySpawnPoint != null)
            {
                emilyGameObject.transform.position = emilySpawnPoint.position;

                if (emilyAgent != null && emilyAgent.isActiveAndEnabled)
                {
                    emilyAgent.Warp(emilySpawnPoint.position);
                }

                if (debugMode) Debug.Log($"[CinematicChase] Emily spawned at: {emilySpawnPoint.position}");
            }

            // Activate Emily
            emilyGameObject.SetActive(true);

            // Configure NavMeshAgent
            if (emilyAgent != null)
            {
                emilyAgent.enabled = true;
                emilyAgent.speed = emilyChaseSpeed;

                if (debugMode) Debug.Log($"[CinematicChase] Emily speed set to: {emilyChaseSpeed}");
            }

            // Start chase music
            if (chaseLoopMusic != null)
            {
                if (audioSource != null)
                {
                    audioSource.clip = chaseLoopMusic;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                else
                {
                    AudioManager.Instance?.PlayLoopingSFX(chaseLoopMusic, "cinematic_chase");
                }

                if (debugMode) Debug.Log("[CinematicChase] Chase music started.");
            }

            isChasing = true;
        }

        // Disable trigger to prevent re-triggering
        if (triggerOnce)
        {
            gameObject.SetActive(false);
        }
    }

    private void TriggerGameOver()
    {
        if (!isChasing) return; // Already triggered

        isChasing = false;

        if (debugMode) Debug.Log("[CinematicChase] Emily caught player! Triggering Game Over.");

        // Stop Emily movement
        if (emilyAgent != null && emilyAgent.isActiveAndEnabled)
        {
            emilyAgent.isStopped = true;
            emilyAgent.velocity = Vector3.zero;
        }

        // Stop chase music
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Trigger jumpscare + game over
        if (JumpscareManager.Instance != null)
        {
            JumpscareManager.Instance.TriggerJumpscare(gameOverMessage);
        }
        else
        {
            // Fallback to direct game over if jumpscare not available
            GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
        }
    }

    // Public method to manually start chase (can be called from other scripts)
    public void ManuallyStartChase()
    {
        if (triggerOnce && hasTriggered)
        {
            if (debugMode) Debug.Log("[CinematicChase] Already triggered, ignoring manual start.");
            return;
        }

        hasTriggered = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Collider2D playerCollider = playerObj.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                StartCoroutine(StartChaseSequence(playerCollider));
            }
        }
    }

    // Public method to stop chase (can be called from other scripts)
    public void StopChase()
    {
        isChasing = false;

        if (emilyGameObject != null)
        {
            emilyGameObject.SetActive(false);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (debugMode) Debug.Log("[CinematicChase] Chase stopped manually.");
    }

    // Visualize trigger area in editor
    private void OnDrawGizmos()
    {
        // Draw trigger area
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position, col.bounds.size);
        }

        // Draw spawn point
        if (emilySpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(emilySpawnPoint.position, 0.5f);
            Gizmos.DrawLine(transform.position, emilySpawnPoint.position);
        }

        // Draw catch distance
        if (emilySpawnPoint != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawWireSphere(emilySpawnPoint.position, catchDistance);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw knockback direction
        if (enableKnockback)
        {
            Gizmos.color = Color.yellow;
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(knockbackDirection.normalized * 2f);
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.2f);
        }
    }
}
