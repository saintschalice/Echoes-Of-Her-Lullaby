using UnityEngine;

/// <summary>
/// Handles placing the floorboard bridge over the gap in Room 04.
/// Persists the state using PlayerPrefs so it remains placed after scene transitions.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BridgePlacement : KitchenBaseInteractable
{
    [Header("Bridge References")]
    public GameObject gapBlocker;   // FloorGap_Blocker
    public GameObject bridgeVisual; // BridgeVisual

    [Header("Puzzle Logic")]
    public string requiredItemId = "floorboard_bridge";

    [Header("Persistence (Memory)")]
    // Gamit ang ID na nakita sa iyong Inspector
    public string bridgeSaveID = "Room04_Bridge_Completed";

    [Header("Audio")]
    public AudioClip placeBridgeSound;

    private bool bridgePlaced = false;

    protected override void Start()
    {
        base.Start();
        // 1. Pag-start ng scene, i-sync agad ang state base sa memory
        SyncState();
    }

    private void SyncState()
    {
        // Check kung ang value sa memory ay 1 (Nagawa na)
        if (PlayerPrefs.GetInt(bridgeSaveID, 0) == 1)
        {
            bridgePlaced = true;
        }
        else if (KitchenRoomController.Instance != null)
        {
            // Fallback check sa local controller session
            bridgePlaced = KitchenRoomController.Instance.bridgePlaced;
        }

        ApplyBridgeState();
    }

    private void ApplyBridgeState()
    {
        if (bridgePlaced)
        {
            // Kung tapos na: Buhay ang tulay, patay ang harang
            if (bridgeVisual != null) bridgeVisual.SetActive(true);
            if (gapBlocker != null) gapBlocker.SetActive(false);
        }
        else
        {
            // Kung HINDI PA: Patay ang tulay, buhay ang harang
            if (bridgeVisual != null) bridgeVisual.SetActive(false);
            if (gapBlocker != null) gapBlocker.SetActive(true);
        }
    }

    public override void Interact()
    {
        SyncState();

        if (bridgePlaced)
        {
            ShowDialogue("That should hold. I can cross now.");
            return;
        }

        // Check kung nasa inventory na ni Lisa ang floorboard
        bool hasBoard = InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId);

        if (!hasBoard)
        {
            ShowDialogue("I need something to cover this gap...");
            return;
        }

        // --- PUZZLE COMPLETE LOGIC ---

        // 1. Remove Item mula sa Inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(requiredItemId);
        }

        bridgePlaced = true;

        // 2. I-save sa PlayerPrefs para maging PERMANENT
        PlayerPrefs.SetInt(bridgeSaveID, 1);
        PlayerPrefs.Save();
        Debug.Log("[BridgePlacement] State saved to PlayerPrefs!");

        // 3. I-update ang session controller
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnBridgePlaced();
        }

        // 4. Play Sound
        if (AudioManager.Instance != null && placeBridgeSound != null)
        {
            AudioManager.Instance.PlaySFX(placeBridgeSound);
        }

        ApplyBridgeState();
        ShowDialogue("This should be enough to cross.");
    }

    // ==========================================================
    // DEV TOOL: Right-click ang component sa Inspector para i-reset
    // ==========================================================
    [ContextMenu("Reset Bridge Save")]
    public void ResetBridgeSave()
    {
        PlayerPrefs.DeleteKey(bridgeSaveID);
        PlayerPrefs.Save();
        bridgePlaced = false;
        ApplyBridgeState();
        Debug.Log("[BridgePlacement] Save cleared! Pwede mo na i-test ulit ang puzzle.");
    }

    protected override void OnAlreadyCollected()
    {
        // Handled by SyncState
    }
}