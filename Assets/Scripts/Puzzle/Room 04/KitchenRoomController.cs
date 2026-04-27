using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenRoomController : MonoBehaviour
{
    public static KitchenRoomController Instance { get; private set; }

    [Header("Puzzle Configuration")]
    public string puzzleId = "kitchen_cookie_puzzle";
    public string roomName = "Room04_KitchenDining";

    [Header("Ingredient GameObjects (To Hide)")]
    // I-drag dito ang mga objects sa Hierarchy para mawala sila pag nakuha na
    public GameObject flourObject;
    public GameObject sugarObject;
    public GameObject vanillaObject;
    public GameObject chocolateObject;
    public GameObject eggObject;
    public GameObject saltObject;

    [Header("Puzzle Flags (Persistent)")]
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
        LoadRoomState();
        SyncIngredientObjects(); // Itago ang mga nakuhang gamit
        CheckFirstVisit();
    }

    private void LoadRoomState()
    {
        // 1. Load from PlayerPrefs para siguradong hindi na umuulit ang puzzle
        bridgePlaced = PlayerPrefs.GetInt(puzzleId + "_bridge", 0) == 1;
        doughMixed = PlayerPrefs.GetInt(puzzleId + "_dough", 0) == 1;
        ovenSetCorrect = PlayerPrefs.GetInt(puzzleId + "_oven", 0) == 1;
        cookiesBakedAndStored = PlayerPrefs.GetInt(puzzleId + "_cookies", 0) == 1;
        emilyIntroDone = PlayerPrefs.GetInt("emily_kitchen_intro", 0) == 1;

        if (SaveSystem.Instance == null) return;

        // 2. Load from SaveSystem for items and visit state
        RoomState state = SaveSystem.Instance.GetRoomState(roomName);
        if (state == null) return;

        floorboardObtained = SaveSystem.Instance.HasItem("floorboard_bridge");
        recipeRead = SaveSystem.Instance.HasItem("recipe_book_kitchen");

        // Sync flags based on Inventory
        hasFlour = SaveSystem.Instance.HasItem("flour");
        hasSugar = SaveSystem.Instance.HasItem("sugar");
        hasVanilla = SaveSystem.Instance.HasItem("vanilla");
        hasChocolate = SaveSystem.Instance.HasItem("chocolate");
        hasEgg = SaveSystem.Instance.HasItem("egg");
        hasSalt = SaveSystem.Instance.HasItem("salt");

        Debug.Log($"[KitchenRoomController] State Loaded. Bridge: {bridgePlaced}, Mixed: {doughMixed}");
    }

    private void SyncIngredientObjects()
    {
        // Kung nakuha na ni Lisa o tapos na ang mix, itago na ang physical objects sa scene
        if (hasFlour || doughMixed) if (flourObject != null) flourObject.SetActive(false);
        if (hasSugar || doughMixed) if (sugarObject != null) sugarObject.SetActive(false);
        if (hasVanilla || doughMixed) if (vanillaObject != null) vanillaObject.SetActive(false);
        if (hasChocolate || doughMixed) if (chocolateObject != null) chocolateObject.SetActive(false);
        if (hasEgg || doughMixed) if (eggObject != null) eggObject.SetActive(false);
        if (hasSalt || doughMixed) if (saltObject != null) saltObject.SetActive(false);
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

    public void OnIngredientCollected(string itemId)
    {
        switch (itemId)
        {
            case "flour": hasFlour = true; if (flourObject != null) flourObject.SetActive(false); break;
            case "sugar": hasSugar = true; if (sugarObject != null) sugarObject.SetActive(false); break;
            case "vanilla": hasVanilla = true; if (vanillaObject != null) vanillaObject.SetActive(false); break;
            case "chocolate": hasChocolate = true; if (chocolateObject != null) chocolateObject.SetActive(false); break;
            case "egg": hasEgg = true; if (eggObject != null) eggObject.SetActive(false); break;
            case "salt": hasSalt = true; if (saltObject != null) saltObject.SetActive(false); break;
        }
    }

    public void OnDoughMixed()
    {
        doughMixed = true;
        PlayerPrefs.SetInt(puzzleId + "_dough", 1);
        PlayerPrefs.Save();
        SyncIngredientObjects(); // Siguradong mawawala ang mga gamit sa counter
    }

    public void OnOvenSetCorrect()
    {
        ovenSetCorrect = true;
        PlayerPrefs.SetInt(puzzleId + "_oven", 1);
        PlayerPrefs.Save();
    }

    public void OnCookiesBakedAndStored()
    {
        cookiesBakedAndStored = true;
        PlayerPrefs.SetInt(puzzleId + "_cookies", 1);
        PlayerPrefs.Save();
    }

    public void OnBridgePlaced()
    {
        bridgePlaced = true;
        PlayerPrefs.SetInt(puzzleId + "_bridge", 1);
        PlayerPrefs.Save();

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkPuzzleSolved(puzzleId + "_bridge");
        }
    }
    public void OnRecipeBookRead()
    {
        recipeRead = true;
        // Optional: I-save sa memory kung gusto mong permanent
        PlayerPrefs.SetInt(puzzleId + "_recipe", 1);
        PlayerPrefs.Save();
    }

    public void OnFloorboardObtained()
    {
        floorboardObtained = true;
        // Optional: I-save sa memory kung gusto mong permanent
        PlayerPrefs.SetInt(puzzleId + "_floorboard", 1);
        PlayerPrefs.Save();
    }

    void Update()
    {
        // Pindutin ang 'R' habang naka-Play para i-reset ang buong kusina
        if (Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey(puzzleId + "_bridge");
            PlayerPrefs.DeleteKey(puzzleId + "_dough");
            PlayerPrefs.DeleteKey(puzzleId + "_oven");
            PlayerPrefs.DeleteKey(puzzleId + "_cookies");
            PlayerPrefs.DeleteKey("emily_kitchen_intro");
            PlayerPrefs.Save();

            Debug.Log("DEBUG: Kitchen States Cleared! Please restart the scene.");
        }
    }

    // --- EMILY INTRO LOGIC ---

    public void StartEmilyKitchenIntro(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint)
    {
        if (emilyIntroDone) return;
        StartCoroutine(EmilyIntroRoutine(player, emilyPrefab, emilySpawnPoint));
    }

    private IEnumerator EmilyIntroRoutine(Transform player, EmilyGhost emilyPrefab, Transform emilySpawnPoint)
    {
        introInProgress = true;
        JoystickPlayerController playerController = player.GetComponent<JoystickPlayerController>();

        EmilyGhost emilyInstance = Instantiate(emilyPrefab, emilySpawnPoint.position, emilySpawnPoint.rotation);
        Animator emilyAnim = emilyInstance.GetComponentInChildren<Animator>();

        if (AudioManager.Instance != null && introJumpscareSFX != null)
        {
            AudioManager.Instance.PlaySFX(introJumpscareSFX);
        }

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

        Vector2[] path = new Vector2[] { new Vector2(2.5f, 1.2f), new Vector2(-2.5f, 1.2f) };
        StartCoroutine(FadeWalkSound(true, 1.0f));
        yield return StartCoroutine(MoveEmilyAlongPath(emilyInstance.transform, emilyAnim, path, 1.8f));
        StartCoroutine(FadeWalkSound(false, 1.0f));

        if (isPlayerHidden)
        {
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("I WILL FIND YOU!", "Emily");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
                DialogueSystemV2.Instance.StartDialogue("I need to keep quiet...", "Lisa");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            }
        }

        if (emilyAgent != null)
        {
            emilyAgent.enabled = true;
            emilyAgent.Warp(emilyInstance.transform.position);
        }

        emilyInstance.enabled = true;
        emilyInstance.SetStateExternal(isPlayerHidden ? EmilyGhost.State.Search : EmilyGhost.State.Hunt);

        emilyIntroDone = true;
        introInProgress = false;
        PlayerPrefs.SetInt("emily_kitchen_intro", 1);
        PlayerPrefs.Save();
    }

    // --- HELPER METHODS ---

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
        if (!fadeIn) walkSource.Stop();
    }

    private IEnumerator PushLisaToPosition(Transform playerTransform, JoystickPlayerController controller, Vector2 targetPos, float duration)
    {
        if (controller != null) controller.enabled = false;
        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        Vector3 startPos = playerTransform.position;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Sin((timer / duration) * Mathf.PI * 0.5f);
            Vector3 newPos = Vector3.Lerp(startPos, new Vector3(targetPos.x, targetPos.y, 0), t);
            if (rb != null) rb.MovePosition(newPos);
            else playerTransform.position = newPos;
            yield return null;
        }
        if (controller != null) controller.enabled = true;
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
                }
                yield return null;
            }
        }
        if (anim != null) anim.SetBool("isWalking", false);
    }

    [ContextMenu("Reset Kitchen Puzzle")]
    public void ResetPuzzle()
    {
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

        if (flourObject != null) flourObject.SetActive(true);
        if (sugarObject != null) sugarObject.SetActive(true);
        if (vanillaObject != null) vanillaObject.SetActive(true);
        if (chocolateObject != null) chocolateObject.SetActive(true);
        if (eggObject != null) eggObject.SetActive(true);
        if (saltObject != null) saltObject.SetActive(true);

        Debug.Log("DEBUG: Kitchen Puzzle Reset! (NOTE: Kung nasa SaveSystem pa rin ang items mo, baka kailangan mong i-clear din ang main save data mo).");
    }
}