using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for simple click-to-interact objects in the Kitchen.
/// reverted to OnMouseDown to match Island behavior.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class KitchenBaseInteractable : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("Unique ID for saving whether this object was collected/interacted with.")]
    public string objectId;
    public float interactionRadius = 2.0f;

    [Header("Visuals")]
    [Tooltip("Assign the sprite renderer to disable it upon collection (optional).")]
    public SpriteRenderer visualRenderer;

    protected bool isCollected = false;
    protected JoystickPlayerController player;
    protected Collider2D myCollider;

    protected virtual void Start()
    {
        myCollider = GetComponent<Collider2D>();

        // 1. Find Player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<JoystickPlayerController>();

        // 2. Check Persistence
        CheckPersistence();
    }

    // Reverted to OnMouseDown as requested, since Island works this way
    protected virtual void OnMouseDown()
    {
        // Safety check if UI is blocking (Optional: Remove this if you want to click THROUGH UI)
        // if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (player == null)
        {
            // Try finding player again if missing (safety)
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.GetComponent<JoystickPlayerController>();
        }

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist > interactionRadius)
        {
            ShowDialogue("It's too far to reach.");
            return;
        }

        Interact();
    }

    protected void CheckPersistence()
    {
        if (SaveSystem.Instance != null)
        {
            RoomState state = SaveSystem.Instance.GetRoomState(SceneManager.GetActiveScene().name);
            if (state != null && state.collectedItems.Contains(objectId))
            {
                OnAlreadyCollected();
            }
        }
    }

    protected virtual void OnAlreadyCollected()
    {
        isCollected = true;
        DisableVisuals();
    }

    // Child classes implement specific logic here
    protected abstract void Interact();

    protected void MarkAsCollected()
    {
        if (isCollected) return;

        isCollected = true;

        // Save to RoomState
        if (SaveSystem.Instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            RoomState state = SaveSystem.Instance.GetRoomState(sceneName);

            if (!state.collectedItems.Contains(objectId))
            {
                state.collectedItems.Add(objectId);
                SaveSystem.Instance.UpdateRoomState(sceneName, state);
                SaveSystem.Instance.SaveGame(0);
            }
        }

        DisableVisuals();
    }

    protected void DisableVisuals()
    {
        if (visualRenderer != null)
        {
            visualRenderer.enabled = false;
        }
        else
        {
            // If no renderer, disable the collider so we can't click again
            if (myCollider != null) myCollider.enabled = false;
        }
    }

    protected void ShowDialogue(string text, string speaker = "Lisa")
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(text, speaker);
        }
        else
        {
            Debug.Log($"[{speaker}]: {text}");
        }
    }

    protected void AddItemToInventory(string itemId)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemId);
        }
    }

    protected void NotifyKitchenController(string ingredientId)
    {
        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.OnIngredientCollected(ingredientId);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}