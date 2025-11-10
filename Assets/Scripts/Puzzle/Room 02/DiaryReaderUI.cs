using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Persistent diary UI. Displays pages from GlobalDiaryManager (not from inventory).
/// After closing, if the knowledge flag isn't set, triggers a quiz. Wrong answers loop:
/// "Ah. Let's try again." then automatically re-open the diary.
/// </summary>
public class DiaryReaderUI : MonoBehaviour
{
    public static DiaryReaderUI Instance { get; private set; }

    [Header("UI Components")]
    public GameObject diaryPanel;
    public Image diaryPageImage;
    public Button closeButton;
    public Button nextPageButton;
    public Button previousPageButton;
    public TextMeshProUGUI pageNumberText;
    public TextMeshProUGUI titleText;

    [Header("Player UI References")]
    public GameObject joystickObject;

    [Header("Empty State")]
    public string emptyDiaryMessage = "No diary pages collected yet.";
    public TextMeshProUGUI emptyStateText;

    [Header("Display Settings")]
    public string diaryTitle = "Diary Entries";

    private List<Sprite> currentPages = new List<Sprite>();
    private int currentPageIndex = 0;
    private JoystickPlayerController playerController;
    private bool isInitialized = false;

    // quiz flags
    private const string FLAG_UNDERSTOOD = "understood_snuggles_clue";
    private const string FLAG_QUIZ_DONE = "understood_snuggles_clue_quiz_done";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[DiaryReaderUI] Instance created and set to persist");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        InitializeUI();
    }

    void OnEnable() { SubscribeToEvents(); }
    void OnDisable() { UnsubscribeFromEvents(); }
    void OnDestroy() { UnsubscribeFromEvents(); }

    void InitializeUI()
    {
        if (isInitialized) return;

        if (diaryPanel != null)
            diaryPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDiary);

        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);

        if (previousPageButton != null)
            previousPageButton.onClick.AddListener(PreviousPage);

        SubscribeToEvents();
        RefreshPages();

        isInitialized = true;
    }

    void SubscribeToEvents()
    {
        if (GlobalDiaryManager.Instance != null)
        {
            GlobalDiaryManager.Instance.OnPagesChanged -= OnPagesChanged;
            GlobalDiaryManager.Instance.OnPagesChanged += OnPagesChanged;
        }
    }

    void UnsubscribeFromEvents()
    {
        if (GlobalDiaryManager.Instance != null)
            GlobalDiaryManager.Instance.OnPagesChanged -= OnPagesChanged;
    }

    void OnPagesChanged()
    {
        RefreshPages();
    }

    void RefreshPages()
    {
        if (GlobalDiaryManager.Instance != null)
        {
            currentPages = GlobalDiaryManager.Instance.GetCollectedSprites();
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, Mathf.Max(0, currentPages.Count - 1));
        }
        else
        {
            currentPages.Clear();
        }

        if (diaryPanel != null && diaryPanel.activeSelf)
            DisplayCurrentPage();
    }

    public void ShowDiary()
    {
        if (!isInitialized) InitializeUI();

        RefreshPages();

        if (diaryPanel != null)
            diaryPanel.SetActive(true);

        if (titleText != null)
            titleText.text = diaryTitle;

        DisplayCurrentPage();
        DisablePlayerControls();

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.CloseInventoryUI();
    }

    public void CloseDiary()
    {
        if (diaryPanel != null)
            diaryPanel.SetActive(false);

        EnablePlayerControls();
        StartCoroutine(PostDiaryQuizIfNeeded());
    }

    void DisplayCurrentPage()
    {
        bool hasPages = currentPages != null && currentPages.Count > 0;

        if (emptyStateText != null)
        {
            emptyStateText.gameObject.SetActive(!hasPages);
            if (!hasPages) emptyStateText.text = emptyDiaryMessage;
        }

        if (diaryPageImage != null)
        {
            diaryPageImage.gameObject.SetActive(hasPages);
            if (hasPages)
            {
                diaryPageImage.sprite = currentPages[currentPageIndex];
                diaryPageImage.SetNativeSize();
            }
        }

        if (pageNumberText != null)
        {
            pageNumberText.text = hasPages ? $"Page {currentPageIndex + 1} of {currentPages.Count}" : "";
        }

        if (previousPageButton != null)
            previousPageButton.interactable = hasPages && currentPageIndex > 0;

        if (nextPageButton != null)
            nextPageButton.interactable = hasPages && currentPageIndex < currentPages.Count - 1;
    }

    void NextPage()
    {
        if (currentPages != null && currentPageIndex < currentPages.Count - 1)
        {
            currentPageIndex++;
            DisplayCurrentPage();
        }
    }

    void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayCurrentPage();
        }
    }

    void DisablePlayerControls()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<JoystickPlayerController>();

        if (playerController != null) playerController.enabled = false;
        if (joystickObject != null) joystickObject.SetActive(false);
    }

    void EnablePlayerControls()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<JoystickPlayerController>();

        if (playerController != null) playerController.enabled = true;
        if (joystickObject != null) joystickObject.SetActive(true);
    }

    public bool IsReaderOpen()
    {
        return diaryPanel != null && diaryPanel.activeSelf;
    }

    public void OpenDiaryFromMenu()
    {
        ShowDiary();
    }

    IEnumerator PostDiaryQuizIfNeeded()
    {
        // Only if player has the combined diary and hasn't passed the quiz yet
        if (!SaveSystem.Instance.HasItem("diary_entries")) yield break;
        if (SaveSystem.Instance.WasDialogueTriggered(FLAG_QUIZ_DONE)) yield break;

        yield return null; // small delay for UI settle

        DialogueSystemV2.Instance?.StartDialogue(
            "I think I read something about this teddy bear in the diary entry... Which diary entry was it referring to?",
            "Lisa"
        );

        // Wait for this line to display
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        ShowQuiz();
    }

    void ShowQuiz()
    {
        DialogueSystemV2.Instance?.ShowChoices(
            new string[]
            {
                "The teddy bear.",
                "The music box.",
                "The photograph."
            },
            new System.Action[]
            {
                // Correct
                () =>
                {
                    SaveSystem.Instance.TriggerDialogue(FLAG_UNDERSTOOD);
                    SaveSystem.Instance.TriggerDialogue(FLAG_QUIZ_DONE);
                    SaveSystem.Instance.OnStoryProgressMade();

                    DialogueSystemV2.Instance?.StartDialogue(
                        "Right... that entry was pointing me toward the teddy bear.",
                        "Lisa"
                    );
                },
                // Wrong
                () => RetryAfterWrong(),
                // Wrong
                () => RetryAfterWrong()
            }
        );
    }

    void RetryAfterWrong()
    {
        DialogueSystemV2.Instance?.StartDialogue("Ah. Let's try again.", "Lisa");
        StartCoroutine(ReopenAfterLine());
    }

    IEnumerator ReopenAfterLine()
    {
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        ShowDiary(); // reopen the diary so player can read again
    }

    // Debug helpers
    [ContextMenu("Test: Open Diary")]
    void TestOpenDiary() => ShowDiary();

    [ContextMenu("Test: Close Diary")]
    void TestCloseDiary() => CloseDiary();
}
