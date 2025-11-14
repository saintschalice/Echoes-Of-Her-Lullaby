using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private bool pendingQuiz;
    private bool waitingRetry;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (quizPanel != null) quizPanel.SetActive(false);
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
        if (snuggles == null) return;
        if (pendingQuiz || waitingRetry) return;
        pendingQuiz = true;
    }

    void OnDiaryClosed()
    {
        if (waitingRetry)
        {
            waitingRetry = false;
            ShowQuiz();
            return;
        }

        if (pendingQuiz)
        {
            ShowQuiz();
        }
    }

    void ShowQuiz()
    {
        pendingQuiz = false;
        if (quizPanel == null) return;

        quizPanel.SetActive(true);
        if (questionText) questionText.text = "What page references Mr. Snuggles again?";

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();
        option3Button.onClick.RemoveAllListeners();
        option4Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(() => Submit(1));
        option2Button.onClick.AddListener(() => Submit(2));
        option3Button.onClick.AddListener(() => Submit(3)); // ✅ correct
        option4Button.onClick.AddListener(() => Submit(4));
    }

    void Submit(int choice)
    {
        quizPanel.SetActive(false);

        if (choice == 3)
        {
            DialogueSystemV2.Instance?.StartDialogue("That seems to be correct.", "Lisa");
            snuggles?.MarkQuizSolved();
        }
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("Wait, that's not correct...", "Lisa");
            waitingRetry = true;
            DiaryReaderUI.Instance?.ShowDiary();

        }
    }
}
