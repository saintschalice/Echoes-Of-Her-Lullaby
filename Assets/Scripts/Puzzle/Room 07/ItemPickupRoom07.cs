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
            // 1. Show dialogue first (if any)
            if (!string.IsNullOrEmpty(pickupMessage))
            {
                DialogueSystemV2.Instance?.StartDialogue(pickupMessage, "Lisa");
            }

            // 2. Add to inventory with notification (will wait for dialogue to finish)
            StartCoroutine(AddItemAfterDialogue());

            Debug.Log($"[Pickup] {itemName} will be added to inventory after dialogue.");
        }
        else
        {
            Debug.LogError("InventoryManager instance not found in the scene!");
        }
    }

    private System.Collections.IEnumerator AddItemAfterDialogue()
    {
        // Wait for dialogue to finish first
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Add item with notification
        InventoryManager.Instance?.AddItemWithNotification(itemId);

        // Wait for notification to finish before destroying object
        while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
        {
            yield return null;
        }

        // Destroy the object after notification is done
        Destroy(gameObject);
    }
}