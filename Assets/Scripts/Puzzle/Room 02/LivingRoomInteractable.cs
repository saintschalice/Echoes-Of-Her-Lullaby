using UnityEngine;

public class LivingRoomInteractable : MonoBehaviour, IInteractable
{
    public enum InteractableType
    {
        TV,
        Frame,
        Bookshelf,
        Bookshelf2,
        ToyBox,
        Couch,
        LooseFloorboard,
        SmallKey,
        CoffeeTableKey
    }

    [Header("Settings")]
    public InteractableType type;
    public float interactionRange = 2f;

    [Header("UI")]
    public GameObject interactPrompt;

    private Transform player;
    private Room02_LivingRoomController roomController;
    private bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        roomController = FindFirstObjectByType<Room02_LivingRoomController>();

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        // Safety check if dialogue is open
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            return;

        if (roomController == null)
        {
            roomController = FindFirstObjectByType<Room02_LivingRoomController>();
            if (roomController == null) return;
        }

        // Logic copied from OnInteract, but without the manual Distance check 
        // because the InteractionTracker already confirmed we are close enough.

        switch (type)
        {
            case InteractableType.TV:
                roomController.OnTVInteract();
                break;
            case InteractableType.Frame:
                roomController.OnFrameExamine();
                break;
            case InteractableType.Bookshelf:
                DialogueSystemV2.Instance?.StartDialogue("Just a bookshelf with old, dusty books.", "Lisa");
                break;
            case InteractableType.Bookshelf2:
                roomController.OnBookshelf2Interact();
                break;
            case InteractableType.ToyBox:
                roomController.OnToyBoxInteract();
                break;
            case InteractableType.Couch:
                roomController.OnCouchInteract();
                break;
            case InteractableType.LooseFloorboard:
                roomController.OnLooseFloorboardInteract();
                break;
            case InteractableType.SmallKey:
                roomController.OnSmallKeyInteract();
                break;
            case InteractableType.CoffeeTableKey:
                roomController.OnCoffeeTableKeyInteract();
                break;
        }
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        player = context.Transform;
        playerInRange = IsInRange(player);

        if (!playerInRange || (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()))
            return;

        if (roomController == null) return;

        switch (type)
        {
            case InteractableType.TV:
                roomController.OnTVInteract();
                break;
            case InteractableType.Frame:
                roomController.OnFrameExamine();
                break;
            case InteractableType.Bookshelf:
                DialogueSystemV2.Instance?.StartDialogue("Just a bookshelf with old, dusty books.", "Lisa");
                break;
            case InteractableType.Bookshelf2:
                roomController.OnBookshelf2Interact();
                break;
            case InteractableType.ToyBox:
                roomController.OnToyBoxInteract();
                break;
            case InteractableType.Couch:
                roomController.OnCouchInteract();
                break;
            case InteractableType.LooseFloorboard:
                roomController.OnLooseFloorboardInteract();
                break;
            case InteractableType.SmallKey:
                roomController.OnSmallKeyInteract();
                break;
            case InteractableType.CoffeeTableKey:
                roomController.OnCoffeeTableKeyInteract();
                break;
        }
    }

    public void OnFocus(PlayerContext context)
    {
        player = context.Transform;
        playerInRange = IsInRange(player);

        if (interactPrompt != null)
        {
            bool canShow = playerInRange && (DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
            interactPrompt.SetActive(canShow);
        }
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    bool IsInRange(Transform target)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= interactionRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}