using UnityEngine;
using System.Collections;

/// <summary>
/// OPTIMIZED - Triggers Emily's first appearance with ZERO lag
/// Key fixes:
/// - Cached player controller reference (no FindObjectOfType)
/// - Uses ActivateEmily() instead of SpawnEmily()
/// - Removed redundant disable/enable cycle
/// </summary>
public class EmilyFirstAppearanceTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool hasTriggered = false;
    public string triggerId = "emily_first_appearance";

    [Header("Sequence Timing")]
    public float windPushDelay = 0.5f;
    public float dialogueDelay = 1f;

    [Header("Player Knockback")]
    public Vector3 playerKnockbackPosition = new Vector3(-6, 0, 0);
    public bool useRelativeKnockback = false;
    public float knockbackDistance = 3f;

    [Header("Audio")]
    public AudioClip windPushSound;

    [Header("Dialogue")]
    [TextArea] public string appearanceDialogue = "Something's blocking the way...";
    public string speakerName = "Lisa";

    [Header("Effects (Optional)")]
    public GameObject visualEffect;
    public float screenShakeDuration = 0.3f;
    public float screenShakeIntensity = 0.2f;

    // OPTIMIZATION: Cache player controller reference
    private JoystickPlayerController cachedPlayerController;
    private Camera mainCamera;

    private void Awake()
    {
        // Cache references at startup (fast)
        cachedPlayerController = FindFirstObjectByType<JoystickPlayerController>();
        mainCamera = Camera.main;

        if (cachedPlayerController == null)
        {
            Debug.LogWarning("[EmilyTrigger] JoystickPlayerController not found at startup");
        }
    }

    private void Start()
    {
        // Check if this event has already happened
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.WasDialogueTriggered(triggerId))
            {
                hasTriggered = true;
                gameObject.SetActive(false);
                Debug.Log("[EmilyTrigger] Event already triggered, disabling");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(TriggerEmilyAppearance(other.transform));
        }
    }

    IEnumerator TriggerEmilyAppearance(Transform player)
    {
        Debug.Log("[EmilyTrigger] First appearance sequence started!");

        // Disable player controls (using cached reference)
        DisablePlayerControls();

        // CRITICAL FIX: Use ActivateEmily() instead of SpawnEmily()
        // This uses the pre-instantiated pooled Emily (no lag!)
        if (PersistentEmilyManager.Instance != null)
        {
            PersistentEmilyManager.Instance.ActivateEmily();
        }
        else
        {
            Debug.LogError("[EmilyTrigger] PersistentEmilyManager not found!");
            yield break;
        }

        // Wait a frame for Emily to activate
        yield return null;

        // Get Emily reference
        EmilyAIController emily = PersistentEmilyManager.Instance.currentEmily;

        if (emily == null)
        {
            Debug.LogError("[EmilyTrigger] Emily instance is NULL after activation!");
            EnablePlayerControls();
            yield break;
        }

        // Set initial state
        emily.ForceState(EmilyState.INVESTIGATE);

        // Wait for dramatic pause
        yield return new WaitForSeconds(windPushDelay);

        // Play visual effect
        if (visualEffect != null)
        {
            Instantiate(visualEffect, player.position, Quaternion.identity);
        }

        // Wind push sound
        if (windPushSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(windPushSound, player.position);
        }

        // Screen shake
        if (screenShakeDuration > 0f)
        {
            StartCoroutine(ScreenShake());
        }

        // Knockback player
        if (player != null)
        {
            if (useRelativeKnockback)
            {
                Vector3 knockbackDir = (player.position - transform.position).normalized;
                player.position = player.position + (knockbackDir * knockbackDistance);
            }
            else
            {
                player.position = playerKnockbackPosition;
            }

            Debug.Log($"[EmilyTrigger] Player knocked back to {player.position}");
        }

        // Wait before dialogue
        yield return new WaitForSeconds(dialogueDelay);

        // Show dialogue
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(appearanceDialogue, speakerName);
        }
        else
        {
            Debug.LogWarning("[EmilyTrigger] DialogueSystemV2 not found");
        }

        // Switch Emily to HUNT mode
        emily.ForceState(EmilyState.HUNT);
        Debug.Log("[EmilyTrigger] Emily switched to HUNT after dialogue");

        // Save event
        if (SaveSystem.Instance != null && !string.IsNullOrEmpty(triggerId))
        {
            SaveSystem.Instance.TriggerDialogue(triggerId);
            Debug.Log("[EmilyTrigger] Event saved to SaveSystem");
        }

        // Re-enable player controls
        yield return new WaitForSeconds(0.5f);
        EnablePlayerControls();

        Debug.Log("[EmilyTrigger] First appearance sequence complete!");
    }

    IEnumerator ScreenShake()
    {
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < screenShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * screenShakeIntensity;
            float y = Random.Range(-1f, 1f) * screenShakeIntensity;

            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPos;
    }

    // OPTIMIZED: Uses cached reference instead of FindObjectOfType
    void DisablePlayerControls()
    {
        if (cachedPlayerController == null)
        {
            // Try to find it again
            cachedPlayerController = FindFirstObjectByType<JoystickPlayerController>();
        }

        if (cachedPlayerController != null)
        {
            cachedPlayerController.enabled = false;
        }
        else
        {
            Debug.LogError("[EmilyTrigger] CRITICAL: Could not find player controller to disable!");
        }
    }

    void EnablePlayerControls()
    {
        if (cachedPlayerController == null)
        {
            cachedPlayerController = FindFirstObjectByType<JoystickPlayerController>();
        }

        if (cachedPlayerController != null)
        {
            cachedPlayerController.enabled = true;
        }
    }

    /// <summary>
    /// Manual trigger method (can be called from other scripts)
    /// </summary>
    public void ManualTrigger()
    {
        if (!hasTriggered)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                StartCoroutine(TriggerEmilyAppearance(playerObj.transform));
            }
        }
    }

    /// <summary>
    /// Reset trigger (for testing)
    /// </summary>
    [ContextMenu("Reset Trigger")]
    public void ResetTrigger()
    {
        hasTriggered = false;
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.ClearObjectExamined(triggerId);
        }
        Debug.Log("[EmilyTrigger] Trigger reset");
    }

    private void OnDrawGizmos()
    {
        // Visualize trigger area
        Gizmos.color = hasTriggered ? Color.red : Color.yellow;

        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        if (boxCol != null)
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)boxCol.offset, boxCol.size);
        }

        // Visualize knockback position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerKnockbackPosition, 0.5f);
        Gizmos.DrawLine(transform.position, playerKnockbackPosition);
    }
}