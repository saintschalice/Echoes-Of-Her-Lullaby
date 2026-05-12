using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class Room05_CinematicChaseTrigger : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Saan lilitaw si Emily kapag na-trigger ito?")]
    public Transform spawnPoint;

    [Header("AI Configuration")]
    [Tooltip("The state Emily will enter immediately after the cutscene ends.")]
    public EmilyGhost.State spawnState = EmilyGhost.State.Hunt;

    [Tooltip("Direction she should face when she spawns (x=1 is Right, x=-1 is Left).")]
    public Vector2 spawnFacing = Vector2.right;
    
    [Header("Emily Stats")]
    [Tooltip("Emily's movement speed during this chase")]
    [Range(1f, 10f)]
    public float emilyChaseSpeed = 3.5f;
    
    [Tooltip("Distance at which Emily catches player (Game Over)")]
    [Range(0.5f, 3f)]
    public float catchDistance = 1.0f;

    [Header("Cinematic Settings")]
    [Tooltip("Initial shove force")]
    [Range(0f, 30f)]
    public float pushForce = 15f;
    
    [Tooltip("Stops player quickly after shove")]
    [Range(0f, 20f)]
    public float shoveFriction = 10f;
    
    [Tooltip("Time before Emily starts attacking")]
    [Range(0f, 2f)]
    public float resumeDelay = 0.3f;

    [Header("Audio")]
    public AudioClip jumpscareClip;

    [Header("Narrative")]
    [TextArea] public string emilyShout = "YOU NEED TO GET OUT!";
    [TextArea] public string lisaPanic = "Holy- I need to hide!";

    [Header("Persistence")]
    [Tooltip("Unique ID to ensure dialogue doesn't play twice on Retry.")]
    public string triggerID = "Room05_EmilySpawn_Intro";
    
    [Header("Debug")]
    [Tooltip("Show debug messages in console")]
    public bool debugMode = true;

    private BoxCollider2D _triggerCollider;

    void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider2D>();
        _triggerCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        // Safety check: Wag na mag-trigger kung tapos na ang puzzle o naghahabol na si Emily
        if (Room05_DiningRoomController.Instance != null)
        {
            if (Room05_DiningRoomController.Instance.puzzleCompleted || Room05_DiningRoomController.Instance.isEmilyHunting)
            {
                if (debugMode) Debug.Log("[CinematicTrigger] Skipped - Puzzle complete or Emily already hunting");
                return;
            }
        }

        if (_triggerCollider != null) _triggerCollider.enabled = false;
        
        if (debugMode) Debug.Log("[CinematicTrigger] Player entered trigger - Starting spawn sequence");

        StartCoroutine(SpawnSequence(col.gameObject));
    }

    IEnumerator SpawnSequence(GameObject playerObj)
    {
        // --- 1. GRAB EXISTING EMILY FROM ROOM CONTROLLER ---
        var roomController = Room05_DiningRoomController.Instance;
        if (roomController == null || roomController.emilyEnemy == null)
        {
            Debug.LogError("[CinematicTrigger] Missing Room Controller or Emily reference!");
            yield break;
        }

        GameObject emilyInstance = roomController.emilyEnemy;
        EmilyGhost emilyScript = emilyInstance.GetComponent<EmilyGhost>();

        // Position her and turn her on (Frozen state)
        if (spawnPoint != null)
        {
            emilyInstance.transform.position = spawnPoint.position;
            emilyInstance.SetActive(true);

            if (emilyScript != null)
            {
                emilyScript.ForceFacing(spawnFacing);
                emilyScript.enabled = false; // Freeze AI logic

                if (emilyInstance.GetComponent<EmilyMovement>() != null)
                    emilyInstance.GetComponent<EmilyMovement>().enabled = false;
                    
                if (debugMode) Debug.Log($"[CinematicTrigger] Emily spawned at {spawnPoint.position}, facing {spawnFacing}");
            }
        }

        // --- 2. JUMPSCARE AUDIO ---
        if (jumpscareClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(jumpscareClip);
            if (debugMode) Debug.Log("[CinematicTrigger] Jumpscare audio played");
        }
        else if (jumpscareClip != null && roomController.roomAudioSource != null)
        {
            roomController.roomAudioSource.PlayOneShot(jumpscareClip);
            if (debugMode) Debug.Log("[CinematicTrigger] Jumpscare audio played (room source)");
        }

        // --- 3. PUSH MECHANIC ---
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        JoystickPlayerController playerController = playerObj.GetComponent<JoystickPlayerController>();
        float originalDrag = 0f;

        if (playerRb != null)
        {
            originalDrag = playerRb.linearDamping;
            playerRb.linearDamping = shoveFriction;

            if (playerController != null)
            {
                Animator playerAnim = playerObj.GetComponent<Animator>();
                if (playerAnim != null)
                {
                    playerAnim.SetFloat("Speed", 0f);
                    playerAnim.SetBool("IsMoving", false);
                }
                playerController.enabled = false;
            }

            Vector2 pushDir = (playerObj.transform.position - spawnPoint.position).normalized;
            if (pushDir == Vector2.zero) pushDir = Vector2.down;
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            
            if (debugMode) Debug.Log($"[CinematicTrigger] Player pushed with force {pushForce} in direction {pushDir}");
        }

        // --- 4. DIALOGUE ---
        bool skipDialogue = false;
        if (SaveSystem.Instance != null && SaveSystem.Instance.WasDialogueTriggered(triggerID))
        {
            skipDialogue = true;
            if (debugMode) Debug.Log("[CinematicTrigger] Dialogue already seen - skipping");
        }

        if (!skipDialogue && DialogueSystemV2.Instance != null)
        {
            if (SaveSystem.Instance != null)
                SaveSystem.Instance.TriggerDialogue(triggerID);

            yield return new WaitForSeconds(0.2f);
            DialogueSystemV2.Instance.StartDialogue(emilyShout, "???");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;

            DialogueSystemV2.Instance.StartDialogue(lisaPanic, "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            
            if (debugMode) Debug.Log("[CinematicTrigger] Dialogue sequence complete");
        }
        else
        {
            yield return new WaitForSeconds(skipDialogue ? 0.5f : 2.0f);
        }

        // --- 5. WAIT DELAY ---
        yield return new WaitForSeconds(resumeDelay);

        // --- 6. APPLY EMILY STATS ---
        ApplyEmilyStats(emilyScript);

        // --- 7. RESTORE PLAYER ---
        if (playerRb != null) playerRb.linearDamping = originalDrag;
        if (playerController != null) playerController.enabled = true;

        // --- 8. START THE HUNT ---
        if (emilyScript != null)
        {
            if (emilyInstance.GetComponent<EmilyMovement>() != null)
                emilyInstance.GetComponent<EmilyMovement>().enabled = true;

            emilyScript.enabled = true;
            emilyScript.SetStateExternal(spawnState);
            
            if (debugMode) Debug.Log($"[CinematicTrigger] Emily AI enabled - State: {spawnState}, Speed: {emilyChaseSpeed}");
        }

        // IMPORTANT: Tell the Room 5 Controller that the hunt is ON so the Table Hiding works!
        roomController.isEmilyHunting = true;
        
        // Apply speed to Room Controller for consistency
        roomController.initialChaseSpeed = emilyChaseSpeed;

        if (debugMode) Debug.Log("[CinematicTrigger] Sequence Complete. Emily is hunting!");
        Destroy(gameObject); // Remove trigger so it doesn't happen again
    }
    
    /// <summary>
    /// Apply configured Emily stats
    /// </summary>
    private void ApplyEmilyStats(EmilyGhost emilyScript)
    {
        if (emilyScript == null) return;
        
        // Apply chase speed
        emilyScript.huntSpeed = emilyChaseSpeed;
        
        if (debugMode) Debug.Log($"[CinematicTrigger] Emily stats applied - Speed: {emilyChaseSpeed}, Catch Distance: {catchDistance}");
    }
    
    // Visualize trigger area in editor
    private void OnDrawGizmos()
    {
        // Draw trigger area
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(col.offset, col.size);
        }
        
        // Draw spawn point
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            Gizmos.DrawLine(transform.position, spawnPoint.position);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw push direction preview
        if (spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 pushDir = (transform.position - spawnPoint.position).normalized;
            if (pushDir == Vector3.zero) pushDir = Vector3.down;
            Vector3 start = transform.position;
            Vector3 end = start + pushDir * 2f;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.2f);
        }
        
        // Draw catch distance preview
        if (spawnPoint != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawWireSphere(spawnPoint.position, catchDistance);
        }
    }
}