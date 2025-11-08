using UnityEngine;

public class DiaryPagePickup : MonoBehaviour
{
    public string pageId;
    [TextArea] public string pickupDialogue = "Another diary page...";

    public void Collect()
    {
        GlobalDiaryManager.Instance.AddDiaryPage(pageId);
        DialogueSystemV2.Instance?.StartDialogue(pickupDialogue, "Lisa");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Collect();
    }
}
