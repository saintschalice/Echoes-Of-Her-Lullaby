using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 3: Vanity Terror Puzzle
/// Player arranges 8 diary page fragments in chronological order
/// Time limit: 90 seconds (longer because 8 pages)
/// </summary>
public class Mirror3_VanityTerror : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public GameObject[] numberedSlots; // 8 slots numbered 1-8
    public GameObject[] diaryPages; // 8 diary pages
    
    [Header("Puzzle Settings")]
    public float timeLimit = 90f;
    
    [Header("Audio")]
    public AudioClip paperRustleSound;
    public AudioClip successSound;
    public AudioClip failSound;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    
    // Correct sequence: page1 → page2 → page3 → ... → page8
    private string[] correctSequence = { "page1", "page2", "page3", "page4", "page5", "page6", "page7", "page8" };
    private Dictionary<GameObject, string> slotContents = new Dictionary<GameObject, string>();

    private void Start()
    {
        // Hide panel at start
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Initialize slot contents
        foreach (GameObject slot in numberedSlots)
        {
            slotContents[slot] = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPuzzleSolved)
        {
            // Show interaction prompt (optional - can use UI text)
            // InteractionPromptHelper.Instance?.ShowPrompt("Press E to examine vanity mirror");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide interaction prompt
            // InteractionPromptHelper.Instance?.HidePrompt();
        }
    }

    private void Update()
    {
        // Update timer if puzzle active
        if (isPuzzleActive && !isPuzzleSolved)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            
            if (currentTime <= 0)
            {
                OnPuzzleFailed();
            }
        }
    }

    public void StartPuzzle()
    {
        if (isPuzzleActive || isPuzzleSolved) return;
        
        isPuzzleActive = true;
        currentTime = timeLimit;
        
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Disable player movement
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = false;
        
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
        
        // Randomize diary page positions
        foreach (GameObject page in diaryPages)
        {
            if (page != null)
            {
                RectTransform rt = page.GetComponent<RectTransform>();
                if (rt != null)
                {
                    // Random position within panel bounds
                    float randomX = Random.Range(-300f, 300f);
                    float randomY = Random.Range(-200f, 200f);
                    rt.anchoredPosition = new Vector2(randomX, randomY);
                }
            }
        }
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_EXAMINE, "Lisa");
    }

    // Called by draggable items when placed in slot
    public void OnPagePlacedInSlot(GameObject slot, string pageId)
    {
        slotContents[slot] = pageId;
        
        // Play sound
        if (paperRustleSound != null)
        {
            AudioSource.PlayClipAtPoint(paperRustleSound, Camera.main.transform.position, 0.5f);
        }
        
        // Check if puzzle solved
        CheckPuzzleSolution();
    }

    private void CheckPuzzleSolution()
    {
        // Check if all slots filled
        foreach (var content in slotContents.Values)
        {
            if (string.IsNullOrEmpty(content)) return;
        }
        
        // Check if correct sequence
        bool isCorrect = true;
        for (int i = 0; i < numberedSlots.Length; i++)
        {
            if (slotContents[numberedSlots[i]] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }
        
        if (isCorrect)
        {
            OnPuzzleSolved();
        }
    }

    private void OnPuzzleSolved()
    {
        isPuzzleSolved = true;
        isPuzzleActive = false;
        
        // Play success sound
        if (successSound != null)
        {
            AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position);
        }
        
        StartCoroutine(PuzzleSolvedSequence());
    }

    private System.Collections.IEnumerator PuzzleSolvedSequence()
    {
        // Show success dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR3_SUCCESS_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Close panel
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Re-enable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = true;
        
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
        
        // Notify flow controller
        Room09_FlowController.Instance?.OnMirrorComplete(3);
        
        // Visual feedback (glow effect)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.green;
        }
    }

    private void OnPuzzleFailed()
    {
        isPuzzleActive = false;
        
        // Play fail sound
        if (failSound != null)
        {
            AudioSource.PlayClipAtPoint(failSound, Camera.main.transform.position);
        }
        
        StartCoroutine(PuzzleFailedSequence());
    }

    private System.Collections.IEnumerator PuzzleFailedSequence()
    {
        // Show Emily jumpscare
        GameObject jumpscarePanel = GameObject.Find("Emily_Jumpscare_Panel");
        if (jumpscarePanel != null)
        {
            jumpscarePanel.SetActive(true);
            
            // Play Emily scream
            AudioSource emilyAudio = jumpscarePanel.GetComponent<AudioSource>();
            if (emilyAudio != null) emilyAudio.Play();
        }
        
        // Show failure dialogue
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
        
        yield return new WaitForSeconds(2f);
        
        // Reload scene (game over)
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
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
