using UnityEngine;

/// <summary>
/// Handles placing the floorboard bridge over the gap.
/// Toggles between a blocking collider (for the bridge spot only) and a walkable visual bridge.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BridgePlacement : KitchenBaseInteractable
{
    [Header("Bridge References")]
    public GameObject gapBlocker;
    public GameObject bridgeVisual;

    [Header("Puzzle Logic")]
    public string requiredItemId = "floorboard_bridge";

    [Header("Audio")]
    public AudioClip placeBridgeSound; // NEW: Sound effect

    private bool bridgePlaced = false;

    protected override void Start()
    {
        base.Start();
        SyncState();
    }

    private void SyncState()
    {
        if (KitchenRoomController.Instance != null)
        {
            bridgePlaced = KitchenRoomController.Instance.bridgePlaced;
        }

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

    protected override void Interact()
    {
        SyncState();

        if (bridgePlaced)
        {
            ShowDialogue("That should hold. I can cross now.");
            return;
        }

        bool hasBoard = InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId);

        if (!hasBoard)
        {
            ShowDialogue("I need something to cover this gap...");
            return;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(requiredItemId);
        }

        bridgePlaced = true;

        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnBridgePlaced();
        }

        // Play Sound
        if (AudioManager.Instance != null && placeBridgeSound != null)
        {
            AudioManager.Instance.PlaySFX(placeBridgeSound);
        }

        ApplyBridgeState();
        ShowDialogue("This should be enough to cross.");
    }

    protected override void OnAlreadyCollected()
    {
        // Do nothing specific here, SyncState handles the visuals.
    }
}