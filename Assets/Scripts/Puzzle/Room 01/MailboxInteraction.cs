using UnityEngine;

public class MailboxInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Audio")]
    public AudioClip openMailboxSound;
    public AudioClip takeMailSound;

    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        mainCamera = Camera.main;

        CheckSaveState();
    }

    void Update()
    {
        // CRITICAL FIX: Always sync state with save data
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

        // Only allow interaction if NOT already opened
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

    // NEW: Sync state with SAVE DATA not inventory
    void SyncStateWithSave()
    {
        if (SaveSystem.Instance == null) return;

        // Check if mailbox was opened (this is the SOURCE OF TRUTH)
        hasBeenOpened = SaveSystem.Instance.WasObjectExamined(MAILBOX_OPENED_ID);

        // If opened, mail was automatically taken
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
        mailTaken = true; // Mail goes straight to inventory

        // Play sounds
        if (openMailboxSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(openMailboxSound);
        }

        if (takeMailSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(takeMailSound);
        }

        // Mark as opened in save
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(MAILBOX_OPENED_ID);
        }

        // Add mail DIRECTLY to inventory (no physical object)
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(MAIL_ITEM_ID);
        }

        // Notify tutorial
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnMailTaken();
        }

        // Show dialogue
        ShowDialogue("There's a letter inside! I took it and put it in my inventory.");

        Debug.Log("Mail added directly to inventory!");
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
        // Will be synced every frame in Update
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