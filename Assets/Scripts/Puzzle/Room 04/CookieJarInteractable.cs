using UnityEngine;
using System.Collections;

public class CookieJarInteractable : KitchenBaseInteractable
{
    [Header("Puzzle Items")]
    public string doughItemId = "bowl_cookie_mix";
    public string rewardItemId = "floorboard_bridge";

    // FIX: Public override
    public override void Interact()
    {
        if (KitchenRoomController.Instance == null) return;

        if (KitchenRoomController.Instance.cookiesBakedAndStored)
        {
            ShowDialogue("It's finished. Maybe I should put these cookies somewhere...");
            return;
        }

        bool hasDough = KitchenRoomController.Instance.doughMixed;
        bool ovenReady = KitchenRoomController.Instance.ovenSetCorrect;

        if (!hasDough || !ovenReady)
        {
            ShowDialogue("It's a cookie jar, but there's nothing inside it.");
            return;
        }

        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(doughItemId))
        {
            StartCoroutine(BakeRoutine());
        }
        else
        {
            ShowDialogue("I need the dough first.");
        }
    }

    private IEnumerator BakeRoutine()
    {
        // STEP 1: Show dialogue first (baking process)
        ShowDialogue("There's a baking tray in here... I'll make the dough into balls and cook them up.");
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f); // Small delay after dialogue

        // STEP 2: Remove dough from inventory
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RemoveItem(doughItemId);

        // STEP 3: Update puzzle state
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnCookiesBakedAndStored();
            KitchenRoomController.Instance.OnFloorboardObtained();
        }

        // STEP 4: Add item WITH NOTIFICATION (this will show the notification with sprite)
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItemWithNotification(rewardItemId, "A loose floorboard that can be used as a bridge.");
        }

        // STEP 5: Wait for notification to finish
        if (ItemNotificationUI.Instance != null)
        {
            while (ItemNotificationUI.Instance.IsShowing())
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f); // Small delay after notification

        // STEP 6: Show dialogue AFTER notification
        ShowDialogue("The cookies smell great... And look, there was a loose floorboard hidden behind the jar!");
    }

    protected override void OnAlreadyCollected()
    {
        isCollected = true;
    }
}