using UnityEngine;

public class FridgeInteractable : KitchenBaseInteractable
{
    [Header("Fridge Items")]
    public string eggItemId = "egg";
    public string chocolateItemId = "chocolate";

    // FIX: Changed from 'protected' to 'public'
    public override void Interact()
    {
        // 1. Check if collected
        if (isCollected)
        {
            ShowDialogue("Nothing useful left in here.");
            return;
        }

        // 2. Check Recipe Status
        bool recipeRead = false;
        if (KitchenRoomController.Instance != null)
        {
            recipeRead = KitchenRoomController.Instance.recipeRead;
        }

        if (!recipeRead)
        {
            // Pre-Recipe Dialogue
            ShowDialogue("Ugh... It's nasty in here. There's a couple of things that are probably rotten.");
        }
        else
        {
            // Post-Recipe Collection
            ShowDialogue("There's eggs here... and some chocolate. I bet they're inedible.");

            // Give both items
            AddItemToInventory(eggItemId);
            AddItemToInventory(chocolateItemId);

            // Notify Controller for both
            NotifyKitchenController(eggItemId);
            NotifyKitchenController(chocolateItemId);

            // Mark collected (Fridge remains visible usually, so we override MarkAsCollected slightly to NOT disable renderer if we want it to stay)
            // But base class MarkAsCollected disables visuals. 
            // For a Fridge, we probably want the fridge to stay visible, just "collected".
            // So we call the logic manually without disabling visuals if we want the fridge sprite to remain.

            // NOTE: Assuming Fridge Sprite is the OPEN fridge or Closed fridge. 
            // If it's the whole fridge object, we DON'T want to disable it.
            // We'll modify the behavior here:

            isCollected = true;
            if (SaveSystem.Instance != null)
            {
                // Save Logic duplicated from Base to avoid disabling renderer
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                RoomState state = SaveSystem.Instance.GetRoomState(sceneName);
                if (!state.collectedItems.Contains(objectId))
                {
                    state.collectedItems.Add(objectId);
                    SaveSystem.Instance.UpdateRoomState(sceneName, state);
                }
            }
            // DO NOT call DisableVisuals() for the fridge, assuming it's a large static object.
        }
    }

    protected override void OnAlreadyCollected()
    {
        isCollected = true;
        // Do NOT disable visuals for the fridge
    }
}