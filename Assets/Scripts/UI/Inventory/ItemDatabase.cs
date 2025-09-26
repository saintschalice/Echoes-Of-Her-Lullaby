using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [Header("All Game Items")]
    public List<InventoryItem> allItems = new List<InventoryItem>();

    private Dictionary<string, InventoryItem> itemLookup;

    void OnEnable()
    {
        BuildLookupDictionary();
    }

    void BuildLookupDictionary()
    {
        itemLookup = new Dictionary<string, InventoryItem>();
        foreach (var item in allItems)
        {
            if (!string.IsNullOrEmpty(item.itemId))
            {
                itemLookup[item.itemId] = item;
            }
        }
    }

    public InventoryItem GetItem(string itemId)
    {
        if (itemLookup == null)
            BuildLookupDictionary();

        return itemLookup.ContainsKey(itemId) ? itemLookup[itemId] : null;
    }

    public List<InventoryItem> GetItemsByCategory(bool keyItemsOnly)
    {
        return allItems.Where(item => item.isKeyItem == keyItemsOnly).ToList();
    }

    public bool ItemExists(string itemId)
    {
        if (itemLookup == null)
            BuildLookupDictionary();

        return itemLookup.ContainsKey(itemId);
    }

    // Add item to database (for runtime additions)
    public void AddItem(InventoryItem newItem)
    {
        if (!allItems.Any(item => item.itemId == newItem.itemId))
        {
            allItems.Add(newItem);
            BuildLookupDictionary();
        }
    }

    // Validation helper
    [ContextMenu("Validate Database")]
    void ValidateDatabase()
    {
        var duplicateIds = allItems.GroupBy(item => item.itemId)
                                  .Where(group => group.Count() > 1)
                                  .Select(group => group.Key);

        foreach (string duplicateId in duplicateIds)
        {
            Debug.LogError($"Duplicate item ID found: {duplicateId}");
        }

        foreach (var item in allItems)
        {
            if (string.IsNullOrEmpty(item.itemId))
                Debug.LogError($"Item with empty ID: {item.itemName}");

            if (item.itemIcon == null)
                Debug.LogWarning($"Item missing icon: {item.itemName} ({item.itemId})");
        }
    }
}