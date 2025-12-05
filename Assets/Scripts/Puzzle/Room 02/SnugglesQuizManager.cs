using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SnugglesQuizManager : MonoBehaviour
{
    public static SnugglesQuizManager Instance { get; private set; }

    [Header("References")]
    public MrSnugglesController snuggles;
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button; // correct
    public Button option4Button;

    [Header("Button Labels")]
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public TextMeshProUGUI option4Text;

    // Public getter so DiaryReaderUI can check if we are busy even before the panel opens
    public bool IsEventPendingOrActive => IsPanelVisible() || pendingQuiz || waitingRetry;

    [Header("Debug")]
    [SerializeField] private bool pendingQuiz;
    [SerializeField] private bool waitingRetry;
    private JoystickPlayerController playerController;
    private CanvasGroup panelCanvasGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // INITIALIZATION & VISIBILITY FIX
        if (quizPanel != null)
        {
            // Get or Add CanvasGroup to handle visibility without disabling the GameObject
            panelCanvasGroup = quizPanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = quizPanel.AddComponent<CanvasGroup>();
            }

            // Force hide at start
            HidePanel();
        }

        // Find text components if not assigned
        if (option1Text == null && option1Button != null)
            option1Text = option1Button.GetComponentInChildren<TextMeshProUGUI>();
        if (option2Text == null && option2Button != null)
            option2Text = option2Button.GetComponentInChildren<TextMeshProUGUI>();
        if (option3Text == null && option3Button != null)
            option3Text = option3Button.GetComponentInChildren<TextMeshProUGUI>();
        if (option4Text == null && option4Button != null)
            option4Text = option4Button.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        // SAFETY RESET: Ensure pending flags are false on boot to prevent instant triggers
        // unless explicitly armed by the Controller later.
        pendingQuiz = false;
        waitingRetry = false;
        HidePanel();

        playerController = FindFirstObjectByType<JoystickPlayerController>();

        // Ensure subscription happens
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
        DiaryReaderUI.OnDiaryClosed += OnDiaryClosed;
        Debug.Log("[DEBUG_TRACE] SnugglesQuizManager initialized and subscribed.");
    }

    void OnDestroy()
    {
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
    }

    public void ArmQuizOnNextDiaryClose()
    {
        if (snuggles == null)
        {
            Debug.LogWarning("[DEBUG_TRACE] [SnugglesQuiz] MrSnugglesController reference is null!");
            return;
        }

        // Allow re-arming if it's just pending (idempotent)
        if (pendingQuiz)
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Quiz already armed. Ignoring duplicate request.");
            return;
        }

        if (waitingRetry)
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Waiting for retry. Ignoring Arm request.");
            return;
        }

        pendingQuiz = true;
        Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] >>> QUIZ ARMED! It will trigger on next diary close.");
    }

    void OnDiaryClosed()
    {
        Debug.Log($"[DEBUG_TRACE] [SnugglesQuiz] OnDiaryClosed. Pending={pendingQuiz}, Retry={waitingRetry}");

        // If waiting for retry after wrong answer, show quiz again
        if (waitingRetry)
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Triggering Retry sequence.");
            waitingRetry = false;
            StartCoroutine(ShowQuizAfterDelay());
            return;
        }

        // If quiz is armed (first time), show it
        if (pendingQuiz)
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Triggering Armed Quiz sequence.");
            StartCoroutine(ShowQuizAfterDelay());
        }
        else
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Quiz NOT pending. Doing nothing.");
        }
    }

    IEnumerator ShowQuizAfterDelay()
    {
        // Wait a frame to ensure diary is fully closed
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.15f);

        ShowQuiz();
    }

    void ShowQuiz()
    {
        Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] >>> SHOWING QUIZ NOW");
        pendingQuiz = false;

        if (quizPanel == null)
        {
            Debug.LogError("[DEBUG_TRACE] [SnugglesQuiz] Quiz panel is null!");
            return;
        }

        // Disable player controls
        if (playerController != null)
            playerController.enabled = false;

        // Show quiz panel using CanvasGroup or SetActive
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 1f;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            quizPanel.SetActive(true);
        }

        // Set question text
        if (questionText != null)
            questionText.text = "What page did Mr. Snuggles get mentioned?";

        // Set button labels
        if (option1Text != null) option1Text.text = "Page 1";
        if (option2Text != null) option2Text.text = "Page 2";
        if (option3Text != null) option3Text.text = "Page 3";
        if (option4Text != null) option4Text.text = "Page 4";

        // Clear previous listeners
        option1Button?.onClick.RemoveAllListeners();
        option2Button?.onClick.RemoveAllListeners();
        option3Button?.onClick.RemoveAllListeners();
        option4Button?.onClick.RemoveAllListeners();

        // Add new listeners
        option1Button?.onClick.AddListener(() => Submit(1));
        option2Button?.onClick.AddListener(() => Submit(2));
        option3Button?.onClick.AddListener(() => Submit(3)); // ✅ correct
        option4Button?.onClick.AddListener(() => Submit(4));
    }

    void Submit(int choice)
    {
        Debug.Log($"[DEBUG_TRACE] [SnugglesQuiz] Player selected option {choice}");

        // Hide quiz panel
        HidePanel();

        // Re-enable player controls
        if (playerController != null)
            playerController.enabled = true;

        if (choice == 3) // Correct answer
        {
            StartCoroutine(HandleCorrectAnswer());
        }
        else // Wrong answer
        {
            StartCoroutine(HandleWrongAnswer());
        }
    }

    private void HidePanel()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
        else if (quizPanel != null && quizPanel != gameObject)
        {
            quizPanel.SetActive(false);
        }
    }

    private bool IsPanelVisible()
    {
        if (panelCanvasGroup != null) return panelCanvasGroup.alpha > 0;
        return quizPanel != null && quizPanel.activeSelf;
    }

    IEnumerator HandleCorrectAnswer()
    {
        Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Answer Correct.");
        // Show success dialogue
        DialogueSystemV2.Instance?.StartDialogue("Got it!", "Lisa");

        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        // Mark quiz as solved
        snuggles?.MarkQuizSolved();
    }

    IEnumerator HandleWrongAnswer()
    {
        Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Answer Wrong. Retrying.");
        // Show wrong answer dialogue
        DialogueSystemV2.Instance?.StartDialogue("Wait, that's not correct...", "Lisa");

        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.3f);

        // Set flag to retry after diary closes again
        waitingRetry = true;

        // Reopen the diary
        if (DiaryReaderUI.Instance != null)
        {
            Debug.Log("[DEBUG_TRACE] [SnugglesQuiz] Reopening diary for retry...");
            DiaryReaderUI.Instance.ShowDiary();
        }
        else
        {
            Debug.LogError("[DEBUG_TRACE] [SnugglesQuiz] DiaryReaderUI.Instance is null!");
        }
    }
}