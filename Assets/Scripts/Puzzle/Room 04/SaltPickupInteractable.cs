using UnityEngine;

public class SaltPickupInteractable : SimpleInteractable2D
{
    private const string ITEM_ID = "salt";

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(interactionDialogue))
            interactionDialogue = "Some salt left on the counter.";

        // Check if already collected
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(ITEM_ID))
        {
            gameObject.SetActive(false);
        }
    }

    // This runs AFTER the player closes the dialogue box
    protected override void OnDialogueEnded()
    {
        base.OnDialogueEnded(); // Clean up event listener

        Debug.Log("[Salt] picking up salt...");

        // 1. Add to Inventory / Save System
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(ITEM_ID);
        }

        // 2. Notify Room Controller
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnIngredientCollected(ITEM_ID);
        }

        // 3. Remove Sprite from World
        gameObject.SetActive(false);
    }
}