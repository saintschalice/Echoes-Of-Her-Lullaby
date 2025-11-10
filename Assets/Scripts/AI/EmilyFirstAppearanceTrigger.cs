using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers Emily's first appearance in the hallway scene (Room 3)
/// Handles the dramatic staircase confrontation sequence
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
    public bool useRelativeKnockback = false; // If true, knock back relative to current position
    public float knockbackDistance = 3f;

    [Header("Audio")]
    public AudioClip windPushSound;

    [Header("Dialogue")]
    [TextArea] public string appearanceDialogue = "Something's blocking the way...";
    public string speakerName = "Lisa";

    [Header("Effects (Optional)")]
    public GameObject visualEffect; // Particle effect, flash, etc.
    public float screenShakeDuration = 0.3f;
    public float screenShakeIntensity = 0.2f;

    private void Start()
    {
        // Check if this event has already happened (save system integration)
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.WasDialogueTriggered(triggerId))
            {
                hasTriggered = true;
                gameObject.SetActive(false); // Disable trigger if already done
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

        // Disable player controls during sequence
        DisablePlayerControls();

        // Activate Emily through PersistentEmilyManager
        if (PersistentEmilyManager.Instance != null)
        {
            PersistentEmilyManager.Instance.ActivateEmily();
            Debug.Log("[EmilyTrigger] Emily activated through manager");
        }
        else
        {
            Debug.LogError("[EmilyTrigger] PersistentEmilyManager not found!");
        }

        // Wait for dramatic pause
        yield return new WaitForSeconds(windPushDelay);

        // Play visual effect if assigned
        if (visualEffect != null)
        {
            Instantiate(visualEffect, player.position, Quaternion.identity);
        }

        // Wind push sound effect
        if (windPushSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(windPushSound, player.position);
        }

        // Screen shake effect (if you have a camera shake system)
        StartCoroutine(ScreenShake());

        // Knockback player
        if (player != null)
        {
            if (useRelativeKnockback)
            {
                // Knock back relative to current position
                Vector3 knockbackDir = (player.position - transform.position).normalized;
                player.position = player.position + (knockbackDir * knockbackDistance);
            }
            else
            {
                // Knock back to specific position
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

        // Save that this event happened
        if (SaveSystem.Instance != null)
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
        // Simple screen shake implementation
        // You can replace this with your own camera shake system

        Camera mainCam = Camera.main;
        if (mainCam == null) yield break;

        Vector3 originalPos = mainCam.transform.position;
        float elapsed = 0f;

        while (elapsed < screenShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * screenShakeIntensity;
            float y = Random.Range(-1f, 1f) * screenShakeIntensity;

            mainCam.transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalPos;
    }

    void DisablePlayerControls()
    {
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }
    }

    void EnablePlayerControls()
    {
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = true;
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