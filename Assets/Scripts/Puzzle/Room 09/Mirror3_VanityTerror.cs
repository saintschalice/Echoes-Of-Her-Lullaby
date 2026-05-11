using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 3: Vanity Terror Puzzle
/// Player rearranges 8 diary pages in chronological order
/// Pages start in slots but shuffled randomly
/// Time limit: 90 seconds
/// </summary>
public class Mirror3_VanityTerror : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public Transform[] diarySlots; // 8 slots for diary pages (Slot_1 to Slot_8)
    
    [Header("Puzzle Settings")]
    public float timeLimit = 90f; // 90 seconds for 8 pages
    
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
    
    // Correct sequence for diary pages (chronological order)
    // Page_1 = earliest entry, Page_8 = latest entry
    private string[] correctSequence = { 
        "DiaryPage_1", 
        "DiaryPage_2", 
        "DiaryPage_3", 
        "DiaryPage_4", 
        "DiaryPage_5", 
        "DiaryPage_6", 
        "DiaryPage_7", 
        "DiaryPage_8" 
    };
    private Dictionary<GameObject, string> slotContents = new Dictionary<GameObject, string>();

    private void Start()
    {
        // Hide panel at start
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Initialize slot contents
        foreach (Transform slot in diarySlots)
        {
            if (slot != null)
            {
                slotContents[slot.gameObject] = "";
            }
        }
        
        Debug.Log("[Mirror3] Mirror3_VanityTerror initialized");
    }
    
    // PUBLIC method for testing - can be called from Inspector or other scripts
    [ContextMenu("Test Shuffle")]
    public void TestShuffle()
    {
        Debug.Log("[Mirror3] ===== MANUAL TEST SHUFFLE =====");
        ShufflePages();
    }

    private void Update()
    {
        // Update timer if puzzle active
        if (isPuzzleActive && !isPuzzleSolved)
        {
            currentTime -= Time.unscaledDeltaTime; // Use unscaled time
            UpdateTimerDisplay();
            
            if (currentTime <= 0)
            {
                StartCoroutine(EmilyAttack());
            }
        }
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive || isPuzzleSolved) return;
        
        Debug.Log("[Mirror3] ========== STARTING VANITY TERROR PUZZLE ==========");
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        
        // Show panel
        if (puzzlePanel != null) 
        {
            puzzlePanel.SetActive(true);
            Debug.Log("[Mirror3] ✅ Puzzle panel shown");
        }
        else
        {
            Debug.LogError("[Mirror3] ❌ Puzzle panel is NULL!");
        }
        
        // RANDOMIZE: Shuffle pages in slots
        Debug.Log("[Mirror3] About to shuffle pages...");
        ShufflePages();
        
        // Disable player movement
        PauseGame();
        
        // Show dialogue
        StartCoroutine(ShowStartDialogue());
    }
    
    void ShufflePages()
    {
        Debug.Log("[Mirror3] ========== SHUFFLE PAGES START ==========");
        Debug.Log($"[Mirror3] Number of slots: {diarySlots.Length}");
        
        // Get all diary page GameObjects that are children of slots
        List<GameObject> pages = new List<GameObject>();
        
        for (int i = 0; i < diarySlots.Length; i++)
        {
            Transform slot = diarySlots[i];
            Debug.Log($"[Mirror3] Checking slot {i}: {slot.name}");
            
            // Find diary page in this slot
            foreach (Transform child in slot)
            {
                Debug.Log($"[Mirror3]   - Found child: {child.name}");
                
                if (child.name.Contains("DiaryPage"))
                {
                    pages.Add(child.gameObject);
                    Debug.Log($"[Mirror3]   ✅ Added {child.name} to shuffle list");
                    break;
                }
            }
        }
        
        Debug.Log($"[Mirror3] Total pages found: {pages.Count}");
        
        if (pages.Count != diarySlots.Length)
        {
            Debug.LogError($"[Mirror3] ❌ Found {pages.Count} pages but expected {diarySlots.Length}!");
            Debug.LogError("[Mirror3] Make sure each slot has ONE DiaryPage as a child!");
            return;
        }
        
        Debug.Log("[Mirror3] Shuffling pages...");
        
        // Shuffle the pages list (Fisher-Yates algorithm)
        for (int i = 0; i < pages.Count; i++)
        {
            int randomIndex = Random.Range(i, pages.Count);
            GameObject temp = pages[i];
            pages[i] = pages[randomIndex];
            pages[randomIndex] = temp;
        }
        
        Debug.Log("[Mirror3] Pages shuffled! New order:");
        
        // Place shuffled pages into slots
        for (int i = 0; i < diarySlots.Length; i++)
        {
            GameObject page = pages[i];
            Transform slot = diarySlots[i];
            
            // Move page to this slot
            page.transform.SetParent(slot);
            
            RectTransform pageRect = page.GetComponent<RectTransform>();
            if (pageRect != null)
            {
                pageRect.anchoredPosition = Vector2.zero; // Center in slot
                pageRect.localScale = Vector3.one; // Reset scale
            }
            
            // Update slot contents
            slotContents[slot.gameObject] = page.name;
            
            Debug.Log($"[Mirror3] Slot_{i + 1} now has: {page.name}");
        }
        
        Debug.Log("[Mirror3] ========== SHUFFLE COMPLETE ==========");
    }
    
    IEnumerator ShowStartDialogue()
    {
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_EXAMINE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_HINT, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    // Called by DraggableItem when diary page is placed in slot
    public void OnPagePlacedInSlot(GameObject targetSlot, string pageId)
    {
        Debug.Log($"[Mirror3] ========================================");
        Debug.Log($"[Mirror3] OnPagePlacedInSlot called: pageId='{pageId}', targetSlot='{targetSlot.name}'");
        
        // Find the page GameObject that was dragged by searching in ALL slots
        GameObject draggedPage = null;
        Transform draggedPageOriginalSlot = null;
        
        // Search all slots for the dragged page
        foreach (Transform slot in diarySlots)
        {
            foreach (Transform child in slot)
            {
                if (child.name == pageId)
                {
                    draggedPage = child.gameObject;
                    draggedPageOriginalSlot = slot;
                    Debug.Log($"[Mirror3] Found dragged page '{pageId}' in slot '{slot.name}'");
                    break;
                }
            }
            if (draggedPage != null) break;
        }
        
        // Also check if page is at Canvas root (if drag moved it there)
        if (draggedPage == null)
        {
            Debug.LogWarning($"[Mirror3] Page '{pageId}' not found in any slot, searching in Canvas...");
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                foreach (Transform child in canvas.transform)
                {
                    if (child.name == pageId)
                    {
                        draggedPage = child.gameObject;
                        Debug.Log($"[Mirror3] Found page '{pageId}' at Canvas root");
                        break;
                    }
                }
            }
        }
        
        if (draggedPage == null)
        {
            Debug.LogError($"[Mirror3] ❌ Could not find page '{pageId}' anywhere!");
            return;
        }
        
        // Check if target slot already has a page (need to SWAP)
        GameObject targetSlotPage = null;
        foreach (Transform child in targetSlot.transform)
        {
            if (child.name.Contains("DiaryPage") && child.gameObject != draggedPage)
            {
                targetSlotPage = child.gameObject;
                Debug.Log($"[Mirror3] Target slot '{targetSlot.name}' already has page: '{targetSlotPage.name}'");
                break;
            }
        }
        
        if (targetSlotPage != null)
        {
            // SWAP: Target slot has a different page
            Debug.Log($"[Mirror3] 🔄 SWAPPING: '{pageId}' ↔ '{targetSlotPage.name}'");
            
            if (draggedPageOriginalSlot != null)
            {
                // Move target slot's page to dragged page's original slot
                targetSlotPage.transform.SetParent(draggedPageOriginalSlot);
                RectTransform targetRect = targetSlotPage.GetComponent<RectTransform>();
                if (targetRect != null)
                {
                    targetRect.anchoredPosition = Vector2.zero;
                    targetRect.localScale = Vector3.one;
                }
                
                // Update slot contents for original slot
                slotContents[draggedPageOriginalSlot.gameObject] = targetSlotPage.name;
                
                Debug.Log($"[Mirror3] ✅ Moved '{targetSlotPage.name}' to '{draggedPageOriginalSlot.name}'");
            }
            else
            {
                Debug.LogWarning($"[Mirror3] Original slot is null, cannot complete swap!");
            }
        }
        else
        {
            // Target slot is empty - just move
            Debug.Log($"[Mirror3] Target slot '{targetSlot.name}' is empty, moving page");
            
            // Clear original slot if it exists
            if (draggedPageOriginalSlot != null)
            {
                slotContents[draggedPageOriginalSlot.gameObject] = "";
                Debug.Log($"[Mirror3] Cleared original slot '{draggedPageOriginalSlot.name}'");
            }
        }
        
        // Move dragged page to target slot
        draggedPage.transform.SetParent(targetSlot.transform);
        RectTransform draggedRect = draggedPage.GetComponent<RectTransform>();
        if (draggedRect != null)
        {
            draggedRect.anchoredPosition = Vector2.zero;
            draggedRect.localScale = Vector3.one;
        }
        
        // Update slot contents for target slot
        slotContents[targetSlot] = pageId;
        
        Debug.Log($"[Mirror3] ✅ Moved '{pageId}' to '{targetSlot.name}'");
        
        // Play sound
        if (paperRustleSound != null)
        {
            AudioManager.Instance?.PlaySFX(paperRustleSound);
        }
        
        // Show current state
        Debug.Log($"[Mirror3] === Current arrangement ===");
        for (int i = 0; i < diarySlots.Length; i++)
        {
            string content = slotContents[diarySlots[i].gameObject];
            Debug.Log($"[Mirror3]   Slot_{i + 1}: {(string.IsNullOrEmpty(content) ? "EMPTY" : content)}");
        }
        Debug.Log($"[Mirror3] ========================================");
        
        // Check if puzzle solved
        CheckPuzzleSolution();
    }

    private void CheckPuzzleSolution()
    {
        Debug.Log("[Mirror3] Checking solution...");
        
        // Check if all slots filled
        int filledSlots = 0;
        foreach (var content in slotContents.Values)
        {
            if (!string.IsNullOrEmpty(content))
            {
                filledSlots++;
            }
        }
        
        Debug.Log($"[Mirror3] Filled slots: {filledSlots}/8");
        
        if (filledSlots < 8)
        {
            Debug.Log("[Mirror3] Not all slots filled yet");
            return;
        }
        
        // Check if correct sequence
        bool isCorrect = true;
        for (int i = 0; i < diarySlots.Length; i++)
        {
            string expected = correctSequence[i];
            string actual = slotContents[diarySlots[i].gameObject];
            
            Debug.Log($"[Mirror3] Slot {i}: Expected={expected}, Actual={actual}");
            
            if (actual != expected)
            {
                isCorrect = false;
            }
        }
        
        if (isCorrect)
        {
            Debug.Log("[Mirror3] ✅ PUZZLE SOLVED!");
            StartCoroutine(PuzzleSuccess());
        }
        else
        {
            Debug.Log("[Mirror3] ❌ Wrong order! Keep trying...");
        }
    }

    IEnumerator PuzzleSuccess()
    {
        isPuzzleSolved = true;
        isPuzzleActive = false;
        
        // Play success sound
        if (successSound != null)
        {
            AudioManager.Instance?.PlaySFX(successSound);
        }
        
        // Show success effect
        if (successEffect != null)
        {
            successEffect.SetActive(true);
        }
        
        yield return new WaitForSeconds(1f);
        
        // Show success dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Close panel
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Resume game
        ResumeGame();
        
        // Notify flow controller
        Room09_FlowController.Instance?.OnMirrorComplete(3);
    }
    
    IEnumerator EmilyAttack()
    {
        isPuzzleActive = false;
        
        // Play scream sound
        if (emilyScreamSound != null)
        {
            AudioManager.Instance?.PlaySFX(emilyScreamSound);
        }
        
        // Show jumpscare
        if (emilyJumpscarePanel != null)
        {
            emilyJumpscarePanel.SetActive(true);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Show attack dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Game over - reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    void PauseGame()
    {
        Time.timeScale = 1f; // Puzzle uses unscaled time
        
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

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        
        // Change color when time is low
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
