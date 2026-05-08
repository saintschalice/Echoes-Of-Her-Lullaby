using UnityEngine;

public class FlowerPotInteraction : MonoBehaviour, IInteractable
{
    [Header("Flower Pot Objects")]
    public GameObject intactFlowerPot;
    public GameObject brokenFlowerPot;
    public GameObject houseKey;

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Audio - SFX")]
    public AudioClip breakingSound;
    public AudioClip keyFoundSound;

    // REMOVED: No more AudioSource needed!
    private bool hasBeenExamined = false;
    private bool hasBeenBroken = false;
    private bool playerInRange = false;
    private bool keyRevealed = false;
    private bool waitingForDialogueClose = false;
    private bool waitingForResponse = false;
    private DialogueSystemV2 dialogueSystem;

    private const string FLOWERPOT_EXAMINED_ID = "FlowerPot_Foyer_Examined";
    private const string FLOWERPOT_BROKEN_ID = "FlowerPot_Foyer_Broken";
    private const string HOUSE_KEY_ID = "house_key";
    private const string MAIL_ITEM_ID = "foyer_mail";
    private const string LETTER_ITEM_ID = "foyer_letter";

    void Start()
    {
        if (intactFlowerPot != null)
            intactFlowerPot.SetActive(true);

        if (brokenFlowerPot != null)
            brokenFlowerPot.SetActive(false);

        if (houseKey != null)
            houseKey.SetActive(false);

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

                if (hasBeenExamined && !hasBeenBroken)
                {
                    ShowBreakPotChoices();
                }
            }
            return;
        }

    }

    void SyncStateWithSave()
    {
        if (SaveSystem.Instance == null || InventoryManager.Instance == null) return;

        bool hasKeyInInventory = InventoryManager.Instance.HasItem(HOUSE_KEY_ID);
        hasBeenExamined = SaveSystem.Instance.WasObjectExamined(FLOWERPOT_EXAMINED_ID);
        bool shouldBeBroken = SaveSystem.Instance.WasObjectExamined(FLOWERPOT_BROKEN_ID);

        if (shouldBeBroken)
        {
            hasBeenBroken = true;

            if (intactFlowerPot != null)
                intactFlowerPot.SetActive(false);

            if (brokenFlowerPot != null)
                brokenFlowerPot.SetActive(true);

            if (hasKeyInInventory)
            {
                keyRevealed = true;
                if (houseKey != null)
                    houseKey.SetActive(false);
            }
            else
            {
                keyRevealed = true;
                if (houseKey != null)
                    houseKey.SetActive(true);
            }
        }
        else
        {
            hasBeenBroken = false;
            keyRevealed = false;

            if (intactFlowerPot != null)
                intactFlowerPot.SetActive(true);

            if (brokenFlowerPot != null)
                brokenFlowerPot.SetActive(false);

            if (houseKey != null)
                houseKey.SetActive(false);
        }
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        if (waitingForDialogueClose || waitingForResponse)
            return;

        if (keyRevealed && houseKey != null && houseKey.activeSelf)
        {
            PickupKey();
            return;
        }

        if (!hasBeenExamined)
        {
            FirstExamine();
        }
        else if (!hasBeenBroken)
        {
            AskToBreakPot();
        }
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        playerInRange = IsInRange(context.Transform);

        if (!playerInRange || waitingForDialogueClose || waitingForResponse)
            return;

        if (keyRevealed && houseKey != null && houseKey.activeSelf)
        {
            PickupKey();
            return;
        }

        if (!hasBeenExamined)
        {
            FirstExamine();
        }
        else if (!hasBeenBroken)
        {
            AskToBreakPot();
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

    void FirstExamine()
    {
        if (InventoryManager.Instance != null)
        {
            bool hasMail = InventoryManager.Instance.HasItem(MAIL_ITEM_ID);
            bool hasLetter = InventoryManager.Instance.HasItem(LETTER_ITEM_ID);

            if (!hasMail && !hasLetter)
            {
                ShowDialogue("It's just an ordinary flower pot. Nothing special about it.");
                return;
            }
        }

        hasBeenExamined = true;

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(FLOWERPOT_EXAMINED_ID);
        }

        ShowDialogue("I looked inside the flower pot... but there's nothing here. Just dirt and flowers.");
    }

    void AskToBreakPot()
    {
        waitingForDialogueClose = true;

        if (dialogueSystem != null)
        {
            string dialogue = "The letter mentioned something about flowers... Maybe I need to look deeper? Should I break the pot?";
            dialogueSystem.StartDialogue(dialogue, "Lisa");
        }
        else
        {
            BreakFlowerPot();
        }
    }

    void ShowBreakPotChoices()
    {
        waitingForResponse = true;

        if (dialogueSystem != null)
        {
            dialogueSystem.ShowChoices(
                new string[] { "Yes, break it", "No, leave it alone" },
                new System.Action[] { OnChoiceBreakPot, OnChoiceLeavePot }
            );
        }
    }

    void OnChoiceBreakPot()
    {
        waitingForResponse = false;
        BreakFlowerPot();
    }

    void OnChoiceLeavePot()
    {
        waitingForResponse = false;
        ShowDialogue("I'll leave it alone for now. Maybe I should reconsider.");
    }

    void BreakFlowerPot()
    {
        hasBeenBroken = true;

        if (intactFlowerPot != null)
            intactFlowerPot.SetActive(false);

        if (brokenFlowerPot != null)
            brokenFlowerPot.SetActive(true);

        // NEW: Play breaking sound through AudioManager (categorized as SFX)
        if (breakingSound != null)
        {
            AudioManager.Instance?.PlaySFX(breakingSound, transform.position);
        }

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(FLOWERPOT_BROKEN_ID);
        }

        ShowDialogue("The pot shattered! There's something gleaming in the dirt...");

        Invoke(nameof(RevealKey), 1f);
    }

    void RevealKey()
    {
        if (houseKey != null)
        {
            houseKey.SetActive(true);
            keyRevealed = true;

            // NEW: Play key found sound through AudioManager (categorized as SFX)
            if (keyFoundSound != null)
            {
                AudioManager.Instance?.PlaySFX(keyFoundSound, transform.position);
            }

            ShowDialogue("A house key! This must unlock the front door.");
        }
    }

    void PickupKey()
    {
        if (!keyRevealed || houseKey == null || !houseKey.activeSelf) return;

        // Use AddItemWithNotification with proper description
        if (InventoryManager.Instance != null)
        {
            bool added = InventoryManager.Instance.AddItemWithNotification(
                HOUSE_KEY_ID, 
                "A rusty house key found in the broken flower pot."
            );
            
            if (added)
            {
                houseKey.SetActive(false);
                Debug.Log("[FlowerPot] House key added to inventory with notification!");
            }
            else
            {
                Debug.LogWarning("[FlowerPot] Failed to add house key to inventory!");
            }
        }
        else
        {
            Debug.LogError("[FlowerPot] InventoryManager.Instance is null!");
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

        hasBeenExamined = SaveSystem.Instance.WasObjectExamined(FLOWERPOT_EXAMINED_ID);

        if (SaveSystem.Instance.WasObjectExamined(FLOWERPOT_BROKEN_ID))
        {
            hasBeenBroken = true;

            if (intactFlowerPot != null)
                intactFlowerPot.SetActive(false);

            if (brokenFlowerPot != null)
                brokenFlowerPot.SetActive(true);

            if (SaveSystem.Instance.HasItem(HOUSE_KEY_ID))
            {
                keyRevealed = true;
                if (houseKey != null)
                    houseKey.SetActive(false);
            }
            else
            {
                keyRevealed = true;
                if (houseKey != null)
                    houseKey.SetActive(true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}