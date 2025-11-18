using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class EmilySpawnTrigger : MonoBehaviour
{
    [Header("Setup")]
    public EmilyGhost emilyPrefab;
    public Transform spawnPoint;

    [Header("Scene Connections")]
    public HallwayClosetInteractable closetScript;

    [Header("Cinematic Settings")]
    public float pushForce = 3f;       // The initial "shove" strength
    public float shoveFriction = 10f;  // High drag stops the player quickly
    public float resumeDelay = 0.3f;   // Time to wait after dialogue before Emily attacks

    [Header("Narrative")]
    [TextArea] public string emilyShout = "GET OUT OF HERE!";
    [TextArea] public string lisaPanic = "Oh no... I need to hide!";

    private EmilyGhost _instance;
    private BoxCollider2D _triggerCollider;

    void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_instance != null) return;

        if (_triggerCollider != null) _triggerCollider.enabled = false;

        StartCoroutine(SpawnSequence(col.gameObject));
    }

    IEnumerator SpawnSequence(GameObject playerObj)
    {
        // 1. Spawn Emily (Frozen)
        if (emilyPrefab != null && spawnPoint != null)
        {
            _instance = Instantiate(emilyPrefab, spawnPoint.position, Quaternion.identity);

            // CHANGE: Hardcode Y position to -6f
            _instance.transform.position = new Vector3(spawnPoint.position.x, -6f, 0f);

            _instance.gameObject.SetActive(true);
            _instance.enabled = false;
        }

        // 2. Unlock Closet
        if (closetScript != null) closetScript.UnlockForHiding();

        // 3. Push Mechanic (Fixed for your Unity version)
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        JoystickPlayerController playerController = playerObj.GetComponent<JoystickPlayerController>();
        float originalDrag = 0f;

        if (playerRb != null)
        {
            // FIX: Changed 'linearDrag' to 'drag' to resolve compiler error
            originalDrag = playerRb.linearDamping;

            // Apply high friction so they stop fast
            playerRb.linearDamping = shoveFriction;

            // Disable inputs
            if (playerController != null) playerController.enabled = false;

            // Push
            Vector2 pushDir = (playerObj.transform.position - spawnPoint.position).normalized;
            if (pushDir == Vector2.zero) pushDir = Vector2.down;
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }

        // 4. Dialogue: Emily Shouts
        if (DialogueSystemV2.Instance != null)
        {
            yield return new WaitForSeconds(0.2f); // Short pause to see the shove happen
            DialogueSystemV2.Instance.StartDialogue(emilyShout, "???");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        }

        // 5. Dialogue: Lisa Panics
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(lisaPanic, "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        }

        // 6. Resume Delay
        yield return new WaitForSeconds(resumeDelay);

        // 7. Restore Player State
        if (playerRb != null)
        {
            playerRb.linearDamping = originalDrag; // FIX: Reset using .drag
        }
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // 8. Resume Emily
        if (_instance != null)
        {
            _instance.enabled = true;
        }

        Debug.Log("[EMILY SPAWN] Sequence Complete.");
        Destroy(gameObject);
    }
}