using UnityEngine;

public class OvenPuzzleInteractable : KitchenBaseInteractable
{
    public override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;
        var ctrl = KitchenRoomController.Instance;

        // 4. if emily has spawned AND the recipe is read AND all the ingredients are mixed...
        // AND the player has successfully completed the oven puzzle
        if (ctrl.ovenSetCorrect)
        {
            ShowDialogue("We're done there.");
            return;
        }

        // 1. if emily has not spawned yet
        if (!ctrl.emilyIntroDone)
        {
            ShowDialogue("Seems to be working fine.");
            return;
        }

        // Prerequisite: Recipe must be read to proceed to step 2 or 3 logic.
        // If recipe is not read, we default to the generic state.
        if (!ctrl.recipeRead)
        {
            ShowDialogue("Seems to be working fine.");
            return;
        }

        // 2. if emily has spawned AND the recipe is read
        // (We check this BEFORE the mixed check. If doughMixed is false, we are in this state).
        if (!ctrl.doughMixed)
        {
            ShowDialogue("I should get all the ingredients first.");
            return;
        }

        // 3. if emily has spawned AND the recipe is read AND all the ingredients are mixed
        // (doughMixed is true, and ovenSetCorrect is false)
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