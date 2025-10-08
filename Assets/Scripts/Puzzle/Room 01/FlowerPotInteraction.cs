using UnityEngine;

public class FlowerPotInteraction : MonoBehaviour
{
    [Header("Flower Pot Objects")]
    public GameObject intactFlowerPot;
    public GameObject brokenFlowerPot;
    public GameObject houseKey;

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Audio")]
    public AudioClip breakingSound;
    public AudioClip keyFoundSound;

    private AudioSource audioSource;
    private bool hasBeenExamined = false;
    private bool hasBeenBroken = false;
    private bool playerInRange = false;
    private bool keyRevealed = false;
    private bool waitingForDialogueClose = false;
    private bool waitingForResponse = false;
    private DialogueSystemV2 dialogueSystem;
    private Camera mainCamera;

    private const string FLOWERPOT_EXAMINED_ID = "FlowerPot_Foyer_Examined";
    private const string FLOWERPOT_BROKEN_ID = "FlowerPot_Foyer_Broken";
    private const string HOUSE_KEY_ID = "house_key";
    private const string MAIL_ITEM_ID = "foyer_mail"; // Check for sealed mail
    private const string LETTER_ITEM_ID = "foyer_letter"; // Check for opened letter

    void Start()
    {
        if (intactFlowerPot != null)
            intactFlowerPot.SetActive(true);

        if (brokenFlowerPot != null)
            brokenFlowerPot.SetActive(false);

        if (houseKey != null)
            houseKey.SetActive(false);

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
        // CRITICAL FIX: Sync with save data every frame
        SyncStateWithSave();

        CheckPlayerDistance();

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

        if (playerInRange && !waitingForResponse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsTappedOn())
                {
                    if (!hasBeenExamined)
                    {
                        FirstExamine();
                    }
                    else if (hasBeenExamined && !hasBeenBroken)
                    {
                        AskToBreakPot();
                    }
                }
            }
        }

        // Handle key pickup
        if (playerInRange && keyRevealed && houseKey != null && houseKey.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsTappedOn())
                {
                    PickupKey();
                }
            }
        }
    }

    // NEW: Sync state with SAVE DATA
    void SyncStateWithSave()
    {
        if (SaveSystem.Instance == null || InventoryManager.Instance == null) return;

        // Check if key is in inventory
        bool hasKeyInInventory = InventoryManager.Instance.HasItem(HOUSE_KEY_ID);

        // Sync examined state from save
        hasBeenExamined = SaveSystem.Instance.WasObjectExamined(FLOWERPOT_EXAMINED_ID);

        // Sync broken state from save
        bool shouldBeBroken = SaveSystem.Instance.WasObjectExamined(FLOWERPOT_BROKEN_ID);

        if (shouldBeBroken)
        {
            hasBeenBroken = true;

            // Update visuals
            if (intactFlowerPot != null)
                intactFlowerPot.SetActive(false);

            if (brokenFlowerPot != null)
                brokenFlowerPot.SetActive(true);

            // Handle key based on inventory
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
            // Not broken - reset to intact
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

    void FirstExamine()
    {
        // NEW: Check if player has read the mail first
        if (InventoryManager.Instance != null && !InventoryManager.Instance.HasItem(MAIL_ITEM_ID))
        {
            ShowDialogue("It's just an ordinary flower pot. Nothing special about it.");
            return;
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

        if (breakingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(breakingSound);
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

            if (keyFoundSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(keyFoundSound);
            }

            ShowDialogue("A house key! This must unlock the front door.");
        }
    }

    void PickupKey()
    {
        if (!keyRevealed || houseKey == null || !houseKey.activeSelf) return;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(HOUSE_KEY_ID);
        }

        houseKey.SetActive(false);

        ShowDialogue("I picked up the house key. Now I can unlock the front door.");

        Debug.Log("House key added to inventory!");
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