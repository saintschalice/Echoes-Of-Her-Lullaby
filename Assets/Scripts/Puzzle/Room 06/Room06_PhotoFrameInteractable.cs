using UnityEngine;

/// <summary>
/// Photo frame interactable for Room 06 - Hallway Upstairs
/// Triggers the photo scratch sequence and Emily spawn
/// Based on Room 07 interactable pattern
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Room06_PhotoFrameInteractable : MonoBehaviour, IInteractable
{
    [Header("Debug")]
    public bool debugMode = true;

    private void Start()
    {
        // Ensure collider is set as trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
            if (debugMode) Debug.Log("[PhotoFrame] Collider set as trigger");
        }
    }

    // IInteractable implementation
    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    public void OnFocus(PlayerContext context)
    {
        if (debugMode) Debug.Log("[PhotoFrame] Player focused on photo frame");
    }

    public void OnBlur(PlayerContext context)
    {
        if (debugMode) Debug.Log("[PhotoFrame] Player left photo frame");
    }

    // Main interaction method - called by mobile button
    public void Interact()
    {
        DoInteract();
    }

    // Core interaction logic
    private void DoInteract()
    {
        if (debugMode) Debug.Log("[PhotoFrame] OnInteract called!");
        
        if (Room06_HallwayController.Instance != null)
        {
            Room06_HallwayController.Instance.OnPhotoFrameInteract();
        }
        else
        {
            Debug.LogError("[PhotoFrame] Room06_HallwayController not found!");
        }
    }
}
