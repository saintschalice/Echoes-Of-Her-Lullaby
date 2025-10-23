using UnityEngine;

public class ItemExaminationHandler : MonoBehaviour
{
    private Room02_LivingRoomController roomController;

    void Start()
    {
        roomController = FindFirstObjectByType<Room02_LivingRoomController>();

        // Subscribe to inventory item use events
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

    // Put these inside ItemExaminationHandler.cs (replace existing methods)

    void HandleItemExamination(InventoryItem item)
    {
        Debug.Log($"[ItemExamination] HandleItemExamination called for: {item.itemId}");

        // Allow diary, Mr. Snuggles, winding key, AND music_box_complete to safely wait for dialogue
        if (item.itemId == "diary_complete" || item.itemId == "mr_snuggles" ||
            item.itemId == "winding_key" || item.itemId == "music_box_complete")
        {
            StartCoroutine(HandleItemAfterDialogue(item));
            return;
        }

        // For all other items, block if dialogue is active
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            Debug.Log("[ItemExamination] Dialogue already active, ignoring item use");
            return;
        }

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
                // Removed music_box_complete from here since it's handled above
        }
    }

    System.Collections.IEnumerator OpenDiarySafe()
    {
        Debug.Log("[ItemExamination] OpenDiarySafe started");

        // If a dialogue is active, end it cleanly before opening diary.
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            Debug.Log("[ItemExamination] Dialogue active - ending it before opening diary");
            // Prefer to end/close dialogue so state becomes clean
            DialogueSystemV2.Instance.EndDialogue();

            // Wait until DialogueSystem reports no longer active (safety loop)
            float timeout = 1.0f;
            float elapsed = 0f;
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive() && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // small extra frame to ensure UI state settles
            yield return null;
        }
        else
        {
            // no active dialogue — just continue
            yield return null;
        }

        // Now try to find the DiaryReaderUI (include inactive objects)
        DiaryReaderUI diaryReader = FindFirstObjectByType<DiaryReaderUI>(FindObjectsInactive.Include);

        if (diaryReader != null)
        {
            Debug.Log("[ItemExamination] Found DiaryReaderUI — showing diary");
            diaryReader.ShowDiary();
        }
        else
        {
            Debug.LogWarning("[ItemExamination] Could NOT find DiaryReaderUI. Falling back to dialogue text.");
            DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
            {
            new DialogueLine { text = "Page 1: My friend came to visit again today. She likes to watch me play and always knows when I need a hug.", speakerName = "Diary" },
            new DialogueLine { text = "Page 2: Sometimes the house gets really quiet and cold. That's when my friend is thinking hard about something important.", speakerName = "Diary" },
            new DialogueLine { text = "Page 3: I asked my friend why she looks so sad sometimes. She just hummed my favorite song instead of answering.", speakerName = "Diary" },
            new DialogueLine { text = "Page 4: My friend doesn't like it when people are loud. She gets upset and makes things move around the room.", speakerName = "Diary" }
            });
        }
    }


    private System.Collections.IEnumerator HandleItemAfterDialogue(InventoryItem item)
    {
        // Wait until any active dialogue finishes
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.1f); // small safety delay

        if (item.itemId == "diary_complete")
        {
            StartCoroutine(OpenDiarySafe());
        }
        else if (item.itemId == "mr_snuggles")
        {
            ExamineTeddyBear();
        }
        else if (item.itemId == "winding_key")
        {
            ExamineWindingKey();
        }
        else if (item.itemId == "music_box_complete")
        {
            Debug.Log("[ItemExaminationHandler] Music box complete - triggering lullaby cutscene");
            PlayMusicBox(); // This will start the coroutine
        }
    }



    void ExamineTeddyBear()
    {
        Debug.Log($"[ExamineTeddyBear] Checking diary items...");
        Debug.Log($"Has diary_page_1 (Save): {SaveSystem.Instance?.HasItem("diary_page_1")}");
        Debug.Log($"Has diary_page_1 (Inventory): {InventoryManager.Instance?.HasItem("diary_page_1")}");
        Debug.Log($"Has diary_complete (Save): {SaveSystem.Instance?.HasItem("diary_complete")}");
        Debug.Log($"Has diary_complete (Inventory): {InventoryManager.Instance?.HasItem("diary_complete")}");

        // Check for diary page 1 or complete diary in SaveSystem OR inventory (runtime)
        bool hasDiaryPage1 = (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("diary_page_1"))
                             || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("diary_page_1"));

        bool hasDiaryComplete = (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("diary_complete"))
                                || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("diary_complete"));

        if (!hasDiaryPage1 && !hasDiaryComplete)
        {
            DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles? How do I know that name?", "Lisa");
            return;
        }

        // ✅ FIX: Check if player already has winding key OR if music box is already complete
        bool hasWindingKey = (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("winding_key"))
                             || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("winding_key"));

        bool hasMusicBoxComplete = (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("music_box_complete"))
                                   || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("music_box_complete"));

        if (hasWindingKey || hasMusicBoxComplete)
        {
            if (hasMusicBoxComplete)
            {
                DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles, my childhood teddy bear. I already used the winding key to fix the music box.", "Lisa");
            }
            else
            {
                DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles, my childhood teddy bear. I already found the winding key he was hiding.", "Lisa");
            }
            return;
        }

        // Show the discovery dialogue
        DialogueSystemV2.Instance?.StartDialogue(
            "Wait... the diary said Mr. Snuggles likes to steal things. Maybe he's hiding the winding key!",
            "Lisa"
        );

        // Add winding key to inventory AND SaveSystem to persist it
        if (InventoryManager.Instance != null)
        {
            if (InventoryManager.Instance.GetType().GetMethod("AddItemAndSave") != null)
            {
                InventoryManager.Instance?.AddItemAndSave("winding_key");
            }
            else
            {
                InventoryManager.Instance?.AddItem("winding_key");
                SaveSystem.Instance?.AddInventoryItem("winding_key");
            }
        }
        else
        {
            SaveSystem.Instance?.AddInventoryItem("winding_key");
        }

        // Show confirmation message
        StartCoroutine(ShowWindingKeyConfirmation());
    }


    System.Collections.IEnumerator ShowWindingKeyConfirmation()
    {
        // Wait for current dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        DialogueSystemV2.Instance?.StartDialogue("Found a winding key inside Mr. Snuggles! Added to inventory.", "Lisa");
    }

    void ExamineBrokenMusicBox()
    {
        DialogueSystemV2.Instance?.StartDialogue("This music box doesn't have a winding key.", "Lisa");
    }

    void ExamineWindingKey()
    {
        Debug.Log("[ItemExamination] Examining winding key...");

        // Wait until any dialogue finishes
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            Debug.Log("[ItemExamination] Dialogue active — delaying combination check");
            StartCoroutine(HandleWindingKeyAfterDialogue());
            return;
        }

        // Check for broken music box in inventory OR save data
        bool hasBrokenBox = (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("broken_music_box"))
                            || (SaveSystem.Instance != null && SaveSystem.Instance.HasItem("broken_music_box"));

        if (hasBrokenBox)
        {
            Debug.Log("[ItemExamination] Found broken music box — showing combine choice...");
            StartCoroutine(ShowCombineChoiceAfterDelay());
        }
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("A small winding key. It might fit into something.", "Lisa");
        }
    }

    System.Collections.IEnumerator HandleWindingKeyAfterDialogue()
    {
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.1f);

        ExamineWindingKey();
    }



    System.Collections.IEnumerator ShowCombineChoiceAfterDelay()
    {
        // Wait for any existing dialogue to close
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        // Small delay for safety
        yield return new WaitForSeconds(0.2f);

        // NEW: Keep inventory open during choice prompt
        // Don't close inventory - just show the choice dialogue on top

        // Now show the combine choice
        DialogueSystemV2.Instance?.ShowChoices(
            new string[] { "Use key on music box", "Keep separate" },
            new System.Action[]
            {
            () => CombineMusicBoxParts(),
            () => {
                DialogueSystemV2.Instance?.StartDialogue("I'll keep them separate for now.", "Lisa");
                // Inventory stays open naturally
            }
            }
        );

        Debug.Log("[ItemExamination] Showing combine choice");
    }
    void CombineMusicBoxParts()
    {
        bool success = InventoryManager.Instance?.CombineItems("broken_music_box", "winding_key", "music_box_complete") ?? false;

        if (success)
        {
            DialogueSystemV2.Instance?.StartDialogue("I attached the winding key to the music box. It's complete now!", "Lisa");

            // Refresh inventory after combining
            if (InventoryManager.Instance != null && InventoryManager.Instance.inventoryUI != null)
            {
                InventoryManager.Instance.inventoryUI.RefreshInventory();
            }

            roomController?.CheckPuzzleCompletion();
        }
    }

    void PlayMusicBox()
    {
        Debug.Log("[ItemExamination] PlayMusicBox called");
        // double-check we actually have a roomController and lullaby clip
        if (roomController == null)
        {
            roomController = FindFirstObjectByType<Room02_LivingRoomController>();
        }

        if (roomController == null)
        {
            Debug.LogWarning("[ItemExamination] No Room02_LivingRoomController found. Aborting lullaby.");
            return;
        }

        StartCoroutine(PlayLullabySequence());
    }


    System.Collections.IEnumerator PlayLullabySequence()
    {
        Debug.Log("[ItemExamination] PlayLullabySequence started");

        // Find all the key objects
        FadeScreen fadeScreen = FadeScreen.Instance ?? FindFirstObjectByType<FadeScreen>(FindObjectsInactive.Include);
        GameObject dialoguePanel = DialogueSystemV2.Instance?.dialoguePanel;

        Debug.Log($"[ItemExamination] FadeScreen found: {fadeScreen != null}");
        Debug.Log($"[ItemExamination] DialoguePanel found: {dialoguePanel != null}");

        // Start music
        if (roomController != null && roomController.lullabyFragment != null)
        {
            AudioManager.Instance?.PlayMusic(roomController.lullabyFragment, loop: false, fadeTime: 0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        // ✅ CRITICAL FIX: Make sure DialoguePanel's parent canvas is active and on top
        if (dialoguePanel != null)
        {
            // Find the Canvas that contains the dialogue panel
            Canvas parentCanvas = dialoguePanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 1000; // Force dialogue canvas on top of everything
                parentCanvas.overrideSorting = true;
                Debug.Log($"[ItemExamination] Set parent canvas sorting order to 1000");
            }

            // Make sure dialogue panel itself is active
            dialoguePanel.SetActive(true);

            CanvasGroup canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
        }

        // Prepare fade screen (but dialogue will render on top)
        if (fadeScreen != null && fadeScreen.fadeImage != null)
        {
            fadeScreen.fadeImage.enabled = true;
            fadeScreen.SetAlpha(0f);

            // ✅ Set fade image canvas order BELOW dialogue
            Canvas fadeCanvas = fadeScreen.GetComponentInParent<Canvas>();
            if (fadeCanvas != null)
            {
                fadeCanvas.sortingOrder = 999; // Below dialogue
                fadeCanvas.overrideSorting = true;
            }
        }

        // Show dialogue FIRST
        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
        new DialogueLine { text = "A memory surfaces from the darkness...", speakerName = "Lisa" },
        new DialogueLine { text = "Young Lisa hugs her teddy bear tightly as a gentle voice sings a lullaby", speakerName = "Lisa" },
        new DialogueLine { text = "♪ Hush now, my darling, don't you cry... ♪", speakerName = "???" },
        new DialogueLine { text = "The memory fades, but the melody lingers...", speakerName = "Lisa" }
        });

        // Wait for dialogue to start rendering
        yield return new WaitForSeconds(0.5f);

        // NOW fade to black (dialogue will stay visible on top)
        if (fadeScreen != null)
        {
            fadeScreen.fadeImage.raycastTarget = false; // Don't block dialogue clicks
            fadeScreen.FadeOut(2f);
            Debug.Log("[ItemExamination] Started fade out");
        }

        GameObject joystick = GameObject.Find("Joystick");
        GameObject inventoryUIObj = GameObject.Find("InventoryUI");

        if (joystick != null) joystick.SetActive(false);
        if (inventoryUIObj != null) inventoryUIObj.SetActive(false);


        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        Debug.Log("[ItemExamination] Dialogue finished");

        yield return new WaitForSeconds(0.5f);

        // Restore UI
        if (joystick != null) joystick.SetActive(true);
        if (inventoryUIObj != null) inventoryUIObj.SetActive(true);

        // Fade back in
        if (fadeScreen != null)
        {
            fadeScreen.FadeIn(2f);
            Debug.Log("[ItemExamination] Fading back in");
        }

        yield return new WaitForSeconds(2f);

        // Clean up fade
        if (fadeScreen != null && fadeScreen.fadeImage != null)
        {
            fadeScreen.SetAlpha(0f);
            fadeScreen.fadeImage.raycastTarget = false;

            // Reset canvas sorting
            Canvas fadeCanvas = fadeScreen.GetComponentInParent<Canvas>();
            if (fadeCanvas != null)
            {
                fadeCanvas.sortingOrder = 0;
                fadeCanvas.overrideSorting = false;
            }
        }

        // Reset dialogue canvas sorting
        if (dialoguePanel != null)
        {
            Canvas parentCanvas = dialoguePanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 0;
                parentCanvas.overrideSorting = false;
            }
        }

        Debug.Log("[ItemExamination] Sequence complete");
        roomController?.OnLullabyPlayed();
    }


    void ShowCompleteDiary()
    {
        Debug.Log("[ItemExaminationHandler] Trying to open complete diary...");

        DiaryReaderUI diaryReader = FindFirstObjectByType<DiaryReaderUI>(FindObjectsInactive.Include);
        if (diaryReader != null)
        {
            Debug.Log("[ItemExaminationHandler] Found DiaryReaderUI — showing diary");
            if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
                DialogueSystemV2.Instance.EndDialogue(); // optional safety

            diaryReader.ShowDiary();
        }
        else
        {
            // Fallback: show as dialogue
            DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
            {
                new DialogueLine { text = "Page 1: My friend came to visit again today. She likes to watch me play and always knows when I need a hug.", speakerName = "Diary" },
                new DialogueLine { text = "Page 2: Sometimes the house gets really quiet and cold. That's when my friend is thinking hard about something important.", speakerName = "Diary" },
                new DialogueLine { text = "Page 3: I asked my friend why she looks so sad sometimes. She just hummed my favorite song instead of answering.", speakerName = "Diary" },
                new DialogueLine { text = "Page 4: My friend doesn't like it when people are loud. She gets upset and makes things move around the room.", speakerName = "Diary" }
            });
        }
    }
}