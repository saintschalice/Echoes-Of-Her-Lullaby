using UnityEngine;

public class DiningRoomInteractable : MonoBehaviour, IInteractable
{
    // Idinagdag ang Floorboard sa huli
    public enum InteractableType { Chair, MotherChair, FatherChair, Table, Cabinet, Calendar, Spoon, Key, Floorboard }

    [Header("Settings")]
    public InteractableType type;
    public float interactionRange = 2f;

    private Room05_DiningRoomController roomController;

    void Start()
    {
        roomController = FindFirstObjectByType<Room05_DiningRoomController>();
    }

    public void Interact()
    {
        if (roomController == null) return;

        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            return;

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
            
            // Idinagdag ang case para sa Floorboard
            case InteractableType.Floorboard: roomController.OnFloorboardInteract(); break;
        }
    }

    public void OnInteract(PlayerContext context) => Interact();
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }
}