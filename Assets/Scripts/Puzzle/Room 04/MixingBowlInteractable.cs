using UnityEngine;
using System.Collections;

public class MixingBowlInteractable : KitchenBaseInteractable
{
    [Header("Mixing Configuration")]
    public string emptyBowlId = "bowl_empty";
    public string mixedBowlId = "bowl_cookie_mix";

    private readonly string[] ingredientIds = new string[]
    {
        "flour", "sugar", "vanilla", "chocolate", "egg", "salt"
    };

    // FIX: Public override
    public override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;

        if (KitchenRoomController.Instance.doughMixed)
        {
            ShowDialogue("The dough is already ready.");
            return;
        }

        if (!HasAllIngredients())
        {
            ShowDialogue("I don't have everything I need yet.");
            return;
        }

        StartCoroutine(MixRoutine());
    }

    private bool HasAllIngredients()
    {
        var ctrl = KitchenRoomController.Instance;
        return ctrl.hasFlour && ctrl.hasSugar && ctrl.hasVanilla &&
               ctrl.hasChocolate && ctrl.hasEgg && ctrl.hasSalt;
    }

    private IEnumerator MixRoutine()
    {
        // STEP 1: Show initial dialogue
        ShowDialogue("Let's mix these together...");
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f); // Small delay

        // STEP 2: Remove ingredients from inventory
        if (InventoryManager.Instance != null)
        {
            foreach (string id in ingredientIds) InventoryManager.Instance.RemoveItem(id);
            if (InventoryManager.Instance.HasItem(emptyBowlId)) InventoryManager.Instance.RemoveItem(emptyBowlId);
            
            // STEP 3: Add mixed bowl WITH NOTIFICATION
            InventoryManager.Instance.AddItemWithNotification(mixedBowlId, "Cookie dough ready to be baked.");
        }

        // STEP 4: Wait for notification to finish
        if (ItemNotificationUI.Instance != null)
        {
            while (ItemNotificationUI.Instance.IsShowing())
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f); // Small delay after notification

        // STEP 5: Update puzzle state
        if (KitchenRoomController.Instance != null)
            KitchenRoomController.Instance.OnDoughMixed();

        MarkAsCollected();
        
        // STEP 6: Show dialogue AFTER notification
        ShowDialogue("Looks like good cookie dough.");
    }
}