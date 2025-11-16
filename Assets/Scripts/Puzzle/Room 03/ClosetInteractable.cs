using UnityEngine;

public class HallwayClosetInteractable : MonoBehaviour
{
    [Header("References")]
    public Animator closetAnimator;
    public ClosetHideSequence hideSequence; // reference to hiding script

    [Header("Audio")]
    public AudioClip scratchSound;
    public AudioClip doorCreakSound;

    [Header("Dialogue")]
    [TextArea]
    public string firstDialogue = "There are scratches inside... someone was trying to get out.";

    [Header("Settings")]
    public float interactionRange = 2f;
    public GameObject interactPrompt;

    private Transform player;
    private bool firstExamined = false;
    private bool canHide = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool inRange = distance <= interactionRange;

        if (interactPrompt != null)
            interactPrompt.SetActive(inRange && !DialogueSystemV2.Instance.IsDialogueActive());

        // Touch interaction
        if (inRange && Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                HandleClosetInteraction();
        }
    }

    void HandleClosetInteraction()
    {
        if (!firstExamined)
        {
            StartCoroutine(FirstExamineRoutine());
        }
        else if (canHide)
        {
            hideSequence?.HideInCloset();
        }
    }

    System.Collections.IEnumerator FirstExamineRoutine()
    {
        firstExamined = true;

        // Open
        closetAnimator?.SetTrigger("Open");
        if (doorCreakSound) AudioManager.Instance?.PlaySFX(doorCreakSound);
        yield return new WaitForSeconds(0.5f);

        // Dialogue
        DialogueSystemV2.Instance?.StartDialogue(firstDialogue, "Lisa");
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        // Scratching sound
        if (scratchSound) AudioManager.Instance?.PlaySFX(scratchSound);
        yield return new WaitForSeconds(1.5f);

        // Close again
        closetAnimator?.SetTrigger("Close");
        if (doorCreakSound) AudioManager.Instance?.PlaySFX(doorCreakSound);

        // Enable hiding only after Emily has appeared
        yield return new WaitForSeconds(0.5f);
        canHide = true;
        Debug.Log("[HallwayClosetInteractable] Closet examined, can now be used for hiding.");
    }
}
