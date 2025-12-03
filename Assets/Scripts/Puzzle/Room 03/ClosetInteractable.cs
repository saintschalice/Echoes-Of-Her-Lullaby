using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    [TextArea] public string firstDialogue = "There are scratches inside... someone was trying to get out.";

    [Header("Settings")]
    public float interactionRange = 2f;

    // State tracking
    private bool playerInRange = false;
    private bool waitingForDialogueClose = false;
    private bool canHide = false;
    private bool hasExaminedOnce = false;

    private DialogueSystemV2 dialogueSystem;

    // Audio tracking
    private float lockedSoundCooldown = 0f;

    // Button Locking References (NEW)
    private OnScreenInteractButton cachedButton;
    private Button cachedUnityButton;
    private UnityAction onHiddenInteractAction;

    void Awake()
    {
        // Cache the delegate to safely add/remove listeners
        onHiddenInteractAction = new UnityAction(OnHiddenInteract);
    }

    void Start()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        RefreshButtonReference();
    }

    void RefreshButtonReference()
    {
        if (cachedButton == null)
        {
            cachedButton = FindFirstObjectByType<OnScreenInteractButton>();
            if (cachedButton != null)
            {
                cachedUnityButton = cachedButton.GetComponent<Button>();
            }
        }
    }

    void OnDisable()
    {
        // Safety: Ensure button is unlocked if this object is disabled
        SetButtonLock(false);
    }

    void Update()
    {
        if (lockedSoundCooldown > 0) lockedSoundCooldown -= Time.deltaTime;

        if (waitingForDialogueClose)
        {
            if (dialogueSystem != null && !dialogueSystem.IsDialogueActive())
            {
                waitingForDialogueClose = false;
            }
        }
    }

    // =================================================================================
    // PRIMARY INTERACTION ENTRY POINT
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
            if (hideSequence != null)
            {
                RefreshButtonReference();

                if (hideSequence.IsHiding)
                {
                    // --- EXITING ---
                    // Called if normal Interaction somehow hits this (unlikely when hidden, but good fallback)
                    ExecuteExitLogic();
                }
                else
                {
                    // --- ENTERING ---
                    // 1. Lock the button FIRST so it doesn't flicker/disable when Player disables
                    SetButtonLock(true);

                    // 2. Start Hiding
                    hideSequence.HideInCloset();
                }
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

    // New: Listener called directly by the button when we are hiding
    private void OnHiddenInteract()
    {
        // FORCE EXIT: Bypass Interact() guards (like dialogue checks) 
        // because we are in a special "Hiding" state where the only action is to leave.
        if (hideSequence != null && hideSequence.IsHiding)
        {
            ExecuteExitLogic();
        }
    }

    private void ExecuteExitLogic()
    {
        if (hideSequence != null)
        {
            hideSequence.GetOutOfCloset();
        }

        // Unlock the button immediately so it returns to normal behavior
        SetButtonLock(false);
    }

    // New: Helper to lock/unlock the UI button
    private void SetButtonLock(bool locked)
    {
        if (cachedButton != null && cachedUnityButton != null)
        {
            cachedButton.SetInteractionLock(locked);

            // Clean up listener first to avoid duplicates
            cachedUnityButton.onClick.RemoveListener(onHiddenInteractAction);

            if (locked)
            {
                cachedUnityButton.onClick.AddListener(onHiddenInteractAction);
            }
        }
    }

    // =================================================================================
    // INTERFACE METHODS (IInteractable)
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        // Typically empty if you call Interact() directly from the button,
        // but if your system calls OnInteract(), we forward it:
        Interact();
    }

    public void OnFocus(PlayerContext context)
    {
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
        waitingForDialogueClose = true;

        if (!hasExaminedOnce)
        {
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue(examineDialogue, "Lisa");
            }
            hasExaminedOnce = true;
        }
        else
        {
            if (lockedSound != null && lockedSoundCooldown <= 0)
            {
                AudioManager.Instance?.PlaySFX(lockedSound, transform.position);
                lockedSoundCooldown = 1.0f;
            }

            if (closetAnimator != null)
            {
                closetAnimator.SetTrigger("Rattle");
            }

            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue(lockedDialogue, "Lisa");
            }
        }
    }

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