using UnityEngine;

public class OvenPuzzleInteractable : KitchenBaseInteractable
{
    protected override void Interact()
    {
        // 1. Check Controller
        if (KitchenRoomController.Instance == null) return;

        // 2. Check if already solved
        if (KitchenRoomController.Instance.ovenSetCorrect)
        {
            ShowDialogue("The oven is already set.");
            return;
        }

        // 3. Check Pre-requisite (Recipe)
        if (!KitchenRoomController.Instance.recipeRead)
        {
            ShowDialogue("Seems to be working fine.");
            return;
        }

        // 4. Open UI
        ShowDialogue("I should set the timer to...");

        if (OvenUI.Instance != null)
        {
            OvenUI.Instance.OpenUI();
        }
        else
        {
            Debug.LogError("OvenUI not found in scene!");
        }
    }
}