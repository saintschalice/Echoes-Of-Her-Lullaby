using UnityEngine;
using System.Collections;

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
    public GameObject interactPrompt;

    private Transform player;
    private bool canHide = false; // Default to false until Emily spawns
    private bool isLocked = true;
    private bool hasExaminedOnce = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    public void OnInteract(PlayerContext context)
    {
        player = context.Transform;

        if (!IsInRange(player) || (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()))
            return;

        // If the chase sequence is active (unlocked by spawn trigger), allow hiding
        if (canHide)
        {
            // If we haven't done the "Scratches inside" dialogue yet, do that quickly first? 
            // Or just go straight to hiding because we are being chased. 
            // Based on the prompt: "The player hides inside the closet." -> Implies immediate hiding.
            hideSequence?.HideInCloset();
        }
        else
        {
            // Before the chase: Examine and find it locked
            StartCoroutine(ExamineRoutine());
        }
    }

    public void OnFocus(PlayerContext context)
    {
        player = context.Transform;
        bool inRange = IsInRange(player);
        if (interactPrompt != null)
            interactPrompt.SetActive(inRange && (DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive()));
    }

    public void OnBlur(PlayerContext context)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    bool IsInRange(Transform target)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= interactionRange;
    }

    IEnumerator ExamineRoutine()
    {
        // Prevent spamming
        if (DialogueSystemV2.Instance.IsDialogueActive()) yield break;

        if (!hasExaminedOnce)
        {
            // 1. Initial thought
            DialogueSystemV2.Instance?.StartDialogue(examineDialogue, "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            hasExaminedOnce = true;
        }

        // 2. Attempt to open (Sound + Animation rattle if you have one)
        if (lockedSound) AudioManager.Instance?.PlaySFX(lockedSound);

        // Optional: Trigger a "Rattle" animation here if exists
        // closetAnimator?.SetTrigger("Rattle"); 

        yield return new WaitForSeconds(0.5f);

        // 3. Locked conclusion
        DialogueSystemV2.Instance?.StartDialogue(lockedDialogue, "Lisa");
    }

    // CALLED BY EMILY SPAWN TRIGGER
    public void UnlockForHiding()
    {
        isLocked = false;
        canHide = true;
        Debug.Log("[Closet] Unlocked for hiding sequence.");
    }
}