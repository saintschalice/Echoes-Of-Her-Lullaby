using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PuzzleInteractable : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    public string puzzleId;
    public List<string> requiredItemIds = new List<string>();
    public bool consumeItemsOnUse = true;
    public bool canUseMultipleTimes = false;

    [Header("Interaction")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Feedback")]
    public GameObject interactionPrompt;
    public string puzzleCompleteName = "Puzzle";
    public string missingItemsMessage = "You need the right items to solve this.";
    public string alreadySolvedMessage = "This puzzle has already been solved.";
    public string successMessage = "Puzzle solved!";

    [Header("Audio")]
    public AudioClip useItemSound;
    public AudioClip puzzleCompleteSound;
    public AudioClip errorSound;

    // Events
    public System.Action<string> OnPuzzleSolved;
    public System.Action<string, string> OnItemUsed; // puzzleId, itemId

    private bool isPlayerInRange = false;
    private bool isPuzzleSolved = false;
    private Transform playerTransform;

    void Start()
    {
        // Check if puzzle was already solved
        if (SaveSystem.Instance != null)
        {
            isPuzzleSolved = SaveSystem.Instance.IsPuzzleSolved(puzzleId);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        CheckPlayerProximity();

        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void CheckPlayerProximity()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool wasInRange = isPlayerInRange;
        isPlayerInRange = distance <= interactionRange;

        if (isPlayerInRange != wasInRange)
        {
            UpdateInteractionPrompt();
        }
    }

    void UpdateInteractionPrompt()
    {
        if (interactionPrompt == null) return;

        if (isPlayerInRange && CanInteract())
        {
            interactionPrompt.SetActive(true);
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
    }

    public bool CanInteract()
    {
        // Can't interact if already solved (unless multiple uses allowed)
        if (isPuzzleSolved && !canUseMultipleTimes)
        {
            return false;
        }

        return true;
    }

    public void TryInteract()
    {
        if (!CanInteract())
        {
            if (isPuzzleSolved)
            {
                ShowMessage(alreadySolvedMessage);
                PlaySound(errorSound);
            }
            return;
        }

        // Check if player has required items
        if (!HasRequiredItems())
        {
            ShowMissingItemsMessage();
            PlaySound(errorSound);
            return;
        }

        // Use items and solve puzzle
        UsePuzzleItems();
    }

    bool HasRequiredItems()
    {
        if (InventoryManager.Instance == null) return false;

        return requiredItemIds.All(itemId => InventoryManager.Instance.HasItem(itemId));
    }

    void ShowMissingItemsMessage()
    {
        if (requiredItemIds.Count == 0)
        {
            ShowMessage(missingItemsMessage);
            return;
        }

        // Show specific missing items
        List<string> missingItems = new List<string>();

        foreach (string itemId in requiredItemIds)
        {
            if (!InventoryManager.Instance.HasItem(itemId))
            {
                InventoryItem item = InventoryManager.Instance.itemDatabase?.GetItem(itemId);
                string itemName = item?.itemName ?? itemId;
                missingItems.Add(itemName);
            }
        }

        if (missingItems.Count > 0)
        {
            string message = $"You need: {string.Join(", ", missingItems)}";
            ShowMessage(message);
        }
        else
        {
            ShowMessage(missingItemsMessage);
        }
    }

    void UsePuzzleItems()
    {
        // Consume required items if specified
        if (consumeItemsOnUse)
        {
            foreach (string itemId in requiredItemIds)
            {
                InventoryManager.Instance?.RemoveItem(itemId);
                OnItemUsed?.Invoke(puzzleId, itemId);
            }
        }

        // Mark puzzle as solved
        isPuzzleSolved = true;
        SaveSystem.Instance?.MarkPuzzleSolved(puzzleId);

        // Play sounds
        PlaySound(useItemSound);
        PlaySound(puzzleCompleteSound);

        // Show success message
        ShowMessage(successMessage);

        // Trigger events
        OnPuzzleSolved?.Invoke(puzzleId);

        // Handle puzzle-specific behavior
        HandlePuzzleCompletion();

        Debug.Log($"Puzzle solved: {puzzleId}");
    }

    // Override this in derived classes for specific puzzle behavior
    protected virtual void HandlePuzzleCompletion()
    {
        // Default behavior - you can override this in specific puzzle scripts

        // Example: Open a door
        // Example: Activate an object
        // Example: Trigger a cutscene
        // Example: Spawn new items

        Debug.Log($"Puzzle {puzzleId} completed! Override HandlePuzzleCompletion() for custom behavior.");
    }

    // For inventory system to check if item can be used here
    public bool CanUseItem(string itemId)
    {
        return CanInteract() && requiredItemIds.Contains(itemId);
    }

    // For inventory system to use specific item
    public bool UseItem(string itemId)
    {
        if (!CanUseItem(itemId)) return false;

        // Check if this completes the puzzle
        List<string> stillNeeded = requiredItemIds.Where(id =>
            id != itemId && !InventoryManager.Instance.HasItem(id)).ToList();

        if (stillNeeded.Count == 0)
        {
            // This item completes the puzzle
            UsePuzzleItems();
            return true;
        }
        else
        {
            // Partial completion - consume this item but don't solve yet
            if (consumeItemsOnUse)
            {
                InventoryManager.Instance?.RemoveItem(itemId);
                OnItemUsed?.Invoke(puzzleId, itemId);
            }

            PlaySound(useItemSound);
            ShowMessage($"Used {itemId}. Still need: {string.Join(", ", stillNeeded)}");
            return true;
        }
    }

    void ShowMessage(string message)
    {
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(message, "System");
        }
        else
        {
            Debug.Log($"[{puzzleCompleteName}] {message}");
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.PlayOneShot(clip);
    }

    // Public getters
    public bool IsSolved => isPuzzleSolved;
    public List<string> RequiredItems => requiredItemIds;
    public string PuzzleId => puzzleId;

    void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = isPuzzleSolved ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // Draw connection to player if in range
        if (isPlayerInRange && playerTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
    }
}