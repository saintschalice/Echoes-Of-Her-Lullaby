using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DiningRoomInteractable : MonoBehaviour, IInteractable
{
    // Tinanggal na ang Floorboard. 
    public enum InteractableType { Chair, MotherChair, FatherChair, Table, Cabinet, Calendar, Spoon, Key, Cutlery, Fork }

    [Header("Settings")]
    public InteractableType type;
    public float interactionRange = 2f;

    public void Interact()
    {
        var roomController = Room05_DiningRoomController.Instance;
        if (roomController == null)
        {
            Debug.LogWarning($"Walang Room05_DiningRoomController sa scene para sa {gameObject.name}");
            return;
        }

        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > interactionRange) return;
        }

        switch (type)
        {
            case InteractableType.Chair: roomController.OnChairInteract(); break;
            case InteractableType.MotherChair: roomController.OnMotherChairInteract(); break;
            case InteractableType.FatherChair: roomController.OnFatherChairInteract(); break;
            case InteractableType.Table: roomController.OnTableInteract(); break;
            case InteractableType.Cabinet: roomController.OnCabinetInteract(); break;
            case InteractableType.Calendar: roomController.OnCalendarInteract(); break;
            case InteractableType.Spoon: roomController.OnSpoonInteract(); break;
            case InteractableType.Key: roomController.OnKeyInteract(); break;
            case InteractableType.Cutlery: roomController.OnCutleryInteract(); break;
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