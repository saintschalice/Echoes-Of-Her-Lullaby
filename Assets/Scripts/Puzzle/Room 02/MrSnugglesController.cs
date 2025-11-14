using UnityEngine;

public class MrSnugglesController : MonoBehaviour
{
    [Header("IDs (match your project)")]
    [SerializeField] private string diaryPage3Id = "diary_page_3";
    [SerializeField] private string windingKeyId = "winding_key";
    [SerializeField] private string musicBoxCompleteId = "music_box_complete";

    [Header("Debug")]
    [SerializeField] private bool firstExamineDone;
    [SerializeField] private bool quizArmed;
    [SerializeField] private bool quizSolved;
    [SerializeField] private bool windingKeyGiven;

    // Call this from your interactable
    public void OnExamine()
    {
        // If music box is already complete, never give another key
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(musicBoxCompleteId))
        {
            Say("He feels empty now. I already used what he was hiding.");
            return;
        }

        // If quiz solved and key not yet given → give winding key once
        if (quizSolved && !windingKeyGiven)
        {
            GiveWindingKey();
            return;
        }

        // Pre-quiz guidance
        HandleHints();
    }

    void HandleHints()
    {
        if (!firstExamineDone)
        {
            Say("Why does he seem so familiar?");
            firstExamineDone = true;
            return;
        }

        // Require diary page 3 in inventory (your flow uses SaveSystem.HasItem for pages in some spots)
        bool hasDiary3 = SaveSystem.Instance != null && SaveSystem.Instance.HasItem(diaryPage3Id);

        if (!hasDiary3)
        {
            Say("Why does he seem so familiar?");
            Say("Maybe I should look for clues about the bear.");
            return;
        }

        // If we have diary 3, arm the quiz (after diary close)
        if (!quizArmed && !quizSolved)
        {
            Say("Why does he seem so familiar?");
            Say("I think I read something about Mr. Snuggles in the diary entries...");
            quizArmed = true;

            if (SnugglesQuizManager.Instance != null)
                SnugglesQuizManager.Instance.ArmQuizOnNextDiaryClose();

            return;
        }

        if (quizArmed && !quizSolved)
        {
            Say("I should double-check the diary. There was something about Mr. Snuggles...");
        }
    }

    void GiveWindingKey()
    {
        windingKeyGiven = true;

        // Use your InventoryManager pattern (consistent with Room02) 
        InventoryManager.Instance?.AddItem(windingKeyId);

        Say("Got the winding key. It's nestled in there quite tight.");
    }

    public void MarkQuizSolved()
    {
        quizSolved = true;
    }

    void Say(string text)
    {
        DialogueSystemV2.Instance?.StartDialogue(text, "Lisa");
    }
}
