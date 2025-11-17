using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnugglesQuizManager : MonoBehaviour
{
    public static SnugglesQuizManager Instance { get; private set; }

    [Header("Quiz State")]
    private bool hasOpenedDiaryOnce = false;
    private bool hasExaminedTeddy = false;
    private bool hasReopenedDiaryAfterExamine = false;
    private bool hasAnsweredCorrectly = false;

    [Header("Quiz UI")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public TextMeshProUGUI option4Text;

    [Header("Feedback")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public Button feedbackOkButton;

    [Header("Quiz Content")]
    public string quizQuestion = "Where in the diary was Snuggles mentioned?";
    public string option1Label = "Page 1 - The Beginning";
    public string option2Label = "Page 2 - The Garden";
    public string option3Label = "Page 3 - Lisa's Room"; // CORRECT
    public string option4Label = "Page 4 - The Attic";
    public string correctFeedback = "Correct! You remember now. There's something inside Snuggles.";
    public string wrongFeedback = "That's not right... Maybe I should read the diary again.";

    [Header("Items")]
    [Tooltip("Item ID for Mr. Snuggles teddy bear")]
    public string snugglesItemId = "mr_snuggles";

    [Tooltip("Item ID for the winding key")]
    public string windingKeyItemId = "winding_key";

    private const int CORRECT_ANSWER = 3; // Option 3 is correct

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
    }

    void Start()
    {
        SetupUI();
        LoadState();
    }

    void OnEnable()
    {
        DiaryReaderUI.OnDiaryClosed += OnDiaryClosed;
    }

    void OnDisable()
    {
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
    }

    void SetupUI()
    {
        if (quizPanel != null) quizPanel.SetActive(false);
        if (feedbackPanel != null) feedbackPanel.SetActive(false);

        if (option1Button != null)
            option1Button.onClick.AddListener(() => SubmitAnswer(1));
        if (option2Button != null)
            option2Button.onClick.AddListener(() => SubmitAnswer(2));
        if (option3Button != null)
            option3Button.onClick.AddListener(() => SubmitAnswer(3));
        if (option4Button != null)
            option4Button.onClick.AddListener(() => SubmitAnswer(4));

        if (feedbackOkButton != null)
            feedbackOkButton.onClick.AddListener(CloseFeedback);

        if (option1Text != null) option1Text.text = option1Label;
        if (option2Text != null) option2Text.text = option2Label;
        if (option3Text != null) option3Text.text = option3Label;
        if (option4Text != null) option4Text.text = option4Label;
    }

    #region Public API

    /// <summary>
    /// Call this when the player examines the teddy bear from inventory
    /// </summary>
    public void OnTeddyBearExamined()
    {
        Debug.Log($"[SnugglesQuiz] Teddy examined. State: openedDiary={hasOpenedDiaryOnce}, examined={hasExaminedTeddy}, answered={hasAnsweredCorrectly}");

        // Case 1: Quiz solved - give the key NOW
        if (hasAnsweredCorrectly)
        {
            UnlockWindingKey();
            return;
        }

        // Case 2: Haven't opened diary yet
        if (!hasOpenedDiaryOnce)
        {
            ShowDialogue("It's an old teddy bear. I should explore more first.");
            return;
        }

        // Case 3: First examination after reading diary
        if (!hasExaminedTeddy)
        {
            hasExaminedTeddy = true;
            SaveState();
            ShowDialogue("I think I read about him somewhere in the diary entries...");

            Debug.Log("[SnugglesQuiz] First examine - opening diary");
            AutoOpenDiary();
            return;
        }

        // Case 4: Already examined, remind to check diary
        ShowDialogue("I should check the diary again to remember where Snuggles was mentioned.");

        Debug.Log("[SnugglesQuiz] Re-examine - opening diary");
        AutoOpenDiary();
    }

    /// <summary>
    /// Closes inventory and opens diary immediately
    /// </summary>
    void AutoOpenDiary()
    {
        // Close inventory first
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.CloseInventoryUI();
            Debug.Log("[SnugglesQuiz] Inventory closed");
        }

        // Open diary
        if (DiaryReaderUI.Instance != null)
        {
            DiaryReaderUI.Instance.ShowDiary();
            Debug.Log("[SnugglesQuiz] Diary opened");
        }
        else
        {
            Debug.LogError("[SnugglesQuiz] DiaryReaderUI.Instance is null! Cannot open diary.");
        }
    }

    System.Collections.IEnumerator OpenDiaryAfterDelay()
    {
        // Wait a bit for dialogue to show
        yield return new WaitForSeconds(0.5f);

        // Close inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.CloseInventoryUI();
        }

        // Open diary
        if (DiaryReaderUI.Instance != null && !DiaryReaderUI.Instance.IsReaderOpen())
        {
            DiaryReaderUI.Instance.ShowDiary();
        }
    }

    #endregion

    #region Quiz Flow

    void OnDiaryClosed()
    {
        Debug.Log($"[SnugglesQuiz] Diary closed. State: openedOnce={hasOpenedDiaryOnce}, examined={hasExaminedTeddy}, answered={hasAnsweredCorrectly}");

        // Track diary opened
        if (!hasOpenedDiaryOnce)
        {
            hasOpenedDiaryOnce = true;
            SaveState();
        }

        // Show quiz if conditions met
        if (hasExaminedTeddy && !hasAnsweredCorrectly)
        {
            hasReopenedDiaryAfterExamine = true;
            ShowQuiz();
        }
    }

    void ShowQuiz()
    {
        if (quizPanel == null)
        {
            Debug.LogError("[SnugglesQuiz] Quiz panel not assigned!");
            return;
        }

        if (questionText != null)
            questionText.text = quizQuestion;

        quizPanel.SetActive(true);
        DisablePlayerControls();

        Debug.Log("[SnugglesQuiz] Quiz shown");
    }

    void SubmitAnswer(int answerNumber)
    {
        Debug.Log($"[SnugglesQuiz] Answer submitted: {answerNumber}");

        if (answerNumber == CORRECT_ANSWER)
        {
            hasAnsweredCorrectly = true;
            SaveState();
            ShowFeedback(correctFeedback, true);
        }
        else
        {
            ShowFeedback(wrongFeedback, false);
        }

        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    void ShowFeedback(string message, bool isCorrect)
    {
        if (feedbackPanel == null || feedbackText == null)
        {
            Debug.LogError("[SnugglesQuiz] Feedback UI not assigned!");
            EnablePlayerControls();
            return;
        }

        feedbackText.text = message;
        feedbackPanel.SetActive(true);

        Debug.Log($"[SnugglesQuiz] Showing feedback - Correct: {isCorrect}");
    }

    void CloseFeedback()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        EnablePlayerControls();

        if (!hasAnsweredCorrectly)
        {
            // Wrong answer - reopen diary
            if (DiaryReaderUI.Instance != null)
            {
                DiaryReaderUI.Instance.ShowDiary();
            }
        }
        else
        {
            // Correct answer - hint to examine teddy again
            ShowDialogue("There should be something inside Mr. Snuggles. I should examine him again.");
        }
    }

    #endregion

    #region Winding Key

    void UnlockWindingKey()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("[SnugglesQuiz] InventoryManager not found!");
            return;
        }

        // Check if already has the key
        if (InventoryManager.Instance.HasItem(windingKeyItemId))
        {
            ShowDialogue("I already took the winding key from Mr. Snuggles.");
            return;
        }

        // Give the key
        InventoryManager.Instance.AddItem(windingKeyItemId);
        ShowDialogue("You found a winding key hidden inside Mr. Snuggles!");

        Debug.Log("[SnugglesQuiz] Winding key added to inventory");
    }

    #endregion

    #region Player Control

    void DisablePlayerControls()
    {
        var playerController = FindFirstObjectByType<JoystickPlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        var joystick = GameObject.Find("Joystick");
        if (joystick != null)
            joystick.SetActive(false);
    }

    void EnablePlayerControls()
    {
        var playerController = FindFirstObjectByType<JoystickPlayerController>();
        if (playerController != null)
            playerController.enabled = true;

        var joystick = GameObject.Find("Joystick");
        if (joystick != null)
            joystick.SetActive(true);
    }

    #endregion

    #region Dialogue Helper

    void ShowDialogue(string message)
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(message, "Lisa");
        }
        else
        {
            Debug.Log($"[SnugglesQuiz] Dialogue: {message}");
        }
    }

    #endregion

    #region Save/Load

    void SaveState()
    {
        PlayerPrefs.SetInt("Snuggles_OpenedDiary", hasOpenedDiaryOnce ? 1 : 0);
        PlayerPrefs.SetInt("Snuggles_ExaminedTeddy", hasExaminedTeddy ? 1 : 0);
        PlayerPrefs.SetInt("Snuggles_Reopened", hasReopenedDiaryAfterExamine ? 1 : 0);
        PlayerPrefs.SetInt("Snuggles_Answered", hasAnsweredCorrectly ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadState()
    {
        hasOpenedDiaryOnce = PlayerPrefs.GetInt("Snuggles_OpenedDiary", 0) == 1;
        hasExaminedTeddy = PlayerPrefs.GetInt("Snuggles_ExaminedTeddy", 0) == 1;
        hasReopenedDiaryAfterExamine = PlayerPrefs.GetInt("Snuggles_Reopened", 0) == 1;
        hasAnsweredCorrectly = PlayerPrefs.GetInt("Snuggles_Answered", 0) == 1;

        Debug.Log($"[SnugglesQuiz] State loaded: openedDiary={hasOpenedDiaryOnce}, examined={hasExaminedTeddy}, answered={hasAnsweredCorrectly}");
    }

    public void ResetQuiz()
    {
        hasOpenedDiaryOnce = false;
        hasExaminedTeddy = false;
        hasReopenedDiaryAfterExamine = false;
        hasAnsweredCorrectly = false;
        SaveState();
        Debug.Log("[SnugglesQuiz] Quiz reset");
    }

    #endregion

    #region Public Getters

    public bool HasAnsweredCorrectly() => hasAnsweredCorrectly;
    public bool HasExaminedTeddy() => hasExaminedTeddy;
    public bool HasOpenedDiary() => hasOpenedDiaryOnce;

    #endregion

    #region Context Menu

    [ContextMenu("Test: Examine Teddy")]
    void TestExamine() => OnTeddyBearExamined();

    [ContextMenu("Test: Reset Quiz")]
    void TestReset() => ResetQuiz();

    #endregion
}