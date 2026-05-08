using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    
    [Header("Bottle Prefabs")]
    public GameObject bottlePrefab;
    
    [Header("Bottle Data")]
    public BottleData[] bottles = new BottleData[6];
    
    [Header("Success")]
    public GameObject successEffect;
    public AudioClip successSound;
    
    [Header("Failure (Emily Attack)")]
    public GameObject emilyJumpscarePanel;
    public AudioClip emilyScreamSound;
    public float timeLimit = 60f; // 60 seconds before Emily attacks
    
    private List<BottleSlot> spawnedBottles = new List<BottleSlot>();
    private float timeRemaining;
    private bool puzzleActive = false;
    private bool puzzleComplete = false;

    [System.Serializable]
    public class BottleData
    {
        public string label; // e.g., "Valium 1975"
        public int year; // e.g., 1975
        public Sprite bottleSprite;
        public int correctSlotIndex; // 0-5, where this bottle should go
    }

    public void StartPuzzle()
    {
        if (puzzleComplete) return;
        
        // Show panel
        if (puzzlePanel != null) puzzlePanel.SetActive(true);
        
        // Spawn bottles randomly
        SpawnBottles();
        
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

    void SpawnBottles()
    {
        // Clear existing
        foreach (var bottle in spawnedBottles)
        {
            if (bottle != null) Destroy(bottle.gameObject);
        }
        spawnedBottles.Clear();
        
        // Shuffle bottle order
        List<BottleData> shuffled = new List<BottleData>(bottles);
        for (int i = 0; i < shuffled.Count; i++)
        {
            BottleData temp = shuffled[i];
            int randomIndex = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }
        
        // Spawn bottles in random positions
        for (int i = 0; i < shuffled.Count && i < bottleSlots.Length; i++)
        {
            GameObject bottleObj = Instantiate(bottlePrefab, bottleSlots[i]);
            BottleSlot slot = bottleObj.GetComponent<BottleSlot>();
            if (slot != null)
            {
                slot.Initialize(shuffled[i], i, this);
                spawnedBottles.Add(slot);
            }
        }
    }

    void Update()
    {
        if (!puzzleActive || puzzleComplete) return;
        
        // Update timer
        timeRemaining -= Time.unscaledDeltaTime;
        
        // Check timeout
        if (timeRemaining <= 0)
        {
            StartCoroutine(EmilyAttack());
        }
    }

    public void OnBottlePlaced(int slotIndex, BottleData bottle)
    {
        // Check if all bottles are placed
        CheckSolution();
    }

    void CheckSolution()
    {
        // Check if all bottles are in correct positions
        bool allCorrect = true;
        
        for (int i = 0; i < spawnedBottles.Count; i++)
        {
            BottleSlot slot = spawnedBottles[i];
            if (slot.currentSlotIndex != slot.bottleData.correctSlotIndex)
            {
                allCorrect = false;
                break;
            }
        }
        
        if (allCorrect)
        {
            StartCoroutine(PuzzleSuccess());
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
/// Attach to each bottle prefab
/// </summary>
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
