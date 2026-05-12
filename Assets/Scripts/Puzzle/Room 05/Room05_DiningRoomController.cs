// Developer: Jhon Jellar Z. Miranda
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.AI;

public class Room05_DiningRoomController : MonoBehaviour
{
    public static Room05_DiningRoomController Instance { get; private set; }

    [Header("Puzzle Configuration")]
    public string correctCode = "332412";
    public GameObject sideboard;
    public GameObject spoonPickup;
    public GameObject bedroomKey;
    [Tooltip("SIGURADUHIN: Ang naka-assign dito ay ang PARENT object ni Emily, hindi lang yung Sprite!")]
    public GameObject emilyEnemy;
    private NavMeshAgent emilyAgent;

    [Header("UI Blocking (Auto-Find)")]
    private GameObject dynamicJoystick;
    private bool isGamePausedForUI = false;

    [Header("Cabinet Puzzle System")]
    public CabinetPuzzleUI cabinetPuzzleLogic;
    public GameObject cabinetUIPanel;
    public Sprite cabinetClosedSprite;
    public Sprite cabinetOpenSprite;

    [Header("Table & Ritual")]
    public GameObject tablePanel;
    public GameObject ghostlyCutlery;

    [Header("Calendar UI")]
    public CalendarViewer calendarViewerUI;

    [Header("Chair Movement Settings")]
    public GameObject childChairObj;
    public GameObject motherChairObj;
    public GameObject fatherChairObj;
    public Vector3 childChairTarget;
    public Vector3 motherChairTarget;
    public Vector3 fatherChairTarget;
    public float chairMoveSpeed = 3f;

    [Header("Hunting System")]
    public bool isEmilyHunting = false;
    public bool puzzleCompleted = false;
    public Transform emilyAngrySpawnPoint;
    public Transform emilyFinalChaseSpawnPoint;
    public float initialChaseSpeed = 3.5f;
    public float finalChaseSpeed = 5.5f;
    
    [Header("Final Chase Trigger")]
    [Tooltip("Trigger GameObject na mag-activate pag tapos na ang puzzle")]
    public GameObject finalChaseTrigger;

    [Header("Jumpscare & Audio Settings")]
    public AudioSource roomAudioSource;
    public AudioClip introJumpscareSFX;
    public AudioClip scriptedWalkSFX;

    [Header("Player Setup")]
    public GameObject playerFlashlight;

    private Rigidbody2D playerRb;
    private Transform playerTransform;

    public float introKnockbackDuration = 0.5f;
    public Vector2 introKnockbackTarget = new Vector2(2.5f, 1.2f);

    public bool isFirstTimeHidingDone { get { return PlayerPrefs.GetInt("R05_FirstHide", 0) == 1; } set { PlayerPrefs.SetInt("R05_FirstHide", value ? 1 : 0); } }

    // --- PERSISTENT FLAGS ---
    public bool isCalendarSeen { get { return PlayerPrefs.GetInt("R05_Calendar", 0) == 1; } set { PlayerPrefs.SetInt("R05_Calendar", value ? 1 : 0); } }
    public bool isCabinetOpen { get { return PlayerPrefs.GetInt("R05_Cabinet", 0) == 1; } set { PlayerPrefs.SetInt("R05_Cabinet", value ? 1 : 0); } }
    public bool hasSpoon { get { return PlayerPrefs.GetInt("R05_HasSpoon", 0) == 1; } set { PlayerPrefs.SetInt("R05_HasSpoon", value ? 1 : 0); } }
    public bool isSpoonPlaced { get { return PlayerPrefs.GetInt("R05_SpoonPlaced", 0) == 1; } set { PlayerPrefs.SetInt("R05_SpoonPlaced", value ? 1 : 0); } }

    // --- CHAIR FLAGS ---
    public bool isChildChairFixed { get { return PlayerPrefs.GetInt("R05_ChildChair", 0) == 1; } set { PlayerPrefs.SetInt("R05_ChildChair", value ? 1 : 0); } }
    public bool isMotherChairFixed { get { return PlayerPrefs.GetInt("R05_MotherChair", 0) == 1; } set { PlayerPrefs.SetInt("R05_MotherChair", value ? 1 : 0); } }
    public bool isFatherChairFixed { get { return PlayerPrefs.GetInt("R05_FatherChair", 0) == 1; } set { PlayerPrefs.SetInt("R05_FatherChair", value ? 1 : 0); } }

    public int chairsFixed { get { return (isChildChairFixed ? 1 : 0) + (isMotherChairFixed ? 1 : 0) + (isFatherChairFixed ? 1 : 0); } }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (emilyEnemy != null) emilyAgent = emilyEnemy.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (cabinetUIPanel != null) cabinetUIPanel.SetActive(false);
        if (tablePanel != null) tablePanel.SetActive(false);
        if (emilyEnemy != null) emilyEnemy.SetActive(false);
        
        // Disable final chase trigger initially (will activate after puzzle complete)
        if (finalChaseTrigger != null) finalChaseTrigger.SetActive(false);

        dynamicJoystick = GameObject.Find("Joystick");

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerRb = playerObj.GetComponent<Rigidbody2D>();
            playerTransform = playerObj.transform;

            if (playerFlashlight == null)
            {
                Transform lightSys = playerTransform.Find("FlashlightSystem");
                if (lightSys != null) playerFlashlight = lightSys.gameObject;
            }
        }

        SyncRoomState();
    }

    void Update()
    {
        if (isGamePausedForUI) return;

        if (isEmilyHunting && emilyEnemy != null && emilyEnemy.activeInHierarchy)
        {
            if (emilyAgent != null && playerTransform != null && emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.SetDestination(playerTransform.position);
            }
        }
    }

    private void SyncRoomState()
    {
        if (spoonPickup != null) spoonPickup.SetActive(isCabinetOpen && !hasSpoon && !isSpoonPlaced);
        if (sideboard != null)
        {
            SpriteRenderer sr = sideboard.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = isCabinetOpen ? cabinetOpenSprite : cabinetClosedSprite;
        }

        if (isChildChairFixed && childChairObj != null) childChairObj.transform.position = childChairTarget;
        if (isMotherChairFixed && motherChairObj != null) motherChairObj.transform.position = motherChairTarget;
        if (isFatherChairFixed && fatherChairObj != null) fatherChairObj.transform.position = fatherChairTarget;

        if (ghostlyCutlery != null) ghostlyCutlery.SetActive(chairsFixed >= 3);
        if (bedroomKey != null) bedroomKey.SetActive(false);
    }

    // ==========================================
    // --- SAFE PAUSE LOGIC ---
    // ==========================================
    public void PauseGameForUI()
    {
        isGamePausedForUI = true;

        if (dynamicJoystick != null) dynamicJoystick.SetActive(false);

        if (playerTransform != null)
        {
            MonoBehaviour playerController = playerTransform.GetComponent("JoystickPlayerController") as MonoBehaviour;
            if (playerController != null) playerController.enabled = false;

            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.angularVelocity = 0f;
            }

            Animator anim = playerTransform.GetComponent<Animator>();
            if (anim != null) anim.SetFloat("Speed", 0f);
        }

        if (emilyEnemy != null && emilyEnemy.activeInHierarchy)
        {
            if (emilyAgent != null && emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.isStopped = true;
                emilyAgent.velocity = Vector3.zero;
            }

            EmilyGhost emilyScript = emilyEnemy.GetComponent<EmilyGhost>();
            if (emilyScript != null) emilyScript.isPaused = true;
        }
    }

    public void ResumeGameFromUI()
    {
        isGamePausedForUI = false;

        if (dynamicJoystick != null) dynamicJoystick.SetActive(true);

        if (playerTransform != null)
        {
            MonoBehaviour playerController = playerTransform.GetComponent("JoystickPlayerController") as MonoBehaviour;
            if (playerController != null) playerController.enabled = true;
        }

        if (emilyEnemy != null && emilyEnemy.activeInHierarchy)
        {
            if (emilyAgent != null && emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.isStopped = false;
            }

            EmilyGhost emilyScript = emilyEnemy.GetComponent<EmilyGhost>();
            if (emilyScript != null) emilyScript.isPaused = false;
        }
    }

    // ==========================================
    // --- PUZZLE INTERACTION METHODS ---
    // ==========================================
    public void OnCalendarInteract()
    {
        isCalendarSeen = true; PlayerPrefs.Save();
        if (calendarViewerUI != null) calendarViewerUI.OpenCalendar();
        PauseGameForUI();
        TryShowDialogue(EnhancedGameDialogues.R05_CALENDAR);
    }

    public void CloseCalendarUI()
    {
        if (calendarViewerUI != null) calendarViewerUI.gameObject.SetActive(false);
        ResumeGameFromUI();
        
        // Don't start chase here - let the trigger zone handle it
        // Just mark calendar as seen so trigger can activate
    }

    public IEnumerator EmilyGetsAngrySequence()
    {
        // INTRO DIALOGUE FIRST (before knockback)
        TryShowDialogue(EnhancedGameDialogues.R05_ANGRY_1);
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // NOW the jumpscare and knockback
        if (roomAudioSource != null && introJumpscareSFX != null) 
        {
            roomAudioSource.PlayOneShot(introJumpscareSFX);
        }
        
        if (playerRb != null) 
        {
            StartCoroutine(ApplyKnockbackRoutine());
        }

        TryShowDialogue(EnhancedGameDialogues.R05_ANGRY_2);
        yield return new WaitForSeconds(introKnockbackDuration);

        // Start hunting
        isEmilyHunting = true;
        if (emilyEnemy != null)
        {
            emilyEnemy.SetActive(true);
            if (emilyAgent != null)
            {
                emilyAgent.enabled = true;
                emilyAgent.speed = initialChaseSpeed;
                if (emilyAngrySpawnPoint != null) 
                {
                    emilyAgent.Warp(emilyAngrySpawnPoint.position);
                }
            }
            if (roomAudioSource != null && scriptedWalkSFX != null)
            {
                roomAudioSource.clip = scriptedWalkSFX; 
                roomAudioSource.loop = true; 
                roomAudioSource.Play();
            }
        }
    }

    IEnumerator ApplyKnockbackRoutine()
    {
        float timer = 0f;
        while (timer < introKnockbackDuration)
        {
            playerRb.linearVelocity = new Vector2(introKnockbackTarget.x, introKnockbackTarget.y);
            timer += Time.deltaTime;
            yield return null;
        }
        playerRb.linearVelocity = Vector2.zero;
    }

    public void OnCutleryInteract()
    {
        if (chairsFixed < 3) { TryShowDialogue(EnhancedGameDialogues.R05_TABLE_EMPTY); return; }

        if (!isSpoonPlaced)
        {
            bool playerHasSpoon = hasSpoon || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("spoon"));
            if (playerHasSpoon)
            {
                if (tablePanel != null)
                {
                    tablePanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    tablePanel.SetActive(true);
                    PauseGameForUI();
                }
            }
            else { TryShowDialogue(EnhancedGameDialogues.R05_TABLE_MISSING); }
        }
        else { TryShowDialogue("The table is already set."); }
    }

    public void CloseTableUI()
    {
        if (tablePanel != null) tablePanel.SetActive(false);
        ResumeGameFromUI();
    }

    public void OnPlaceSpoonConfirmed()
    {
        isSpoonPlaced = true; PlayerPrefs.Save();
        if (tablePanel != null) tablePanel.SetActive(false);

        ResumeGameFromUI();
        TryShowDialogue(EnhancedGameDialogues.R05_TABLE_COMPLETE);
    }

    public void OnTableInteract()
    {
        if (isSpoonPlaced && isEmilyHunting)
        {
            if (!isFirstTimeHidingDone)
            {
                TryShowDialogue(EnhancedGameDialogues.R05_HIDING_1);
                isFirstTimeHidingDone = true; PlayerPrefs.Save();
            }
            StartCoroutine(EmilyDisappearsSequence());
        }
        else if (isSpoonPlaced && !isEmilyHunting) { TryShowDialogue("There's space to hide under here, but it's safe for now."); }
        else { TryShowDialogue("It's a heavy dining table."); }
    }

    IEnumerator EmilyDisappearsSequence()
    {
        if (playerRb != null) playerRb.linearVelocity = Vector2.zero;
        MonoBehaviour playerController = playerTransform.GetComponent("JoystickPlayerController") as MonoBehaviour;
        if (playerController != null) playerController.enabled = false;

        if (playerFlashlight != null) playerFlashlight.SetActive(false);

        SpriteRenderer[] allSprites = playerTransform.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in allSprites) sr.enabled = false;
        Collider2D[] allColliders = playerTransform.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in allColliders) col.enabled = false;

        yield return new WaitForSeconds(3f);

        if (emilyEnemy != null)
        {
            if (emilyAgent != null) emilyAgent.enabled = false;
            emilyEnemy.transform.position = new Vector3(9999f, 9999f, 0f);
            emilyEnemy.SetActive(false);
        }

        if (roomAudioSource != null) roomAudioSource.Stop();

        isEmilyHunting = false;
        puzzleCompleted = true;

        foreach (SpriteRenderer sr in allSprites) sr.enabled = true;
        foreach (Collider2D col in allColliders) col.enabled = true;
        if (playerController != null) playerController.enabled = true;
        if (playerFlashlight != null) playerFlashlight.SetActive(true);

        if (InventoryManager.Instance != null && !InventoryManager.Instance.HasItem("bedroom_key"))
        {
            InventoryManager.Instance.AddItem("bedroom_key");
        }
        
        // ACTIVATE FINAL CHASE TRIGGER (pag tapos na ang puzzle)
        if (finalChaseTrigger != null)
        {
            finalChaseTrigger.SetActive(true);
            Debug.Log("[Room05] Final Chase Trigger activated - puzzle complete!");
        }

        TryShowDialogue(EnhancedGameDialogues.R05_EMILY_GONE_1);
    }

    public void OnTriggerExitRoom()
    {
        if (puzzleCompleted && !isEmilyHunting)
        {
            StartCoroutine(FinalChaseSequence());
        }
    }

    IEnumerator FinalChaseSequence()
    {
        // NO DIALOGUE - Pure hunt!
        // Just quick jumpscare sound
        if (roomAudioSource != null && introJumpscareSFX != null)
        {
            roomAudioSource.PlayOneShot(introJumpscareSFX);
        }
        
        yield return new WaitForSeconds(0.2f); // Very short delay
        
        // Spawn Emily FAST
        if (emilyEnemy != null)
        {
            Transform spawnPt = emilyFinalChaseSpawnPoint != null ? emilyFinalChaseSpawnPoint : emilyAngrySpawnPoint;
            if (spawnPt != null)
            {
                emilyEnemy.transform.position = spawnPt.position;
            }

            emilyEnemy.SetActive(true);

            if (emilyAgent != null)
            {
                emilyAgent.enabled = true;
                if (spawnPt != null) emilyAgent.Warp(spawnPt.position);
                emilyAgent.speed = finalChaseSpeed; // Faster speed!
            }

            if (roomAudioSource != null && scriptedWalkSFX != null) 
            {
                roomAudioSource.clip = scriptedWalkSFX;
                roomAudioSource.loop = true;
                roomAudioSource.Play();
            }
        }

        isEmilyHunting = true;
        
        // Optional: Quick warning dialogue (very short)
        TryShowDialogue(EnhancedGameDialogues.R05_FINAL_CHASE_2);
    }

    public void OnCabinetInteract()
    {
        if (isCabinetOpen) { TryShowDialogue(EnhancedGameDialogues.R05_CABINET_OPEN); return; }
        if (!isCalendarSeen) TryShowDialogue(EnhancedGameDialogues.R05_CABINET_LOCKED);
        else StartCoroutine(ShowCabinetUISequence_Cabinet());
    }

    IEnumerator ShowCabinetUISequence_Cabinet()
    {
        yield return new WaitForSeconds(0.5f);
        if (cabinetUIPanel != null)
        {
            cabinetUIPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            cabinetUIPanel.SetActive(true);
            PauseGameForUI();
        }
    }

    public void CloseCabinetUI()
    {
        if (cabinetUIPanel != null) cabinetUIPanel.SetActive(false);
        ResumeGameFromUI();
    }

    public void OnEnterPressed()
    {
        if (cabinetPuzzleLogic.GetEnteredCode() == correctCode)
        {
            isCabinetOpen = true; PlayerPrefs.Save();

            ResumeGameFromUI();
            StartCoroutine(UnlockSequence());
        }
        else
        {
            CloseCabinetUI();
            TryShowDialogue(EnhancedGameDialogues.R05_CABINET_WRONG);
        }
    }

    IEnumerator UnlockSequence()
    {
        cabinetPuzzleLogic.ShowUnlockVisual();
        yield return new WaitForSeconds(1.5f);
        SyncRoomState();

        if (cabinetUIPanel != null) cabinetUIPanel.SetActive(false);
        TryShowDialogue(EnhancedGameDialogues.R05_CABINET_OPEN);
    }

    public void OnSpoonInteract()
    {
        hasSpoon = true; PlayerPrefs.Save();
        if (spoonPickup != null) spoonPickup.SetActive(false);
        
        // Add to inventory with notification
        if (InventoryManager.Instance != null) 
        {
            InventoryManager.Instance.AddItemWithNotification("spoon");
        }
        
        TryShowDialogue(EnhancedGameDialogues.R05_SPOON);
    }

    public void FixChair(string type)
    {
        bool playerHasSpoon = hasSpoon || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("spoon"));
        if (!playerHasSpoon) { TryShowDialogue(EnhancedGameDialogues.R05_CHAIR_LOCKED); return; }

        GameObject targetObj = null; Vector3 targetPos = Vector3.zero;

        if (type == "Child") { if (isChildChairFixed) return; targetObj = childChairObj; targetPos = childChairTarget; isChildChairFixed = true; }
        else if (type == "Mother") { if (isMotherChairFixed) return; targetObj = motherChairObj; targetPos = motherChairTarget; isMotherChairFixed = true; }
        else if (type == "Father") { if (isFatherChairFixed) return; targetObj = fatherChairObj; targetPos = fatherChairTarget; isFatherChairFixed = true; }

        PlayerPrefs.Save();
        if (targetObj != null) { TryShowDialogue(EnhancedGameDialogues.R05_CHAIR_MOVED); StartCoroutine(MoveChairRoutine(targetObj, targetPos)); }
    }

    IEnumerator MoveChairRoutine(GameObject chair, Vector3 target)
    {
        while (Vector2.Distance(chair.transform.position, target) > 0.01f)
        {
            chair.transform.position = Vector3.MoveTowards(chair.transform.position, target, chairMoveSpeed * Time.deltaTime);
            yield return null;
        }
        chair.transform.position = target;
        if (chairsFixed >= 3) SyncRoomState();
    }

    public void OnChairInteract() => FixChair("Child");
    public void OnMotherChairInteract() => FixChair("Mother");
    public void OnFatherChairInteract() => FixChair("Father");

    public void OnKeyInteract()
    {
        if (bedroomKey != null) bedroomKey.SetActive(false);
        
        // Add to inventory with notification
        if (InventoryManager.Instance != null) 
        {
            InventoryManager.Instance.AddItemWithNotification("bedroom_key");
        }
        
        TryShowDialogue("I got the key. Time to get out of here.");
    }

    public void TryShowDialogue(string text)
    {
        if (DialogueSystemV2.Instance != null) DialogueSystemV2.Instance.StartDialogue(text, "Lisa");
    }

    [ContextMenu("Reset Room 05 Puzzle")]
    public void ResetPuzzle()
    {
        PlayerPrefs.DeleteKey("R05_Calendar"); PlayerPrefs.DeleteKey("R05_Cabinet");
        PlayerPrefs.DeleteKey("R05_HasSpoon"); PlayerPrefs.DeleteKey("R05_SpoonPlaced");
        PlayerPrefs.DeleteKey("R05_FirstHide"); PlayerPrefs.DeleteKey("R05_Chairs");
        PlayerPrefs.DeleteKey("R05_ChildChair"); PlayerPrefs.DeleteKey("R05_MotherChair");
        PlayerPrefs.DeleteKey("R05_FatherChair"); PlayerPrefs.Save();
        Debug.Log("Room 05 Puzzle Reset!");
    }
}