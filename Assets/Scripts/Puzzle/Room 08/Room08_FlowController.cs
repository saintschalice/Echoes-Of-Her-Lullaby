using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Main flow controller for Room 08 - Lisa's Bathroom
/// Manages progression through the bathroom sequence
/// </summary>
public class Room08_FlowController : MonoBehaviour
{
    public static Room08_FlowController Instance;

    [Header("Story Milestones")]
    public bool isIntroDone = false;
    
    [Header("Environmental Checks")]
    public bool hasCheckedBathtub = false;
    public bool hasCheckedMedicine = false;
    
    [Header("Evidence Collection")]
    public bool hasFoundTornClothes = false;
    public bool hasFoundApologyNote = false;
    public bool hasFoundHammer = false;
    
    [Header("Emily Hunt")]
    public bool isEmilyHunting = false;
    public GameObject emilyAI; // Emily AI that hunts player
    public Transform emilySpawnPoint; // Where Emily spawns
    public AudioClip emilyEnterSound; // Sound when Emily enters
    
    [Header("Mirror Progress")]
    public bool hasExaminedMirror = false;
    public bool hasBrokenMirror = false;
    public bool canClimbThrough = false;
    
    [Header("Mirror Sprites")]
    public SpriteRenderer mirrorSpriteRenderer; // The mirror object in scene
    public Sprite mirrorNormalSprite; // Normal mirror before breaking
    public Sprite mirrorBrokenSprite; // Broken mirror after QTE
    public GameObject passageObject; // The passage behind mirror (initially hidden)
    
    [Header("Emily AI (Outside)")]
    public AudioClip emilyHummingSound;
    public AudioSource emilyAudioSource;
    
    [Header("Door")]
    public GameObject bathroomDoor;
    public bool isDoorLocked = true;
    
    [Header("Scene Transition")]
    public string nextSceneName = "Room09_Master's_Bathroom";

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Hide Emily AI initially
        if (emilyAI != null) emilyAI.SetActive(false);
        
        // Hide passage initially
        if (passageObject != null) passageObject.SetActive(false);
        
        // Set normal mirror sprite
        if (mirrorSpriteRenderer != null && mirrorNormalSprite != null)
        {
            mirrorSpriteRenderer.sprite = mirrorNormalSprite;
        }
        
        // Play Emily humming sound (looping, ambient) - she's outside
        if (emilyHummingSound != null && emilyAudioSource != null)
        {
            emilyAudioSource.clip = emilyHummingSound;
            emilyAudioSource.loop = true;
            emilyAudioSource.Play();
        }
        
        // Trigger intro dialogue
        Invoke(nameof(PlayIntro), 1f);
    }

    private void PlayIntro()
    {
        StartCoroutine(PlayIntroSequence());
    }
    
    System.Collections.IEnumerator PlayIntroSequence()
    {
        // Disable player movement during intro
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Part 1
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.ENTRY_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Part 2
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.ENTRY_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Part 3 - Door locked
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.DOOR_LOCKED, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Part 4 - Emily outside
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.EMILY_OUTSIDE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player movement
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        isIntroDone = true;
    }

    // Check if all evidence has been found
    public bool IsAllEvidenceFound()
    {
        return hasFoundTornClothes && hasFoundApologyNote && hasFoundHammer;
    }

    // Check if ready for mirror QTE
    public bool IsReadyForMirror()
    {
        return IsAllEvidenceFound() && isEmilyHunting;
    }
    
    // Called when all evidence is collected - Emily enters and starts hunting
    public void OnAllEvidenceCollected()
    {
        if (isEmilyHunting) return; // Already hunting
        
        StartCoroutine(EmilyEntersRoom());
    }
    
    System.Collections.IEnumerator EmilyEntersRoom()
    {
        // Stop humming sound
        if (emilyAudioSource != null && emilyAudioSource.isPlaying)
        {
            emilyAudioSource.Stop();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Play Emily enter sound
        if (emilyEnterSound != null && emilyAudioSource != null)
        {
            emilyAudioSource.PlayOneShot(emilyEnterSound);
        }
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.EMILY_ENTERS, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Spawn Emily and start hunting
        if (emilyAI != null)
        {
            if (emilySpawnPoint != null)
            {
                emilyAI.transform.position = emilySpawnPoint.position;
            }
            emilyAI.SetActive(true);
            
            // Enable Emily AI
            EmilyGhost emilyScript = emilyAI.GetComponent<EmilyGhost>();
            if (emilyScript != null)
            {
                emilyScript.enabled = true;
            }
        }
        
        isEmilyHunting = true;
        
        // Show hunting dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.EMILY_HUNTING, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }
    
    // Helper method for dialogue sequences
    System.Collections.IEnumerator ShowDialogueSequence(params string[] dialogues)
    {
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        bool wasPlayerEnabled = player != null && player.enabled;
        bool wasJoystickActive = joystick != null && joystick.activeSelf;
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        foreach (string dialogue in dialogues)
        {
            DialogueSystemV2.Instance?.StartDialogue(dialogue, "Lisa");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        if (player != null && wasPlayerEnabled) player.enabled = true;
        if (joystick != null && wasJoystickActive) joystick.SetActive(true);
    }

    // Called when mirror QTE is completed successfully
    public void OnMirrorBroken()
    {
        hasBrokenMirror = true;
        canClimbThrough = true;
        
        // Stop Emily hunting
        if (emilyAI != null)
        {
            emilyAI.SetActive(false);
        }
        
        isEmilyHunting = false;
        
        // Change mirror sprite to broken
        if (mirrorSpriteRenderer != null && mirrorBrokenSprite != null)
        {
            mirrorSpriteRenderer.sprite = mirrorBrokenSprite;
        }
        
        // Show passage
        if (passageObject != null)
        {
            passageObject.SetActive(true);
        }
        
        StartCoroutine(MirrorBrokenSequence());
    }
    
    System.Collections.IEnumerator MirrorBrokenSequence()
    {
        // Show passage found dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.PASSAGE_FOUND_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.PASSAGE_FOUND_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Show final door dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.FINAL_DOOR_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.FINAL_DOOR_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    // Called when player interacts with passage to climb through
    public void ClimbThroughPassage()
    {
        if (!canClimbThrough) return;
        
        StartCoroutine(TransitionToNextRoom());
    }
    
    System.Collections.IEnumerator TransitionToNextRoom()
    {
        // Show climb through dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.CLIMB_THROUGH, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Fade out and load next scene
        // TODO: Add fade effect
        
        // Mark puzzle as solved
        SaveSystem.Instance?.MarkPuzzleSolved("bathroom_mirror_qte");
        
        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
