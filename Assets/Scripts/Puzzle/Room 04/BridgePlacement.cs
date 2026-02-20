using UnityEngine;

/// <summary>
/// Handles placing the floorboard bridge over the gap.
/// Now saves progress using PlayerPrefs so it remembers when you return to the room.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BridgePlacement : KitchenBaseInteractable
{
    [Header("Bridge References")]
    public GameObject gapBlocker;
    public GameObject bridgeVisual;

    [Header("Puzzle Logic")]
    public string requiredItemId = "floorboard_bridge";

    [Header("Persistence (Memory)")]
    // Ito ang susi! Ito ang hahanapin niya sa memory ng game.
    public string bridgeSaveID = "Room04_Bridge_Completed"; 

    [Header("Audio")]
    public AudioClip placeBridgeSound; 

    private bool bridgePlaced = false;

    protected override void Start()
    {
        base.Start();
        SyncState();
    }

    private void SyncState()
    {
        // 1. UNAHING CHECK ANG MEMORY (PlayerPrefs)
        // Kung may record na na-solve ito (value is 1), i-force natin na TRUE.
        if (PlayerPrefs.GetInt(bridgeSaveID, 0) == 1)
        {
            bridgePlaced = true;
        }
        // 2. Fallback sa Controller (kung meron man)
        else if (KitchenRoomController.Instance != null)
        {
            if (KitchenRoomController.Instance.bridgePlaced)
            {
                bridgePlaced = true;
            }
        }

        // 3. I-apply ang visuals base sa result
        ApplyBridgeState();
    }

    private void ApplyBridgeState()
    {
        if (bridgePlaced)
        {
            if (bridgeVisual != null) bridgeVisual.SetActive(true);
            if (gapBlocker != null) gapBlocker.SetActive(false);
        }
        else
        {
            if (bridgeVisual != null) bridgeVisual.SetActive(false);
            if (gapBlocker != null) gapBlocker.SetActive(true);
        }
    }

    public override void Interact()
    {
        // Check ulit baka na-save na sa ibang paraan
        SyncState();

        // Kung tapos na, wag na ulitin
        if (bridgePlaced)
        {
            ShowDialogue("That should hold. I can cross now.");
            return;
        }

        // Check Inventory
        bool hasBoard = InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId);

        if (!hasBoard)
        {
            ShowDialogue("I need something to cover this gap...");
            return;
        }

        // --- SUCCESS SEQUENCE ---

        // 1. Remove Item
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(requiredItemId);
        }

        // 2. Update Local State
        bridgePlaced = true;

        // 3. Update Controller (para sa session na ito)
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnBridgePlaced();
        }

        // 4. SAVE TO MEMORY (Para maalala pagbalik galing Room 5)
        PlayerPrefs.SetInt(bridgeSaveID, 1);
        PlayerPrefs.Save();
        Debug.Log("Bridge Saved to Memory!");

        // 5. Play Sound
        if (AudioManager.Instance != null && placeBridgeSound != null)
        {
            AudioManager.Instance.PlaySFX(placeBridgeSound);
        }

        // 6. Update Visuals & Dialogue
        ApplyBridgeState();
        ShowDialogue("This should be enough to cross.");
    }

    protected override void OnAlreadyCollected()
    {
        // Do nothing specific here, SyncState handles the visuals.
    }
}