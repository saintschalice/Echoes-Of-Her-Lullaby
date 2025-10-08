using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper script to setup required items in ItemDatabase
/// Attach this to a GameObject and click "Setup Required Items" in inspector
/// </summary>
public class ItemDatabaseSetupHelper : MonoBehaviour
{
    public ItemDatabase database;

#if UNITY_EDITOR
    [ContextMenu("Setup Required Items")]
    public void SetupRequiredItems()
    {
        if (database == null)
        {
            Debug.LogError("ItemDatabase not assigned!");
            return;
        }

        // Check if items already exist
        bool houseKeyExists = database.ItemExists("house_key");
        bool mailExists = database.ItemExists("foyer_mail");

        if (!houseKeyExists)
        {
            InventoryItem houseKey = new InventoryItem
            {
                itemId = "house_key",
                itemName = "House Key",
                description = "An old brass key found in the broken flower pot. It should unlock the front door.",
                isKeyItem = true,
                isUsable = true,
                isConsumable = false,
                triggersMemory = false,
                requiredForPuzzle = "front_door"
            };

            database.allItems.Add(houseKey);
            Debug.Log("Added House Key to database");
        }

        if (!mailExists)
        {
            InventoryItem mail = new InventoryItem
            {
                itemId = "foyer_mail",
                itemName = "Mysterious Letter",
                description = "A letter from the mailbox. It mentions: 'The flowers hide more than beauty. Break the surface to reveal what lies beneath.'",
                isKeyItem = true,
                isUsable = true,
                isConsumable = false,
                triggersMemory = false,
                requiredForPuzzle = ""
            };

            database.allItems.Add(mail);
            Debug.Log("Added Mysterious Letter to database");
        }

        // Mark database as dirty to save changes
        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log("ItemDatabase setup complete!");
    }
#endif
}