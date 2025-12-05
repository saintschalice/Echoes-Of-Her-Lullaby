using UnityEngine;
using System.Collections;

public class MrSnugglesController : MonoBehaviour, IInteractable
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

        // AUTO-ARM FIX: If quiz was previously armed (in save data) but not solved,
        // re-arm the manager immediately on load so the player doesn't have to examine Snuggles again.
        if (quizArmed && !quizSolved)
        {
            StartCoroutine(AutoArmQuizNextFrame());
        }
    }

    void OnDestroy()
    {
        DiaryReaderUI.OnDiaryClosed -= OnDiaryClosed;
    }

    IEnumerator AutoArmQuizNextFrame()
    {
        // Wait one frame to ensure SnugglesQuizManager.Instance is initialized
        yield return null;

        if (SnugglesQuizManager.Instance != null)
        {
            Debug.Log("[MrSnuggles] Detected armed state on load. Re-arming Quiz Manager now.");
            SnugglesQuizManager.Instance.ArmQuizOnNextDiaryClose();
        }
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
        Debug.Log($"[DEBUG_TRACE] [MrSnuggles] OnExamine. diaryExamined={diaryExamined}, snugglesExamined={snugglesExamined}, quizArmed={quizArmed}, quizSolved={quizSolved}");

        // 7. If quiz is solved, checking snuggles again grants the key
        if (quizSolved && !windingKeyGiven)
        {
            GiveWindingKey();
            return;
        }

        // If music box is already complete or key given, standard msg
        if (SaveSystem.Instance != null && (SaveSystem.Instance.HasItem(musicBoxCompleteId) || windingKeyGiven))
        {
            Say("He feels empty now. I already used what he was hiding.");
            return;
        }

        // Handle the examination flow (Steps 1, 2, 3)
        HandleExamination();
    }

    void HandleExamination()
    {
        // 2. The player needs to examine mr snuggles at least once
        if (!snugglesExamined)
        {
            Say("Why does he seem so familiar?");
            snugglesExamined = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_SNUGGLES_EXAMINED);
            SaveSystem.Instance?.OnStoryProgressMade();
            return;
        }

        // Check if diary UI has been opened at least once (Step 1 requirement)
        if (!diaryExamined)
        {
            Say("Maybe I should look for clues about this teddy bear in the diary.");
            return;
        }

        // 3. After (both done), when player examines mr snuggles again, start dialogue
        // IF NOT YET ARMED:
        if (!quizArmed)
        {
            Say("I think I read something about this teddy bear in the diary entries.");

            quizArmed = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_QUIZ_ARMED);
            SaveSystem.Instance?.OnStoryProgressMade();

            Debug.Log("[DEBUG_TRACE] [MrSnuggles] Arming quiz now for NEXT diary close.");

            // 4. Arms the quiz so it appears next time Diary closes
            if (SnugglesQuizManager.Instance != null)
                SnugglesQuizManager.Instance.ArmQuizOnNextDiaryClose();
            else
                Debug.LogError("[DEBUG_TRACE] [MrSnuggles] SnugglesQuizManager.Instance is NULL!");

            return;
        }

        // IF ARMED BUT NOT SOLVED (Reminder):
        if (quizArmed && !quizSolved)
        {
            Say("I should check the diary again. Which page mentioned Mr. Snuggles?");

            // Re-arm just in case it was lost
            if (SnugglesQuizManager.Instance != null)
                SnugglesQuizManager.Instance.ArmQuizOnNextDiaryClose();
        }
    }

    void OnDiaryClosed()
    {
        // 1. Mark that the diary UI has been examined at least once
        if (!diaryExamined)
        {
            diaryExamined = true;
            SaveSystem.Instance?.TriggerDialogue(FLAG_DIARY_EXAMINED);
            SaveSystem.Instance?.OnStoryProgressMade();
            Debug.Log("[DEBUG_TRACE] [MrSnuggles] Diary has been examined for the first time");
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

        // 6. & 7. Dialogue and item get
        Say("Wait... there's something between the cushions! I got the winding key!");
    }

    public void MarkQuizSolved()
    {
        quizSolved = true;
        SaveSystem.Instance?.TriggerDialogue(FLAG_QUIZ_SOLVED);
        SaveSystem.Instance?.OnStoryProgressMade();
        Debug.Log("[DEBUG_TRACE] [MrSnuggles] Quiz solved! Player can now get winding key on next examination.");
    }

    void Say(string text)
    {
        DialogueSystemV2.Instance?.StartDialogue(text, "Lisa");
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        OnExamine();
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        OnExamine();
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }
}