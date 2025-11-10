using UnityEngine;

public class DiaryPagePickup : MonoBehaviour
{
    public string pageId;
    [TextArea] public string pickupDialogue = "Another diary page...";
    public AudioClip pickupSFX; // assign a short pickup sound in the Inspector (optional)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Collect();
    }

    void Collect()
    {
        if (GlobalDiaryManager.Instance == null)
        {
            Debug.LogError("[DiaryPagePickup] No GlobalDiaryManager found in scene!");
            return;
        }

        // Register diary page
        GlobalDiaryManager.Instance.AddDiaryPage(pageId);

        // Feedback
        DialogueSystemV2.Instance?.StartDialogue(pickupDialogue, "Lisa");

        // Play a pickup sound if provided
        if (pickupSFX != null)
            AudioManager.Instance?.PlaySFX(pickupSFX);

        // Destroy to prevent duplicate pickup
        Destroy(gameObject);
    }
}
