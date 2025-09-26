using UnityEngine;

using UnityEngine;

public class AddHouseKeys: MonoBehaviour
{
    void Update()
    {
        // Debug input detection
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I KEY PRESSED - Inventory should toggle");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("TAB KEY PRESSED - Inventory should toggle");
        }

        // Test manual item add with keyboard input
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddHouseKey();
        }

        // Test inventory toggle
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("=== INVENTORY TOGGLE TEST ===");

            InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.ToggleInventory();
                Debug.Log($"Inventory toggled. IsOpen: {inventoryUI.IsOpen}");
            }
            else
            {
                Debug.Log("ERROR: InventoryUI not found!");
            }
        }

        // Debug key pickup detection
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("=== KEY PICKUP DEBUG ===");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.Log("ERROR: No Player tagged object found!");
                return;
            }

            ItemPickup[] pickups = FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
            Debug.Log($"Found {pickups.Length} ItemPickup objects");

            foreach (ItemPickup pickup in pickups)
            {
                float distance = Vector3.Distance(player.transform.position, pickup.transform.position);
                Debug.Log($"Pickup '{pickup.GetItemId()}' distance: {distance:F2} (range: {pickup.interactionRange})");

                if (distance <= pickup.interactionRange)
                {
                    Debug.Log($"Player IS in range of {pickup.GetItemId()}!");
                }
                else
                {
                    Debug.Log($"Player NOT in range of {pickup.GetItemId()}");
                }
            }
        }

        // Debug layers and tags
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("=== LAYER/TAG DEBUG ===");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log($"Player found: {player.name}, Layer: {LayerMask.LayerToName(player.layer)} ({player.layer})");
            }
            else
            {
                Debug.Log("ERROR: No GameObject with 'Player' tag found!");

                // Look for possible player objects
                JoystickPlayerController[] controllers = FindObjectsByType<JoystickPlayerController>(FindObjectsSortMode.None);
                Debug.Log($"Found {controllers.Length} JoystickPlayerController objects:");

                foreach (var controller in controllers)
                {
                    Debug.Log($"- {controller.name}, Tag: '{controller.tag}', Layer: {LayerMask.LayerToName(controller.gameObject.layer)}");
                }
            }
        }

        // Show all managers and systems
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("=== SYSTEM CHECK ===");
            Debug.Log($"InventoryManager.Instance: {(InventoryManager.Instance != null ? "CHECK" : "NULL")}");
            Debug.Log($"DialogueSystemV2.Instance: {(DialogueSystemV2.Instance != null ? "CHECK" : "NULL")}");
            Debug.Log($"SaveSystem.Instance: {(SaveSystem.Instance != null ? "CHECK" : "NULL")}");

            InventoryUI ui = FindFirstObjectByType<InventoryUI>();
            Debug.Log($"InventoryUI found: {(ui != null ? "CHECK" : "NULL")}");

            if (ui != null)
            {
                Debug.Log($"InventoryUI IsOpen: {ui.IsOpen}");
                Debug.Log($"InventoryUI IsAnimating: {ui.IsAnimating}");
            }
        }

        // Debug button click detection
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MOUSE CLICKED - Check if inventory button was clicked");
        }
    }

    // This is the new public method that can be called by a UI Button.
    public void AddHouseKey()
    {
        Debug.Log("=== MANUAL ITEM ADD TEST ===");

        if (InventoryManager.Instance != null)
        {
            bool success = InventoryManager.Instance.AddItem("house_key");
            Debug.Log($"Added house_key: {success}");
        }
        else
        {
            Debug.Log("ERROR: InventoryManager.Instance is NULL!");
        }
    }
}