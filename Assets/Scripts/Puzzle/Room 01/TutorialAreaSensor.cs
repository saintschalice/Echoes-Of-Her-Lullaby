using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TutorialAreaSensor : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Only triggers if the tutorial hasn't been completed yet.")]
    public bool oneTimeUse = true;

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && oneTimeUse) return;

        if (other.CompareTag("Player"))
        {
            // Only trigger if we are in a valid tutorial state
            if (TutorialManager.Instance != null && !TutorialManager.Instance.IsTutorialCompleted())
            {
                Debug.Log("[TutorialSensor] Player entered sensor area. Triggering interaction tutorial.");
                TutorialManager.Instance.TriggerInteractionTutorial();
                hasTriggered = true;

                if (oneTimeUse)
                {
                    // Disable collider so it doesn't trigger again immediately
                    GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
}