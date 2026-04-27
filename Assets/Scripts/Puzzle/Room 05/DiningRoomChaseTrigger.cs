using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DiningRoomChaseTrigger : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool hasTriggered = false;

    private void Start()
    {
        // Safety check: Ensure the collider is a trigger
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("[DiningRoomChaseTrigger] Collider is not set to IsTrigger! Fixing automatically.");
            GetComponent<Collider2D>().isTrigger = true;
        }

        // Optional: If you want to disable this trigger permanently after the puzzle is done
        if (Room05_DiningRoomController.Instance != null && Room05_DiningRoomController.Instance.puzzleCompleted)
        {
            hasTriggered = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        if (Room05_DiningRoomController.Instance == null)
        {
            Debug.LogError("[DiningRoomChaseTrigger] Room05_DiningRoomController not found in the scene!");
            return;
        }

        // We only want this physical trigger to fire if Emily is NOT already hunting
        // and the puzzle isn't finished yet.
        if (!Room05_DiningRoomController.Instance.isEmilyHunting && !Room05_DiningRoomController.Instance.puzzleCompleted)
        {
            Debug.Log("[DiningRoomChaseTrigger] Player hit trigger. Starting Phase 1 Chase.");
            hasTriggered = true;

            // Call the calendar interact method to kick off the sequence
            // Note: If you want a DIFFERENT sequence than the calendar one, 
            // you'll need to create a new public method in the controller.
            Room05_DiningRoomController.Instance.OnCalendarInteract();

            // Disable the collider
            GetComponent<Collider2D>().enabled = false;
        }
    }
}