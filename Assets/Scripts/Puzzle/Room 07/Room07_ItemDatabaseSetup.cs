using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper script to add all Room 07 items to the ItemDatabase
/// Attach to any GameObject and click "Add Room 07 Items" in Inspector
/// </summary>
public class Room07_ItemDatabaseSetup : MonoBehaviour
{
    public ItemDatabase database;

#if UNITY_EDITOR
    [ContextMenu("Add Room 07 Items to Database")]
    public void AddRoom07Items()
    {
        if (database == null)
        {
            Debug.LogError("ItemDatabase not assigned!");
            return;
        }

        Debug.Log("Adding Room 07 items to database...");

        // Emily's Cup
        AddOrUpdateItem(
            "emily_cup",
            "Emily's Cup",
            "A small porcelain cup with delicate floral patterns. It feels cold to the touch. This was Emily's favorite cup for their tea parties.",
            null, // Assign sprite manually in Inspector
            isKeyItem: true,
            requiredForPuzzle: "tea_party"
        );

        // Emily Doll
        AddOrUpdateItem(
            "emily_doll",
            "Emily Doll",
            "A handmade doll with button eyes and yarn hair. The note attached reads: 'Dear Emily, thank you for making mommy stop hurting me yesterday.'",
            null,
            isKeyItem: true,
            requiredForPuzzle: "dollhouse",
            triggersMemory: true,
            memoryFragmentId: "memory_doll"
        );

        // Diary Page 5 (Lisa's Bedroom)
        AddOrUpdateItem(
            "diary_page_5",
            "Diary Page 5",
            "Lisa's diary entry: 'Emily came to me again last night. She sang the pretty song and made the scary dreams go away. I wish she could stay forever.'",
            null,
            isKeyItem: true,
            triggersMemory: true,
            memoryFragmentId: "memory_diary_5"
        );

        // Lullaby Fragment 3
        AddOrUpdateItem(
            "lullaby_fragment_3",
            "Lullaby Fragment 3",
            "A haunting melody fragment. The music box in the toy chest plays this tune. It triggers a memory of someone tucking young Lisa into bed, singing softly.",
            null,
            isKeyItem: true,
            triggersMemory: true,
            memoryFragmentId: "memory_lullaby_3"
        );

        // Bedroom Key (if needed for other rooms)
        AddOrUpdateItem(
            "bedroom_key",
            "Bedroom Key",
            "An old brass key with a tag labeled 'Lisa's Room'. The metal is tarnished but the key still works.",
            null,
            isKeyItem: true,
            requiredForPuzzle: "bedroom_door"
        );

        // Fairy Tale Book (optional collectible)
        AddOrUpdateItem(
            "fairy_tale_book",
            "Fairy Tale Book",
            "A worn children's book. A note inside reads: 'Emily likes the stories where the princess gets saved.' Several pages are bookmarked.",
            null,
            isKeyItem: false,
            triggersMemory: true,
            memoryFragmentId: "memory_fairy_tales"
        );

        // Emily's Chair Note (optional collectible)
        AddOrUpdateItem(
            "emily_chair_note",
            "Emily's Chair Note",
            "A small note attached to a child's chair: 'Emily's Chair - Do Not Sit.' The chair is always cold to the touch.",
            null,
            isKeyItem: false
        );

        // Closet Scratches Photo (optional collectible)
        AddOrUpdateItem(
            "closet_scratches",
            "Closet Scratches Photo",
            "A photo of deep scratches inside the closet. They look like they were made by small fingers. Lisa hid here often when she was scared.",
            null,
            isKeyItem: false,
            triggersMemory: true,
            memoryFragmentId: "memory_closet"
        );

        // Wall Drawing (optional collectible)
        AddOrUpdateItem(
            "wall_drawing",
            "Wall Drawing",
            "A crayon drawing showing two figures holding hands - one labeled 'Me' and another labeled 'Emily'. They're playing together under a smiling sun.",
            null,
            isKeyItem: false,
            triggersMemory: true,
            memoryFragmentId: "memory_drawing"
        );

        // Bed Note (optional collectible)
        AddOrUpdateItem(
            "bed_note",
            "Bed Note",
            "A note pinned to the bed: 'For my friend Emily - she keeps me safe at night.' The bed has two pillow indentations.",
            null,
            isKeyItem: false
        );

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log("✅ Room 07 items added to database successfully!");
        Debug.Log("📝 Remember to assign sprites/icons in the ItemDatabase Inspector!");
    }

    void AddOrUpdateItem(
        string itemId,
        string itemName,
        string description,
        Sprite icon,
        bool isKeyItem = false,
        string requiredForPuzzle = "",
        bool triggersMemory = false,
        string memoryFragmentId = ""
    )
    {
        // Check if item already exists
        InventoryItem existingItem = database.GetItem(itemId);

        if (existingItem != null)
        {
            // Update existing item
            existingItem.itemName = itemName;
            existingItem.description = description;
            if (icon != null) existingItem.itemIcon = icon;
            existingItem.isKeyItem = isKeyItem;
            existingItem.requiredForPuzzle = requiredForPuzzle;
            existingItem.triggersMemory = triggersMemory;
            existingItem.memoryFragmentId = memoryFragmentId;

            Debug.Log($"Updated: {itemName} ({itemId})");
        }
        else
        {
            // Create new item
            InventoryItem newItem = new InventoryItem
            {
                itemId = itemId,
                itemName = itemName,
                description = description,
                itemIcon = icon,
                isKeyItem = isKeyItem,
                isUsable = true,
                isConsumable = false,
                requiredForPuzzle = requiredForPuzzle,
                triggersMemory = triggersMemory,
                memoryFragmentId = memoryFragmentId
            };

            database.allItems.Add(newItem);
            Debug.Log($"Added: {itemName} ({itemId})");
        }
    }
#endif
}
