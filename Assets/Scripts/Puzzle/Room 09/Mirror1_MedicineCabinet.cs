using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // For TextMeshProUGUI

/// <summary>
/// Mirror 1: Medicine Cabinet Puzzle
/// Arrange 6 prescription bottles chronologically by year
/// </summary>
public class Mirror1_MedicineCabinet : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject puzzlePanel;
    
    [Header("Bottle Slots (6 slots, left to right)")]
    public Transform[] bottleSlots = new Transform[6];
    
    [Header("Timer Display")]
    public TextMeshProUGUI timerText;
    
    [Header("Success")]
    public GameObject successEffect;
    public AudioClip successSound;
    
    [Header("Failure (Emily Attack)")]
    public GameObject emilyJumpscarePanel;
    public AudioClip emilyScreamSound;
    public float timeLimit = 60f; // 60 seconds before Emily attacks
    
    [Header("Mistakes System")]
    public TextMeshProUGUI mistakesText; // Shows "Mistakes: X/3"
    public int maxMistakes = 3; // 3 strikes and you're out
    public AudioClip wrongPlacementSound; // Sound when wrong bottle placed
    
    [Header("Visual Hints")]
    public TextMeshProUGUI hintText; // Shows "Arrange chronologically: 1973 → 1976"
    
    private Dictionary<GameObject, string> slotContents = new Dictionary<GameObject, string>(); // Track what's in each slot
    private float timeRemaining;
    private bool puzzleActive = false;
    private bool puzzleComplete = false;
    private int mistakeCount = 0; // Track wrong placements

    public void StartPuzzle()
    {
        if (puzzleComplete) return;
        
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Initialize slot tracking
        slotContents.Clear();
        foreach (Transform slot in bottleSlots)
        {
            if (slot != null)
            {
                slotContents[slot.gameObject] = ""; // Empty at start
            }
        }
        
        // Reset mistakes
        mistakeCount = 0;
        UpdateMistakesDisplay();
        
        // Show hint
        if (hintText != null)
        {
            hintText.text = "Arrange chronologically: 1973 → 1976";
        }
        
        // Start timer
        timeRemaining = timeLimit;
        puzzleActive = true;
        
        // Disable player
        PauseGame();
        
        // Show dialogue
        StartCoroutine(ShowStartDialogue());
    }

    System.Collections.IEnumerator ShowStartDialogue()
    {
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR1_EXAMINE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR1_HINT, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    void Update()
    {
        if (!puzzleActive || puzzleComplete) return;
        
        // Update timer
        timeRemaining -= Time.unscaledDeltaTime;
        
        // Update timer display
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            
            // Change color when time is low
            if (timeRemaining <= 10f)
            {
                timerText.color = Color.red;
            }
            else if (timeRemaining <= 20f)
            {
                timerText.color = Color.yellow;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
        
        // Check timeout
        if (timeRemaining <= 0)
        {
            StartCoroutine(EmilyAttack());
        }
    }

    // Called by DraggableItem system
    // Returns true if placement is valid, false if rejected
    public bool ValidateAndPlaceBottle(GameObject slot, string bottleId)
    {
        Debug.Log($"[Mirror1] 🍾 Validating bottle {bottleId} for slot {slot.name}");
        
        // Check if this slot is one of our tracked slots
        if (!slotContents.ContainsKey(slot))
        {
            Debug.LogError($"[Mirror1] ❌ Slot {slot.name} is not in our tracked slots!");
            return false;
        }
        
        // Get the slot index
        int slotIndex = -1;
        for (int i = 0; i < bottleSlots.Length; i++)
        {
            if (bottleSlots[i].gameObject == slot)
            {
                slotIndex = i;
                break;
            }
        }
        
        if (slotIndex == -1)
        {
            Debug.LogError($"[Mirror1] ❌ Could not find slot index for {slot.name}!");
            return false;
        }
        
        // Check if this is the CORRECT bottle for this slot
        // Based on actual GameObject names in your scene
        string[] correctOrder = { 
            "Antidepressants_1973",  // Slot 1
            "Lithium_1974",          // Slot 2
            "Valium_1975",           // Slot 3
            "PainPills_1975",        // Slot 4
            "SleepingPills_1976",    // Slot 5
            "UnknownPills_1976"      // Slot 6
        };
        string expectedBottle = correctOrder[slotIndex];
        
        bool isCorrectPlacement = (bottleId == expectedBottle);
        
        if (!isCorrectPlacement)
        {
            // WRONG PLACEMENT!
            Debug.Log($"[Mirror1] ❌ WRONG! Slot {slotIndex} expects {expectedBottle}, got {bottleId}");
            
            // Increment mistake counter
            mistakeCount++;
            UpdateMistakesDisplay();
            
            // Play wrong sound
            if (wrongPlacementSound != null)
            {
                AudioManager.Instance?.PlaySFX(wrongPlacementSound);
            }
            
            // Show feedback
            if (hintText != null)
            {
                hintText.text = $"Wrong! That bottle doesn't belong there. ({mistakeCount}/3 mistakes)";
            }
            
            // Check if game over
            if (mistakeCount >= maxMistakes)
            {
                Debug.Log($"[Mirror1] ☠️ TOO MANY MISTAKES! EMILY ATTACKS!");
                StartCoroutine(EmilyAttack());
            }
            
            // Reject the placement
            return false;
        }
        
        // CORRECT PLACEMENT!
        Debug.Log($"[Mirror1] ✅ CORRECT! {bottleId} belongs in slot {slotIndex}");
        
        // Remove this bottle from any other slot it might be in
        foreach (var kvp in slotContents)
        {
            if (kvp.Value == bottleId && kvp.Key != slot)
            {
                Debug.Log($"[Mirror1] Removing {bottleId} from previous slot {kvp.Key.name}");
                slotContents[kvp.Key] = ""; // Clear old slot
            }
        }
        
        // Update slot contents
        slotContents[slot] = bottleId;
        
        // Reset hint text
        if (hintText != null)
        {
            hintText.text = "Arrange chronologically: 1973 → 1976";
        }
        
        Debug.Log($"[Mirror1] Current slot contents:");
        for (int i = 0; i < bottleSlots.Length; i++)
        {
            GameObject slotObj = bottleSlots[i].gameObject;
            string content = slotContents.ContainsKey(slotObj) ? slotContents[slotObj] : "NOT TRACKED";
            Debug.Log($"  Slot {i} ({slotObj.name}): {(string.IsNullOrEmpty(content) ? "EMPTY" : content)}");
        }
        
        // Check if puzzle solved
        CheckSolution();
        
        // Accept the placement
        return true;
    }

    void CheckSolution()
    {
        Debug.Log("[Mirror1] ═══════════════════════════════════");
        Debug.Log("[Mirror1] Checking solution...");
        
        // Check if ALL 6 slots are filled
        int filledSlots = 0;
        foreach (var kvp in slotContents)
        {
            if (!string.IsNullOrEmpty(kvp.Value))
            {
                filledSlots++;
            }
        }
        
        Debug.Log($"[Mirror1] 📊 Filled slots: {filledSlots}/6");
        
        // If not all slots filled, don't check solution yet
        if (filledSlots < 6)
        {
            Debug.Log("[Mirror1] ⏳ Not all slots filled yet. Waiting for more bottles...");
            Debug.Log("[Mirror1] ═══════════════════════════════════");
            return;
        }
        
        Debug.Log("[Mirror1] ✅ All 6 slots are filled! Checking order...");
        
        // Check if bottles are in CORRECT order
        // NOTE: We already validated each placement, so if all 6 are filled, they MUST be correct!
        string[] correctOrder = { 
            "Antidepressants_1973",  // Slot 1
            "Lithium_1974",          // Slot 2
            "Valium_1975",           // Slot 3
            "PainPills_1975",        // Slot 4
            "SleepingPills_1976",    // Slot 5
            "UnknownPills_1976"      // Slot 6
        };
        
        bool allCorrect = true;
        for (int i = 0; i < bottleSlots.Length; i++)
        {
            GameObject slot = bottleSlots[i].gameObject;
            string expectedBottle = correctOrder[i];
            string actualBottle = slotContents.ContainsKey(slot) ? slotContents[slot] : "MISSING";
            
            bool isCorrect = actualBottle == expectedBottle;
            string icon = isCorrect ? "✅" : "❌";
            
            Debug.Log($"[Mirror1] {icon} Slot {i}: Expected={expectedBottle}, Actual={actualBottle}");
            
            if (!isCorrect)
            {
                allCorrect = false;
            }
        }
        
        if (allCorrect)
        {
            Debug.Log("[Mirror1] ═══════════════════════════════════");
            Debug.Log("[Mirror1] 🎉🎉🎉 ALL BOTTLES CORRECT! PUZZLE SOLVED! 🎉🎉🎉");
            Debug.Log("[Mirror1] ═══════════════════════════════════");
            StartCoroutine(PuzzleSuccess());
        }
        else
        {
            // This should never happen since we validate each placement
            Debug.LogWarning("[Mirror1] ⚠️ All slots filled but not all correct - this shouldn't happen!");
            Debug.Log("[Mirror1] ═══════════════════════════════════");
        }
    }
    
    void UpdateMistakesDisplay()
    {
        if (mistakesText != null)
        {
            mistakesText.text = $"Mistakes: {mistakeCount}/{maxMistakes}";
            
            // Change color based on mistakes
            if (mistakeCount >= maxMistakes)
            {
                mistakesText.color = Color.red;
            }
            else if (mistakeCount >= 2)
            {
                mistakesText.color = new Color(1f, 0.5f, 0f); // Orange
            }
            else if (mistakeCount >= 1)
            {
                mistakesText.color = Color.yellow;
            }
            else
            {
                mistakesText.color = Color.white;
            }
        }
    }

    System.Collections.IEnumerator PuzzleSuccess()
    {
        puzzleActive = false;
        puzzleComplete = true;
        
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
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR1_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR1_SUCCESS_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Hide panel
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Resume game
        ResumeGame();
        
        // Notify flow controller
        Room09_FlowController.Instance?.OnMirrorComplete(1);
    }

    System.Collections.IEnumerator EmilyAttack()
    {
        puzzleActive = false;
        
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
        
        // Game over - reload scene or checkpoint
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
}

/// <summary>
/// Individual bottle slot component
/// NOTE: This class is NOT USED when using DraggableItem system
/// It was for the old prefab-based system
/// Keeping it here for reference, but it's not needed
/// </summary>
/*
public class BottleSlot : MonoBehaviour
{
    public Mirror1_MedicineCabinet.BottleData bottleData;
    public int currentSlotIndex;
    private Mirror1_MedicineCabinet puzzleController;
    
    public void Initialize(Mirror1_MedicineCabinet.BottleData data, int slotIndex, Mirror1_MedicineCabinet controller)
    {
        bottleData = data;
        currentSlotIndex = slotIndex;
        puzzleController = controller;
        
        // Set sprite
        Image img = GetComponent<Image>();
        if (img != null && data.bottleSprite != null)
        {
            img.sprite = data.bottleSprite;
        }
        
        // Set label
        Text label = GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = data.label;
        }
    }
    
    public void OnDrop(int newSlotIndex)
    {
        currentSlotIndex = newSlotIndex;
        puzzleController.OnBottlePlaced(newSlotIndex, bottleData);
    }
}
*/
