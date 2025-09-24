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
        // Find UI if not assigned
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>();
        }

        // Load inventory from save system
        LoadInventoryFromSave();
    }

    void LoadInventoryFromSave()
    {
        if (SaveSystem.Instance == null) return;

        GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();
        if (saveData != null && saveData.inventoryItems != null)
        {
            // Inventory is loaded from save system automatically
            // Just refresh the UI
            RefreshUI();
        }
    }

    public List<InventoryItem> GetAllItems()
    {
        Debug.Log($"[InventoryManager] GetAllItems called");
        Debug.Log($"[InventoryManager] SaveSystem.Instance: {(SaveSystem.Instance != null ? "CHECK" : "NULL")}");
        Debug.Log($"[InventoryManager] itemDatabase: {(itemDatabase != null ? "CHECK" : "NULL")}");

        if (SaveSystem.Instance == null || itemDatabase == null)
            return new List<InventoryItem>();

        GameSaveData saveData = SaveSystem.Instance.GetCurrentSaveData();
        Debug.Log($"[InventoryManager] saveData: {(saveData != null ? "CHECK" : "NULL")}");
        Debug.Log($"[InventoryManager] saveData.inventoryItems count: {(saveData?.inventoryItems?.Count ?? 0)}");

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

        // Check if already have the item (for non-stackable items)
        if (HasItem(itemId) && !item.isConsumable)
        {
            Debug.Log($"Already have item: {item.itemName}");
            return false;
        }

        // Add to save system
        SaveSystem.Instance?.AddInventoryItem(itemId);

        // Play pickup sound
        PlaySound(itemPickupSound);

        // Trigger events
        OnItemAdded?.Invoke(item);

        // Handle memory fragments
        if (item.triggersMemory && !string.IsNullOrEmpty(item.memoryFragmentId))
        {
            SaveSystem.Instance?.AddMemoryFragment(item.memoryFragmentId);
            PlaySound(memoryTriggerSound);
            TriggerMemorySequence(item);
        }

        // Refresh UI
        RefreshUI();

        Debug.Log($"Added item to inventory: {item.itemName}");
        return true;
    }

    public bool RemoveItem(string itemId)
    {
        if (!HasItem(itemId)) return false;

        InventoryItem item = GetItem(itemId);

        // Remove from save system
        SaveSystem.Instance?.RemoveInventoryItem(itemId);

        // Trigger events
        OnItemRemoved?.Invoke(item);

        // Refresh UI
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

        // Play use sound
        PlaySound(itemUseSound);

        // Handle item usage based on type
        bool wasUsed = HandleItemUsage(item);

        if (wasUsed)
        {
            // Trigger events
            OnItemUsed?.Invoke(item);

            // Remove if consumable
            if (item.isConsumable)
            {
                RemoveItem(itemId);
            }

            Debug.Log($"Used item: {item.itemName}");
        }

        return wasUsed;
    }

    bool HandleItemUsage(InventoryItem item)
    {
        // Handle memory trigger items
        if (item.triggersMemory && !string.IsNullOrEmpty(item.memoryFragmentId))
        {
            if (!SaveSystem.Instance.HasMemoryFragment(item.memoryFragmentId))
            {
                SaveSystem.Instance?.AddMemoryFragment(item.memoryFragmentId);
                TriggerMemorySequence(item);
                return true;
            }
        }

        // Handle puzzle items
        if (!string.IsNullOrEmpty(item.requiredForPuzzle))
        {
            // Try to use item with nearby puzzle
            return TryUsePuzzleItem(item);
        }

        // Default: just show description
        ShowItemDescription(item);
        return true;
    }

    bool TryUsePuzzleItem(InventoryItem item)
    {
        // Find nearby puzzle systems that can use this item
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

        // No nearby puzzle found
        ShowItemDescription(item);
        return false;
    }

    void TriggerMemorySequence(InventoryItem item)
    {
        // Hook into your dialogue/memory system
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            // Trigger memory dialogue using the correct method
            string memoryDialogue = $"*Lisa examines the {item.itemName}*\n\n{item.description}";
            dialogueSystem.StartDialogue(memoryDialogue, "Lisa");
        }

        Debug.Log($"Memory fragment triggered: {item.memoryFragmentId}");
    }

    void ShowItemDescription(InventoryItem item)
    {
        // Show item description in dialogue system
        DialogueSystemV2 dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogueSystem != null)
        {
            string description = $"*{item.itemName}*\n\n{item.description}";
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

        // Hook into your audio system
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.PlayOneShot(clip);
    }

    // Public utility methods
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

    // For puzzle systems to check requirements
    public bool HasRequiredItems(List<string> requiredItemIds)
    {
        return requiredItemIds.All(itemId => HasItem(itemId));
    }

    // Debug methods
    [ContextMenu("Debug Add Test Item")]
    void DebugAddTestItem()
    {
        AddItem("house_key"); // Replace with actual item ID from your database
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
        // Draw pickup range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}