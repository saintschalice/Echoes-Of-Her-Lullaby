using UnityEngine;
using UnityEngine.SceneManagement;

public class VentInteractable : MonoBehaviour
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

    void Update()
    {
        if (player == null) return;

        // Check distance
        float distance = Vector2.Distance(transform.position, player.position);
        bool inRange = distance <= interactionRange;

        // Show UI prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(inRange && !DialogueSystemV2.Instance.IsDialogueActive());

        // Interaction
        if (inRange && Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                EnterVent();
            }
        }
    }

    void EnterVent()
    {
        if (ventSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(ventSound);
        }

        Debug.Log("Exiting to Kitchen...");
        SceneManager.LoadScene(nextSceneName);
    }
}