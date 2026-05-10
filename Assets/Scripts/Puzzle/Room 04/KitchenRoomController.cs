using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public bool isPlayerHidden = false;
    public bool introInProgress = false;

    [Header("Intro Settings")]
    public float introKnockbackDuration = 0.5f;
    public Vector2 introKnockbackTarget = new Vector2(2.5f, 1.2f);

    [Header("Audio")]
    public AudioClip introJumpscareSFX;
    public AudioClip scriptedWalkSFX;

    private AudioSource walkSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        walkSource = gameObject.AddComponent<AudioSource>();
        walkSource.loop = true;
        walkSource.playOnAwake = false;
        walkSource.spatialBlend = 0f;
        walkSource.volume = 0f;
    }

    private void Start()
    {
        // Stop any lingering audio from previous scene
        if (walkSource != null && walkSource.isPlaying)
        {
            walkSource.Stop();
        }

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

        // FIX: Restore ingredient flags based on Inventory
        hasFlour = SaveSystem.Instance.HasItem("flour");
        hasSugar = SaveSystem.Instance.HasItem("sugar");
        hasVanilla = SaveSystem.Instance.HasItem("vanilla");
        hasChocolate = SaveSystem.Instance.HasItem("chocolate");
        hasEgg = SaveSystem.Instance.HasItem("egg");
        hasSalt = SaveSystem.Instance.HasItem("salt");

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

    public void OnRecipeBookRead()
    {
        recipeRead = true;
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
        // No logic needed here, MixingBowlInteractable checks flags directly
    }

    public void OnDoughMixed() { doughMixed = true; }
    public void OnOvenSetCorrect() { ovenSetCorrect = true; }
    public void OnCookiesBakedAndStored() { cookiesBakedAndStored = true; }
    public void OnFloorboardObtained() { floorboardObtained = true; }

    public void OnBridgePlaced()
    {
        bridgePlaced = true;
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkPuzzleSolved(puzzleId);
            SaveSystem.Instance.MarkPuzzleSolved(puzzleId + "_bridge");
        }
    }

    public void StartEmilyKitchenIntro(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint, EmilyGhost existingEmily = null)
    {
        if (emilyIntroDone) return;
        StartCoroutine(EmilyIntroRoutine(player, emilyPrefab, emilySpawnPoint, existingEmily));
    }

    private IEnumerator EmilyIntroRoutine(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint, EmilyGhost existingEmily = null)
    {
        Debug.Log("[KitchenRoomController] Starting Emily Intro Sequence...");
        introInProgress = true;

        JoystickPlayerController playerController = player.GetComponent<JoystickPlayerController>();

        // CRITICAL FIX: Use existing Emily if provided, otherwise spawn new one
        EmilyGhost emilyInstance;
        if (existingEmily != null)
        {
            Debug.Log("[KitchenController] Using existing Emily instance");
            emilyInstance = existingEmily;
            
            // CRITICAL: Properly reset existing Emily
            // Disable AI temporarily
            emilyInstance.enabled = false;
            
            // Get and disable NavMeshAgent
            UnityEngine.AI.NavMeshAgent existingAgent = emilyInstance.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (existingAgent != null)
            {
                existingAgent.enabled = false;
            }
            
            // Move to spawn point
            emilyInstance.transform.position = emilySpawnPoint.position;
            emilyInstance.transform.rotation = emilySpawnPoint.rotation;
            
            // Re-enable agent and warp to position
            if (existingAgent != null)
            {
                existingAgent.enabled = true;
                existingAgent.Warp(emilySpawnPoint.position);
            }
            
            Debug.Log($"[KitchenController] Existing Emily reset and moved to: {emilySpawnPoint.position}");
        }
        else
        {
            // Verify prefab is not null before spawning
            if (emilyPrefab == null)
            {
                Debug.LogError("[KitchenController] Emily Prefab is NULL! Cannot spawn Emily. Aborting intro.");
                introInProgress = false;
                yield break;
            }
            
            Debug.Log("[KitchenController] Spawning new Emily instance from prefab");
            emilyInstance = Instantiate(emilyPrefab, emilySpawnPoint.position, emilySpawnPoint.rotation);
            
            if (emilyInstance == null)
            {
                Debug.LogError("[KitchenController] Failed to instantiate Emily! Aborting intro.");
                introInProgress = false;
                yield break;
            }
        }
        
        Animator emilyAnim = emilyInstance.GetComponentInChildren<Animator>();

        if (AudioManager.Instance != null && introJumpscareSFX != null)
        {
            AudioManager.Instance.PlaySFX(introJumpscareSFX);
        }

        // Disable Emily for intro sequence (applies to both new and existing)
        emilyInstance.enabled = false;
        UnityEngine.AI.NavMeshAgent emilyAgent = emilyInstance.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (emilyAgent != null) emilyAgent.enabled = false;

        yield return StartCoroutine(PushLisaToPosition(player, playerController, introKnockbackTarget, introKnockbackDuration));

        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("GET OUT OF HERE!", "Emily");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            yield return new WaitForSeconds(0.3f);
            DialogueSystemV2.Instance.StartDialogue("I need to hide under the island!", "Lisa");
            while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        Vector2[] path = new Vector2[] { new Vector2(2.5f, 1.2f), new Vector2(-2.5f, 1.2f) };

        StartCoroutine(FadeWalkSound(true, 1.0f));
        yield return StartCoroutine(MoveEmilyAlongPath(emilyInstance.transform, emilyAnim, path, 1.8f));
        StartCoroutine(FadeWalkSound(false, 1.0f));

        yield return new WaitForSeconds(0.8f);

        if (isPlayerHidden)
        {
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("I WILL FIND YOU!", "Emily");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
                yield return new WaitForSeconds(0.1f);
                DialogueSystemV2.Instance.StartDialogue("I need to keep quiet...", "Lisa");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            }
        }

        // CRITICAL FIX: Properly enable Emily AI with frame delays
        if (emilyAgent != null)
        {
            emilyAgent.enabled = true;
            emilyAgent.Warp(emilyInstance.transform.position);
            
            // Wait for agent to be fully ready on NavMesh
            yield return new WaitForEndOfFrame();
        }

        // NOW enable Emily AI component
        emilyInstance.enabled = true;
        
        // CRITICAL: Wait for Emily's OnEnable to complete initialization
        yield return new WaitForEndOfFrame();

        // NOW set the state - Emily is fully ready
        EmilyGhost.State targetState = isPlayerHidden ? EmilyGhost.State.Search : EmilyGhost.State.Hunt;
        emilyInstance.SetStateExternal(targetState);
        
        Debug.Log($"[KitchenController] Emily AI fully enabled. State: {targetState}");

        emilyIntroDone = true;
        introInProgress = false;

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkPuzzleSolved("emily_kitchen_intro");
            SaveSystem.Instance.SaveGame(0);
        }
    }

    private IEnumerator FadeWalkSound(bool fadeIn, float duration)
    {
        if (scriptedWalkSFX == null || walkSource == null) yield break;

        walkSource.clip = scriptedWalkSFX;
        if (fadeIn && !walkSource.isPlaying) walkSource.Play();

        float start = walkSource.volume;
        float target = fadeIn ? 1.0f : 0.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            walkSource.volume = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }
        walkSource.volume = target;
        if (!fadeIn) walkSource.Stop();
    }

    private IEnumerator PushLisaToPosition(Transform playerTransform, JoystickPlayerController controller, Vector2 targetPos, float duration)
    {
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
                float t = Mathf.Sin((timer / duration) * Mathf.PI * 0.5f);
                Vector3 newPos = Vector3.Lerp(startPos, new Vector3(targetPos.x, targetPos.y, 0), t);
                if (rb != null) rb.MovePosition(newPos);
                else playerTransform.position = newPos;
                yield return null;
            }
        }
        finally { if (controller != null) controller.enabled = true; }
    }

    private IEnumerator MoveEmilyAlongPath(Transform emilyTransform, Animator anim, Vector2[] waypoints, float speed)
    {
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
        if (anim != null) anim.SetBool("isWalking", false);
    }

    [ContextMenu("Reset Kitchen Puzzle")]
    public void ResetPuzzle()
    {
        // Stop any playing audio first
        if (walkSource != null && walkSource.isPlaying)
        {
            walkSource.Stop();
        }

        PlayerPrefs.DeleteKey(puzzleId + "_bridge");
        PlayerPrefs.DeleteKey(puzzleId + "_dough");
        PlayerPrefs.DeleteKey(puzzleId + "_oven");
        PlayerPrefs.DeleteKey(puzzleId + "_cookies");
        PlayerPrefs.DeleteKey(puzzleId + "_recipe");
        PlayerPrefs.DeleteKey(puzzleId + "_floorboard");
        PlayerPrefs.DeleteKey("emily_kitchen_intro");
        PlayerPrefs.Save();

        recipeRead = false;
        hasFlour = false;
        hasSugar = false;
        hasVanilla = false;
        hasChocolate = false;
        hasEgg = false;
        hasSalt = false;
        doughMixed = false;
        ovenSetCorrect = false;
        cookiesBakedAndStored = false;
        floorboardObtained = false;
        bridgePlaced = false;
        emilyIntroDone = false;
        isPlayerHidden = false;
        introInProgress = false;

        Debug.Log("DEBUG: Kitchen Puzzle Reset!");
    }
}
