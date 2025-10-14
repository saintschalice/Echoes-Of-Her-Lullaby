using UnityEngine;

public class MailboxInteraction : MonoBehaviour
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
    private Camera mainCamera;

    private enum NextAction { None, ShowLookInsideChoices }
    private NextAction nextAction = NextAction.None;

    private const string MAILBOX_OPENED_ID = "Mailbox_Foyer_Opened";
    private const string MAIL_ITEM_ID = "foyer_mail";

    void Start()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        mainCamera = Camera.main;

        CheckSaveState();
    }

    void Update()
    {
        SyncStateWithSave();
        CheckPlayerDistance();

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

        if (playerInRange && !hasBeenOpened && !waitingForResponse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsTappedOn())
                {
                    AskToLookInside();
                }
            }
        }
    }

    void SyncStateWithSave()
    {
        if (SaveSystem.Instance == null) return;

        hasBeenOpened = SaveSystem.Instance.WasObjectExamined(MAILBOX_OPENED_ID);
        mailTaken = hasBeenOpened;
    }

    bool IsTappedOn()
    {
        if (mainCamera == null) return false;

        Vector2 touchPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            return hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform);
        }

        return false;
    }

    void CheckPlayerDistance()
    {
        GameObject player = GameObject.FindGameObjectWithTag(requiredTag);

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            playerInRange = distance <= interactionRadius;
        }
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