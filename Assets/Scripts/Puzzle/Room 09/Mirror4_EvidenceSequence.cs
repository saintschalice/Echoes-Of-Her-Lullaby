using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Mirror 4: Evidence Sequence Puzzle
/// Player arranges 4 evidence items in correct order: Rope → Pills → Knife → Towel
/// Each correct placement shows a flashback
/// Time limit: 60 seconds
/// </summary>
public class Mirror4_EvidenceSequence : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public TextMeshProUGUI timerText;
    public GameObject[] pictureFrames; // 4 frames numbered 1-4
    public GameObject[] evidenceItems; // 4 items: rope, pills, knife, towel
    public Image flashbackImage; // Shows flashback when item placed correctly
    
    [Header("Flashback Sprites")]
    public Sprite flashback_Rope; // Mother buying rope
    public Sprite flashback_Pills; // Mother crushing pills
    public Sprite flashback_Knife; // Mother sharpening knife
    public Sprite flashback_Towel; // Mother preparing cleanup
    
    [Header("Puzzle Settings")]
    public float timeLimit = 60f;
    public float flashbackDuration = 2f;
    
    [Header("Audio")]
    public AudioClip itemPlaceSound;
    public AudioClip flashbackSound;
    public AudioClip successSound;
    public AudioClip failSound;
    
    private float currentTime;
    private bool isPuzzleActive = false;
    private bool isPuzzleSolved = false;
    
    // Correct sequence: rope → pills → knife → towel
    private string[] correctSequence = { "rope", "pills", "knife", "towel" };
    private Dictionary<GameObject, string> frameContents = new Dictionary<GameObject, string>();
    private bool isShowingFlashback = false;

    private void Start()
    {
        // Hide panel at start
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        
        // Hide flashback image
        if (flashbackImage != null) flashbackImage.gameObject.SetActive(false);
        
        // Initialize frame contents
        foreach (GameObject frame in pictureFrames)
        {
            frameContents[frame] = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPuzzleSolved)
        {
            // Show interaction prompt (optional - can use UI text)
            // InteractionPromptHelper.Instance?.ShowPrompt("Press E to examine large mirror");
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
        if (isPuzzleActive && !isPuzzleSolved && !isShowingFlashback)
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
        
        // Randomize evidence item positions
        foreach (GameObject item in evidenceItems)
        {
            if (item != null)
            {
                RectTransform rt = item.GetComponent<RectTransform>();
                if (rt != null)
                {
                    float randomX = Random.Range(-250f, 250f);
                    float randomY = Random.Range(-150f, -300f);
                    rt.anchoredPosition = new Vector2(randomX, randomY);
                }
            }
        }
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_EXAMINE, "Lisa");
    }

    // Called by draggable items when placed in frame
    public void OnItemPlacedInFrame(GameObject frame, string itemId)
    {
        // Get frame index
        int frameIndex = System.Array.IndexOf(pictureFrames, frame);
        if (frameIndex < 0) return;
        
        // Check if correct item for this frame
        string correctItem = correctSequence[frameIndex];
        
        if (itemId == correctItem)
        {
            // Correct placement!
            frameContents[frame] = itemId;
            
            // Play sound
            if (itemPlaceSound != null)
            {
                AudioSource.PlayClipAtPoint(itemPlaceSound, Camera.main.transform.position, 0.7f);
            }
            
            // Show flashback
            StartCoroutine(ShowFlashback(itemId));
        }
        else
        {
            // Wrong placement - item returns to original position
            // (handled by drag system)
        }
        
        // Check if puzzle solved
        CheckPuzzleSolution();
    }

    private System.Collections.IEnumerator ShowFlashback(string itemId)
    {
        isShowingFlashback = true;
        
        // Get flashback sprite
        Sprite flashbackSprite = null;
        switch (itemId)
        {
            case "rope":
                flashbackSprite = flashback_Rope;
                break;
            case "pills":
                flashbackSprite = flashback_Pills;
                break;
            case "knife":
                flashbackSprite = flashback_Knife;
                break;
            case "towel":
                flashbackSprite = flashback_Towel;
                break;
        }
        
        if (flashbackSprite != null && flashbackImage != null)
        {
            // Show flashback
            flashbackImage.sprite = flashbackSprite;
            flashbackImage.gameObject.SetActive(true);
            
            // Play sound
            if (flashbackSound != null)
            {
                AudioSource.PlayClipAtPoint(flashbackSound, Camera.main.transform.position, 0.5f);
            }
            
            // Fade in
            Color c = flashbackImage.color;
            c.a = 0f;
            flashbackImage.color = c;
            
            float elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, elapsed / 0.5f);
                flashbackImage.color = c;
                yield return null;
            }
            
            // Hold
            yield return new WaitForSeconds(flashbackDuration);
            
            // Fade out
            elapsed = 0f;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
                flashbackImage.color = c;
                yield return null;
            }
            
            flashbackImage.gameObject.SetActive(false);
        }
        
        isShowingFlashback = false;
    }

    private void CheckPuzzleSolution()
    {
        // Check if all frames filled
        foreach (var content in frameContents.Values)
        {
            if (string.IsNullOrEmpty(content)) return;
        }
        
        // Check if correct sequence
        bool isCorrect = true;
        for (int i = 0; i < pictureFrames.Length; i++)
        {
            if (frameContents[pictureFrames[i]] != correctSequence[i])
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
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_SUCCESS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MIRROR4_SUCCESS_2, "Lisa");
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
        Room09_FlowController.Instance?.OnMirrorComplete(4);
        
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
