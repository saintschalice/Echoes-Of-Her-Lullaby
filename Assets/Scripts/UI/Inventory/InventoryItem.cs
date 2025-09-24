using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [Header("Basic Info")]
    public string itemId;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite itemIcon;

    [Header("Item Properties")]
    public bool isKeyItem = false; // Important quest items
    public bool isUsable = true;   // Can be used/examined
    public bool isConsumable = false; // Gets removed after use

    [Header("Story Integration")]
    public bool triggersMemory = false;
    public string memoryFragmentId = "";
    public string requiredForPuzzle = ""; // Which puzzle this item is needed for

    public InventoryItem(string id, string name, string desc, Sprite icon)
    {
        itemId = id;
        itemName = name;
        description = desc;
        itemIcon = icon;
    }

    public InventoryItem() { } // Default constructor for serialization
}