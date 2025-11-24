using UnityEngine;

/// <summary>
/// Unified class for Salt and Bowl interactions.
/// Can now be configured to examine only (no pickup).
/// </summary>
public class SimpleKitchenPickup : KitchenBaseInteractable
{
    [Header("Item Settings")]
    public string itemId;
    [TextArea]
    public string pickupDialogue;

    [Header("Behavior")]
    [Tooltip("If true, adds item to inventory and destroys this object. If false, just plays dialogue.")]
    public bool shouldPickupItem = true;

    protected override void Interact()
    {
        // Always show the dialogue first
        ShowDialogue(pickupDialogue);

        // If this is just an examine object (like the static bowl), stop here.
        if (!shouldPickupItem)
        {
            return;
        }

        // Standard Pickup Logic (for Salt)
        if (isCollected) return;

        AddItemToInventory(itemId);

        // Notify controller
        NotifyKitchenController(itemId);

        MarkAsCollected(); // Disables the sprite
    }
}