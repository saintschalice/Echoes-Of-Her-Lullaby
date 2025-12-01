using UnityEngine;

public class HallwayClosetInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Animator closetAnimator;
    public ClosetHideSequence hideSequence;

    [Header("Audio")]
    public AudioClip lockedSound; // Assign a "rattle" or "locked" sound here
    public AudioClip doorCreakSound;
    public AudioClip scratchSound;

    [Header("Dialogue")]
    [TextArea] public string examineDialogue = "This is really big... I could probably fit inside.";
    [TextArea] public string lockedDialogue = "It won't open. It's stuck.";
    [TextArea] public string firstDialogue = "There are scratches inside... someone was trying to get out."; // Kept if needed for later, though not currently used in logic below

    [Header("Settings")]
    public float interactionRange = 2f;

    // State tracking (Matching MailboxInteraction style)
    private bool playerInRange = false;
    private bool waitingForDialogueClose = false;
    private bool canHide = false; // Default to false until Emily spawns
    private bool hasExaminedOnce = false;

    private DialogueSystemV2 dialogueSystem;

    // Audio tracking
    private float lockedSoundCooldown = 0f;

    void Start()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
    }

    void Update()
    {
        // Cooldown for the rattle sound so it doesn't play every frame if spammed
        if (lockedSoundCooldown > 0) lockedSoundCooldown -= Time.deltaTime;

        // State Machine: Watch for dialogue to close (Just like MailboxInteraction)
        if (waitingForDialogueClose)
        {
            if (dialogueSystem != null && !dialogueSystem.IsDialogueActive())
            {
                waitingForDialogueClose = false;
                // Interaction cycle complete.
            }
        }
    }

    // =================================================================================
    // PRIMARY INTERACTION ENTRY POINT (Called by OnScreenInteractButton)
    // =================================================================================
    public void Interact()
    {
        // 1. Guard: Busy waiting for dialogue?
        if (waitingForDialogueClose)
        {
            Debug.Log("Closet busy: waiting for dialogue to close.");
            return;
        }

        // 2. Guard: Dialogue system active elsewhere?
        if (dialogueSystem != null && dialogueSystem.IsDialogueActive())
            return;

        // 3. Logic: Chase Sequence vs. Locked State
        if (canHide)
        {
            // The chase is on! Hide immediately.
            if (hideSequence != null)
            {
                hideSequence.HideInCloset();
            }
            else
            {
                Debug.LogError("Closet unlocked but HideSequence reference is missing!");
            }
        }
        else
        {
            // Normal exploration state
            PerformExamineLogic();
        }
    }

    // =================================================================================
    // INTERFACE METHODS (IInteractable)
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        // STRICTLY EMPTY: 
        // We do not want touching/clicking the closet directly to do anything.
        // The interaction must come via the OnScreenInteractButton calling Interact() above.
    }

    public void OnFocus(PlayerContext context)
    {
        // Only update range for internal logic or debug, but DO NOT show prompts.
        playerInRange = IsInRange(context.Transform);
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
    }

    // =================================================================================
    // INTERNAL LOGIC
    // =================================================================================

    private void PerformExamineLogic()
    {
        // Set flag so we don't interact again until dialogue finishes
        waitingForDialogueClose = true;

        if (!hasExaminedOnce)
        {
            // 1. First time examination
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue(examineDialogue, "Lisa");
            }
            hasExaminedOnce = true;
        }
        else
        {
            // 2. Subsequent times: It's locked
            // Play Sound
            if (lockedSound != null && lockedSoundCooldown <= 0)
            {
                AudioManager.Instance?.PlaySFX(lockedSound, transform.position);
                lockedSoundCooldown = 1.0f; // Prevent sound spam
            }

            // Optional: Animation
            if (closetAnimator != null)
            {
                closetAnimator.SetTrigger("Rattle");
            }

            // Show Dialogue
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue(lockedDialogue, "Lisa");
            }
        }
    }

    // Called by Emily Spawn Trigger / Game Event
    public void UnlockForHiding()
    {
        canHide = true;
        Debug.Log("[Closet] Unlocked for hiding sequence.");
    }

    bool IsInRange(Transform target)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= interactionRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}