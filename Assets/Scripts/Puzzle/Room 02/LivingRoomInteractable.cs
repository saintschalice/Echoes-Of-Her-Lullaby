using UnityEngine;

public class LivingRoomInteractable : MonoBehaviour
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

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRange;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(playerInRange && !DialogueSystemV2.Instance.IsDialogueActive());
        }

        // Touch-based interaction
        if (playerInRange && Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Interact();
            }
        }
    }

    void Interact()
    {
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}