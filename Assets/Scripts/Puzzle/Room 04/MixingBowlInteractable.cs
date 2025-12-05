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
        ShowDialogue("Let's mix these together...");
        yield return new WaitForSeconds(1.0f);

        if (InventoryManager.Instance != null)
        {
            foreach (string id in ingredientIds) InventoryManager.Instance.RemoveItem(id);
            if (InventoryManager.Instance.HasItem(emptyBowlId)) InventoryManager.Instance.RemoveItem(emptyBowlId);
            InventoryManager.Instance.AddItem(mixedBowlId);
        }

        if (KitchenRoomController.Instance != null)
            KitchenRoomController.Instance.OnDoughMixed();

        MarkAsCollected();
        ShowDialogue("Looks like good cookie dough.");
    }
}