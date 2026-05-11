using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 4: Evidence Sequence Puzzle
/// Arrange 4 evidence items in correct order showing mother's murder plan
/// Time limit: 90 seconds
/// </summary>
public class Mirror4_EvidenceSequence : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public Transform[] pictureFrames; // 4 frames (Frame_1 to Frame_4)
    public GameObject[] evidenceItems; // 4 items (Rope, Pills, Knife, BloodyTowel)
    
    [Header("Flashback System")]
    public Image flashbackImage; // Shows flashback when item placed correctly
    public Sprite[] flashbackSprites; // 4 flashback images (one per item)
    public float flashbackDuration = 2f; // How long flashback shows
    
    [Header("Settings")]
    public float timeLimit = 90f;
    public float snapDistance = 200f; // How close to snap to frame
    
    [Header("Audio")]
    public AudioClip itemPlaceSound;
    public AudioClip flashbackSound;
    public AudioClip successSound;
    public AudioClip failSound;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    
    // Correct sequence: Rope → Pills → Knife → BloodyTowel
    private string[] correctSequence = { "Rope", "Pills", "Knife", "BloodyTowel" };
    private Dictionary<int, string> frameContents = new Dictionary<int, string>(); // frameIndex → itemName
    
    // Currently dragging
    private int draggingItemIndex = -1;
    private Vector2 dragOffset;
    private Vector3[] originalPositions; // Store original positions for reset

    void Start()
    {
        // Hide panel at start
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Hide flashback image
        if (flashbackImage != null) flashbackImage.gameObject.SetActive(false);
        
        // Initialize frame contents
        for (int i = 0; i < pictureFrames.Length; i++)
        {
            frameContents[i] = ""; // Empty at start
        }
        
        // Store original positions
        originalPositions = new Vector3[evidenceItems.Length];
        for (int i = 0; i < evidenceItems.Length; i++)
        {
            if (evidenceItems[i] != null)
            {
                originalPositions[i] = evidenceItems[i].transform.position;
            }
        }
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive || isPuzzleSolved) return;
        
        Debug.Log("[Mirror4] Starting Evidence Sequence puzzle");
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Reset items to original positions
        ResetItems();
        
        // Clear frame contents
        for (int i = 0; i < pictureFrames.Length; i++)
        {
            frameContents[i] = "";
        }
        
        PauseGame();
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_EXAMINE, "Lisa");
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
            int itemIndex = GetItemAtPosition(inputPos);
            
            if (itemIndex >= 0)
            {
                draggingItemIndex = itemIndex;
                
                // Calculate offset
                RectTransform itemRect = evidenceItems[itemIndex].GetComponent<RectTransform>();
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    puzzlePanel.GetComponent<RectTransform>(),
                    inputPos,
                    null,
                    out localPoint
                );
                dragOffset = itemRect.anchoredPosition - localPoint;
                
                // Make semi-transparent
                SetItemAlpha(itemIndex, 0.6f);
                
                // Remove from frame if it was in one
                RemoveItemFromFrames(GetItemName(itemIndex));
                
                Debug.Log($"[Mirror4] Started dragging {GetItemName(itemIndex)}");
            }
        }
        else if (Input.GetMouseButton(0) && draggingItemIndex >= 0)
        {
            // Drag the item
            Vector2 inputPos = Input.mousePosition;
            RectTransform itemRect = evidenceItems[draggingItemIndex].GetComponent<RectTransform>();
            
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                puzzlePanel.GetComponent<RectTransform>(),
                inputPos,
                null,
                out localPoint
            );
            
            itemRect.anchoredPosition = localPoint + dragOffset;
        }
        else if (Input.GetMouseButtonUp(0) && draggingItemIndex >= 0)
        {
            // Drop the item
            Vector2 inputPos = Input.mousePosition;
            int targetFrameIndex = GetFrameAtPosition(inputPos);
            
            SetItemAlpha(draggingItemIndex, 1f);
            
            if (targetFrameIndex >= 0)
            {
                // Place in frame
                string itemName = GetItemName(draggingItemIndex);
                PlaceItemInFrame(draggingItemIndex, targetFrameIndex);
                frameContents[targetFrameIndex] = itemName;
                
                // Play sound
                if (itemPlaceSound != null)
                {
                    AudioManager.Instance?.PlaySFX(itemPlaceSound);
                }
                
                // Check if correct placement
                if (IsCorrectPlacement(targetFrameIndex, itemName))
                {
                    Debug.Log($"[Mirror4] ✅ Correct! {itemName} in Frame {targetFrameIndex}");
                    
                    // Show flashback
                    StartCoroutine(ShowFlashback(targetFrameIndex));
                }
                else
                {
                    Debug.Log($"[Mirror4] ❌ Wrong placement: {itemName} in Frame {targetFrameIndex}");
                }
                
                // Check if puzzle solved
                CheckSolution();
            }
            else
            {
                // No frame found, return to original position
                ResetItem(draggingItemIndex);
            }
            
            draggingItemIndex = -1;
        }
    }

    int GetItemAtPosition(Vector2 screenPos)
    {
        for (int i = 0; i < evidenceItems.Length; i++)
        {
            if (evidenceItems[i] == null) continue;
            
            RectTransform rect = evidenceItems[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, null))
            {
                return i;
            }
        }
        return -1;
    }

    int GetFrameAtPosition(Vector2 screenPos)
    {
        float closestDistance = snapDistance;
        int closestFrame = -1;
        
        for (int i = 0; i < pictureFrames.Length; i++)
        {
            if (pictureFrames[i] == null) continue;
            
            RectTransform rect = pictureFrames[i].GetComponent<RectTransform>();
            float distance = Vector2.Distance(rect.position, screenPos);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFrame = i;
            }
        }
        
        return closestFrame;
    }

    void PlaceItemInFrame(int itemIndex, int frameIndex)
    {
        if (itemIndex < 0 || itemIndex >= evidenceItems.Length) return;
        if (frameIndex < 0 || frameIndex >= pictureFrames.Length) return;
        
        RectTransform itemRect = evidenceItems[itemIndex].GetComponent<RectTransform>();
        RectTransform frameRect = pictureFrames[frameIndex].GetComponent<RectTransform>();
        
        itemRect.SetParent(frameRect);
        itemRect.anchoredPosition = Vector2.zero;
        itemRect.localScale = Vector3.one;
    }

    void ResetItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= evidenceItems.Length) return;
        
        evidenceItems[itemIndex].transform.position = originalPositions[itemIndex];
        evidenceItems[itemIndex].transform.SetParent(puzzlePanel.transform);
    }

    void ResetItems()
    {
        for (int i = 0; i < evidenceItems.Length; i++)
        {
            ResetItem(i);
        }
    }

    void SetItemAlpha(int itemIndex, float alpha)
    {
        if (itemIndex < 0 || itemIndex >= evidenceItems.Length) return;
        
        Image img = evidenceItems[itemIndex].GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    string GetItemName(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= evidenceItems.Length) return "";
        return evidenceItems[itemIndex].name;
    }

    void RemoveItemFromFrames(string itemName)
    {
        foreach (var kvp in frameContents)
        {
            if (kvp.Value == itemName)
            {
                frameContents[kvp.Key] = "";
                break;
            }
        }
    }

    bool IsCorrectPlacement(int frameIndex, string itemName)
    {
        if (frameIndex < 0 || frameIndex >= correctSequence.Length) return false;
        return correctSequence[frameIndex] == itemName;
    }

    IEnumerator ShowFlashback(int frameIndex)
    {
        if (flashbackImage == null || flashbackSprites == null) yield break;
        if (frameIndex < 0 || frameIndex >= flashbackSprites.Length) yield break;
        
        // Play sound
        if (flashbackSound != null)
        {
            AudioManager.Instance?.PlaySFX(flashbackSound);
        }
        
        // Show flashback
        flashbackImage.sprite = flashbackSprites[frameIndex];
        flashbackImage.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(flashbackDuration);
        
        // Hide flashback
        flashbackImage.gameObject.SetActive(false);
    }

    void CheckSolution()
    {
        Debug.Log("[Mirror4] Checking solution...");
        
        // Check if all frames filled
        int filledFrames = 0;
        foreach (var content in frameContents.Values)
        {
            if (!string.IsNullOrEmpty(content))
            {
                filledFrames++;
            }
        }
        
        Debug.Log($"[Mirror4] Filled frames: {filledFrames}/4");
        
        if (filledFrames < 4)
        {
            Debug.Log("[Mirror4] Not all frames filled yet");
            return;
        }
        
        // Check if correct sequence
        bool isCorrect = true;
        for (int i = 0; i < pictureFrames.Length; i++)
        {
            string expected = correctSequence[i];
            string actual = frameContents[i];
            
            Debug.Log($"[Mirror4] Frame {i}: Expected={expected}, Actual={actual}");
            
            if (actual != expected)
            {
                isCorrect = false;
            }
        }
        
        if (isCorrect)
        {
            Debug.Log("[Mirror4] ✅ PUZZLE SOLVED!");
            StartCoroutine(PuzzleSuccess());
        }
        else
        {
            Debug.Log("[Mirror4] ❌ Wrong order! Keep trying...");
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
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_SUCCESS_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Close panel
        Debug.Log("[Mirror4] Hiding puzzle panel...");
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
            Debug.Log("[Mirror4] ✅ Panel hidden");
        }
        
        ResumeGame();
        
        Room09_FlowController.Instance?.OnMirrorComplete(4);
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
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_ATTACK_2, "Lisa");
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
