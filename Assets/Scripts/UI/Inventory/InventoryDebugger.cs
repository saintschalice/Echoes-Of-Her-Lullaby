using UnityEngine;

public class InventoryDebugger : MonoBehaviour
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

        // Test manual item add
        if (Input.GetKeyDown(KeyCode.G))
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
            Debug.Log($"InventoryManager.Instance: {(InventoryManager.Instance != null ? "✓" : "NULL")}");
            Debug.Log($"DialogueSystemV2.Instance: {(DialogueSystemV2.Instance != null ? "✓" : "NULL")}");
            Debug.Log($"SaveSystem.Instance: {(SaveSystem.Instance != null ? "✓" : "NULL")}");

            InventoryUI ui = FindFirstObjectByType<InventoryUI>();
            Debug.Log($"InventoryUI found: {(ui != null ? "✓" : "NULL")}");

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

    void OnGUI()
    {
        // Show debug instructions on screen
        GUI.Box(new Rect(10, 10, 300, 170), "INVENTORY DEBUG KEYS:");
        GUI.Label(new Rect(20, 35, 280, 20), "G = Add house_key manually");
        GUI.Label(new Rect(20, 55, 280, 20), "H = Toggle inventory manually");
        GUI.Label(new Rect(20, 75, 280, 20), "J = Debug key pickup detection");
        GUI.Label(new Rect(20, 95, 280, 20), "K = Check player layer/tag");
        GUI.Label(new Rect(20, 115, 280, 20), "L = Check all systems");
        GUI.Label(new Rect(20, 135, 280, 20), "I/Tab should work but debug above");
        GUI.Label(new Rect(20, 155, 280, 20), "Watch Console for results!");
    }
}