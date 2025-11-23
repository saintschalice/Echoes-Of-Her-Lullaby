using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class SimpleInteractable2D : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("If true, the collider is a Trigger (walk-through). If false, it's a solid wall.")]
    public bool isTrigger = true;
    public bool interactable = true;

    [Header("Distance Settings")]
    public float interactionRadius = 2.5f; // Distance within which interaction is allowed
    public bool debugRadius = true;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    public string interactionDialogue;
    public string speakerName = "Lisa";
    [Tooltip("Message shown if player taps but is too far away.")]
    public string tooFarMessage = "I need to get closer.";

    [Header("Audio")]
    public AudioClip interactionSFX;

    protected virtual void Start()
    {
        // Force the collider to match the script setting
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = isTrigger;
        }
    }

    /// <summary>
    /// Built-in Unity method that detects Clicks (Mouse) and Taps (Touch) on this collider.
    /// This makes the object "Tappable" without needing an extra Raycast script.
    /// </summary>
    protected virtual void OnMouseDown()
    {
        // If UI is blocking the tap, you might want to check EventSystem.current.IsPointerOverGameObject() here
        // For now, we assume direct tap.
        Interact();
    }

    /// <summary>
    /// Checks distance and performs interaction.
    /// </summary>
    public virtual void Interact()
    {
        if (!interactable) return;

        // 1. Find Player
        Vector3 playerPos = Vector3.zero;
        if (JoystickPlayerController.Instance != null)
        {
            playerPos = JoystickPlayerController.Instance.transform.position;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerPos = player.transform.position;
            else return; // No player found
        }

        // 2. Check Distance (Radius)
        float distance = Vector2.Distance(transform.position, playerPos);
        if (distance > interactionRadius)
        {
            Debug.Log($"[Interaction] Too far! Dist: {distance:F1} / Radius: {interactionRadius}");

            // Optional: Show "Too far" dialogue
            if (DialogueSystemV2.Instance != null && !string.IsNullOrEmpty(tooFarMessage))
            {
                // Only show if not already talking (prevents spamming)
                if (!DialogueSystemV2.Instance.IsDialogueActive())
                {
                    DialogueSystemV2.Instance.StartDialogue(tooFarMessage, "Lisa");
                }
            }
            return;
        }

        // 3. Perform Interaction (In Range)
        Debug.Log($"[Interaction] Interacting with {gameObject.name}");

        // Play Sound
        if (interactionSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(interactionSFX);
        }

        // Show Dialogue
        if (!string.IsNullOrEmpty(interactionDialogue) && DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(interactionDialogue, speakerName);

            DialogueSystemV2.Instance.OnDialogueEnded -= OnDialogueEnded;
            DialogueSystemV2.Instance.OnDialogueEnded += OnDialogueEnded;
        }
        else
        {
            OnDialogueEnded();
        }
    }

    protected virtual void OnDialogueEnded()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.OnDialogueEnded -= OnDialogueEnded;
        }
    }

    // Visualization for the Editor
    protected virtual void OnDrawGizmosSelected()
    {
        if (debugRadius)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}