// Developer: Jhon Jellar Z. Miranda
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Room08Interactable : MonoBehaviour, IInteractable
{
    public enum InteractableType { Bathtub, Cabinet, Mirror }

    [Header("Settings")]
    public InteractableType type;
    public float interactionRange = 2f;

    public void Interact()
    {
        var roomController = Room8Manager.Instance;
        if (roomController == null)
        {
            Debug.LogWarning($"Walang Room8Manager sa scene para sa {gameObject.name}");
            return;
        }

        // Wag mag-interact kung may nag-p-play na dialogue
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            return;

        // I-check ang distance (tulad ng sa Dining Room mo)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > interactionRange) return;
        }

        switch (type)
        {
            case InteractableType.Bathtub:
                roomController.InteractWith("Bathtub");
                break;
            case InteractableType.Cabinet:
                roomController.InteractWith("Cabinet");
                break;
            case InteractableType.Mirror:
                roomController.InteractWith("Mirror");
                break;
        }
    }

    public void OnInteract(PlayerContext context) => Interact();
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}