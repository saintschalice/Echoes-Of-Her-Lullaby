using UnityEngine;

/// <summary>
/// Handles Vanilla, Sugar, and Flour cabinets.
/// Replaces separate classes by using inspector fields.
/// </summary>
public class KitchenCabinetInteractable : KitchenBaseInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("The ID of the item to give (e.g., 'vanilla', 'sugar', 'flour')")]
    public string ingredientItemId;

    [TextArea]
    public string foundDialogue;

    // FIX: Changed from 'protected' to 'public'
    public override void Interact()
    {
        // 1. Check if collected
        if (isCollected)
        {
            ShowDialogue("Nothing left here.");
            return;
        }

        // 2. Check if Recipe Read (Gate)
        bool recipeRead = false;
        if (KitchenRoomController.Instance != null)
        {
            recipeRead = KitchenRoomController.Instance.recipeRead;
        }

        // Redundant check in case the Phase 6 manager hasn't hidden this yet,
        // or if we want to give feedback if clicked too early.
        if (!recipeRead)
        {
            // If the object is visible but recipe not read, we can show generic text
            ShowDialogue("Just some old kitchen cabinets.");
            return;
        }

        // 3. Collection Logic
        ShowDialogue(foundDialogue);
        AddItemToInventory(ingredientItemId);
        NotifyKitchenController(ingredientItemId);
        MarkAsCollected();
    }
}