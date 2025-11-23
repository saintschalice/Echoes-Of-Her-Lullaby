using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenRoomController : MonoBehaviour
{
    public static KitchenRoomController Instance { get; private set; }

    [Header("Puzzle Configuration")]
    public string puzzleId = "kitchen_cookie_puzzle";
    public string roomName = "Room04_KitchenDining";

    [Header("Puzzle Flags (Read Only)")]
    public bool recipeRead;
    public bool hasFlour;
    public bool hasSugar;
    public bool hasVanilla;
    public bool hasChocolate;
    public bool hasEgg;
    public bool hasSalt;
    public bool doughMixed;
    public bool ovenSetCorrect;
    public bool cookiesBakedAndStored;
    public bool floorboardObtained;
    public bool bridgePlaced;
    public bool emilyIntroDone;

    [Header("State Flags")]
    // The Island interaction script updates this when Lisa hides/unhides
    public bool isPlayerHidden = false;
    // NEW: Allows Island interaction during the intro sequence
    public bool introInProgress = false;

    [Header("Intro Settings")]
    public float introKnockbackDuration = 0.5f;
    public Vector2 introKnockbackTarget = new Vector2(2.5f, 1.2f);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        LoadRoomState();
        CheckFirstVisit();
    }

    private void LoadRoomState()
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogWarning("[KitchenRoomController] SaveSystem missing. Using default state.");
            return;
        }

        RoomState state = SaveSystem.Instance.GetRoomState(roomName);
        if (state == null) return;

        bridgePlaced = state.solvedPuzzles.Contains(puzzleId + "_bridge");
        emilyIntroDone = state.solvedPuzzles.Contains("emily_kitchen_intro");
        floorboardObtained = state.collectedItems.Contains("floorboard_bridge");
        recipeRead = SaveSystem.Instance.HasItem("recipe_book_kitchen");

        Debug.Log($"[KitchenRoomController] State Loaded. Intro Done: {emilyIntroDone}");
    }

    private void CheckFirstVisit()
    {
        if (SaveSystem.Instance == null) return;

        RoomState state = SaveSystem.Instance.GetRoomState(roomName);
        if (!state.hasBeenVisited)
        {
            StartCoroutine(PlayEntryDialogue());
            state.hasBeenVisited = true;
            SaveSystem.Instance.UpdateRoomState(roomName, state);
            SaveSystem.Instance.SaveGame(0);
        }
    }

    private IEnumerator PlayEntryDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("This kitchen smells like death masked by old vanilla. Something terrible happened here.", "Lisa");
        }
    }

    // ========================================================================
    // PUZZLE INTERFACE
    // ========================================================================

    public void OnRecipeBookRead()
    {
        recipeRead = true;
        Debug.Log("[KitchenRoomController] Recipe read.");
    }

    public void OnIngredientCollected(string itemId)
    {
        switch (itemId)
        {
            case "flour": hasFlour = true; break;
            case "sugar": hasSugar = true; break;
            case "vanilla": hasVanilla = true; break;
            case "chocolate": hasChocolate = true; break;
            case "egg": hasEgg = true; break;
            case "salt": hasSalt = true; break;
        }
        CheckDoughIngredients();
    }

    private void CheckDoughIngredients()
    {
        // Internal check logic
    }

    public void OnDoughMixed()
    {
        doughMixed = true;
        Debug.Log("[KitchenRoomController] Dough mixed.");
    }

    public void OnOvenSetCorrect()
    {
        ovenSetCorrect = true;
        Debug.Log("[KitchenRoomController] Oven set correctly.");
    }

    public void OnCookiesBakedAndStored()
    {
        cookiesBakedAndStored = true;
        Debug.Log("[KitchenRoomController] Cookies baked and stored.");
    }

    public void OnFloorboardObtained()
    {
        floorboardObtained = true;
        Debug.Log("[KitchenRoomController] Floorboard obtained.");
    }

    public void OnBridgePlaced()
    {
        bridgePlaced = true;
        Debug.Log("[KitchenRoomController] Bridge placed.");

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkPuzzleSolved(puzzleId);
            SaveSystem.Instance.MarkPuzzleSolved(puzzleId + "_bridge");
        }
    }

    // ========================================================================
    // EMILY INTRO SEQUENCE
    // ========================================================================

    public void StartEmilyKitchenIntro(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint)
    {
        if (emilyIntroDone)
        {
            Debug.Log("[KitchenRoomController] Emily intro already finished, skipping.");
            return;
        }

        if (player == null || emilyPrefab == null || emilySpawnPoint == null)
        {
            Debug.LogError("[KitchenRoomController] Missing references for Emily Intro!");
            return;
        }

        StartCoroutine(EmilyIntroRoutine(player, emilyPrefab, emilySpawnPoint));
    }

    private IEnumerator EmilyIntroRoutine(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint)
    {
        Debug.Log("[KitchenRoomController] Starting Emily Intro Sequence...");
        introInProgress = true; // Enable flag to allow Island hiding

        JoystickPlayerController playerController = player.GetComponent<JoystickPlayerController>();

        // 1. Spawn Emily (AI Disabled)
        EmilyGhost emilyInstance = Instantiate(emilyPrefab, emilySpawnPoint.position, emilySpawnPoint.rotation);
        Animator emilyAnim = emilyInstance.GetComponentInChildren<Animator>();

        emilyInstance.enabled = false;
        UnityEngine.AI.NavMeshAgent emilyAgent = emilyInstance.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (emilyAgent != null) emilyAgent.enabled = false;

        // 2. Knockback / Shock (Disables Input)
        yield return StartCoroutine(PushLisaToPosition(player, playerController, introKnockbackTarget, introKnockbackDuration));

        // 3. Dialogue - Part 1
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("GET OUT OF HERE!", "Emily");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;

            yield return new WaitForSeconds(0.3f);

            DialogueSystemV2.Instance.StartDialogue("I need to hide!", "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // 4. Player Free To Move / Emily Scripted Walk
        Vector2[] path = new Vector2[] {
            new Vector2(2.5f, 1.2f),
            new Vector2(-2.5f, 1.2f)
        };

        // Give player time to move/hide while Emily walks slowly
        yield return StartCoroutine(MoveEmilyAlongPath(emilyInstance.transform, emilyAnim, path, 1.8f));

        // 5. Tension Pause & Hiding Check
        yield return new WaitForSeconds(0.8f);

        // Logic Branch based on hiding
        if (isPlayerHidden)
        {
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("I WILL FIND YOU!", "Emily");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;

                // BUG 3 FIX: Small delay to ensure speaker context switches cleanly
                yield return new WaitForSeconds(0.1f);

                DialogueSystemV2.Instance.StartDialogue("I need to keep quiet...", "Lisa");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            }
        }
        else
        {
            // BUG 2 FIX: Removed "THERE YOU ARE!" dialogue.
            // If player isn't hidden, we just silently proceed to the Hunt.
            Debug.Log("[KitchenRoomController] Player not hidden, skipping dialogue and going to Hunt.");
        }

        // 6. AI Handover
        Debug.Log("[KitchenRoomController] Handing over to Emily AI...");

        // Re-enable Agent & Warp to sync internal position
        if (emilyAgent != null)
        {
            emilyAgent.enabled = true;
            emilyAgent.Warp(emilyInstance.transform.position);
        }

        emilyInstance.enabled = true;

        // Set Initial State
        if (isPlayerHidden)
        {
            emilyInstance.SetStateExternal(EmilyGhost.State.Search);
        }
        else
        {
            emilyInstance.SetStateExternal(EmilyGhost.State.Hunt);
        }

        // 7. Save State
        emilyIntroDone = true;
        introInProgress = false; // Reset flag

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkPuzzleSolved("emily_kitchen_intro");
            SaveSystem.Instance.SaveGame(0);
        }

        Debug.Log("[KitchenRoomController] Intro Sequence Complete.");
    }

    // --- Helpers ---

    private IEnumerator PushLisaToPosition(Transform playerTransform, JoystickPlayerController controller, Vector2 targetPos, float duration)
    {
        Debug.Log("[KitchenRoomController] Pushing Lisa...");

        if (controller != null) controller.enabled = false;

        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Vector3 startPos = playerTransform.position;
        float timer = 0f;

        try
        {
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                Vector3 newPos = Vector3.Lerp(startPos, new Vector3(targetPos.x, targetPos.y, 0), t);

                if (rb != null) rb.MovePosition(newPos);
                else playerTransform.position = newPos;

                yield return null;
            }
        }
        finally
        {
            if (controller != null) controller.enabled = true;
        }
    }

    private IEnumerator MoveEmilyAlongPath(Transform emilyTransform, Animator anim, Vector2[] waypoints, float speed)
    {
        Debug.Log("[KitchenRoomController] Emily walking scripted path...");

        foreach (Vector2 target in waypoints)
        {
            while (Vector2.Distance(emilyTransform.position, target) > 0.1f)
            {
                Vector3 dir = ((Vector3)target - emilyTransform.position).normalized;
                emilyTransform.position += dir * speed * Time.deltaTime;

                if (anim != null)
                {
                    anim.SetBool("isWalking", true);
                    anim.SetFloat("InputX", dir.x);
                    anim.SetFloat("InputY", dir.y);
                    anim.SetFloat("MoveX", dir.x);
                    anim.SetFloat("MoveY", dir.y);
                }

                yield return null;
            }
        }

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }
    }
}