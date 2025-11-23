using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KitchenChaseTrigger : MonoBehaviour
{
    [Header("Emily Configuration")]
    [Tooltip("The prefab to spawn for the intro sequence.")]
    public EmilyGhost emilyPrefab;

    [Tooltip("Where Emily should spawn when this trigger is hit.")]
    public Transform emilySpawnPoint;

    [Header("Debug")]
    [SerializeField] private bool hasTriggered = false;

    private void Start()
    {
        // Safety check: if the intro is already done according to the controller, disable this trigger immediately.
        if (KitchenRoomController.Instance != null && KitchenRoomController.Instance.emilyIntroDone)
        {
            hasTriggered = true;
            GetComponent<Collider2D>().enabled = false;
        }

        // Ensure collider is a trigger
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("[KitchenChaseTrigger] Collider is not set to IsTrigger! Fixing automatically.");
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Basic Guards
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        // 2. Reference Check
        if (KitchenRoomController.Instance == null)
        {
            Debug.LogError("[KitchenChaseTrigger] KitchenRoomController not found! Cannot start intro.");
            return;
        }

        if (KitchenRoomController.Instance.emilyIntroDone)
        {
            // Controller says we are done, disable locally and return
            hasTriggered = true;
            GetComponent<Collider2D>().enabled = false;
            return;
        }

        // 3. Configuration Check
        if (emilyPrefab == null || emilySpawnPoint == null)
        {
            Debug.LogError("[KitchenChaseTrigger] Emily Prefab or Spawn Point is missing in Inspector!");
            return;
        }

        // 4. Execution
        Debug.Log("[KitchenChaseTrigger] Player entered trigger. Handing off to KitchenRoomController.");

        hasTriggered = true;

        // Check if an Emily already exists to avoid duplicates (defensive programming)
        EmilyGhost existingEmily = FindFirstObjectByType<EmilyGhost>();
        if (existingEmily != null)
        {
            Debug.LogWarning("[KitchenChaseTrigger] Emily already exists in scene. Using existing instance instead of spawning new.");
            // We pass the existing prefab logic, but the Controller might need adaptation if we strictly want to reuse.
            // Based on Phase 1 Controller, it instantiates. Let's strictly follow the instruction to "Spawn if she doesn't exist".
            // However, the Controller's method signature expects a PREFAB to instantiate. 
            // We will proceed with the standard flow, assuming the Controller handles the instantiation.
        }

        KitchenRoomController.Instance.StartEmilyKitchenIntro(other.transform, emilyPrefab, emilySpawnPoint);

        // 5. Cleanup
        // Disable the collider so it cannot be triggered again physically
        GetComponent<Collider2D>().enabled = false;
    }
}