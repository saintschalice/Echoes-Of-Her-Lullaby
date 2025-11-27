using UnityEngine;
using UnityEngine.SceneManagement;

public class VentInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public string nextSceneName = "Room04_KitchenDining";
    public float interactionRange = 2f;
    public GameObject interactPrompt;
    public AudioClip ventSound;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    public void OnInteract(PlayerContext context)
    {
        player = context.Transform;

        if (!IsInRange(player) || (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()))
            return;

        if (ventSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(ventSound);
        }

        Debug.Log("Exiting to Kitchen...");
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnFocus(PlayerContext context)
    {
        player = context.Transform;
        bool inRange = IsInRange(player);
        if (interactPrompt != null)
        {
            bool canShow = inRange && (DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
            interactPrompt.SetActive(canShow);
        }
    }

    public void OnBlur(PlayerContext context)
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    bool IsInRange(Transform target)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= interactionRange;
    }
}