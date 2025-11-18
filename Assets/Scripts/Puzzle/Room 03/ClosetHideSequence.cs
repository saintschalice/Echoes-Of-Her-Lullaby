using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClosetHideSequence : MonoBehaviour
{
    [Header("References")]
    public Animator closetAnimator;
    public CanvasGroup fadeCanvas;
    public Button exitButton;
    public AudioClip muffledLoop;
    public float safeDistance = 8f;
    public Vector2 exitOffset = new Vector2(0f, -1.5f);

    [Header("Post-Hide Narrative")]
    [TextArea] public string exitWhisper = "Not ready... never ready... too much pain...";

    private Transform player;
    private Collider2D playerCollider;
    private string originalTag;
    private bool isHiding = false;
    private bool canHide = false; // Updated by Interactable or external checks

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            playerCollider = player.GetComponent<Collider2D>();

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0;
            fadeCanvas.interactable = false;
            fadeCanvas.blocksRaycasts = false;
        }

        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(false);
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(GetOutOfCloset);
        }
    }

    // Called by HallwayClosetInteractable
    public void HideInCloset()
    {
        if (isHiding) return;

        // Safety check: ensure Emily exists or logic allows it
        // For this specific narrative, the Interactable script handles the "Unlock" check
        StartCoroutine(HideRoutine());
    }

    IEnumerator HideRoutine()
    {
        isHiding = true;

        // Disable player controls
        var controller = player.GetComponent<JoystickPlayerController>();
        if (controller) controller.enabled = false;

        // Make Lisa invisible to Emily
        if (player != null)
        {
            originalTag = player.tag;
            player.tag = "Untagged"; // Emily ignores her now
        }
        if (playerCollider != null)
            playerCollider.enabled = false;

        closetAnimator?.SetTrigger("Hide");
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(Fade(1f, 1f));
        if (fadeCanvas != null)
        {
            fadeCanvas.interactable = true;
            fadeCanvas.blocksRaycasts = true;
        }

        // Muffled ambient
        if (muffledLoop != null)
            LoopingSoundManager.Instance.PlayLoopingSound(muffledLoop, "closet_muffle", 0.6f);

        if (exitButton != null)
            exitButton.gameObject.SetActive(true);

        // Wait for player to click exit...
        // Note: In the original prompt logic, we might want to force the player to wait
        // until Emily is far away, but for now we let the Exit Button handle the "Exit" command
        // which triggers GetOutOfCloset()
    }

    public void GetOutOfCloset()
    {
        if (!isHiding) return;

        // Optional: Check if safe before allowing exit? 
        // For now, we assume player decides when to risk it.
        StartCoroutine(ExitRoutine());
    }

    IEnumerator ExitRoutine()
    {
        if (exitButton != null)
            exitButton.gameObject.SetActive(false);

        LoopingSoundManager.Instance.StopLoopingSound("closet_muffle");
        yield return StartCoroutine(Fade(0f, 1f));

        if (fadeCanvas != null)
        {
            fadeCanvas.interactable = false;
            fadeCanvas.blocksRaycasts = false;
        }

        // Re-enable Lisa visibility and controls
        if (player != null)
            player.tag = originalTag;
        if (playerCollider != null)
            playerCollider.enabled = true;

        var controller = player.GetComponent<JoystickPlayerController>();
        if (controller) controller.enabled = true;

        // Move Lisa to exit position
        if (player != null)
        {
            Vector3 newPos = transform.position + (Vector3)exitOffset;
            player.position = newPos;
        }

        // Exit animation
        closetAnimator?.SetTrigger("Open");
        yield return new WaitForSeconds(0.5f);
        closetAnimator?.SetTrigger("Close");

        isHiding = false;

        // --- NEW NARRATIVE ADDITION ---
        yield return new WaitForSeconds(0.5f); // Small pause after exiting
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(exitWhisper, "???");
        }
    }

    IEnumerator Fade(float target, float duration)
    {
        if (fadeCanvas == null) yield break;
        float start = fadeCanvas.alpha;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            fadeCanvas.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }
        fadeCanvas.alpha = target;
    }
}