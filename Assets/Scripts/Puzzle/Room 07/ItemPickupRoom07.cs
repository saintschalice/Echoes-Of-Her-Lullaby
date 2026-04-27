using UnityEngine;

public class ItemPickupRoom07 : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    public string itemId;       // Dito mo ilalagay ang 'emily_cup' o 'emily_doll'
    public string itemName;     // Display name (hal. "Emily's Cup")

    [Header("Dialogue")]
    [TextArea]
    public string pickupMessage; // Mensahe pagkapulot (hal. "I found her favorite cup.")

    public void Interact() { Pickup(); }
    public void OnInteract(PlayerContext context) { Pickup(); }
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    private void Pickup()
    {
        if (InventoryManager.Instance != null)
        {
            // 1. Idagdag sa Inventory
            InventoryManager.Instance.AddItem(itemId);

            // 2. Magpakita ng dialogue
            string fullMessage = string.IsNullOrEmpty(pickupMessage)
                ? $"Picked up {itemName}."
                : pickupMessage;

            DialogueSystemV2.Instance?.StartDialogue(fullMessage, "Lisa");

            Debug.Log($"[Pickup] {itemName} added to inventory.");

            // 3. Burahin ang object sa scene dahil nasa bulsa na ni Lisa
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager instance not found in the scene!");
        }
    }
}