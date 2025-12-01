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
        ShowDialogue("There's a baking tray in here... I'll make the dough into balls and cook them up.");
        yield return new WaitForSeconds(2.0f);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RemoveItem(doughItemId);

        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnCookiesBakedAndStored();
            KitchenRoomController.Instance.OnFloorboardObtained();
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(rewardItemId);
            ShowDialogue("The cookies smell great... And look, there was a loose floorboard hidden behind the jar!");
        }
    }

    protected override void OnAlreadyCollected()
    {
        isCollected = true;
    }
}