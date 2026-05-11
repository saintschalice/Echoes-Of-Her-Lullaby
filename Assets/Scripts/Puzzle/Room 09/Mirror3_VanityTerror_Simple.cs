using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 3: Vanity Terror Puzzle - SIMPLE VERSION
/// Self-contained drag and drop with swap
/// NO external DraggableItem needed!
/// </summary>
public class Mirror3_VanityTerror_Simple : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public Transform slotsContainer; // Parent of all slots
    
    [Header("Puzzle Settings")]
    public float timeLimit = 90f;
    
    [Header("Audio")]
    public AudioClip paperRustleSound;
    public AudioClip successSound;
    public AudioClip emilyScreamSound;
    
    [Header("Success/Failure")]
    public GameObject successEffect;
    public GameObject emilyJumpscarePanel;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    
    private List<DiarySlot> slots = new List<DiarySlot>();
    private DiaryPage currentlyDragging = null;
    
    // Correct sequence
    private string[] correctSequence = { 
        "DiaryPage_1", "DiaryPage_2", "DiaryPage_3", "DiaryPage_4", 
        "DiaryPage_5", "DiaryPage_6", "DiaryPage_7", "DiaryPage_8" 
    };

    void Start()
    {
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        SetupSlots();
    }

    void SetupSlots()
    {
        // Find all slots and pages
        foreach (Transform slotTransform in slotsContainer)
        {
            if (slotTransform.name.Contains("Slot"))
            {
                DiarySlot slot = new DiarySlot();
                slot.slotTransform = slotTransform;
                slot.slotRect = slotTransform.GetComponent<RectTransform>();
                
                // Find page in this slot
                foreach (Transform child in slotTransform)
                {
                    if (child.name.Contains("DiaryPage"))
                    {
                        DiaryPage page = child.gameObject.AddComponent<DiaryPage>();
                        page.pageId = child.name;
                        page.pageRect = child.GetComponent<RectTransform>();
                        page.pageImage = child.GetComponent<Image>();
                        page.parentSlot = slot;
                        page.puzzle = this;
                        
                        slot.currentPage = page;
                        break;
                    }
                }
                
                slots.Add(slot);
            }
        }
        
        Debug.Log($"[Mirror3Simple] Setup complete: {slots.Count} slots found");
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive || isPuzzleSolved) return;
        
        Debug.Log("[Mirror3Simple] Starting puzzle");
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        ShufflePages();
        PauseGame();
        StartCoroutine(ShowStartDialogue());
    }

    void ShufflePages()
    {
        Debug.Log("[Mirror3Simple] Shuffling pages...");
        
        // Get all pages
        List<DiaryPage> pages = new List<DiaryPage>();
        foreach (var slot in slots)
        {
            if (slot.currentPage != null)
            {
                pages.Add(slot.currentPage);
            }
        }
        
        // Shuffle
        for (int i = 0; i < pages.Count; i++)
        {
            int randomIndex = Random.Range(i, pages.Count);
            DiaryPage temp = pages[i];
            pages[i] = pages[randomIndex];
            pages[randomIndex] = temp;
        }
        
        // Assign to slots
        for (int i = 0; i < slots.Count && i < pages.Count; i++)
        {
            PlacePageInSlot(pages[i], slots[i]);
        }
        
        Debug.Log("[Mirror3Simple] Shuffle complete!");
    }

    public void OnPageDragStart(DiaryPage page)
    {
        currentlyDragging = page;
        
        if (page.pageImage != null)
        {
            Color c = page.pageImage.color;
            c.a = 0.6f;
            page.pageImage.color = c;
        }
        
        Debug.Log($"[Mirror3Simple] Started dragging: {page.pageId}");
    }

    public void OnPageDrag(DiaryPage page, PointerEventData eventData)
    {
        if (page.pageRect != null)
        {
            // Get the canvas
            Canvas canvas = page.pageRect.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                // Convert screen position to canvas position
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    eventData.position,
                    canvas.worldCamera,
                    out localPoint
                );
                
                // Set position directly
                page.pageRect.anchoredPosition = localPoint;
            }
            else
            {
                // Fallback: use delta movement
                page.pageRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
    }

    public void OnPageDragEnd(DiaryPage page, PointerEventData eventData)
    {
        currentlyDragging = null;
        
        // Restore alpha
        if (page.pageImage != null)
        {
            Color c = page.pageImage.color;
            c.a = 1f;
            page.pageImage.color = c;
        }
        
        Debug.Log($"[Mirror3Simple] Drag ended for {page.pageId}");
        Debug.Log($"[Mirror3Simple] Page position: {page.pageRect.position}");
        
        // Find closest slot
        DiarySlot closestSlot = FindClosestSlot(page.pageRect.position);
        
        if (closestSlot != null)
        {
            Debug.Log($"[Mirror3Simple] Closest slot: {closestSlot.slotTransform.name}");
            
            if (closestSlot != page.parentSlot)
            {
                Debug.Log($"[Mirror3Simple] Different slot detected!");
                
                // SWAP if slot has a page
                if (closestSlot.currentPage != null)
                {
                    Debug.Log($"[Mirror3Simple] 🔄 SWAPPING: {page.pageId} ↔ {closestSlot.currentPage.pageId}");
                    
                    DiaryPage otherPage = closestSlot.currentPage;
                    DiarySlot originalSlot = page.parentSlot;
                    
                    // Swap
                    PlacePageInSlot(page, closestSlot);
                    PlacePageInSlot(otherPage, originalSlot);
                    
                    Debug.Log($"[Mirror3Simple] ✅ Swap complete!");
                }
                else
                {
                    // Just move
                    Debug.Log($"[Mirror3Simple] Moving to empty slot");
                    
                    DiarySlot originalSlot = page.parentSlot;
                    PlacePageInSlot(page, closestSlot);
                    
                    if (originalSlot != null)
                    {
                        originalSlot.currentPage = null;
                    }
                }
                
                // Play sound
                if (paperRustleSound != null)
                {
                    AudioManager.Instance?.PlaySFX(paperRustleSound);
                }
                
                // Show current arrangement
                Debug.Log($"[Mirror3Simple] === Current Arrangement ===");
                for (int i = 0; i < slots.Count; i++)
                {
                    string pageName = slots[i].currentPage != null ? slots[i].currentPage.pageId : "EMPTY";
                    Debug.Log($"[Mirror3Simple] Slot {i + 1}: {pageName}");
                }
                
                CheckSolution();
            }
            else
            {
                Debug.Log($"[Mirror3Simple] Same slot, returning to position");
                // Return to original position
                if (page.parentSlot != null)
                {
                    PlacePageInSlot(page, page.parentSlot);
                }
            }
        }
        else
        {
            Debug.Log($"[Mirror3Simple] No slot found, returning to original");
            // Return to original position
            if (page.parentSlot != null)
            {
                PlacePageInSlot(page, page.parentSlot);
            }
        }
    }

    void PlacePageInSlot(DiaryPage page, DiarySlot slot)
    {
        if (page == null || slot == null) return;
        
        page.transform.SetParent(slot.slotTransform);
        page.pageRect.anchoredPosition = Vector2.zero;
        page.pageRect.localScale = Vector3.one;
        
        page.parentSlot = slot;
        slot.currentPage = page;
    }

    DiarySlot FindClosestSlot(Vector3 position)
    {
        DiarySlot closest = null;
        float minDistance = 300f; // Increased detection radius for easier snapping
        
        Debug.Log($"[Mirror3Simple] Finding closest slot to position: {position}");
        
        foreach (var slot in slots)
        {
            float distance = Vector3.Distance(position, slot.slotRect.position);
            Debug.Log($"[Mirror3Simple] Distance to {slot.slotTransform.name}: {distance:F1}");
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = slot;
            }
        }
        
        if (closest != null)
        {
            Debug.Log($"[Mirror3Simple] Closest slot: {closest.slotTransform.name} (distance: {minDistance:F1})");
        }
        else
        {
            Debug.Log($"[Mirror3Simple] No slot within detection radius ({300f})");
        }
        
        return closest;
    }

    void CheckSolution()
    {
        bool isCorrect = true;
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].currentPage == null || slots[i].currentPage.pageId != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }
        
        if (isCorrect)
        {
            Debug.Log("[Mirror3Simple] ✅ PUZZLE SOLVED!");
            StartCoroutine(PuzzleSuccess());
        }
    }

    void Update()
    {
        if (isPuzzleActive && !isPuzzleSolved)
        {
            currentTime -= Time.unscaledDeltaTime;
            UpdateTimerDisplay();
            
            if (currentTime <= 0)
            {
                StartCoroutine(EmilyAttack());
            }
        }
    }

    IEnumerator ShowStartDialogue()
    {
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_EXAMINE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    IEnumerator PuzzleSuccess()
    {
        isPuzzleSolved = true;
        isPuzzleActive = false;
        
        if (successSound != null)
        {
            AudioManager.Instance?.PlaySFX(successSound);
        }
        
        if (successEffect != null)
        {
            successEffect.SetActive(true);
        }
        
        yield return new WaitForSeconds(1f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        ResumeGame();
        
        Room09_FlowController.Instance?.OnMirrorComplete(3);
    }

    IEnumerator EmilyAttack()
    {
        isPuzzleActive = false;
        
        if (emilyScreamSound != null)
        {
            AudioManager.Instance?.PlaySFX(emilyScreamSound);
        }
        
        if (emilyJumpscarePanel != null)
        {
            emilyJumpscarePanel.SetActive(true);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    void PauseGame()
    {
        Time.timeScale = 1f;
        
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = true;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        
        if (currentTime <= 15f)
        {
            timerText.color = Color.red;
        }
        else if (currentTime <= 30f)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }
}

// Helper classes
public class DiarySlot
{
    public Transform slotTransform;
    public RectTransform slotRect;
    public DiaryPage currentPage;
}

public class DiaryPage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string pageId;
    public RectTransform pageRect;
    public Image pageImage;
    public DiarySlot parentSlot;
    public Mirror3_VanityTerror_Simple puzzle;
    
    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        // Add CanvasGroup if not present
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (puzzle != null)
        {
            // Disable raycast blocking so we can detect slots underneath
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = false;
            }
            
            puzzle.OnPageDragStart(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (puzzle != null) puzzle.OnPageDrag(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycast blocking
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
        
        if (puzzle != null) puzzle.OnPageDragEnd(this, eventData);
    }
}
