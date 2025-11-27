using UnityEngine;

/// <summary>
/// Handles table notes pickup with proper audio integration
/// </summary>
public class TableInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    public GameObject tableNotesObject;

    [Header("Audio")]
    public AudioClip pickupSound; // Assign in inspector

    [Header("Settings")]
    public string diaryPageId = "diary_page_1";
    [TextArea] public string pickupDialogue = "These diary pages... they're covered in blood.";

    private bool hasBeenPickedUp = false;

    void PickupNotes()
    {
        hasBeenPickedUp = true;

        // Hide only the notes sprite
        if (tableNotesObject != null)
        {
            tableNotesObject.SetActive(false);
        }

        // Add diary pages through GlobalDiaryManager
        if (GlobalDiaryManager.Instance != null)
        {
            GlobalDiaryManager.Instance.AddDiaryPage(diaryPageId);
        }
        else
        {
            Debug.LogWarning("[TableInteractable] GlobalDiaryManager not found!");
        }

        // Show dialogue
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(pickupDialogue, "Lisa");
        }
        else
        {
            Debug.LogWarning("[TableInteractable] DialogueSystemV2 not found!");
        }

        // Play pickup sound if assigned
        if (pickupSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(pickupSound);
        }

        Debug.Log($"[TableInteractable] Picked up diary page: {diaryPageId}");
    }

    public void Interact()
    {
        if (!hasBeenPickedUp)
        {
            PickupNotes();
        }
    }

    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }
}