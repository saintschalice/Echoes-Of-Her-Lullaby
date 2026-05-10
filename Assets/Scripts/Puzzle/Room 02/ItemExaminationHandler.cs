using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemExaminationHandler : MonoBehaviour
{
    private Room02_LivingRoomController roomController;
    private MrSnugglesController snugglesController;

    [Header("Cutscene Audio")]
    [Tooltip("Assign the 44-second lullaby audio clip here.")]
    [SerializeField] private AudioClip fullLullabyAudio;

    // flags
    private const string FLAG_UNDERSTOOD = "understood_snuggles_clue";

    // Track if we are currently playing the sequence so we can stop music if destroyed
    private bool isPlayingLullabySequence = false;
    private bool isSkippingCutscene = false;

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

        // FIX: Ensure music stops if this object is destroyed (e.g. loading save) while the cutscene is playing
        if (isPlayingLullabySequence && AudioManager.Instance != null)
        {
            Debug.Log("[ItemExaminationHandler] Destroyed while playing lullaby - Stopping Music.");
            AudioManager.Instance.StopMusic(0f);
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

    // Helper to wait for a duration OR break if skipped
    System.Collections.IEnumerator WaitCheckSkip(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (isSkippingCutscene) yield break;

            // Check for tap/click
            if (Input.GetMouseButtonDown(0))
            {
                // Simple logic: Tap anywhere to skip the cutscene.
                // If you wanted to exclude the dialogue box specifically, you could check EventSystem.
                // However, usually cutscenes are skipped by tapping screen, and "not dialogue" likely means 
                // "don't make this a dialogue-advancement mechanic", but a full skip.
                Debug.Log("[ItemExaminationHandler] Cutscene skipped by user input.");
                isSkippingCutscene = true;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    System.Collections.IEnumerator PlayLullabySequence()
    {
        isPlayingLullabySequence = true;
        isSkippingCutscene = false;

        FadeScreen fadeScreen = FadeScreen.Instance ?? FindFirstObjectByType<FadeScreen>(FindObjectsInactive.Include);
        GameObject dialoguePanel = DialogueSystemV2.Instance?.dialoguePanel;

        // --- AUDIO HANDLING ---
        // Stop all other audio sources to ensure isolation for the cutscene
        // Using the methods found in AudioManager.cs
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllDialogue();
            AudioManager.Instance.StopAllSFX();

            // Stop ambient with a quick fade out
            AudioManager.Instance.StopAmbient(0.1f);

            // Access LoopingSoundManager directly as seen in AudioManager.cs extensions
            LoopingSoundManager.Instance?.StopAllLoopingSounds();
        }

        // 1. Play Lullaby Audio (Prioritize inspector clip, fallback to roomController)
        AudioClip clipToPlay = fullLullabyAudio;
        if (clipToPlay == null && roomController != null)
        {
            clipToPlay = roomController.lullabyFragment;
        }

        if (clipToPlay != null)
        {
            // PlayMusic usually replaces the BGM channel.
            // Ensure loop is false.
            AudioManager.Instance?.PlayMusic(clipToPlay, loop: false, fadeTime: 0.5f);
        }

        // 2. Setup UI and Fades
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

        // 3. Disable Controls
        GameObject joystick = GameObject.Find("Joystick");
        GameObject inventoryUIObj = GameObject.Find("InventoryUI");
        bool joystickWasActive = joystick != null && joystick.activeSelf;
        bool inventoryWasActive = inventoryUIObj != null && inventoryUIObj.activeSelf;
        
        if (joystick != null) joystick.SetActive(false);
        if (inventoryUIObj != null) inventoryUIObj.SetActive(false);

        // Pause Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        // Fade out to black for the memory
        if (fadeScreen != null)
            fadeScreen.FadeOut(2f);

        // ==========================================================
        // 4. Timed Dialogue Sequence (With Skip Checks)
        // ==========================================================

        // Wait until 11.169s - "Hush now, my darling"
        yield return StartCoroutine(WaitCheckSkip(11.169f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ Hush now, my darling... ♪", "???");

        // Wait until 15.983s - "Don't you cry"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(15.983f - 11.169f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ ...don't you cry... ♪", "???");

        // Wait until 18.756s - "I'm right here, sweetheart"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(18.756f - 15.983f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ I'm right here, sweetheart... ♪", "???");

        // Wait until 24.931s - "Lullaby"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(24.931f - 18.756f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ ...lullaby... ♪", "???");

        // Wait until 28.359s - "Close your eyes"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(28.359f - 24.931f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ Close your eyes... ♪", "???");

        // Wait until 30.600s - "My precious one"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(30.6f - 28.359f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ ...my precious one... ♪", "???");

        // Wait until 33.727s - "Rest now, angel"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(33.727f - 30.6f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ Rest now, angel... ♪", "???");

        // Wait until 38.844s - "Day is done"
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(38.844f - 33.727f));
        if (!isSkippingCutscene) DialogueSystemV2.Instance?.StartDialogue("♪ ...day is done. ♪", "???");

        // Wait remainder of song until 44 seconds
        if (!isSkippingCutscene) yield return StartCoroutine(WaitCheckSkip(44f - 38.844f));


        // 5. Cleanup and Restore

        // If we skipped, we need to handle the state immediately
        if (isSkippingCutscene)
        {
            // Stop the music!
            AudioManager.Instance?.StopMusic(0.5f);

            // If the screen was black (fade out), we should fade it back in quickly
            if (fadeScreen != null)
            {
                // Force alpha to 1 so we can fade in naturally, or just clear it?
                // The next steps will call FadeIn(2f).
                // Let's ensure it's black first so the fade in looks correct.
                fadeScreen.SetAlpha(1f);
            }
        }

        DialogueSystemV2.Instance?.EndDialogue();

        // Re-enable controls - use the stored references
        if (joystick != null && joystickWasActive)
        {
            joystick.SetActive(true);
            Debug.Log("[Lullaby] Re-enabled Joystick");
        }
        
        if (inventoryUIObj != null && inventoryWasActive)
        {
            inventoryUIObj.SetActive(true);
            Debug.Log("[Lullaby] Re-enabled InventoryUI");
        }

        // Re-enable player controller
        JoystickPlayerController playerController = FindFirstObjectByType<JoystickPlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("[Lullaby] Re-enabled PlayerController");
        }

        // Resume Emily AI
        if (emilyAI != null) emilyAI.isPaused = false;

        if (fadeScreen != null)
            fadeScreen.FadeIn(2f);

        // Wait for fade in
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

        // Notify RoomController cutscene is done.
        // NOTE: This call should trigger the RoomController to restart the room's Ambience and Background Music.
        if (roomController != null)
        {
            roomController.OnMusicBoxCutsceneEnded();
        }

        roomController?.OnLullabyPlayed();
        isPlayingLullabySequence = false;
    }
}