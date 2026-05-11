using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 2: Bathtub Drain Puzzle
/// Player removes drain cover and reassembles 4 torn note pieces
/// Time limit: 90 seconds
/// </summary>
public class Mirror2_BathtubDrain : MonoBehaviour
{
    [Header("Bathtub Sprites")]
    public Image bathtubImage; // Reference to Bathtub_Image
    public Sprite bathtubWithWater; // Sprite with water
    public Sprite bathtubWithoutWater; // Sprite after draining (empty)
    
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public Button drainCoverButton; // DrainCover_Button
    public Transform[] assemblySlots; // 4 slots for note pieces (Slot_1 to Slot_4)
    
    [Header("Puzzle Settings")]
    public float timeLimit = 90f; // 90 seconds for this puzzle
    
    [Header("Audio")]
    public AudioClip drainOpenSound; // Sound when drain opens
    public AudioClip waterDrainSound; // Sound of water draining
    public AudioClip paperRustleSound; // Sound when placing note pieces
    public AudioClip successSound;
    public AudioClip emilyScreamSound;
    
    [Header("Success/Failure")]
    public GameObject successEffect;
    public GameObject emilyJumpscarePanel;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    private bool drainCoverRemoved = false;
    
    // Correct sequence for torn notes
    // Based on your GameObjects: Note_Piece_1, Note_Piece_2, Note_Piece_3, Note_Piece_4
    private string[] correctSequence = { "Note_Piece_1", "Note_Piece_2", "Note_Piece_3", "Note_Piece_4" };
    private Dictionary<GameObject, string> slotContents = new Dictionary<GameObject, string>();

    private void Start()
    {
        // Hide panel at start
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Setup drain cover button
        if (drainCoverButton != null)
        {
            drainCoverButton.onClick.AddListener(OnDrainCoverClicked);
        }
        
        // Make sure bathtub starts with water
        if (bathtubImage != null && bathtubWithWater != null)
        {
            bathtubImage.sprite = bathtubWithWater;
        }
        
        // Initialize slot contents
        foreach (Transform slot in assemblySlots)
        {
            if (slot != null)
            {
                slotContents[slot.gameObject] = "";
            }
        }
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
        
        Debug.Log("[Mirror2] Starting Bathtub Drain puzzle");
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        drainCoverRemoved = false;
        
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Disable player movement
        PauseGame();
        
        // Show dialogue
        StartCoroutine(ShowStartDialogue());
    }
    
    IEnumerator ShowStartDialogue()
    {
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_EXAMINE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_HINT, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    private void OnDrainCoverClicked()
    {
        if (drainCoverRemoved) return;
        
        Debug.Log("[Mirror2] Drain cover clicked - draining water");
        
        drainCoverRemoved = true;
        
        StartCoroutine(DrainWaterSequence());
    }
    
    IEnumerator DrainWaterSequence()
    {
        // Play drain open sound
        if (drainOpenSound != null)
        {
            AudioManager.Instance?.PlaySFX(drainOpenSound);
        }
        
        // Hide drain cover button
        if (drainCoverButton != null) 
        {
            drainCoverButton.gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Play water draining sound
        if (waterDrainSound != null)
        {
            AudioManager.Instance?.PlaySFX(waterDrainSound);
        }
        
        // Change bathtub sprite to empty (no water)
        if (bathtubImage != null && bathtubWithoutWater != null)
        {
            bathtubImage.sprite = bathtubWithoutWater;
            Debug.Log("[Mirror2] Bathtub sprite changed to empty");
        }
        
        yield return new WaitForSeconds(1f);
        
        // Show dialogue about finding torn notes
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_DRAIN_OPEN, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_HINT, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        Debug.Log("[Mirror2] Water drained! Now assemble the torn notes.");
    }

    // Called by DraggableItem when note piece is placed in slot
    public void OnPiecePlacedInSlot(GameObject slot, string pieceId)
    {
        Debug.Log($"[Mirror2] Note piece {pieceId} placed in slot {slot.name}");
        
        slotContents[slot] = pieceId;
        
        // Play sound
        if (paperRustleSound != null)
        {
            AudioManager.Instance?.PlaySFX(paperRustleSound);
        }
        
        // Check if puzzle solved
        CheckPuzzleSolution();
    }

    private void CheckPuzzleSolution()
    {
        Debug.Log("[Mirror2] Checking solution...");
        
        // Check if all slots filled
        int filledSlots = 0;
        foreach (var content in slotContents.Values)
        {
            if (!string.IsNullOrEmpty(content))
            {
                filledSlots++;
            }
        }
        
        Debug.Log($"[Mirror2] Filled slots: {filledSlots}/4");
        
        if (filledSlots < 4)
        {
            Debug.Log("[Mirror2] Not all slots filled yet");
            return;
        }
        
        // Check if correct sequence
        bool isCorrect = true;
        for (int i = 0; i < assemblySlots.Length; i++)
        {
            string expected = correctSequence[i];
            string actual = slotContents[assemblySlots[i].gameObject];
            
            Debug.Log($"[Mirror2] Slot {i}: Expected={expected}, Actual={actual}");
            
            if (actual != expected)
            {
                isCorrect = false;
            }
        }
        
        if (isCorrect)
        {
            Debug.Log("[Mirror2] ✅ PUZZLE SOLVED!");
            StartCoroutine(PuzzleSuccess());
        }
        else
        {
            Debug.Log("[Mirror2] ❌ Wrong order! Keep trying...");
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
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR2_SUCCESS_2, "Lisa");
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
        Room09_FlowController.Instance?.OnMirrorComplete(2);
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
        if (currentTime <= 10f)
        {
            timerText.color = Color.red;
        }
        else if (currentTime <= 20f)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }
}
