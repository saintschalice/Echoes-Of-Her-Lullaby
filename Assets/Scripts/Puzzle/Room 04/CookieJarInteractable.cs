using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the final step of the cookie puzzle: Baking the cookies and unlocking the floorboard.
/// </summary>
public class CookieJarInteractable : KitchenBaseInteractable
{
    [Header("Puzzle Items")]
    public string doughItemId = "bowl_cookie_mix";
    public string rewardItemId = "floorboard_bridge";

    protected override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;

        // 1. Check if already done
        if (KitchenRoomController.Instance.cookiesBakedAndStored)
        {
            ShowDialogue("It's finished. Maybe I should put these cookies somewhere...");
            return;
        }

        // 2. Check Prerequisites (Dough Mixed & Oven Set)
        bool hasDough = KitchenRoomController.Instance.doughMixed; // Or check inventory: InventoryManager.Instance.HasItem(doughItemId)
        bool ovenReady = KitchenRoomController.Instance.ovenSetCorrect;

        if (!hasDough || !ovenReady)
        {
            ShowDialogue("It's a cookie jar, but there's nothing inside it.");
            return;
        }

        // 3. Baking Sequence
        // Double check they actually have the item in inventory to remove it
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(doughItemId))
        {
            StartCoroutine(BakeRoutine());
        }
        else
        {
            // Fallback if flags match but item missing (shouldn't happen in normal flow)
            ShowDialogue("I need the dough first.");
        }
    }

    private IEnumerator BakeRoutine()
    {
        ShowDialogue("There's a baking tray in here... I'll make the dough into balls and cook them up.");

        // Disable input during "baking" simulation? Optional. 
        // For now, just a delay.
        yield return new WaitForSeconds(2.0f);

        // 1. Remove Dough
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(doughItemId);
        }

        // 2. Update Controller State
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnCookiesBakedAndStored();

            // Also mark floorboard as obtained if we are giving it directly
            KitchenRoomController.Instance.OnFloorboardObtained();
        }

        // 3. Give Reward (Floorboard)
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(rewardItemId);
            ShowDialogue("The cookies smell great... And look, there was a loose floorboard hidden behind the jar!");
        }

        // 4. Mark this specific interaction object as "Collected" (or just leave it to show the 'finished' dialogue)
        // We won't disable visuals because the jar stays there.
        // We just rely on 'cookiesBakedAndStored' flag in the controller for future interactions.
    }

    // Override to prevent disabling renderer on "AlreadyCollected" if we want the jar to stay visible
    protected override void OnAlreadyCollected()
    {
        // Do nothing visual, just set internal flag. 
        // The Interact() method checks the controller state anyway.
        isCollected = true;
    }
}