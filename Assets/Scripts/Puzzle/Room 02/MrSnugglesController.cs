using UnityEngine;

public class MrSnugglesController : MonoBehaviour
{
    [Header("IDs")]
    [SerializeField] private string diaryPage3Id = "diary_page_3";
    [SerializeField] private string windingKeyId = "winding_key";
    [SerializeField] private string musicBoxCompleteId = "music_box_complete";

    [Header("Save Flags")]
    private const string FLAG_DIARY_EXAMINED = "snuggles_diary_examined";
    private const string FLAG_SNUGGLES_EXAMINED = "snuggles_first_examined";
    private const string FLAG_QUIZ_ARMED = "snuggles_quiz_armed";
    private const string FLAG_QUIZ_SOLVED = "snuggles_quiz_solved";
    private const string FLAG_KEY_GIVEN = "snuggles_key_given";

    [Header("Debug View")]
    [SerializeField] private bool diaryExamined;
    [SerializeField] private bool snugglesExamined;
    [SerializeField] private bool quizArmed;
    [SerializeField] private bool quizSolved;
    [SerializeField] private bool windingKeyGiven;

    void Start()
    {
        // Subscribe to diary close event
        DiaryReaderUI.OnDiaryClosed += OnDiaryClosed;

        // Load state from SaveSystem
        LoadState();
    }

    void OnDestroy()
    {
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
    }

    void LoadState()
    {
        if (SaveSystem.Instance == null) return;

        diaryExamined = SaveSystem.Instance.WasDialogueTriggered(FLAG_DIARY_EXAMINED);
        snugglesExamined = SaveSystem.Instance.WasDialogueTriggered(FLAG_SNUGGLES_EXAMINED);
        quizArmed = SaveSystem.Instance.WasDialogueTriggered(FLAG_QUIZ_ARMED);
        quizSolved = SaveSystem.Instance.WasDialogueTriggered(FLAG_QUIZ_SOLVED);
        windingKeyGiven = SaveSystem.Instance.WasDialogueTriggered(FLAG_KEY_GIVEN);
    }

    // Call this from your interactable
    public void OnExamine()
    {
        // If music box is already complete, never give another key
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(musicBoxCompleteId))
        {
            Say("He feels empty now. I already used what he was hiding.");
            return;
        }

        // If quiz is solved and key not yet given → give winding key
        if (quizSolved && !windingKeyGiven)
        {
            GiveWindingKey();
            return;
        }

        // Handle the examination flow
        HandleExamination();
    }

    void HandleExamination()
    {
        // First examination ever
        if (!snugglesExamined)
        {
            Say("Why does he seem so familiar?");
            snugglesExamined = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_SNUGGLES_EXAMINED);
            SaveSystem.Instance?.OnStoryProgressMade();
            return;
        }

        // Check if we have diary page 3
        bool hasDiary3 = (GlobalDiaryManager.Instance != null && GlobalDiaryManager.Instance.HasDiaryPage(diaryPage3Id)) ||
                         (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(diaryPage3Id));

        // If no diary page 3, prompt to find clues
        if (!hasDiary3)
        {
            Say("Maybe I should look for clues about this teddy bear.");
            return;
        }

        // If we have diary 3 but haven't examined the diary yet
        if (!diaryExamined)
        {
            Say("I think I read something about this teddy bear in the diary entries.");
            return;
        }

        // If diary examined, but quiz not armed yet → arm the quiz
        if (!quizArmed && !quizSolved)
        {
            Say("I think I read something about this teddy bear in the diary entries.");
            quizArmed = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_QUIZ_ARMED);
            SaveSystem.Instance?.OnStoryProgressMade();

            // Arm the quiz to trigger on next diary close
            if (SnugglesQuizManager.Instance != null)
                SnugglesQuizManager.Instance.ArmQuizOnNextDiaryClose();

            return;
        }

        // If quiz is armed but not solved, remind to check diary
        if (quizArmed && !quizSolved)
        {
            Say("I should check the diary again. Which page mentioned Mr. Snuggles?");
        }
    }

    void OnDiaryClosed()
    {
        // Mark that the diary has been examined at least once
        if (!diaryExamined)
        {
            diaryExamined = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_DIARY_EXAMINED);
            SaveSystem.Instance?.OnStoryProgressMade();
            Debug.Log("[MrSnuggles] Diary has been examined for the first time");
        }
    }

    void GiveWindingKey()
    {
        windingKeyGiven = true;
        SaveSystem.Instance?.TriggerDialogue(FLAG_KEY_GIVEN);
        SaveSystem.Instance?.OnStoryProgressMade();

        // Add to inventory
        InventoryManager.Instance?.AddItem(windingKeyId);

        // Add to save system
        if (SaveSystem.Instance != null && !SaveSystem.Instance.HasItem(windingKeyId))
        {
            SaveSystem.Instance.AddInventoryItem(windingKeyId);
        }

        Say("Wait... there's something between the cushions! I got the winding key!");
    }

    public void MarkQuizSolved()
    {
        quizSolved = true;
        SaveSystem.Instance?.TriggerDialogue(FLAG_QUIZ_SOLVED);
        SaveSystem.Instance?.OnStoryProgressMade();
        Debug.Log("[MrSnuggles] Quiz solved! Player can now get winding key on next examination.");
    }

    void Say(string text)
    {
        DialogueSystemV2.Instance?.StartDialogue(text, "Lisa");
    }
}