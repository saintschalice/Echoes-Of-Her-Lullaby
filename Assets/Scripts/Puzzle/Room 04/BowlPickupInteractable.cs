using UnityEngine;

public class BowlPickupInteractable : SimpleInteractable2D
{
    private const string ITEM_ID = "bowl_empty";

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(interactionDialogue))
            interactionDialogue = "It's a big bowl, but there's nothing inside it.";

        // Check if already collected
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(ITEM_ID))
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnDialogueEnded()
    {
        base.OnDialogueEnded();

        Debug.Log("[Bowl] picking up empty bowl...");

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(ITEM_ID);
        }

        // No specific method in Controller for bowl, but we can rely on InventoryManager
        // or add a generic notification if strictly needed.

        gameObject.SetActive(false);
    }
}