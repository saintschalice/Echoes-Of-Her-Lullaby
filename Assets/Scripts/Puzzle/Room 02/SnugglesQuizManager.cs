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

    private bool pendingQuiz;
    private bool waitingRetry;
    private JoystickPlayerController playerController;

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

        if (quizPanel != null)
            quizPanel.SetActive(false);

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
        playerController = FindFirstObjectByType<JoystickPlayerController>();
    }

    void OnEnable()
    {
        DiaryReaderUI.OnDiaryClosed += OnDiaryClosed;
    }

    void OnDisable()
    {
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
    }

    public void ArmQuizOnNextDiaryClose()
    {
        if (snuggles == null)
        {
            Debug.LogWarning("[SnugglesQuiz] MrSnugglesController reference is null!");
            return;
        }

        if (pendingQuiz || waitingRetry)
        {
            Debug.Log("[SnugglesQuiz] Quiz already armed or waiting for retry");
            return;
        }

        pendingQuiz = true;
        Debug.Log("[SnugglesQuiz] Quiz armed and will trigger on next diary close");
    }

    void OnDiaryClosed()
    {
        Debug.Log($"[SnugglesQuiz] Diary closed. pendingQuiz={pendingQuiz}, waitingRetry={waitingRetry}");

        // If waiting for retry after wrong answer, show quiz again
        if (waitingRetry)
        {
            waitingRetry = false;
            StartCoroutine(ShowQuizAfterDelay());
            return;
        }

        // If quiz is armed (first time), show it
        if (pendingQuiz)
        {
            StartCoroutine(ShowQuizAfterDelay());
        }
    }

    IEnumerator ShowQuizAfterDelay()
    {
        // Wait a frame to ensure diary is fully closed
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);

        ShowQuiz();
    }

    void ShowQuiz()
    {
        pendingQuiz = false;

        if (quizPanel == null)
        {
            Debug.LogError("[SnugglesQuiz] Quiz panel is null!");
            return;
        }

        // Disable player controls
        if (playerController != null)
            playerController.enabled = false;

        // Show quiz panel
        quizPanel.SetActive(true);

        // Set question text
        if (questionText != null)
            questionText.text = "What page mentioned Mr. Snuggles?";

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

        Debug.Log("[SnugglesQuiz] Quiz displayed");
    }

    void Submit(int choice)
    {
        Debug.Log($"[SnugglesQuiz] Player selected option {choice}");

        // Hide quiz panel
        if (quizPanel != null)
            quizPanel.SetActive(false);

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

    IEnumerator HandleCorrectAnswer()
    {
        // Show success dialogue
        DialogueSystemV2.Instance?.StartDialogue("Got it!", "Lisa");

        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        // Mark quiz as solved
        snuggles?.MarkQuizSolved();

        Debug.Log("[SnugglesQuiz] Correct answer! Quiz completed.");
    }

    IEnumerator HandleWrongAnswer()
    {
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
            Debug.Log("[SnugglesQuiz] Wrong answer - reopening diary");
            DiaryReaderUI.Instance.ShowDiary();
        }
        else
        {
            Debug.LogError("[SnugglesQuiz] DiaryReaderUI.Instance is null!");
        }
    }
}