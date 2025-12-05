using UnityEngine;

// FIX: Added RequireComponent to ensure Tracker sees it
[RequireComponent(typeof(BoxCollider2D))]
public class RugLockedInteractable : SimpleInteractable2D
{
    [Header("Locked Settings")]
    public AudioClip lockedSFX;

    // FIX: Auto-configure collider
    private void Reset()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null) box.isTrigger = true;
    }

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(interactionDialogue))
            interactionDialogue = "It's locked.";

        // Layer check
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
            Debug.LogWarning($"[Rug] '{name}' is on Default layer. Check Tracker settings.", this);
    }

    // FIX: Ensure Public Override
    public override void Interact()
    {
        if (lockedSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(lockedSFX);
        }

        base.Interact();
    }
}