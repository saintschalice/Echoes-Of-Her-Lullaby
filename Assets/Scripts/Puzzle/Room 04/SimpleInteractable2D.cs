using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class SimpleInteractable2D : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("If true, the collider is a Trigger. If false, it's a physical wall.")]
    public bool isTrigger = true;
    public bool interactable = true;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    public string interactionDialogue;
    public string speakerName = "Lisa";

    [Header("Audio")]
    public AudioClip interactionSFX;

    protected bool isPlayerInRange = false;

    protected virtual void Start()
    {
        // Ensure collider is set up correctly
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = isTrigger;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Optional: Show an "Interact" UI button here if you have one
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Optional: Hide "Interact" UI button here
        }
    }

    /// <summary>
    /// Call this method from your Input System (e.g. Interaction Button onClick).
    /// </summary>
    public virtual void Interact()
    {
        if (!interactable || !isPlayerInRange) return;

        Debug.Log($"[Interaction] Interacting with {gameObject.name}");

        // 1. Play Sound
        if (interactionSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(interactionSFX);
        }

        // 2. Show Dialogue
        if (!string.IsNullOrEmpty(interactionDialogue) && DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(interactionDialogue, speakerName);

            // Hook into dialogue end for post-interaction logic (like pickups)
            DialogueSystemV2.Instance.OnDialogueEnded -= OnDialogueEnded; // Safety remove
            DialogueSystemV2.Instance.OnDialogueEnded += OnDialogueEnded;
        }
        else
        {
            // If no dialogue, trigger end logic immediately
            OnDialogueEnded();
        }
    }

    /// <summary>
    /// Override this to add logic AFTER the dialogue finishes (e.g. adding item to inventory).
    /// </summary>
    protected virtual void OnDialogueEnded()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.OnDialogueEnded -= OnDialogueEnded;
        }
    }
}