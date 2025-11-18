using UnityEngine;
using System.Collections;

public class ItemExaminationHandler : MonoBehaviour
{
    private Room02_LivingRoomController roomController;
    private MrSnugglesController snugglesController;

    // flags
    private const string FLAG_UNDERSTOOD = "understood_snuggles_clue";

    void Start()
    {
        roomController = FindFirstObjectByType<Room02_LivingRoomController>();
        // Find the specific controller for Snuggles logic
        snugglesController = FindFirstObjectByType<MrSnugglesController>();

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemUsed += HandleItemExamination;
        }
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemUsed -= HandleItemExamination;
        }
    }

    void HandleItemExamination(InventoryItem item)
    {
        if (item == null) return;

        // Open diary when using either the combined item or any individual diary page.
        if (!string.IsNullOrEmpty(item.itemId) &&
            (item.itemId == "diary_entries" || item.itemId.StartsWith("diary_page_", System.StringComparison.OrdinalIgnoreCase)))
        {
            StartCoroutine(OpenDiarySafe());
            return;
        }


        // Allow these items to queue until dialogue ends
        if (item.itemId == "mr_snuggles" ||
            item.itemId == "winding_key" ||
            item.itemId == "music_box_complete")
        {
            StartCoroutine(HandleItemAfterDialogue(item));
            return;
        }

        // Block other interactions if dialogue is active
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            return;

        switch (item.itemId)
        {
            case "mr_snuggles":
                ExamineTeddyBear();
                break;
            case "broken_music_box":
                ExamineBrokenMusicBox();
                break;
            case "winding_key":
                ExamineWindingKey();
                break;
        }
    }

    System.Collections.IEnumerator OpenDiarySafe()
    {
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            DialogueSystemV2.Instance.EndDialogue();
            float timeout = 1.0f;
            float elapsed = 0f;
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive() && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        else
        {
            yield return null;
        }

        DiaryReaderUI diaryReader = FindFirstObjectByType<DiaryReaderUI>(FindObjectsInactive.Include);
        if (diaryReader != null)
        {
            Debug.Log("[ItemExaminationHandler] Found DiaryReaderUI instance -> calling ShowDiary()");
            diaryReader.ShowDiary();
        }
        else
        {
            Debug.LogWarning("[ItemExaminationHandler] DiaryReaderUI not found (inactive or missing). Make sure DiaryReaderUI exists in scene and diaryPanel is assigned.");
        }

    }

    private System.Collections.IEnumerator HandleItemAfterDialogue(InventoryItem item)
    {
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.1f);

        if (item.itemId == "mr_snuggles")
        {
            ExamineTeddyBear();
        }
        else if (item.itemId == "winding_key")
        {
            ExamineWindingKey();
        }
        else if (item.itemId == "music_box_complete")
        {
            PlayMusicBox();
        }
    }

    void ExamineTeddyBear()
    {
        // FIX: Delegate directly to the MrSnugglesController if it exists in the scene.
        // This ensures the complex state machine (First look -> Diary Check -> Arm Quiz) runs correctly.
        if (snugglesController == null) snugglesController = FindFirstObjectByType<MrSnugglesController>();

        if (snugglesController != null)
        {
            Debug.Log("[ItemExaminationHandler] Delegating examination to MrSnugglesController.");
            snugglesController.OnExamine();
            return;
        }

        // Fallback logic only if the controller is missing (e.g. different scene)
        bool hasWindingKey =
            (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("winding_key")) ||
            (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("winding_key"));

        if (hasWindingKey)
        {
            DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles, my childhood teddy bear. I already found the winding key he was hiding.", "Lisa");
            return;
        }

        DialogueSystemV2.Instance?.StartDialogue("A worn teddy bear. It feels heavy...", "Lisa");
    }

    void ExamineBrokenMusicBox()
    {
        bool hasWindingKey = SaveSystem.Instance.HasItem("winding_key");

        if (!hasWindingKey)
            DialogueSystemV2.Instance?.StartDialogue("The music box is broken. The winding mechanism is missing...", "Lisa");
        else
            DialogueSystemV2.Instance?.StartDialogue("I can use the winding key to fix this music box.", "Lisa");
    }

    void ExamineWindingKey()
    {
        bool hasMusicBox = SaveSystem.Instance.HasItem("broken_music_box");

        if (!hasMusicBox)
        {
            DialogueSystemV2.Instance?.StartDialogue("A small winding key. I wonder what it's for?", "Lisa");
        }
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("This winding key looks like it fits the broken music box.", "Lisa");
            StartCoroutine(TryCombineMusicBox());
        }
    }

    System.Collections.IEnumerator TryCombineMusicBox()
    {
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.3f);

        DialogueSystemV2.Instance?.ShowChoices(
            new string[] { "Combine winding key with music box", "Not now" },
            new System.Action[]
            {
                () => CombineMusicBox(),
                () => DialogueSystemV2.Instance?.StartDialogue("I'll keep them separate for now.", "Lisa")
            }
        );
    }

    void CombineMusicBox()
    {
        bool success = InventoryManager.Instance?.CombineItems("broken_music_box", "winding_key", "music_box_complete") ?? false;

        if (success)
        {
            DialogueSystemV2.Instance?.StartDialogue("I fixed the music box! Maybe if I wind it up, it will play a song.", "Lisa");

            if (InventoryManager.Instance != null && InventoryManager.Instance.inventoryUI != null)
                InventoryManager.Instance.inventoryUI.RefreshInventory();

            roomController?.CheckPuzzleCompletion();
        }
    }

    void PlayMusicBox()
    {
        if (roomController == null)
            roomController = FindFirstObjectByType<Room02_LivingRoomController>();

        if (roomController == null) return;

        StartCoroutine(PlayLullabySequence());
    }

    System.Collections.IEnumerator PlayLullabySequence()
    {
        FadeScreen fadeScreen = FadeScreen.Instance ?? FindFirstObjectByType<FadeScreen>(FindObjectsInactive.Include);
        GameObject dialoguePanel = DialogueSystemV2.Instance?.dialoguePanel;

        if (roomController != null && roomController.lullabyFragment != null)
            AudioManager.Instance?.PlayMusic(roomController.lullabyFragment, loop: false, fadeTime: 0.5f);

        yield return new WaitForSeconds(0.5f);

        if (dialoguePanel != null)
        {
            Canvas parentCanvas = dialoguePanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 1000;
                parentCanvas.overrideSorting = true;
            }

            dialoguePanel.SetActive(true);

            CanvasGroup canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
        }

        if (fadeScreen != null && fadeScreen.fadeImage != null)
        {
            fadeScreen.fadeImage.enabled = true;
            fadeScreen.SetAlpha(0f);

            Canvas fadeCanvas = fadeScreen.GetComponentInParent<Canvas>();
            if (fadeCanvas != null)
            {
                fadeCanvas.sortingOrder = 999;
                fadeCanvas.overrideSorting = true;
            }
        }

        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = "A memory surfaces from the darkness...", speakerName = "Lisa" },
            new DialogueLine { text = "Young Lisa hugs her teddy bear tightly as a gentle voice sings a lullaby", speakerName = "Lisa" },
            new DialogueLine { text = "♪ Hush now, my darling, don't you cry... ♪", speakerName = "???" },
            new DialogueLine { text = "The memory fades, but the melody lingers...", speakerName = "Lisa" }
        });

        yield return new WaitForSeconds(0.5f);

        if (fadeScreen != null)
            fadeScreen.FadeOut(2f);

        GameObject joystick = GameObject.Find("Joystick");
        GameObject inventoryUIObj = GameObject.Find("InventoryUI");
        if (joystick != null) joystick.SetActive(false);
        if (inventoryUIObj != null) inventoryUIObj.SetActive(false);

        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        if (joystick != null) joystick.SetActive(true);
        if (inventoryUIObj != null) inventoryUIObj.SetActive(true);

        if (fadeScreen != null)
            fadeScreen.FadeIn(2f);

        yield return new WaitForSeconds(2f);

        if (fadeScreen != null && fadeScreen.fadeImage != null)
        {
            fadeScreen.SetAlpha(0f);
            fadeScreen.fadeImage.raycastTarget = false;

            Canvas fadeCanvas = fadeScreen.GetComponentInParent<Canvas>();
            if (fadeCanvas != null)
            {
                fadeCanvas.sortingOrder = 0;
                fadeCanvas.overrideSorting = false;
            }
        }

        if (dialoguePanel != null)
        {
            Canvas parentCanvas = dialoguePanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 0;
                parentCanvas.overrideSorting = false;
            }
        }

        // --- NEW LINE ADDED HERE ---
        // This is crucial: it notifies the RoomController that the lullaby memory/cutscene is complete.
        if (roomController != null)
        {
            roomController.OnMusicBoxCutsceneEnded();
        }
        // --- END NEW LINE ---

        roomController?.OnLullabyPlayed();
    }
}