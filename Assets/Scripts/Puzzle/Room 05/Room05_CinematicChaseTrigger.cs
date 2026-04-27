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

    [Header("Cinematic Settings")]
    public float pushForce = 15f;        // Initial shove
    public float shoveFriction = 10f;    // Stops player quickly
    public float resumeDelay = 0.3f;     // Time before she attacks

    [Header("Audio")]
    public AudioClip jumpscareClip;

    [Header("Narrative")]
    [TextArea] public string emilyShout = "YOU NEED TO GET OUT!";
    [TextArea] public string lisaPanic = "Holy- I need to hide!";

    [Header("Persistence")]
    [Tooltip("Unique ID to ensure dialogue doesn't play twice on Retry.")]
    public string triggerID = "Room05_EmilySpawn_Intro";

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
                return;
        }

        if (_triggerCollider != null) _triggerCollider.enabled = false;

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
            }
        }

        // --- 2. JUMPSCARE AUDIO ---
        if (jumpscareClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(jumpscareClip);
        }
        else if (jumpscareClip != null && roomController.roomAudioSource != null)
        {
            roomController.roomAudioSource.PlayOneShot(jumpscareClip);
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
        }

        // --- 4. DIALOGUE ---
        bool skipDialogue = false;
        if (SaveSystem.Instance != null && SaveSystem.Instance.WasDialogueTriggered(triggerID))
        {
            skipDialogue = true;
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
        }
        else
        {
            yield return new WaitForSeconds(skipDialogue ? 0.5f : 2.0f);
        }

        // --- 5. WAIT DELAY ---
        yield return new WaitForSeconds(resumeDelay);

        // --- 6. RESTORE PLAYER ---
        if (playerRb != null) playerRb.linearDamping = originalDrag;
        if (playerController != null) playerController.enabled = true;

        // --- 7. START THE HUNT ---
        if (emilyScript != null)
        {
            if (emilyInstance.GetComponent<EmilyMovement>() != null)
                emilyInstance.GetComponent<EmilyMovement>().enabled = true;

            emilyScript.enabled = true;
            emilyScript.SetStateExternal(spawnState);
        }

        // IMPORTANT: Tell the Room 5 Controller that the hunt is ON so the Table Hiding works!
        roomController.isEmilyHunting = true;

        Debug.Log("[EMILY SPAWN] Sequence Complete. Emily is hunting!");
        Destroy(gameObject); // Remove trigger so it doesn't happen again
    }
}