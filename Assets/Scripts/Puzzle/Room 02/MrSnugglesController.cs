using UnityEngine;

/// <summary>
/// Manages Mr. Snuggles quiz logic when examined from inventory
/// No scene object needed - this is triggered when player examines the teddy in inventory
/// </summary>
public class MrSnugglesController : MonoBehaviour
{
    public static MrSnugglesController Instance { get; private set; }

    [Header("State")]
    private bool hasOpenedDiaryOnce = false;
    private bool hasExaminedSnuggles = false;
    private bool quizSolved = false;

    [Header("Items")]
    [Tooltip("Item ID for Mr. Snuggles in inventory")]
    public string snugglesItemId = "mr_snuggles";

    [Tooltip("Item ID for the winding key")]
    public string windingKeyItemId = "winding_key";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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

    #region Public API - Called by Inventory System

    /// <summary>
    /// Call this when player examines Mr. Snuggles from inventory
    /// </summary>
    public void OnSnugglesExamined()
    {
        Debug.Log($"[MrSnuggles] Examined from inventory. State: diary={hasOpenedDiaryOnce}, examined={hasExaminedSnuggles}, solved={quizSolved}");

        // Case 1: Quiz already solved - give the key
        if (quizSolved)
        {
            GiveWindingKey();
            return;
        }

        // Case 2: Haven't opened diary yet
        if (!hasOpenedDiaryOnce)
        {
            ShowDialogue("It's an old teddy bear. I should explore more first.");
            return;
        }

        // Case 3: Opened diary but first time examining after
        if (!hasExaminedSnuggles)
        {
            hasExaminedSnuggles = true;
            SaveState();
            ShowDialogue("I think I read about him somewhere in the diary entries...");
            return;
        }

        // Case 4: Already examined, remind to check diary
        ShowDialogue("I should check the diary again to remember where Mr. Snuggles was mentioned.");

        // Auto-open diary
        if (DiaryReaderUI.Instance != null && !DiaryReaderUI.Instance.IsReaderOpen())
        {
            // Close inventory first
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.CloseInventoryUI();
            }

            // Open diary
            DiaryReaderUI.Instance.ShowDiary();
        }
    }

    /// <summary>
    /// Alternative: Use this if you want to check from inventory examine
    /// </summary>
    public void OnItemExamined(string itemId)
    {
        if (itemId == snugglesItemId)
        {
            OnSnugglesExamined();
        }
    }

    #endregion

    #region Diary Tracking

    void OnDiaryClosed()
    {
        // Track that diary has been opened at least once
        if (!hasOpenedDiaryOnce)
        {
            hasOpenedDiaryOnce = true;
            SaveState();
            Debug.Log("[MrSnuggles] Diary opened for the first time");
        }
    }

    #endregion

    #region Winding Key

    void GiveWindingKey()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("[MrSnuggles] InventoryManager not found!");
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

        Debug.Log("[MrSnuggles] Winding key given to player");
    }

    #endregion

    #region Quiz Callback

    /// <summary>
    /// Called by SnugglesQuizManager when quiz is solved correctly
    /// </summary>
    public void MarkQuizSolved()
    {
        quizSolved = true;
        SaveState();
        Debug.Log("[MrSnuggles] Quiz solved! Winding key is now available.");

        ShowDialogue("That's right! I remember now. There should be something inside Mr. Snuggles.");
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
            Debug.Log($"[MrSnuggles] Dialogue: {message}");
        }
    }

    #endregion

    #region Save/Load

    void SaveState()
    {
        PlayerPrefs.SetInt("MrSnuggles_DiaryOpened", hasOpenedDiaryOnce ? 1 : 0);
        PlayerPrefs.SetInt("MrSnuggles_Examined", hasExaminedSnuggles ? 1 : 0);
        PlayerPrefs.SetInt("MrSnuggles_QuizSolved", quizSolved ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadState()
    {
        hasOpenedDiaryOnce = PlayerPrefs.GetInt("MrSnuggles_DiaryOpened", 0) == 1;
        hasExaminedSnuggles = PlayerPrefs.GetInt("MrSnuggles_Examined", 0) == 1;
        quizSolved = PlayerPrefs.GetInt("MrSnuggles_QuizSolved", 0) == 1;

        Debug.Log($"[MrSnuggles] State loaded: diary={hasOpenedDiaryOnce}, examined={hasExaminedSnuggles}, solved={quizSolved}");
    }

    public void ResetState()
    {
        hasOpenedDiaryOnce = false;
        hasExaminedSnuggles = false;
        quizSolved = false;
        SaveState();

        Debug.Log("[MrSnuggles] State reset");
    }

    #endregion

    #region Public Getters

    public bool IsQuizSolved() => quizSolved;
    public bool HasExaminedSnuggles() => hasExaminedSnuggles;

    #endregion

    #region Context Menu

    [ContextMenu("Test: Examine Snuggles")]
    void TestExamine() => OnSnugglesExamined();

    [ContextMenu("Test: Mark Quiz Solved")]
    void TestSolve() => MarkQuizSolved();

    [ContextMenu("Test: Reset State")]
    void TestReset() => ResetState();

    #endregion
}