using UnityEngine;

public class RugLockedInteractable : SimpleInteractable2D
{
    [Header("Locked Settings")]
    public AudioClip lockedSFX; // Specific locked sound if different from generic interaction

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(interactionDialogue))
            interactionDialogue = "It's locked.";
    }

    // =================================================================================
    // FIX: Confirmed this is 'public' to satisfy the button requirement.
    // =================================================================================
    public override void Interact()
    {
        // Play specific locked sound if assigned, otherwise base handles it
        if (lockedSFX != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(lockedSFX);
        }

        base.Interact(); // Shows dialogue
    }
}