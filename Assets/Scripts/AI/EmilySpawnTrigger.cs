using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class EmilySpawnTrigger : MonoBehaviour
{
    [Header("Setup")]
    public EmilyGhost emilyPrefab;
    public Transform spawnPoint;

    [Header("AI Configuration")]
    [Tooltip("The state Emily will enter immediately after the cutscene ends.")]
    public EmilyGhost.State spawnState = EmilyGhost.State.Hunt;

    [Tooltip("Direction she should face when she spawns (x=1 is Right, x=-1 is Left).")]
    public Vector2 spawnFacing = Vector2.right;

    [Header("Scene Connections")]
    public HallwayClosetInteractable closetScript;

    [Header("Cinematic Settings")]
    public float pushForce = 3f;       // The initial "shove" strength
    public float shoveFriction = 10f;  // High drag stops the player quickly
    public float resumeDelay = 0.5f;   // Time to wait before Emily attacks

    [Header("Audio")]
    public AudioClip jumpscareClip; // NEW: Jumpscare sound

    [Header("Narrative")]
    [TextArea] public string emilyShout = "YOU NEED TO GET OUT!";
    [TextArea] public string lisaPanic = "<i>Holy-</i> I need to hide!";

    private EmilyGhost _instance;
    private BoxCollider2D _triggerCollider;

    void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_instance != null) return;

        if (_triggerCollider != null) _triggerCollider.enabled = false;

        StartCoroutine(SpawnSequence(col.gameObject));
    }

    IEnumerator SpawnSequence(GameObject playerObj)
    {
        // 1. Spawn Emily (Frozen)
        if (emilyPrefab != null && spawnPoint != null)
        {
            // Instantiate directly at the spawnPoint.position
            _instance = Instantiate(emilyPrefab, spawnPoint.position, Quaternion.identity);

            // Explicitly override position again just to be safe
            _instance.transform.position = spawnPoint.position;

            _instance.gameObject.SetActive(true);

            // FORCE FACING: Set animator parameters immediately
            _instance.ForceFacing(spawnFacing);

            // Disable AI update loop initially
            _instance.enabled = false;
        }

        // 2. Play Jumpscare
        if (jumpscareClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(jumpscareClip);
        }

        // 3. Unlock Closet (Legacy support if script exists)
        if (closetScript != null) closetScript.UnlockForHiding();

        // 4. Push Mechanic
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        JoystickPlayerController playerController = playerObj.GetComponent<JoystickPlayerController>();
        float originalDrag = 0f;

        if (playerRb != null)
        {
            originalDrag = playerRb.linearDamping;
            playerRb.linearDamping = shoveFriction;

            if (playerController != null) playerController.enabled = false;

            Vector2 pushDir = (playerObj.transform.position - spawnPoint.position).normalized;
            if (pushDir == Vector2.zero) pushDir = Vector2.down;
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }

        // 5. Dialogue Sequence
        if (DialogueSystemV2.Instance != null)
        {
            // Emily Shouts
            yield return new WaitForSeconds(0.2f); // Short pause to register the push
            DialogueSystemV2.Instance.StartDialogue(emilyShout, "???");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;

            // Lisa Panics
            DialogueSystemV2.Instance.StartDialogue(lisaPanic, "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        }
        else
        {
            // Fallback delay if no dialogue system
            yield return new WaitForSeconds(2.0f);
        }

        // 6. Wait (Requested Delay)
        yield return new WaitForSeconds(resumeDelay);

        // 7. Restore Player State
        if (playerRb != null)
        {
            playerRb.linearDamping = originalDrag;
        }
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // 8. Resume Emily & Apply Configured State
        if (_instance != null)
        {
            _instance.enabled = true;
            _instance.SetStateExternal(spawnState);
            Debug.Log($"[EMILY SPAWN] Resumed in state: {spawnState}");
        }

        Debug.Log("[EMILY SPAWN] Sequence Complete.");
        Destroy(gameObject);
    }
}