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

        // CRITICAL FIX: Only trigger if calendar has been seen
        // This prevents the dialogue from showing on first entry
        if (!Room05_DiningRoomController.Instance.isCalendarSeen)
        {
            Debug.Log("[DiningRoomChaseTrigger] Calendar not seen yet, skipping trigger.");
            return;
        }

        // We only want this physical trigger to fire if Emily is NOT already hunting
        // and the puzzle isn't finished yet.
        if (!Room05_DiningRoomController.Instance.isEmilyHunting && !Room05_DiningRoomController.Instance.puzzleCompleted)
        {
            Debug.Log("[DiningRoomChaseTrigger] Player hit trigger. Starting Emily chase sequence.");
            hasTriggered = true;

            // Start the chase sequence (with intro dialogue first)
            StartCoroutine(Room05_DiningRoomController.Instance.EmilyGetsAngrySequence());

            // Disable the collider
            GetComponent<Collider2D>().enabled = false;
        }
    }
}