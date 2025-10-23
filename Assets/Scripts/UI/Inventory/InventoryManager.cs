using UnityEngine;
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

    // Item IDs for mail transformation
    private const string MAIL_ITEM_ID = "foyer_mail";
    private const string LETTER_ITEM_ID = "foyer_letter";

    public static InventoryManager Instance { get; private set; }

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

        // NEW: Handle mail -> letter transformation
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
        // Ensure both items exist
        if (!HasItem(itemAId) || !HasItem(itemBId))
        {
            Debug.LogWarning($"[InventoryManager] CombineItems failed - missing items: {itemAId}, {itemBId}");
            return false;
        }

        // Remove both items from SaveSystem
        SaveSystem.Instance?.RemoveInventoryItem(itemAId);
        SaveSystem.Instance?.RemoveInventoryItem(itemBId);

        // Add the combined result
        InventoryItem combinedItem = itemDatabase?.GetItem(resultItemId);
        if (combinedItem == null)
        {
            Debug.LogError($"[InventoryManager] Result item not found in database: {resultItemId}");
            return false;
        }

        SaveSystem.Instance?.AddInventoryItem(resultItemId);

        Debug.Log($"[InventoryManager] Combined {itemAId} + {itemBId} → {resultItemId}");

        // Refresh inventory UI
        RefreshUI();

        // Trigger any UI or logic listeners
        OnItemAdded?.Invoke(combinedItem);

        // Optional: play pickup or success sound
        PlaySound(itemPickupSound);

        return true;
    }




    public bool CombineMultipleItems(string[] itemIds, string resultItemId)
    {
        // Check if player has all items
        foreach (string itemId in itemIds)
        {
            if (!HasItem(itemId))
            {
                Debug.Log($"[InventoryManager] Missing item: {itemId}");
                return false;
            }
        }

        // Remove all source items
        foreach (string itemId in itemIds)
        {
            SaveSystem.Instance?.RemoveInventoryItem(itemId);
        }

        // Add combined item
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


    public void AddItemAndSave(string itemId)
    {
        AddItem(itemId);
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.AddInventoryItem(itemId);
        }
    }


    // NEW: Transform mail into readable letter
    bool TransformMailToLetter()
    {
        Debug.Log("[InventoryManager] Transforming mail into letter...");

        // Remove mail from inventory
        SaveSystem.Instance?.RemoveInventoryItem(MAIL_ITEM_ID);

        // Add letter to inventory
        SaveSystem.Instance?.AddInventoryItem(LETTER_ITEM_ID);

        // Refresh UI to show new item
        RefreshUI();

        // ✅ Open the mail UI immediately
        MailReaderUI mailReader = FindFirstObjectByType<MailReaderUI>();
        if (mailReader != null)
        {
            mailReader.OpenMail();
        }
        else
        {
            Debug.LogWarning("[InventoryManager] MailReaderUI not found in the scene.");
        }

        return true;
    }

    bool HandleItemUsage(InventoryItem item)
    {
        // ✅ If player uses the readable letter
        if (item.itemId == LETTER_ITEM_ID)
        {
            MailReaderUI mailReader = FindFirstObjectByType<MailReaderUI>();
            if (mailReader != null)
            {
                mailReader.OpenMail();
                return true;
            }
        }

        // Handle memory fragments
        if (item.triggersMemory && !string.IsNullOrEmpty(item.memoryFragmentId))
        {
            if (!SaveSystem.Instance.HasMemoryFragment(item.memoryFragmentId))
            {
                SaveSystem.Instance?.AddMemoryFragment(item.memoryFragmentId);
                TriggerMemorySequence(item);
                return true;
            }
        }

        // Handle puzzle-related items
        if (!string.IsNullOrEmpty(item.requiredForPuzzle))
        {
            return TryUsePuzzleItem(item);
        }

        // Default: show item description
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

        Debug.Log($"Memory fragment triggered: {item.memoryFragmentId}");
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

    public int GetItemCount()
    {
        return GetAllItems().Count;
    }

    public List<InventoryItem> GetKeyItems()
    {
        return GetAllItems().Where(item => item.isKeyItem).ToList();
    }

    public List<InventoryItem> GetRegularItems()
    {
        return GetAllItems().Where(item => !item.isKeyItem).ToList();
    }

    public bool HasAnyItems()
    {
        return GetItemCount() > 0;
    }

    public bool HasRequiredItems(List<string> requiredItemIds)
    {
        return requiredItemIds.All(itemId => HasItem(itemId));
    }

    [ContextMenu("Debug Add Test Item")]
    void DebugAddTestItem()
    {
        AddItem("house_key");
    }

    [ContextMenu("Debug Print Inventory")]
    void DebugPrintInventory()
    {
        var items = GetAllItems();
        Debug.Log($"Inventory contains {items.Count} items:");
        foreach (var item in items)
        {
            Debug.Log($"- {item.itemName} ({item.itemId})");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}