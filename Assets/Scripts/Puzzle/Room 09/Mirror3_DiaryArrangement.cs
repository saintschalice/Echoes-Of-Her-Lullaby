using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 3: Diary Arrangement Puzzle - SIMPLEST VERSION
/// Just arrange 8 diary pages in correct order
/// Drag and drop with automatic swap
/// </summary>
public class Mirror3_DiaryArrangement : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public GameObject[] slots; // 8 slots (Slot_1 to Slot_8)
    public GameObject[] pages; // 8 pages (DiaryPage_1 to DiaryPage_8)
    
    [Header("Settings")]
    public float timeLimit = 90f;
    public float snapDistance = 200f; // How close to snap to slot
    
    [Header("Audio")]
    public AudioClip swapSound;
    public AudioClip successSound;
    public AudioClip failSound;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    
    // Track which page is in which slot
    private Dictionary<int, int> slotToPage = new Dictionary<int, int>(); // slotIndex -> pageIndex
    
    // Currently dragging
    private int draggingPageIndex = -1;
    private Vector2 dragOffset;

    void Start()
    {
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Initialize - each page starts in its corresponding slot
        for (int i = 0; i < 8; i++)
        {
            slotToPage[i] = i; // Slot 0 has Page 0, etc.
        }
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive || isPuzzleSolved) return;
        
        Debug.Log("[Mirror3] Starting diary arrangement puzzle");
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Randomize initial positions
        RandomizePages();
        
        PauseGame();
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_EXAMINE, "Lisa");
    }

    void RandomizePages()
    {
        // Shuffle the page assignments
        List<int> pageIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        
        // Fisher-Yates shuffle
        for (int i = 0; i < pageIndices.Count; i++)
        {
            int randomIndex = Random.Range(i, pageIndices.Count);
            int temp = pageIndices[i];
            pageIndices[i] = pageIndices[randomIndex];
            pageIndices[randomIndex] = temp;
        }
        
        // Assign pages to slots
        for (int i = 0; i < 8; i++)
        {
            slotToPage[i] = pageIndices[i];
            PlacePageInSlot(pageIndices[i], i);
        }
        
        Debug.Log("[Mirror3] Pages randomized!");
    }

    void Update()
    {
        if (!isPuzzleActive || isPuzzleSolved) return;
        
        // Update timer
        currentTime -= Time.unscaledDeltaTime;
        UpdateTimerDisplay();
        
        if (currentTime <= 0)
        {
            StartCoroutine(TimeOut());
        }
        
        // Handle drag input
        HandleDragInput();
    }

    void HandleDragInput()
    {
        // Touch or mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 inputPos = Input.mousePosition;
            int pageIndex = GetPageAtPosition(inputPos);
            
            if (pageIndex >= 0)
            {
                draggingPageIndex = pageIndex;
                
                // Calculate offset
                RectTransform pageRect = pages[pageIndex].GetComponent<RectTransform>();
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    puzzlePanel.GetComponent<RectTransform>(),
                    inputPos,
                    null,
                    out localPoint
                );
                dragOffset = pageRect.anchoredPosition - localPoint;
                
                // Make semi-transparent
                SetPageAlpha(pageIndex, 0.6f);
                
                Debug.Log($"[Mirror3] Started dragging page {pageIndex}");
            }
        }
        else if (Input.GetMouseButton(0) && draggingPageIndex >= 0)
        {
            // Drag the page
            Vector2 inputPos = Input.mousePosition;
            RectTransform pageRect = pages[draggingPageIndex].GetComponent<RectTransform>();
            
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                puzzlePanel.GetComponent<RectTransform>(),
                inputPos,
                null,
                out localPoint
            );
            
            pageRect.anchoredPosition = localPoint + dragOffset;
        }
        else if (Input.GetMouseButtonUp(0) && draggingPageIndex >= 0)
        {
            // Drop the page
            Vector2 inputPos = Input.mousePosition;
            int targetSlotIndex = GetSlotAtPosition(inputPos);
            
            SetPageAlpha(draggingPageIndex, 1f);
            
            if (targetSlotIndex >= 0)
            {
                // Find which page is in target slot
                int targetPageIndex = -1;
                foreach (var kvp in slotToPage)
                {
                    if (kvp.Value == draggingPageIndex)
                    {
                        // This is the dragged page's current slot
                        continue;
                    }
                    if (kvp.Key == targetSlotIndex)
                    {
                        targetPageIndex = kvp.Value;
                        break;
                    }
                }
                
                // Find dragged page's current slot
                int currentSlotIndex = -1;
                foreach (var kvp in slotToPage)
                {
                    if (kvp.Value == draggingPageIndex)
                    {
                        currentSlotIndex = kvp.Key;
                        break;
                    }
                }
                
                if (currentSlotIndex != targetSlotIndex)
                {
                    // SWAP!
                    Debug.Log($"[Mirror3] Swapping: Page {draggingPageIndex} (slot {currentSlotIndex}) ↔ Page {targetPageIndex} (slot {targetSlotIndex})");
                    
                    slotToPage[currentSlotIndex] = targetPageIndex;
                    slotToPage[targetSlotIndex] = draggingPageIndex;
                    
                    PlacePageInSlot(draggingPageIndex, targetSlotIndex);
                    PlacePageInSlot(targetPageIndex, currentSlotIndex);
                    
                    // Play sound
                    if (swapSound != null)
                    {
                        AudioManager.Instance?.PlaySFX(swapSound);
                    }
                    
                    // Check if solved
                    CheckSolution();
                }
                else
                {
                    // Same slot, return to position
                    PlacePageInSlot(draggingPageIndex, currentSlotIndex);
                }
            }
            else
            {
                // No slot found, return to original position
                int currentSlotIndex = -1;
                foreach (var kvp in slotToPage)
                {
                    if (kvp.Value == draggingPageIndex)
                    {
                        currentSlotIndex = kvp.Key;
                        break;
                    }
                }
                PlacePageInSlot(draggingPageIndex, currentSlotIndex);
            }
            
            draggingPageIndex = -1;
        }
    }

    int GetPageAtPosition(Vector2 screenPos)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] == null) continue;
            
            RectTransform rect = pages[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, null))
            {
                return i;
            }
        }
        return -1;
    }

    int GetSlotAtPosition(Vector2 screenPos)
    {
        float closestDistance = snapDistance;
        int closestSlot = -1;
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;
            
            RectTransform rect = slots[i].GetComponent<RectTransform>();
            float distance = Vector2.Distance(rect.position, screenPos);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSlot = i;
            }
        }
        
        return closestSlot;
    }

    void PlacePageInSlot(int pageIndex, int slotIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length) return;
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        
        RectTransform pageRect = pages[pageIndex].GetComponent<RectTransform>();
        RectTransform slotRect = slots[slotIndex].GetComponent<RectTransform>();
        
        pageRect.SetParent(slotRect.transform);
        pageRect.anchoredPosition = Vector2.zero;
        pageRect.localScale = Vector3.one;
    }

    void SetPageAlpha(int pageIndex, float alpha)
    {
        if (pageIndex < 0 || pageIndex >= pages.Length) return;
        
        Image img = pages[pageIndex].GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    void CheckSolution()
    {
        // Check if pages are in correct order
        // Slot 0 should have Page 0, Slot 1 should have Page 1, etc.
        bool isCorrect = true;
        
        for (int i = 0; i < 8; i++)
        {
            if (slotToPage[i] != i)
            {
                isCorrect = false;
                break;
            }
        }
        
        if (isCorrect)
        {
            Debug.Log("[Mirror3] ✅ PUZZLE SOLVED!");
            StartCoroutine(PuzzleSuccess());
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
        
        yield return new WaitForSeconds(1f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        ResumeGame();
        
        Room09_FlowController.Instance?.OnMirrorComplete(3);
    }

    IEnumerator TimeOut()
    {
        isPuzzleActive = false;
        
        if (failSound != null)
        {
            AudioManager.Instance?.PlaySFX(failSound);
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
