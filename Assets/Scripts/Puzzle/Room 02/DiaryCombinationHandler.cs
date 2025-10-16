using UnityEngine;
using System.Collections;

/// <summary>
/// Automatically combines diary pages when all 4 are collected
/// Attach this to any persistent GameObject in the Living Room scene
/// </summary>
public class DiaryCombinationHandler : MonoBehaviour
{
    private bool hasCheckedForCombine = false;
    private const string DIARY_1_ID = "diary_page_1";
    private const string DIARY_2_ID = "diary_page_2";
    private const string DIARY_3_ID = "diary_page_3";
    private const string DIARY_4_ID = "diary_page_4";
    private const string DIARY_COMPLETE_ID = "diary_complete";

    void Start()
    {
        // Subscribe to item added events
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemAdded += OnItemAdded;
        }
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemAdded -= OnItemAdded;
        }
    }

    void OnItemAdded(InventoryItem item)
    {
        // Check if it's a diary page
        if (item.itemId == DIARY_1_ID || item.itemId == DIARY_2_ID ||
            item.itemId == DIARY_3_ID || item.itemId == DIARY_4_ID)
        {
            CheckAndCombineDiaryPages();
        }
    }

    void CheckAndCombineDiaryPages()
    {
        // Don't combine if already have complete diary
        if (SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID))
        {
            return;
        }

        // Check if player has all 4 pages
        bool hasAll = SaveSystem.Instance.HasItem(DIARY_1_ID) &&
                      SaveSystem.Instance.HasItem(DIARY_2_ID) &&
                      SaveSystem.Instance.HasItem(DIARY_3_ID) &&
                      SaveSystem.Instance.HasItem(DIARY_4_ID);

        if (hasAll)
        {
            StartCoroutine(CombineDiaryPagesSequence());
        }
    }

    IEnumerator CombineDiaryPagesSequence()
    {
        // Wait a moment for any current dialogue to finish
        yield return new WaitForSeconds(0.5f);

        // Show combining message
        DialogueSystemV2.Instance?.StartDialogue("I found all the diary pages! They're combining into a complete diary.", "Lisa");

        // Wait for dialogue
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Combine the items
        string[] diaryPages = { DIARY_1_ID, DIARY_2_ID, DIARY_3_ID, DIARY_4_ID };
        bool success = InventoryManager.Instance?.CombineMultipleItems(diaryPages, DIARY_COMPLETE_ID) ?? false;

        if (success)
        {
            // Show confirmation
            DialogueSystemV2.Instance?.StartDialogue("Complete Diary added to inventory. I can read it anytime now.", "Lisa");

            // Check puzzle completion in room controller
            Room02_LivingRoomController roomController = FindFirstObjectByType<Room02_LivingRoomController>();
            if (roomController != null)
            {
                roomController.CheckPuzzleCompletion();
            }
        }
    }

    // Optional: Manually trigger combine check (for testing)
    [ContextMenu("Force Check Diary Combine")]
    void ForceCheckDiaryCombine()
    {
        CheckAndCombineDiaryPages();
    }
}