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

    [Header("Camera Shake")]
    public float shakeIntensity = 0.08f; // FIX #1: Even more minimal shake
    public float shakeDuration = 0.3f;   // FIX #1: Shorter duration

    [Header("Lisa Jump Back")]
    public float jumpBackDistance = 1f;  // FIX #1: How far Lisa jumps back
    public float jumpBackDuration = 0.3f; // FIX #1: How fast the jump is

    [Header("Emily's Dialogue Sound")]
    public AudioClip emilyDialogueSound; // EDIT #2: Emily's unique dialogue sound

    [Header("Puzzle Tracking")]
    private bool musicBoxPuzzleComplete = false;
    private bool diaryPuzzleComplete = false;
    private bool tvTurnedOff = false;
    private bool hasEnteredRoom = false;
    private bool lullabyPlayed = false;

    private const string ROOM_NAME = "Room02_LivingRoom";

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
    private const string DIARY_COMPLETE_ID = "diary_complete";
    private const string COFFEE_TABLE_KEY_ID = "hallway_door_key";

    void Start()
    {
        InitializeRoom();
    }

    void InitializeRoom()
    {
        // Hide items initially
        if (coffeeTable_Key != null) coffeeTable_Key.SetActive(false);
        if (books_Pushed != null) books_Pushed.SetActive(false);
        if (smallKey != null) smallKey.SetActive(false);

        // Load room state
        LoadRoomState();

        // TV entrance sequence (only if not turned off)
        if (!hasEnteredRoom && !tvTurnedOff)
        {
            StartCoroutine(TVEntranceSequence());
            hasEnteredRoom = true;
            SaveRoomState("hasEnteredRoom", true);
        }
        else if (tvTurnedOff)
        {
            // TV already turned off, set to off state
            if (tvAnimator != null)
            {
                tvAnimator.SetTrigger("TurnOff");
            }
        }

        // Resume rocking chair if lullaby was already played
        if (lullabyPlayed && rockingChairAnimator != null)
        {
            rockingChairAnimator.SetBool("isRocking", true);
            AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
        }

        // Check if puzzles already complete
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

        // Restore visual states
        if (state.interactedObjects.Contains("booksPushed") && books_Pushed != null)
        {
            books_Pushed.SetActive(true);
        }

        if (state.collectedItems.Contains(SMALL_KEY_ID) && smallKey != null)
        {
            smallKey.SetActive(false);
        }

        if (state.collectedItems.Contains(COFFEE_TABLE_KEY_ID) && coffeeTable_Key != null)
        {
            coffeeTable_Key.SetActive(false);
        }
    }

    void SaveRoomState(string key, bool value)
    {
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);

        if (value && !state.interactedObjects.Contains(key))
        {
            state.interactedObjects.Add(key);
        }
        else if (!value)
        {
            state.interactedObjects.Remove(key);
        }

        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
    }

    IEnumerator TVEntranceSequence()
    {
        yield return new WaitForSeconds(0.5f);

        // Play TV static animation
        if (tvAnimator != null)
        {
            tvAnimator.SetTrigger("PlayStatic");
        }

        // Play looping TV static with unique ID
        AudioManager.Instance?.PlayLoopingSFX(tvStaticSound, "tv_static");

        yield return new WaitForSeconds(1f);

        // EDIT #2: Play Emily's dialogue sound
        if (emilyDialogueSound != null)
        {
            AudioManager.Instance?.PlayDialogue(emilyDialogueSound);
        }

        // Emily's warning dialogue
        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = "GO AWAY!!!!!!! GO AWAY!!!!!!", speakerName = "???" },
            new DialogueLine { text = "What is that?!", speakerName = "Lisa" }
        });
    }

    public void OnTVInteract()
    {
        if (tvTurnedOff)
        {
            DialogueSystemV2.Instance?.StartDialogue("The TV is already off.", "Lisa");
            return;
        }

        // EDIT #2: Play Emily's dialogue sound
        if (emilyDialogueSound != null)
        {
            AudioManager.Instance?.PlayDialogue(emilyDialogueSound);
        }

        // Show Emily's second warning
        DialogueSystemV2.Instance?.StartDialogue(new DialogueLine[]
        {
            new DialogueLine { text = "IF YOU DON'T LEAVE THIS HOUSE, YOU'LL REGRET IT.", speakerName = "???" },
            new DialogueLine { text = "It's coming from the TV! I need to turn it off!", speakerName = "Lisa" }
        });

        // Turn off TV after dialogue
        StartCoroutine(TurnOffTVAfterDialogue());
    }

    IEnumerator TurnOffTVAfterDialogue()
    {
        // Wait for dialogue to end
        while (DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        // Stop TV static sound by ID
        AudioManager.Instance?.StopLoopingSFX("tv_static");

        // Change TV to static sprite
        if (tvAnimator != null)
        {
            tvAnimator.SetTrigger("TurnOff");
        }

        tvTurnedOff = true;
        SaveRoomState("tvTurnedOff", true);
    }

    void OnEnable()
    {
        // Recheck puzzle completion when returning to room
        CheckPuzzleCompletion();

        // Restore TV state - FIX BUG #3
        if (tvTurnedOff)
        {
            // Ensure TV static sound is NOT playing
            AudioManager.Instance?.StopLoopingSFX("tv_static");

            // Ensure TV is visually off
            if (tvAnimator != null)
            {
                tvAnimator.SetTrigger("TurnOff");
            }
        }

        // Restore rocking chair state
        if (lullabyPlayed && rockingChairAnimator != null)
        {
            rockingChairAnimator.SetBool("isRocking", true);
            if (rockingChairSound != null)
            {
                AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
            }
        }
    }

    void OnDisable()
    {
        // Stop rocking chair sound when leaving room
        if (rockingChairSound != null)
        {
            AudioManager.Instance?.StopAmbient();
        }
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
            // Books already pushed
            if (!SaveSystem.Instance.HasItem(SMALL_KEY_ID))
            {
                // Key is visible but not taken - EDIT #1
                DialogueSystemV2.Instance?.StartDialogue("There's something underneath the books...", "Lisa");
            }
            else
            {
                DialogueSystemV2.Instance?.StartDialogue("The books are still scattered on the floor.", "Lisa");
            }
            return;
        }

        // First interaction - EDIT #1: Shake screen and play sound
        StartCoroutine(BookshelfShakeSequence());
    }

    IEnumerator BookshelfShakeSequence()
    {
        // FIX #1: Make Lisa jump back first
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            StartCoroutine(JumpPlayerBack(player));
        }

        // Play shake sound
        if (bookshelfShakeSound != null)
        {
            AudioManager.Instance?.PlaySFX(bookshelfShakeSound);
        }

        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue("Woah!", "Lisa");

        // Shake camera (minimal)
        yield return StartCoroutine(ShakeCamera());

        // Show books and key
        if (books_Pushed != null)
        {
            books_Pushed.SetActive(true);
        }

        if (smallKey != null)
        {
            smallKey.SetActive(true);
        }

        // Save state
        RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
        state.interactedObjects.Add("booksPushed");
        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);

        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        // Show discovery message
        DialogueSystemV2.Instance?.StartDialogue("There's something underneath the books...", "Lisa");
    }

    IEnumerator JumpPlayerBack(Transform player)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + Vector3.down * jumpBackDistance; // Jump back downward

        float elapsed = 0f;

        while (elapsed < jumpBackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpBackDuration;

            // Smooth jump with easing
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
        // EDIT #3: Add confirmation message
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

        // Open toy box
        DialogueSystemV2.Instance?.StartDialogue("The small key fits! The toy box is now open.", "Lisa");

        StartCoroutine(ShowToyBoxContents());
    }

    IEnumerator ShowToyBoxContents()
    {
        while (DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        // EDIT #2: Prompt to take all items at once
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
        // Add teddy bear
        InventoryManager.Instance?.AddItem(TEDDY_BEAR_ID);
        yield return new WaitForSeconds(0.3f);

        // Add broken music box
        InventoryManager.Instance?.AddItem(MUSIC_BOX_ID);
        yield return new WaitForSeconds(0.3f);

        // EDIT #3: Confirmation message
        DialogueSystemV2.Instance?.StartDialogue("Mr. Snuggles and a broken music box added to inventory.", "Lisa");
    }

    public void OnCouchInteract()
    {
        // FIX BUG #1: Check for complete diary OR individual pages
        bool hasDiaryComplete = SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);
        bool hasDiary1 = SaveSystem.Instance.HasItem(DIARY_1_ID);
        bool hasDiary2 = SaveSystem.Instance.HasItem(DIARY_2_ID);

        if (hasDiaryComplete || (hasDiary1 && hasDiary2))
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
        {
            yield return null;
        }

        // FIX #4: Add delay to prevent dialogue being too fast
        yield return new WaitForSeconds(0.5f);

        // EDIT #2: Take all items at once
        bool needsDiary1 = !SaveSystem.Instance.HasItem(DIARY_1_ID) && !SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);
        bool needsDiary2 = !SaveSystem.Instance.HasItem(DIARY_2_ID) && !SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);

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
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItem(DIARY_2_ID);
        yield return new WaitForSeconds(0.3f);

        // EDIT #3: Confirmation
        DialogueSystemV2.Instance?.StartDialogue("Diary Pages 1 and 2 added to inventory.", "Lisa");
    }

    public void OnLooseFloorboardInteract()
    {
        // FIX BUG #1: Check for complete diary OR individual pages
        bool hasDiaryComplete = SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);
        bool hasDiary3 = SaveSystem.Instance.HasItem(DIARY_3_ID);
        bool hasDiary4 = SaveSystem.Instance.HasItem(DIARY_4_ID);

        if (hasDiaryComplete || (hasDiary3 && hasDiary4))
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
        {
            yield return null;
        }

        // EDIT #2: Take all items at once
        bool needsDiary3 = !SaveSystem.Instance.HasItem(DIARY_3_ID) && !SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);
        bool needsDiary4 = !SaveSystem.Instance.HasItem(DIARY_4_ID) && !SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID);

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
        yield return new WaitForSeconds(0.3f);

        InventoryManager.Instance?.AddItem(DIARY_4_ID);
        yield return new WaitForSeconds(0.3f);

        // EDIT #3: Confirmation
        DialogueSystemV2.Instance?.StartDialogue("Diary Pages 3 and 4 added to inventory.", "Lisa");
    }

    public void OnCoffeeTableKeyInteract()
    {
        // EDIT #3: Add confirmation message
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

                    RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
                    if (!state.collectedItems.Contains(itemId))
                    {
                        state.collectedItems.Add(itemId);
                        SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
                    }
                    
                    // Hide the pickup object
                    if (itemId == SMALL_KEY_ID && smallKey != null) smallKey.SetActive(false);
                    if (itemId == COFFEE_TABLE_KEY_ID && coffeeTable_Key != null) coffeeTable_Key.SetActive(false);

                    // EDIT #3: Show confirmation message
                    if (!string.IsNullOrEmpty(confirmMessage))
                    {
                        DialogueSystemV2.Instance?.StartDialogue(confirmMessage, "Lisa");
                    }
                },
                () => {
                    DialogueSystemV2.Instance?.StartDialogue("I'll leave it for now.", "Lisa");
                }
            }
        );
    }

    public void CheckPuzzleCompletion()
    {
        // Check music box puzzle
        if (!musicBoxPuzzleComplete && SaveSystem.Instance.HasItem(MUSIC_BOX_COMPLETE_ID))
        {
            musicBoxPuzzleComplete = true;
            RoomState state = SaveSystem.Instance.GetRoomState(ROOM_NAME);
            state.solvedPuzzles.Add("musicBoxPuzzle");
            SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
        }

        // Check diary puzzle
        if (!diaryPuzzleComplete && SaveSystem.Instance.HasItem(DIARY_COMPLETE_ID))
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

                    // Mark as revealed
                    state.interactedObjects.Add("coffeeTableKeyRevealed");
                    SaveSystem.Instance.UpdateRoomState(ROOM_NAME, state);
                }
                else
                {
                    // Just show the key without sound/dialogue if already revealed
                    coffeeTable_Key.SetActive(true);
                }
            }
        }
    }

    IEnumerator RevealCoffeeTableKey()
    {
        // Play reveal sound effect
        if (keyRevealSound != null)
        {
            AudioManager.Instance?.PlaySFX(keyRevealSound);
        }

        yield return new WaitForSeconds(0.2f);

        // Show the key
        if (coffeeTable_Key != null)
        {
            coffeeTable_Key.SetActive(true);
        }

        yield return new WaitForSeconds(0.3f);

        // Lisa's reaction
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

        // Start rocking chair
        if (rockingChairAnimator != null)
        {
            rockingChairAnimator.SetBool("isRocking", true);
        }

        AudioManager.Instance?.PlayAmbient(rockingChairSound, loop: true);
    }
}