using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [Header("Database")]
    public ItemDatabase itemDatabase;

    [Header("UI Reference")]
    public InventoryUI inventoryUI;

    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public LayerMask pickupLayerMask = -1;

    [Header("Audio")]
    public AudioClip itemPickupSound;
    public AudioClip itemUseSound;
    public AudioClip memoryTriggerSound;

    // Events
    public System.Action<InventoryItem> OnItemAdded;
    public System.Action<InventoryItem> OnItemRemoved;
    public System.Action<InventoryItem> OnItemUsed;

    // Item IDs
    private const string MAIL_ITEM_ID = "foyer_mail";
    private const string LETTER_ITEM_ID = "foyer_letter";
    private const string RECIPE_BOOK_ID = "recipe_book_kitchen";

    public static InventoryManager Instance { get; private set; }

    private bool wasInventoryOpenBeforeAction = false;
    private Coroutine reopenCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>();
        }

        LoadInventoryFromSave();
    }

    void LoadInventoryFromSave()
    {
        if (SaveSystem.Instance == null) return;

        GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();
        if (saveData != null && saveData.inventoryItems != null)
        {
            RefreshUI();
        }
    }

    public void NotifyActionStarted()
    {
        // Check if we are currently waiting to reopen.
        bool isPendingReopen = (reopenCoroutine != null);

        if (reopenCoroutine != null)
        {
            StopCoroutine(reopenCoroutine);
            reopenCoroutine = null;
        }

        // Logic Update:
        // 1. If inventory IS open, mark it.
        // 2. If it WAS pending reopen (chained action), mark it.
        // 3. CRITICAL: If wasInventoryOpenBeforeAction is ALREADY true, keep it true. 
        //    (This handles double-taps where the first tap closed it, but we want to remember it was open originally)
        if ((inventoryUI != null && inventoryUI.IsOpen) || isPendingReopen)
        {
            wasInventoryOpenBeforeAction = true;
            inventoryUI.ForceCloseInventory();
        }
        else if (!wasInventoryOpenBeforeAction)
        {
            // Only set to false if it wasn't already true.
            wasInventoryOpenBeforeAction = false;
        }
    }

    public void NotifyActionEnded()
    {
        if (wasInventoryOpenBeforeAction)
        {
            if (reopenCoroutine != null) StopCoroutine(reopenCoroutine);
            reopenCoroutine = StartCoroutine(ReopenInventoryDelay());
        }
    }

    IEnumerator ReopenInventoryDelay()
    {
        // Delay to handle chained events
        yield return new WaitForSeconds(0.1f);

        // CHECK: Is the Diary open?
        // If the Diary is open, we MUST NOT reopen the inventory on top of it.
        // The Diary's own Close() method handles reopening the inventory later if needed.
        if (DiaryReaderUI.Instance != null && DiaryReaderUI.Instance.IsReaderOpen())
        {
            // Reset the flag because the Diary has "taken over" control of the UI state.
            wasInventoryOpenBeforeAction = false;
            reopenCoroutine = null;
            yield break; // Exit immediately
        }

        if (inventoryUI != null)
        {
            inventoryUI.OpenInventory();
        }

        reopenCoroutine = null;
        wasInventoryOpenBeforeAction = false;
    }

    public List<InventoryItem> GetAllItems()
    {
        if (SaveSystem.Instance == null || itemDatabase == null)
            return new List<InventoryItem>();

        GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();

        if (saveData?.inventoryItems == null)
            return new List<InventoryItem>();

        List<InventoryItem> items = new List<InventoryItem>();

        foreach (string itemId in saveData.inventoryItems)
        {
            InventoryItem item = itemDatabase.GetItem(itemId);
            if (item != null)
            {
                items.Add(item);
            }
        }

        return items;
    }

    public bool HasItem(string itemId)
    {
        return SaveSystem.Instance?.HasItem(itemId) ?? false;
    }

    public InventoryItem GetItem(string itemId)
    {
        if (!HasItem(itemId)) return null;
        return itemDatabase?.GetItem(itemId);
    }

    public bool AddItem(string itemId)
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase not assigned to InventoryManager!");
            return false;
        }

        InventoryItem item = itemDatabase.GetItem(itemId);
        if (item == null)
        {
            Debug.LogWarning($"Item not found in database: {itemId}");
            return false;
        }

        if (HasItem(itemId) && !item.isConsumable)
        {
            Debug.Log($"Already have item: {item.itemName}");
            return false;
        }

        SaveSystem.Instance?.AddInventoryItem(itemId);

        PlaySound(itemPickupSound);

        OnItemAdded?.Invoke(item);

        if (item.triggersMemory && !string.IsNullOrEmpty(item.memoryFragmentId))
        {
            SaveSystem.Instance?.AddMemoryFragment(item.memoryFragmentId);
            PlaySound(memoryTriggerSound);
            TriggerMemorySequence(item);
        }

        RefreshUI();

        Debug.Log($"Added item to inventory: {item.itemName}");
        return true;
    }

    public bool RemoveItem(string itemId)
    {
        if (!HasItem(itemId)) return false;

        InventoryItem item = GetItem(itemId);

        SaveSystem.Instance?.RemoveInventoryItem(itemId);

        OnItemRemoved?.Invoke(item);

        RefreshUI();

        Debug.Log($"Removed item from inventory: {item?.itemName}");
        return true;
    }

    public bool UseItem(string itemId)
    {
        InventoryItem item = GetItem(itemId);
        if (item == null) return false;

        if (!item.isUsable)
        {
            Debug.Log($"Item {item.itemName} is not usable");
            return false;
        }

        PlaySound(itemUseSound);

        if (itemId == MAIL_ITEM_ID)
        {
            return TransformMailToLetter();
        }

        bool wasUsed = HandleItemUsage(item);

        if (wasUsed)
        {
            OnItemUsed?.Invoke(item);

            if (item.isConsumable)
            {
                RemoveItem(itemId);
            }

            Debug.Log($"Used item: {item.itemName}");
        }

        return wasUsed;
    }


    public bool CombineItems(string itemAId, string itemBId, string resultItemId)
    {
        if (!HasItem(itemAId) || !HasItem(itemBId))
        {
            Debug.LogWarning($"[InventoryManager] CombineItems failed - missing items: {itemAId}, {itemBId}");
            return false;
        }

        SaveSystem.Instance?.RemoveInventoryItem(itemAId);
        SaveSystem.Instance?.RemoveInventoryItem(itemBId);

        InventoryItem combinedItem = itemDatabase?.GetItem(resultItemId);
        if (combinedItem == null)
        {
            Debug.LogError($"[InventoryManager] Result item not found in database: {resultItemId}");
            return false;
        }

        SaveSystem.Instance?.AddInventoryItem(resultItemId);

        Debug.Log($"[InventoryManager] Combined {itemAId} + {itemBId} -> {resultItemId}");

        RefreshUI();

        OnItemAdded?.Invoke(combinedItem);

        PlaySound(itemPickupSound);

        return true;
    }

    public bool CombineMultipleItems(string[] itemIds, string resultItemId)
    {
        foreach (string itemId in itemIds)
        {
            if (!HasItem(itemId))
            {
                Debug.Log($"[InventoryManager] Missing item: {itemId}");
                return false;
            }
        }

        foreach (string itemId in itemIds)
        {
            SaveSystem.Instance?.RemoveInventoryItem(itemId);
        }

        InventoryItem resultItem = itemDatabase?.GetItem(resultItemId);
        if (resultItem == null)
        {
            Debug.LogError($"[InventoryManager] Result item not found: {resultItemId}");
            return false;
        }

        SaveSystem.Instance?.AddInventoryItem(resultItemId);
        OnItemAdded?.Invoke(resultItem);
        RefreshUI();
        PlaySound(itemPickupSound);

        Debug.Log($"[InventoryManager] Combined {itemIds.Length} items into {resultItemId}");
        return true;
    }

    public void CloseInventoryUI()
    {
        if (inventoryUI != null)
            inventoryUI.ForceCloseInventory();
    }

    public void OpenInventoryUI()
    {
        if (inventoryUI != null)
            inventoryUI.OpenInventory();
    }

    public void AddItemAndSave(string itemId)
    {
        AddItem(itemId);
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddInventoryItem(itemId);
        }
    }


    bool TransformMailToLetter()
    {
        Debug.Log("[InventoryManager] Transforming mail into letter...");

        SaveSystem.Instance?.RemoveInventoryItem(MAIL_ITEM_ID);
        SaveSystem.Instance?.AddInventoryItem(LETTER_ITEM_ID);
        RefreshUI();

        MailReaderUI mailReader = FindFirstObjectByType<MailReaderUI>();
        if (mailReader != null)
        {
            NotifyActionStarted();
            mailReader.OpenMail();
        }

        return true;
    }

    bool HandleItemUsage(InventoryItem item)
    {
        // 1. Recipe Book
        if (item.itemId == RECIPE_BOOK_ID)
        {
            if (RecipeBookUI.Instance != null)
            {
                NotifyActionStarted();
                RecipeBookUI.Instance.OpenBook();
                return true;
            }
        }

        // 2. Letter
        if (item.itemId == LETTER_ITEM_ID)
        {
            MailReaderUI mailReader = FindFirstObjectByType<MailReaderUI>();
            if (mailReader != null)
            {
                NotifyActionStarted();
                mailReader.OpenMail();
                return true;
            }
        }

        // 3. Memory Fragments
        if (item.triggersMemory && !string.IsNullOrEmpty(item.memoryFragmentId))
        {
            if (!SaveSystem.Instance.HasMemoryFragment(item.memoryFragmentId))
            {
                SaveSystem.Instance?.AddMemoryFragment(item.memoryFragmentId);
                TriggerMemorySequence(item);
                return true;
            }
        }

        // 4. Diary
        if (item.itemId == "diary_complete")
        {
            DiaryReaderUI diaryReader = FindFirstObjectByType<DiaryReaderUI>();
            if (diaryReader != null)
            {
                NotifyActionStarted();
                diaryReader.ShowDiary();
                return true;
            }
        }

        // 5. Puzzle Items
        if (!string.IsNullOrEmpty(item.requiredForPuzzle))
        {
            return TryUsePuzzleItem(item);
        }

        // Default
        ShowItemDescription(item);
        return true;
    }


    bool TryUsePuzzleItem(InventoryItem item)
    {
        PuzzleInteractable[] nearbyPuzzles = FindObjectsByType<PuzzleInteractable>(FindObjectsSortMode.None);

        foreach (var puzzle in nearbyPuzzles)
        {
            if (Vector3.Distance(transform.position, puzzle.transform.position) <= pickupRange)
            {
                if (puzzle.CanUseItem(item.itemId))
                {
                    puzzle.UseItem(item.itemId);
                    return true;
                }
            }
        }

        ShowItemDescription(item);
        return false;
    }

    void TriggerMemorySequence(InventoryItem item)
    {
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            string memoryDialogue = $"Lisa examines the {item.itemName}\n\n{item.description}";
            dialogueSystem.StartDialogue(memoryDialogue, "Lisa");
        }
    }

    void ShowItemDescription(InventoryItem item)
    {
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            string description = $"{item.itemName}\n\n{item.description}";
            dialogueSystem.StartDialogue(description, "Lisa");
        }
        else
        {
            Debug.Log($"{item.itemName}: {item.description}");
        }
    }

    void RefreshUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.OnInventoryChanged();
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        AudioManager.Instance?.PlaySFX(clip);
    }
}