using UnityEngine;

public class OvenPuzzleInteractable : KitchenBaseInteractable
{
    public override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;
        var ctrl = KitchenRoomController.Instance;

        // 4. If emily has spawned AND recipe read AND mixed AND puzzle completed
        if (ctrl.ovenSetCorrect)
        {
            ShowDialogue("We're done there.");
            return;
        }

        // 1. If emily has not spawned yet
        if (!ctrl.emilyIntroDone)
        {
            ShowDialogue("Seems to be working fine.");
            return;
        }

        // Prerequisite for further steps: Recipe must be read.
        // If Emily spawned but recipe NOT read, we default to the generic message 
        // (as the player doesn't know about ingredients yet).
        if (!ctrl.recipeRead)
        {
            ShowDialogue("Seems to be working fine.");
            return;
        }

        // 2. If emily spawned AND recipe read, but ingredients NOT mixed yet
        // (This covers gathering ingredients and having them but not mixing them yet)
        if (!ctrl.doughMixed)
        {
            ShowDialogue("I should get all the ingredients first.");
            return;
        }

        // 3. If emily spawned AND recipe read AND ingredients mixed
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