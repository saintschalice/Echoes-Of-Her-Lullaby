using UnityEngine;

public class MailboxInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Audio - SFX")]
    public AudioClip openMailboxSound;
    public AudioClip takeMailSound;

    // REMOVED: No more AudioSource needed!
    private bool hasBeenOpened = false;
    private bool mailTaken = false;
    private bool playerInRange = false;
    private bool waitingForDialogueClose = false;
    private bool waitingForResponse = false;
    private DialogueSystemV2 dialogueSystem;

    private enum NextAction { None, ShowLookInsideChoices }
    private NextAction nextAction = NextAction.None;

    private const string MAILBOX_OPENED_ID = "Mailbox_Foyer_Opened";
    private const string MAIL_ITEM_ID = "foyer_mail";

    void Start()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();

        CheckSaveState();
    }

    void Update()
    {
        SyncStateWithSave();

        if (waitingForDialogueClose)
        {
            if (dialogueSystem != null && !dialogueSystem.IsDialogueActive())
            {
                waitingForDialogueClose = false;

                if (nextAction == NextAction.ShowLookInsideChoices)
                {
                    nextAction = NextAction.None;
                    ShowLookInsideChoices();
                }
            }
            return;
        }
    }

    void SyncStateWithSave()
    {
        if (SaveSystem.Instance == null) return;

        hasBeenOpened = SaveSystem.Instance.WasObjectExamined(MAILBOX_OPENED_ID);
        mailTaken = hasBeenOpened;
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker
    // =================================================================================
    public void Interact()
    {
        Debug.Log("Mailbox Interact() called!"); // DEBUG: Check console to see if this prints

        // The Tracker handles the distance check, so we can execute the logic directly.
        if (waitingForDialogueClose || waitingForResponse)
        {
            Debug.Log("Mailbox busy waiting for dialogue/response.");
            return;
        }

        if (!hasBeenOpened)
        {
            AskToLookInside();
        }
        else
        {
            // Optional: Feedback if already opened
            ShowDialogue("I've already checked the mail.");
        }
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        playerInRange = IsInRange(context.Transform);

        if (!playerInRange || waitingForDialogueClose || waitingForResponse)
            return;

        if (!hasBeenOpened)
        {
            AskToLookInside();
        }
    }

    public void OnFocus(PlayerContext context)
    {
        playerInRange = IsInRange(context.Transform);
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
    }

    bool IsInRange(Transform playerTransform)
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= interactionRadius;
    }

    void AskToLookInside()
    {
        waitingForDialogueClose = true;
        nextAction = NextAction.ShowLookInsideChoices;

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnMailboxExamined();
        }

        if (dialogueSystem != null)
        {
            string dialogue = "There's a mailbox here. Should I look inside?";
            dialogueSystem.StartDialogue(dialogue, "Lisa");
        }
        else
        {
            OpenMailbox();
        }
    }

    void ShowLookInsideChoices()
    {
        waitingForResponse = true;

        if (dialogueSystem != null)
        {
            dialogueSystem.ShowChoices(
                new string[] { "Yes, look inside", "No, leave it" },
                new System.Action[] { OnChoiceLookInside, OnChoiceLeaveMailbox }
            );
        }
    }

    void OnChoiceLookInside()
    {
        waitingForResponse = false;
        OpenMailbox();
    }

    void OnChoiceLeaveMailbox()
    {
        waitingForResponse = false;
        ShowDialogue("I'll leave it alone for now.");
    }

    void OpenMailbox()
    {
        hasBeenOpened = true;
        mailTaken = true;

        // NEW: Play sounds through AudioManager (categorized as SFX)
        if (openMailboxSound != null)
        {
            AudioManager.Instance?.PlaySFX(openMailboxSound, transform.position);
        }

        // Play take mail sound with slight delay for better audio feedback
        if (takeMailSound != null)
        {
            Invoke(nameof(PlayTakeMailSound), 0.3f);
        }

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(MAILBOX_OPENED_ID);
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(MAIL_ITEM_ID);
        }

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnMailTaken();
        }

        ShowDialogue("There's a letter inside! I took it and put it in my inventory.");

        Debug.Log("Mail added directly to inventory!");
    }

    // NEW: Separate method for delayed take mail sound
    void PlayTakeMailSound()
    {
        if (takeMailSound != null)
        {
            AudioManager.Instance?.PlaySFX(takeMailSound, transform.position);
        }
    }

    void ShowDialogue(string message)
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(message, "Lisa");
        }
        else
        {
            Debug.Log($"Lisa: {message}");
        }
    }

    void CheckSaveState()
    {
        if (SaveSystem.Instance == null) return;

        hasBeenOpened = SaveSystem.Instance.WasObjectExamined(MAILBOX_OPENED_ID);
        mailTaken = hasBeenOpened;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}