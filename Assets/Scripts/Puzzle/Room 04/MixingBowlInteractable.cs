using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the logic for mixing ingredients into cookie dough.
/// Attached to the Bowl GameObject on the table.
/// </summary>
public class MixingBowlInteractable : KitchenBaseInteractable
{
    [Header("Mixing Configuration")]
    // We don't need emptyBowlId for checking anymore, only for removing if it WAS in inventory (safety)
    public string emptyBowlId = "bowl_empty";
    public string mixedBowlId = "bowl_cookie_mix";

    private readonly string[] ingredientIds = new string[]
    {
        "flour",
        "sugar",
        "vanilla",
        "chocolate",
        "egg",
        "salt"
    };

    // FIX: Changed from 'protected' to 'public'
    public override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;

        // 1. Check if already mixed
        if (KitchenRoomController.Instance.doughMixed)
        {
            ShowDialogue("The dough is already ready.");
            return;
        }

        // 2. Check Ingredients
        // We NO LONGER check for "HasItem(bowl_empty)" because the bowl is physically here.
        if (!HasAllIngredients())
        {
            ShowDialogue("I don't have everything I need yet.");
            return;
        }

        // 3. Success Sequence
        StartCoroutine(MixRoutine());
    }

    private bool HasAllIngredients()
    {
        var ctrl = KitchenRoomController.Instance;
        // Debugging helper: check logs to see what is missing
        if (!ctrl.hasFlour) Debug.Log("Missing Flour");
        if (!ctrl.hasSugar) Debug.Log("Missing Sugar");
        if (!ctrl.hasVanilla) Debug.Log("Missing Vanilla");
        if (!ctrl.hasChocolate) Debug.Log("Missing Chocolate");
        if (!ctrl.hasEgg) Debug.Log("Missing Egg");
        if (!ctrl.hasSalt) Debug.Log("Missing Salt");

        return ctrl.hasFlour &&
               ctrl.hasSugar &&
               ctrl.hasVanilla &&
               ctrl.hasChocolate &&
               ctrl.hasEgg &&
               ctrl.hasSalt;
    }

    private IEnumerator MixRoutine()
    {
        ShowDialogue("Let's mix these together...");

        yield return new WaitForSeconds(1.0f);

        if (InventoryManager.Instance != null)
        {
            // Remove Ingredients
            foreach (string id in ingredientIds)
            {
                InventoryManager.Instance.RemoveItem(id);
            }

            // Safety: If the player DOES have an empty bowl item for some reason, remove it
            if (InventoryManager.Instance.HasItem(emptyBowlId))
            {
                InventoryManager.Instance.RemoveItem(emptyBowlId);
            }

            // Give the Mixed Bowl
            InventoryManager.Instance.AddItem(mixedBowlId);
        }

        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnDoughMixed();
        }

        // Disable this interactable visuals or collider since the bowl is now "in inventory"
        // If you want the physical bowl to disappear from the table:
        MarkAsCollected();

        ShowDialogue("Looks like good cookie dough.");
    }
}