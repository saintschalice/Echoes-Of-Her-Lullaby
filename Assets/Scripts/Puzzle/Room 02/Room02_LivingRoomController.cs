using UnityEngine;
using System.Collections;

public class Room02_LivingRoomController : MonoBehaviour
{
    [Header("Room Objects")]
    public GameObject tv;
    public GameObject rockingChair;
    public GameObject coffeeTable_Key;
    public GameObject books_Pushed;
    public GameObject smallKey;

    [Header("Animators")]
    public Animator tvAnimator;
    public Animator rockingChairAnimator;

    [Header("Audio Clips")]
    public AudioClip tvStaticSound;
    public AudioClip rockingChairSound;
    public AudioClip lullabyFragment;
    public AudioClip bookshelfShakeSound;
    public AudioClip keyRevealSound;

    [Header("TV Ghost Audio")]
    public AudioSource tvAudioSource;
    public AudioClip ghostTVAudio;
    public float ghostAudioDelay = 10f;

    [Header("Camera Shake")]
    public float shakeIntensity = 0.08f;
    public float shakeDuration = 0.3f;

    [Header("Lisa Jump Back")]
    public float jumpBackDistance = 1f;
    public float jumpBackDuration = 0.3f;

    [Header("Emily's Dialogue Sound")]
    public AudioClip emilyDialogueSound;

    [Header("Puzzle Tracking")]
    private bool musicBoxPuzzleComplete = false;
    private bool diaryPuzzleComplete = false;
    private bool tvTurnedOff = false;
    private bool hasEnteredRoom = false;
    private bool lullabyPlayed = false;

    private const string ROOM_NAME = "Room02_LivingRoom";
    private const string FLAG_TV_INTRO_DONE = "TV_IntroSequenceDone";
    private const string FLAG_TV_GHOST_PLAYED = "TV_GhostAudioPlayed";
    private const string FLAG_TV_GHOST_DIALOGUE = "TV_GhostDialogueShown";

    // Item IDs
    private const string SMALL_KEY_ID = "living_room_small_key";
    private const string TEDDY_BEAR_ID = "mr_snuggles";
    private const string MUSIC_BOX_ID = "broken_music_box";
    private const string WINDING_KEY_ID = "winding_key";
    private const string MUSIC_BOX_COMPLETE_ID = "music_box_complete";
    private const string DIARY_1_ID = "diary_page_1";
    private const string DIARY_2_ID = "diary_page_2";
    private const string DIARY_3_ID = "diary_page_3";
    private const string DIARY_4_ID = "diary_page_4";
    private const string COFFEE_TABLE_KEY_ID = "hallway_door_key";

    // Runtime state
    private bool introSequenceRunning = false;
    private bool ghostAudioRunning = false;
    private bool hasPlayedGhostAudio = false;
    private JoystickPlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<JoystickPlayerController>();
        InitializeRoom();
    }

    void InitializeRoom()
    {
        if (coffeeTable_Key != null) coffeeTable_Key.SetActive(false);
        if (books_Pushed != null) books_Pushed.SetActive(false);
        if (smallKey != null) smallKey.SetActive(false);

        LoadRoomState();

        bool tvIntroDone = SaveSystem.Instance.WasDialogueTriggered(FLAG_TV_INTRO_DONE);
        hasPlayedGhostAudio = SaveSystem.Instance.WasDialogueTriggered(FLAG_TV_GHOST_PLAYED);

        if (!tvIntroDone)
        {
            StartCoroutine(TVEntranceSequence());
            hasEnteredRoom = true;
            SaveRoomState("hasEnteredRoom", true);
        }
        else
        {
            tvTurnedOff = true;
            if (tvAnimator != null) tvAnimator.SetTrigger("TurnOff");

            if (!hasPlayedGhostAudio && ghostTVAudio != null && tvAudioSource != null)
            {
                StartCoroutine(PlayGhostTVAudioAfterDelay());
            }
        }

        if (lullabyPlayed && rockingChairAnimator != null)
        {
            rockingChairAnimator.SetBool("isRocking", true);
            AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
        }

        CheckPuzzleCompletion();
    }

    void LoadRoomState()
    {
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

        hasEnteredRoom = state.interactedObjects.Contains("hasEnteredRoom");
        tvTurnedOff = state.interactedObjects.Contains("tvTurnedOff");
        lullabyPlayed = state.interactedObjects.Contains("lullabyPlayed");
        musicBoxPuzzleComplete = state.solvedPuzzles.Contains("musicBoxPuzzle");
        diaryPuzzleComplete = state.solvedPuzzles.Contains("diaryPuzzle");

        if (state.interactedObjects.Contains("booksPushed") && books_Pushed != null)
            books_Pushed.SetActive(true);

        if (state.collectedItems.Contains(SMALL_KEY_ID) && smallKey != null)
            smallKey.SetActive(false);

        if (state.collectedItems.Contains(COFFEE_TABLE_KEY_ID) && coffeeTable_Key != null)
            coffeeTable_Key.SetActive(false);
    }

    void SaveRoomState(string key, bool value)
    {
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

        if (value && !state.interactedObjects.Contains(key))
            state.interactedObjects.Add(key);
        else if (!value)
            state.interactedObjects.Remove(key);

        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
    }

    IEnumerator TVEntranceSequence()
    {
        introSequenceRunning = true;

        if (playerController != null) playerController.enabled = false;

        yield return new WaitForSeconds(0.5f);

        if (tvAnimator != null) tvAnimator.SetTrigger("PlayStatic");
        AudioManager.Instance?.PlayLoopingSFX(tvStaticSound, "tv_static");

        yield return new WaitForSeconds(1f);

        if (emilyDialogueSound != null)
            AudioManager.Instance?.PlayDialogue(emilyDialogueSound);

        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = "GO AWAY!!!!!!! GO AWAY!!!!!!", speakerName = "???" },
            new DialogueLine { text = "What is that?!", speakerName = "Lisa" }
        });

        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        if (playerController != null) playerController.enabled = true;

        introSequenceRunning = false;
    }

    public void OnTVInteract()
    {
        if (introSequenceRunning) return;

        bool tvIntroDone = SaveSystem.Instance.WasDialogueTriggered(FLAG_TV_INTRO_DONE);

        if (ghostAudioRunning && ghostTVAudio != null)
        {
            AudioManager.Instance?.StopAllSFX();
            ghostAudioRunning = false;

            if (!hasPlayedGhostAudio)
            {
                SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_PLAYED);
                SaveSystem.Instance.OnStoryProgressMade();
                hasPlayedGhostAudio = true;
            }

            DialogueSystemV2.Instance?.StartDialogue("I should turn that off.", "Lisa");
            Debug.Log("[LivingRoom] Lisa manually stopped ghost TV audio.");
            return;
        }

        if (tvTurnedOff)
        {
            DialogueSystemV2.Instance?.StartDialogue("The TV is already off.", "Lisa");
            return;
        }

        if (emilyDialogueSound != null)
            AudioManager.Instance?.PlayDialogue(emilyDialogueSound);

        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = "IF YOU DON'T LEAVE THIS HOUSE, YOU'LL REGRET IT.", speakerName = "???" },
            new DialogueLine { text = "It's coming from the TV! I need to turn it off!", speakerName = "Lisa" }
        });

        StartCoroutine(TurnOffTVAfterDialogue());
    }

    IEnumerator TurnOffTVAfterDialogue()
    {
        if (playerController != null) playerController.enabled = false;

        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        AudioManager.Instance?.StopLoopingSFX("tv_static");

        if (tvAnimator != null) tvAnimator.SetTrigger("TurnOff");

        tvTurnedOff = true;
        SaveRoomState("tvTurnedOff", true);

        SaveSystem.Instance.TriggerDialogue(FLAG_TV_INTRO_DONE);
        SaveSystem.Instance.OnStoryProgressMade();

        if (playerController != null) playerController.enabled = true;

        if (!hasPlayedGhostAudio && ghostTVAudio != null)
        {
            Debug.Log("[LivingRoom] Scheduling ghost TV audio...");
            StartCoroutine(PlayGhostTVAudioAfterDelay());
        }
    }

    IEnumerator PlayGhostTVAudioAfterDelay()
    {
        if (hasPlayedGhostAudio || ghostTVAudio == null)
            yield break;

        Debug.Log("[LivingRoom] Waiting to play ghost TV audio...");

        yield return new WaitForSeconds(ghostAudioDelay);

        Debug.Log("[LivingRoom] Playing ghost TV audio now!");

        ghostAudioRunning = true;

        AudioManager.Instance?.PlaySFX(ghostTVAudio);

        hasPlayedGhostAudio = true;
        SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_PLAYED);
        SaveSystem.Instance.OnStoryProgressMade();

        // NEW: Show Lisa's reaction dialogue
        bool hasShownDialogue = SaveSystem.Instance.WasDialogueTriggered(FLAG_TV_GHOST_DIALOGUE);
        if (!hasShownDialogue)
        {
            // Stop the player
            if (playerController != null) playerController.enabled = false;

            yield return new WaitForSeconds(0.5f);

            DialogueSystemV2.Instance?.StartDialogue("It's the TV again... what's wrong with it? I turned it off already.", "Lisa");

            // Wait for dialogue to finish
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
                yield return null;

            // Re-enable player
            if (playerController != null) playerController.enabled = true;

            SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_DIALOGUE);
            SaveSystem.Instance.OnStoryProgressMade();
        }

        if (ghostTVAudio != null)
            yield return new WaitForSeconds(ghostTVAudio.length);

        ghostAudioRunning = false;
    }

    void OnEnable()
    {
        CheckPuzzleCompletion();

        if (tvTurnedOff)
        {
            AudioManager.Instance?.StopLoopingSFX("tv_static");
            if (tvAnimator != null) tvAnimator.SetTrigger("TurnOff");
        }

        if (lullabyPlayed && rockingChairAnimator != null)
        {
            rockingChairAnimator.SetBool("isRocking", true);
            if (rockingChairSound != null)
                AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
        }
    }

    void OnDisable()
    {
        if (rockingChairSound != null)
            AudioManager.Instance?.StopAmbient();
    }

    public void OnFrameExamine()
    {
        DialogueSystemV2.Instance?.StartDialogue(
            "These photos... the woman's face is scratched out in every single one. Who would do this?",
            "Lisa"
        );
    }

    public void OnBookshelf2Interact()
    {
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

        if (state.interactedObjects.Contains("booksPushed"))
        {
            if (!SaveSystem.Instance.HasItem(SMALL_KEY_ID))
                DialogueSystemV2.Instance?.StartDialogue("There's something underneath the books...", "Lisa");
            else
                DialogueSystemV2.Instance?.StartDialogue("The books are still scattered on the floor.", "Lisa");
            return;
        }

        StartCoroutine(BookshelfShakeSequence());
    }

    IEnumerator BookshelfShakeSequence()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            StartCoroutine(JumpPlayerBack(player));

        if (bookshelfShakeSound != null)
            AudioManager.Instance?.PlaySFX(bookshelfShakeSound);

        DialogueSystemV2.Instance?.StartDialogue("Woah!", "Lisa");

        yield return StartCoroutine(ShakeCamera());

        if (books_Pushed != null) books_Pushed.SetActive(true);
        if (smallKey != null) smallKey.SetActive(true);

        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
        state.interactedObjects.Add("booksPushed");
        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);

        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        DialogueSystemV2.Instance?.StartDialogue("There's something underneath the books...", "Lisa");
    }

    IEnumerator JumpPlayerBack(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.down * jumpBackDistance;

        float elapsed = 0f;

        while (elapsed < jumpBackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpBackDuration;
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);
            player.position = Vector3.Lerp(startPos, targetPos, smoothT);
            yield return null;
        }

        player.position = targetPos;
    }

    IEnumerator ShakeCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    public void OnSmallKeyInteract()
    {
        ShowItemPickupChoice(SMALL_KEY_ID, "Small Golden Key", "A small golden key. Take it?", "Small Golden Key added to inventory.");
    }

    public void OnToyBoxInteract()
    {
        if (!SaveSystem.Instance.HasItem(SMALL_KEY_ID))
        {
            DialogueSystemV2.Instance?.StartDialogue("This toy box is locked. I need a key to open it.", "Lisa");
            return;
        }

        if (SaveSystem.Instance.HasItem(TEDDY_BEAR_ID) && SaveSystem.Instance.HasItem(MUSIC_BOX_ID))
        {
            DialogueSystemV2.Instance?.StartDialogue("The toy box is already empty.", "Lisa");
            return;
        }

        DialogueSystemV2.Instance?.StartDialogue("The small key fits! The toy box is now open.", "Lisa");
        StartCoroutine(ShowToyBoxContents());
    }

    IEnumerator ShowToyBoxContents()
    {
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        DialogueSystemV2.Instance?.ShowChoices(
            new string[] { "Take all items", "Leave them" },
            new System.Action[]
            {
                () => StartCoroutine(TakeAllToyBoxItems()),
                () => DialogueSystemV2.Instance?.StartDialogue("I'll leave them for now.", "Lisa")
            }
        );
    }

    IEnumerator TakeAllToyBoxItems()
    {
        InventoryManager.Instance?.AddItem(TEDDY_BEAR_ID);
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItem(MUSIC_BOX_ID);
        yield return new WaitForSeconds(0.3f);

        DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles and a broken music box added to inventory.", "Lisa");
    }

    public void OnCouchInteract()
    {
        bool hasDiary1 = (GlobalDiaryManager.Instance != null && GlobalDiaryManager.Instance.HasDiaryPage(DIARY_1_ID)) || SaveSystem.Instance.HasItem(DIARY_1_ID);
        bool hasDiary2 = (GlobalDiaryManager.Instance != null && GlobalDiaryManager.Instance.HasDiaryPage(DIARY_2_ID)) || SaveSystem.Instance.HasItem(DIARY_2_ID);

        if (hasDiary1 && hasDiary2)
        {
            DialogueSystemV2.Instance?.StartDialogue("I already searched the couch cushions.", "Lisa");
            return;
        }

        DialogueSystemV2.Instance?.StartDialogue("There are some torn diary pages hidden in the couch cushions.", "Lisa");
        StartCoroutine(ShowCouchItems());
    }

    IEnumerator ShowCouchItems()
    {
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        bool needsDiary1 = GlobalDiaryManager.Instance == null || !GlobalDiaryManager.Instance.HasDiaryPage(DIARY_1_ID);
        bool needsDiary2 = GlobalDiaryManager.Instance == null || !GlobalDiaryManager.Instance.HasDiaryPage(DIARY_2_ID);

        if (needsDiary1 && needsDiary2)
        {
            DialogueSystemV2.Instance?.ShowChoices(
                new string[] { "Take all pages", "Leave them" },
                new System.Action[]
                {
                    () => StartCoroutine(TakeAllCouchItems()),
                    () => DialogueSystemV2.Instance?.StartDialogue("I'll leave them for now.", "Lisa")
                }
            );
        }
        else if (needsDiary1)
        {
            ShowItemPickupChoice(DIARY_1_ID, "Diary Page 1", "Take Diary Page 1?", "Diary Page 1 added to inventory.");
        }
        else if (needsDiary2)
        {
            ShowItemPickupChoice(DIARY_2_ID, "Diary Page 2", "Take Diary Page 2?", "Diary Page 2 added to inventory.");
        }
    }

    IEnumerator TakeAllCouchItems()
    {
        InventoryManager.Instance?.AddItem(DIARY_1_ID);
        GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_1_ID);
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItem(DIARY_2_ID);
        GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_2_ID);
        yield return new WaitForSeconds(0.3f);

        DialogueSystemV2.Instance?.StartDialogue("Diary Pages 1 and 2 added to inventory.", "Lisa");
    }

    public void OnLooseFloorboardInteract()
    {
        bool hasDiary3 = (GlobalDiaryManager.Instance != null && GlobalDiaryManager.Instance.HasDiaryPage(DIARY_3_ID)) || SaveSystem.Instance.HasItem(DIARY_3_ID);
        bool hasDiary4 = (GlobalDiaryManager.Instance != null && GlobalDiaryManager.Instance.HasDiaryPage(DIARY_4_ID)) || SaveSystem.Instance.HasItem(DIARY_4_ID);

        if (hasDiary3 && hasDiary4)
        {
            DialogueSystemV2.Instance?.StartDialogue("The floorboard is loose, but there's nothing else underneath.", "Lisa");
            return;
        }

        DialogueSystemV2.Instance?.StartDialogue("There are more diary pages hidden underneath the loose floorboard.", "Lisa");

        StartCoroutine(ShowFloorboardItems());
    }

    IEnumerator ShowFloorboardItems()
    {
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        bool needsDiary3 = !SaveSystem.Instance.HasItem(DIARY_3_ID);
        bool needsDiary4 = !SaveSystem.Instance.HasItem(DIARY_4_ID);

        if (needsDiary3 && needsDiary4)
        {
            DialogueSystemV2.Instance?.ShowChoices(
                new string[] { "Take all pages", "Leave them" },
                new System.Action[]
                {
                    () => StartCoroutine(TakeAllFloorboardItems()),
                    () => DialogueSystemV2.Instance?.StartDialogue("I'll leave them for now.", "Lisa")
                }
            );
        }
        else if (needsDiary3)
        {
            ShowItemPickupChoice(DIARY_3_ID, "Diary Page 3", "Take Diary Page 3?", "Diary Page 3 added to inventory.");
        }
        else if (needsDiary4)
        {
            ShowItemPickupChoice(DIARY_4_ID, "Diary Page 4", "Take Diary Page 4?", "Diary Page 4 added to inventory.");
        }
    }

    IEnumerator TakeAllFloorboardItems()
    {
        InventoryManager.Instance?.AddItem(DIARY_3_ID);
        GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_3_ID);
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItem(DIARY_4_ID);
        GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_4_ID);
        yield return new WaitForSeconds(0.3f);

        DialogueSystemV2.Instance?.StartDialogue("Diary Pages 3 and 4 added to inventory.", "Lisa");
    }

    public void OnCoffeeTableKeyInteract()
    {
        ShowItemPickupChoice(COFFEE_TABLE_KEY_ID, "Hallway Door Key", "A key to the hallway! Take it?", "Hallway Door Key added to inventory.");
    }

    void ShowItemPickupChoice(string itemId, string itemName, string message, string confirmMessage = "")
    {
        DialogueSystemV2.Instance?.ShowChoices(
            new string[] { "Take it", "Leave it" },
            new System.Action[]
            {
                () => {
                    InventoryManager.Instance?.AddItem(itemId);

                    if (GlobalDiaryManager.Instance != null && itemId.StartsWith("diary_page_"))
                    {
                        GlobalDiaryManager.Instance.AddDiaryPage(itemId);
                    }

                    RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
                    if (!state.collectedItems.Contains(itemId))
                    {
                        state.collectedItems.Add(itemId);
                        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
                    }

                    if (itemId == SMALL_KEY_ID && smallKey != null) smallKey.SetActive(false);
                    if (itemId == COFFEE_TABLE_KEY_ID && coffeeTable_Key != null) coffeeTable_Key.SetActive(false);

                    if (!string.IsNullOrEmpty(confirmMessage))
                        DialogueSystemV2.Instance?.StartDialogue(confirmMessage, "Lisa");
                },
                () => {
                    DialogueSystemV2.Instance?.StartDialogue("I'll leave it for now.", "Lisa");
                }
            }
        );
    }

    public void CheckPuzzleCompletion()
    {
        // ---------------- FIX FOR NRE ----------------
        if (SaveSystem.Instance == null)
        {
            Debug.LogWarning("[Room02_LivingRoomController] SaveSystem.Instance is null. Skipping puzzle check.");
            return;
        }
        // ---------------------------------------------

        if (!musicBoxPuzzleComplete && SaveSystem.Instance.HasItem(MUSIC_BOX_COMPLETE_ID))
        {
            musicBoxPuzzleComplete = true;
            RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
            state.solvedPuzzles.Add("musicBoxPuzzle");
            SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
        }

        if (!diaryPuzzleComplete && SaveSystem.Instance.HasItem(DIARY_1_ID))
        {
            diaryPuzzleComplete = true;
            RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
            state.solvedPuzzles.Add("diaryPuzzle");
            SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
        }

        if (musicBoxPuzzleComplete && diaryPuzzleComplete)
        {
            if (coffeeTable_Key != null && !SaveSystem.Instance.HasItem(COFFEE_TABLE_KEY_ID))
            {
                RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

                if (!state.interactedObjects.Contains("coffeeTableKeyRevealed"))
                {
                    StartCoroutine(RevealCoffeeTableKey());
                    state.interactedObjects.Add("coffeeTableKeyRevealed");
                    SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
                }
                else
                {
                    coffeeTable_Key.SetActive(true);
                }
            }
        }
    }

    IEnumerator RevealCoffeeTableKey()
    {
        if (keyRevealSound != null)
            AudioManager.Instance?.PlaySFX(keyRevealSound);

        yield return new WaitForSeconds(0.2f);

        if (coffeeTable_Key != null)
            coffeeTable_Key.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        DialogueSystemV2.Instance?.StartDialogue("What was that?", "Lisa");
    }

    public void OnLullabyPlayed()
    {
        if (lullabyPlayed) return;

        lullabyPlayed = true;
        SaveRoomState("lullabyPlayed", true);

        StartCoroutine(LullabySequence());
    }

    IEnumerator LullabySequence()
    {
        yield return new WaitForSeconds(1f);

        if (rockingChairAnimator != null)
            rockingChairAnimator.SetBool("isRocking", true);

        AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
    }
}