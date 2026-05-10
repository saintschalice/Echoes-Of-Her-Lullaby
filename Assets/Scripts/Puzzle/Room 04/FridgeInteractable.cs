using UnityEngine;

public class FridgeInteractable : KitchenBaseInteractable
{
    [Header("Fridge Items")]
    public string eggItemId = "egg";
    public string chocolateItemId = "chocolate";

    // FIX: Changed from 'protected' to 'public'
    public override void Interact()
    {
        // 1. Check if collected
        if (isCollected)
        {
            ShowDialogue("Nothing useful left in here.");
            return;
        }

        // 2. Check Recipe Status
        bool recipeRead = false;
        if (KitchenRoomController.Instance != null)
        {
            recipeRead = KitchenRoomController.Instance.recipeRead;
        }

        if (!recipeRead)
        {
            // Pre-Recipe Dialogue
            ShowDialogue("Ugh... It's nasty in here. There's a couple of things that are probably rotten.");
        }
        else
        {
            // Post-Recipe Collection
            ShowDialogue("There's eggs here... and some chocolate. I bet they're inedible.");

            // FIX: Use AddItemWithNotification instead of AddItem
            // Give both items with notifications
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItemWithNotification(eggItemId, "Rotten eggs from the fridge.");
                // Small delay between notifications
                StartCoroutine(AddSecondItemAfterDelay(chocolateItemId));
            }

            // Notify Controller for both
            NotifyKitchenController(eggItemId);
            NotifyKitchenController(chocolateItemId);

            // Mark collected
            isCollected = true;
            if (SaveSystem.Instance != null)
            {
                // Save Logic duplicated from Base to avoid disabling renderer
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                RoomState state = SaveSystem.Instance.GetRoomState(sceneName);
                if (!state.collectedItems.Contains(objectId))
                {
                    state.collectedItems.Add(objectId);
                    SaveSystem.Instance.UpdateRoomState(sceneName, state);
                }
            }
            // DO NOT call DisableVisuals() for the fridge, assuming it's a large static object.
        }
    }

    System.Collections.IEnumerator AddSecondItemAfterDelay(string itemId)
    {
        // Wait for first notification to finish
        while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItemWithNotification(itemId, "Moldy chocolate from the fridge.");
        }
    }

    protected override void OnAlreadyCollected()
    {
        isCollected = true;
        // Do NOT disable visuals for the fridge
    }
}