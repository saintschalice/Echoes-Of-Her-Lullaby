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

    private Transform player;
    private Collider2D playerCollider;
    private string originalTag;
    private bool isHiding = false;
    private bool canHide = false;

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

    IEnumerator CheckEmilyStatusRoutine()
    {
        float checkInterval = 0.5f; // Only check 2 times per second

        while (true)
        {
            var emily = FindFirstObjectByType<EmilyAIController>();
            canHide = (emily != null && emily.isActiveAndEnabled);

            yield return new WaitForSeconds(checkInterval);
        }
    }

    public void HideInCloset()
    {
        if (isHiding) return;

        if (!canHide)
        {
            DialogueSystemV2.Instance?.StartDialogue("There's no reason to hide right now...", "Lisa");
            return;
        }

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

        // Wait until Emily is far enough
        while (true)
        {
            var emily = FindFirstObjectByType<EmilyAIController>();
            if (emily == null) break;

            float dist = Vector2.Distance(emily.transform.position, player.position);
            if (dist >= safeDistance)
                break;

            yield return null;
        }
    }

    public void GetOutOfCloset()
    {
        if (!isHiding) return;
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
