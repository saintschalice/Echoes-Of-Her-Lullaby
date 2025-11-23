using UnityEngine;

public class DoorUnlockInteractable : SimpleInteractable2D
{
    [Header("Unlock Settings")]
    public AudioClip unlockSFX;
    public bool isUnlocked = false;

    protected override void Start()
    {
        base.Start();
        if (string.IsNullOrEmpty(interactionDialogue))
            interactionDialogue = "I can unlock this now.";
    }

    public override void Interact()
    {
        // Logic for future phases: Check if we HAVE the key/condition
        // For now, we assume this interaction performs the unlock

        if (!isUnlocked)
        {
            if (unlockSFX != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(unlockSFX);
            }

            isUnlocked = true;
            Debug.Log($"[Door] Unlocked {gameObject.name}");
        }

        base.Interact();
    }
}