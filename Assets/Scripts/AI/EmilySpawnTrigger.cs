using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public AudioClip jumpscareClip;

    [Header("Narrative")]
    [TextArea] public string emilyShout = "YOU NEED TO GET OUT!";
    [TextArea] public string lisaPanic = "<i>Holy-</i> I need to hide!";

    [Header("Persistence")]
    [Tooltip("Unique ID to ensure dialogue doesn't play twice on Retry.")]
    public string triggerID = "EmilySpawn_Intro";

    private EmilyGhost _instance;
    private BoxCollider2D _triggerCollider;

    void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider2D>();
        
        // Subscribe to scene loaded event to reset trigger on retry
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the trigger when the scene loads (for retry logic)
        _instance = null;
        if (_triggerCollider != null) _triggerCollider.enabled = true;
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

            // Disable Movement component so FixedUpdate doesn't run while frozen
            if (_instance.GetComponent<EmilyMovement>() != null)
                _instance.GetComponent<EmilyMovement>().enabled = false;
        }

        // 2. Play Jumpscare (Always play this for impact, or wrap in check if preferred)
        if (jumpscareClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(jumpscareClip);
        }

        // 3. Unlock Closet (Legacy support if script exists)
        if (closetScript != null) closetScript.UnlockForHiding();

        // 4. Push Mechanic (Always push to reset player position/momentum)
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        JoystickPlayerController playerController = playerObj.GetComponent<JoystickPlayerController>();
        float originalDrag = 0f;

        if (playerRb != null)
        {
            originalDrag = playerRb.linearDamping;
            playerRb.linearDamping = shoveFriction;

            if (playerController != null)
            {
                // --- FIX START: FORCE STOP ANIMATION ---
                // Before we disable the controller, we MUST manually reset the animator.
                // Otherwise, if the player was holding 'Walk', the parameter stays true forever 
                // because the controller script stops running its Update() loop.
                Animator playerAnim = playerObj.GetComponent<Animator>();
                if (playerAnim != null)
                {
                    // Reset standard movement parameters to ensure she looks idle/stunned
                    // (Using safe checks in case your parameters are named differently)
                    playerAnim.SetFloat("Speed", 0f);
                    playerAnim.SetBool("IsMoving", false);

                    // Optional: If you use x/y for blend trees, you might want to keep the last direction
                    // or reset them. Usually Speed=0 is enough to trigger the Idle state.
                }
                // --- FIX END ---

                playerController.enabled = false;
            }

            Vector2 pushDir = (playerObj.transform.position - spawnPoint.position).normalized;
            if (pushDir == Vector2.zero) pushDir = Vector2.down;
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }

        // CHECK: Have we seen this dialogue before?
        bool skipDialogue = false;
        if (SaveSystem.Instance != null && SaveSystem.Instance.WasDialogueTriggered(triggerID))
        {
            skipDialogue = true;
        }

        // 5. Dialogue Sequence
        if (!skipDialogue && DialogueSystemV2.Instance != null)
        {
            // Mark as seen immediately so retries know to skip it
            if (SaveSystem.Instance != null)
                SaveSystem.Instance.TriggerDialogue(triggerID);

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
            // If skipping or no system, use a very short delay just to let the Push finish
            yield return new WaitForSeconds(skipDialogue ? 0.5f : 2.0f);
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
            // Re-enable Movement first
            if (_instance.GetComponent<EmilyMovement>() != null)
                _instance.GetComponent<EmilyMovement>().enabled = true;

            _instance.enabled = true;
            _instance.SetStateExternal(spawnState);
            Debug.Log($"[EMILY SPAWN] Resumed in state: {spawnState}");
        }

        Debug.Log("[EMILY SPAWN] Sequence Complete.");
        Destroy(gameObject);
    }
}