using UnityEngine;
using System.Collections;

public class Room02_LivingRoomController : MonoBehaviour
{
    public static Room02_LivingRoomController Instance { get; private set; }

    [Header("Room Objects")]
    public GameObject tv;
    public GameObject rockingChair;
    public GameObject coffeeTable_Key; // This is the Hallway Key Item (sprite with collider)
    public GameObject books_Pushed;
    public GameObject smallKey;

    [Header("Hallway Event Trigger")]
    [Tooltip("Location where the player must step to trigger the 'What was that?' dialogue. Defaults to CoffeeTable_Key if empty.")]
    public Transform hallwayTriggerLocation;
    public float hallwayTriggerRadius = 2.0f;
    [SerializeField] private bool hallwayTriggerArmed = false; // Set true after cutscene

    [Header("Animators")]
    public Animator tvAnimator;
    public Animator rockingChairAnimator;

    [Header("Audio Clips")]
    public AudioClip tvStaticSound;
    public AudioClip rockingChairSound;
    public AudioClip lullabyFragment;
    public AudioClip bookshelfShakeSound;
    public AudioClip keyRevealSound; // plays when CoffeeTable_Key is revealed
    public AudioClip tvTurnOffSound; // NEW: Played when TV turns off
    public AudioClip toyBoxUnlockSound; // NEW: Played when toy box unlocks

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
    private Transform playerTransform;

    // NEW: guard so hallway sequence doesn't spam
    [SerializeField] private bool hallwayEventRunning = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        playerController = FindFirstObjectByType<JoystickPlayerController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Default trigger location to the key's position if not explicitly set
        if (hallwayTriggerLocation == null && coffeeTable_Key != null)
        {
            hallwayTriggerLocation = coffeeTable_Key.transform;
        }

        InitializeRoom();
    }

    void Update()
    {
        // Check for Hallway Event Trigger (The "What was that?" moment)
        // Only active AFTER cutscene (hallwayTriggerArmed) and BEFORE key is revealed.
        if (hallwayTriggerArmed && !hallwayEventRunning && playerTransform != null)
        {
            Vector3 targetPos = hallwayTriggerLocation != null ? hallwayTriggerLocation.position : Vector3.zero;
            float dist = Vector2.Distance(playerTransform.position, targetPos);

            if (dist <= hallwayTriggerRadius)
            {
                StartCoroutine(PlayHallwayEventSequence());
            }
        }
    }

    void InitializeRoom()
    {
        // Default: hide key, then let LoadRoomState re-show it if already revealed
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
            // CHANGED: Use PlayLoopingSFX instead of PlayAmbient so it doesn't override room music
            AudioManager.Instance?.PlayLoopingSFX(rockingChairSound, "rocking_chair");
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

        // If we already revealed the key but didn't pick it up, ensure it's visible
        if (state.interactedObjects.Contains("coffeeTableKeyRevealed") && !state.collectedItems.Contains(COFFEE_TABLE_KEY_ID))
        {
            if (coffeeTable_Key != null)
            {
                coffeeTable_Key.SetActive(true);
                Debug.Log("[Room02] CoffeeTable_Key restored visible from saved state.");
            }
        }
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
            new DialogueLine { text = EnhancedGameDialogues.R02_TV_MESSAGE_1, speakerName = "???" },
            new DialogueLine { text = EnhancedGameDialogues.R02_TV_MESSAGE_2, speakerName = "Lisa" }
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

        // --- GHOST AUDIO CASE ---
        if (ghostAudioRunning && ghostTVAudio != null)
        {
            // 1. Stop the ghost audio
            // FIX: Stop the local source specifically
            if (tvAudioSource != null && tvAudioSource.isPlaying)
            {
                tvAudioSource.Stop();
            }

            // Safety: Stop any global one-shots if they were used
            AudioManager.Instance?.StopAllSFX();

            ghostAudioRunning = false;

            // 2. Play Turn Off Sound
            if (tvTurnOffSound != null)
            {
                AudioManager.Instance?.PlaySFX(tvTurnOffSound);
            }

            // 3. Trigger Animation
            if (tvAnimator != null)
            {
                tvAnimator.SetTrigger("TurnOff");
            }

            // 4. Update State
            tvTurnedOff = true;
            SaveRoomState("tvTurnedOff", true);

            // 5. Handle Flags
            if (!hasPlayedGhostAudio)
            {
                SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_PLAYED);
                SaveSystem.Instance.OnStoryProgressMade();
                hasPlayedGhostAudio = true;
            }

            // 6. Dialogue
            DialogueSystemV2.Instance?.StartDialogue("I should turn that off.", "Lisa");
            Debug.Log("[LivingRoom] Lisa manually stopped ghost TV audio.");
            return;
        }

        // --- ALREADY OFF CASE ---
        if (tvTurnedOff)
        {
            DialogueSystemV2.Instance?.StartDialogue("The TV is already off.", "Lisa");
            return;
        }

        // --- EMILY / INTRO CASE ---
        if (emilyDialogueSound != null)
            AudioManager.Instance?.PlayDialogue(emilyDialogueSound);

        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = EnhancedGameDialogues.R02_TV_MESSAGE_1, speakerName = "???" },
            new DialogueLine { text = EnhancedGameDialogues.R02_TV_OFF_1, speakerName = "Lisa" }
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

        // Play dedicated turn off sound
        if (tvTurnOffSound != null)
        {
            AudioManager.Instance?.PlaySFX(tvTurnOffSound);
        }

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

        // Resume TV static animation while ghost audio plays
        if (tvAnimator != null)
        {
            tvAnimator.SetTrigger("PlayStatic");
        }

        // FIX: Use the local AudioSource on the TV GameObject.
        // This ensures the sound is attached to the TV object.
        // When the Room02 scene unloads, the TV object is destroyed, and this audio stops automatically.
        if (tvAudioSource != null)
        {
            tvAudioSource.clip = ghostTVAudio;
            tvAudioSource.Play();
        }
        else
        {
            // Fallback to global manager if local source missing (though less safe for bleeding)
            AudioManager.Instance?.PlaySFX(ghostTVAudio);
        }

        hasPlayedGhostAudio = true;
        SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_PLAYED);
        SaveSystem.Instance.OnStoryProgressMade();

        bool hasShownDialogue = SaveSystem.Instance.WasDialogueTriggered(FLAG_TV_GHOST_DIALOGUE);
        if (!hasShownDialogue)
        {
            // Wait 3 seconds before showing Lisa's reaction
            yield return new WaitForSeconds(3.0f);

            if (playerController != null) playerController.enabled = false;

            DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_TV_GHOST_1, "Lisa");

            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
                yield return null;

            if (playerController != null) playerController.enabled = true;

            SaveSystem.Instance.TriggerDialogue(FLAG_TV_GHOST_DIALOGUE);
            SaveSystem.Instance.OnStoryProgressMade();
        }

        if (ghostTVAudio != null)
            yield return new WaitForSeconds(ghostTVAudio.length - 3.0f);

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
            // CHANGED: Use PlayLoopingSFX here as well
            AudioManager.Instance?.PlayLoopingSFX(rockingChairSound, "rocking_chair");
        }
    }

    void OnDisable()
    {
        // FIX: Explicitly stop specific room loops so they don't bleed into next scene
        AudioManager.Instance?.StopLoopingSFX("tv_static");
        AudioManager.Instance?.StopLoopingSFX("rocking_chair");

        // FIX: Ensure local TV audio source stops if scene changes while it's playing
        if (tvAudioSource != null)
        {
            tvAudioSource.Stop();
        }
    }

    public void OnFrameExamine()
    {
        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_FRAME_1, "Lisa");
    }

    public void OnBookshelf2Interact()
    {
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

        if (state.interactedObjects.Contains("booksPushed"))
        {
            if (!SaveSystem.Instance.HasItem(SMALL_KEY_ID))
                DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_BOOKSHELF_2, "Lisa");
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

        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_BOOKSHELF_1, "Lisa");

        yield return StartCoroutine(ShakeCamera());

        if (books_Pushed != null) books_Pushed.SetActive(true);
        if (smallKey != null) smallKey.SetActive(true);

        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
        state.interactedObjects.Add("booksPushed");
        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);

        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_BOOKSHELF_2, "Lisa");
    }

    IEnumerator JumpPlayerBack(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.left * jumpBackDistance;

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
        AutoPickupItem(SMALL_KEY_ID, "Small Golden Key added to inventory.");
    }

    public void OnToyBoxInteract()
    {
        if (!SaveSystem.Instance.HasItem(SMALL_KEY_ID))
        {
            DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_TOYBOX_LOCKED, "Lisa");
            return;
        }

        if (SaveSystem.Instance.HasItem(TEDDY_BEAR_ID) && SaveSystem.Instance.HasItem(MUSIC_BOX_ID))
        {
            DialogueSystemV2.Instance?.StartDialogue("The toy box is already empty.", "Lisa");
            return;
        }

        if (toyBoxUnlockSound != null)
            AudioManager.Instance?.PlaySFX(toyBoxUnlockSound);

        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_TOYBOX_OPEN_1, "Lisa");
        StartCoroutine(ShowToyBoxContents());
    }

    IEnumerator ShowToyBoxContents()
    {
        // Wait for dialogue to finish first
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        // Add items individually with notifications
        InventoryManager.Instance?.AddItemWithNotification(TEDDY_BEAR_ID);
        
        // Wait for notification to finish before showing next one
        while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
            yield return null;
        
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItemWithNotification(MUSIC_BOX_ID);
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

        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_COUCH_DIARY, "Lisa");
        StartCoroutine(ShowCouchItems());
    }

    IEnumerator ShowCouchItems()
    {
        // Wait for dialogue to finish first
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        bool needsDiary1 = GlobalDiaryManager.Instance == null || !GlobalDiaryManager.Instance.HasDiaryPage(DIARY_1_ID);
        bool needsDiary2 = GlobalDiaryManager.Instance == null || !GlobalDiaryManager.Instance.HasDiaryPage(DIARY_2_ID);

        if (needsDiary1 && needsDiary2)
        {
            // Add pages individually with notifications
            InventoryManager.Instance?.AddItemWithNotification(DIARY_1_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_1_ID);
            
            // Wait for notification to finish before showing next one
            while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
                yield return null;
            
            yield return new WaitForSeconds(0.3f);

            InventoryManager.Instance?.AddItemWithNotification(DIARY_2_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_2_ID);
        }
        else if (needsDiary1)
        {
            InventoryManager.Instance?.AddItemWithNotification(DIARY_1_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_1_ID);
        }
        else if (needsDiary2)
        {
            InventoryManager.Instance?.AddItemWithNotification(DIARY_2_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_2_ID);
        }
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

        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_FLOORBOARD, "Lisa");

        StartCoroutine(ShowFloorboardItems());
    }

    IEnumerator ShowFloorboardItems()
    {
        // Wait for dialogue to finish first
        while (DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        bool needsDiary3 = !SaveSystem.Instance.HasItem(DIARY_3_ID);
        bool needsDiary4 = !SaveSystem.Instance.HasItem(DIARY_4_ID);

        if (needsDiary3 && needsDiary4)
        {
            // Add pages individually with notifications
            InventoryManager.Instance?.AddItemWithNotification(DIARY_3_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_3_ID);
            
            // Wait for notification to finish before showing next one
            while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
                yield return null;
            
            yield return new WaitForSeconds(0.3f);

            InventoryManager.Instance?.AddItemWithNotification(DIARY_4_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_4_ID);
        }
        else if (needsDiary3)
        {
            InventoryManager.Instance?.AddItemWithNotification(DIARY_3_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_3_ID);
        }
        else if (needsDiary4)
        {
            InventoryManager.Instance?.AddItemWithNotification(DIARY_4_ID);
            GlobalDiaryManager.Instance?.AddDiaryPage(DIARY_4_ID);
        }
    }

    public void OnCoffeeTableKeyInteract()
    {
        AutoPickupItem(COFFEE_TABLE_KEY_ID, "Hallway Door Key added to inventory.");
    }

    void AutoPickupItem(string itemId, string confirmMessage = "")
    {
        // Hide the object immediately
        if (itemId == SMALL_KEY_ID && smallKey != null) smallKey.SetActive(false);
        if (itemId == COFFEE_TABLE_KEY_ID && coffeeTable_Key != null) coffeeTable_Key.SetActive(false);

        // Add to diary manager if it's a diary page
        if (GlobalDiaryManager.Instance != null && itemId.StartsWith("diary_page_"))
        {
            GlobalDiaryManager.Instance.AddDiaryPage(itemId);
        }

        // Use AddItemWithNotification - it handles everything (inventory + save system + notification)
        InventoryManager.Instance?.AddItemWithNotification(itemId, confirmMessage);
        
        // Update room state to track that this item was collected in this room
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
        if (!state.collectedItems.Contains(itemId))
        {
            state.collectedItems.Add(itemId);
            SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
        }
    }

    public void CheckPuzzleCompletion()
    {
        if (SaveSystem.Instance == null) return;

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
    }

    // Called by MusicBoxController after cutscene finishes
    public void OnMusicBoxCutsceneEnded()
    {
        Debug.Log("[Room02] Cutscene ended. Revealing key immediately.");

        // Disable the trigger logic just in case
        hallwayTriggerArmed = false;

        // Reveal the key and play sound immediately
        RevealCoffeeTableKeyAndSound();

        // Restore scene ambient audio
        SceneAmbientPlayer ambientPlayer = FindFirstObjectByType<SceneAmbientPlayer>();
        if (ambientPlayer != null && ambientPlayer.sceneAmbientConfig != null)
        {
            Debug.Log("[Room02] Restoring scene ambient audio...");
            AudioManager.Instance?.PlayAmbient(
                ambientPlayer.sceneAmbientConfig.ambientClip,
                true,
                2.0f // Smooth fade in
            );
        }

        // Trigger the reaction dialogue
        DialogueSystemV2.Instance?.StartDialogue(EnhancedGameDialogues.R02_KEY_APPEARS_1, "Lisa");
    }

    // Sequence for when player steps into the trigger AFTER cutscene
    IEnumerator PlayHallwayEventSequence()
    {
        if (hallwayEventRunning)
            yield break; // already running

        hallwayEventRunning = true;
        hallwayTriggerArmed = false; // fire only once

        Debug.Log("[Room02] Hallway event triggered (What... was that?).");

        // 1. Dialogue
        DialogueSystemV2.Instance?.StartDialogue("What... was that?", "Lisa");

        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            yield return null;

        // 2. Reveal key + play sound
        RevealCoffeeTableKeyAndSound();

        hallwayEventRunning = false;
    }

    // Central helper that actually reveals the CoffeeTable_Key and plays the sound
    private void RevealCoffeeTableKeyAndSound()
    {
        if (keyRevealSound != null)
        {
            Debug.Log("[Room02] Playing keyRevealSound.");
            AudioManager.Instance?.PlaySFX(keyRevealSound);
        }
        else
        {
            Debug.LogWarning("[Room02] keyRevealSound is NULL – assign an AudioClip in the inspector.");
        }

        if (coffeeTable_Key != null)
        {
            coffeeTable_Key.SetActive(true);
            Debug.Log("[Room02] CoffeeTable_Key revealed. activeSelf = " + coffeeTable_Key.activeSelf);
        }
        else
        {
            Debug.LogError("[Room02] coffeeTable_Key reference is NULL – drag CoffeeTable_Key into the field on this script.");
        }

        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
        if (!state.interactedObjects.Contains("coffeeTableKeyRevealed"))
        {
            state.interactedObjects.Add("coffeeTableKeyRevealed");
            SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
        }
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

        // CHANGED: Use PlayLoopingSFX so it doesn't kill the room ambient
        AudioManager.Instance?.PlayLoopingSFX(rockingChairSound, "rocking_chair");
    }

    void OnDrawGizmosSelected()
    {
        if (hallwayTriggerLocation != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hallwayTriggerLocation.position, hallwayTriggerRadius);
        }
    }
}