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
    public int totalEvidenceItems = 2; // Total evidence items in scene (torn dress + note)
    public int evidenceCollected = 0; // Number of evidence items collected
    public bool hasCollectedAllEvidence = false; // All evidence collected
    public bool hasFoundHammer = false; // Found hammer in cabinet
    public bool hasInteractedWithBathtub = false; // Interacted with bathtub
    
    [Header("Emily Hunt")]
    public bool isEmilyHunting = false;
    public GameObject emilyAI; // Emily AI that hunts player
    public Transform emilySpawnPoint; // Where Emily spawns
    public AudioClip emilyEnterSound; // Sound when Emily enters
    
    [Header("Mirror Progress")]
    public bool hasExaminedMirror = false;
    public bool hasBrokenMirror = false;
    public bool canClimbThrough = false;
    
    [Header("Mirror GameObject")]
    [Tooltip("The mirror GameObject in scene (will change sprite and become passage)")]
    public GameObject mirrorGameObject;
    
    [Tooltip("Normal mirror sprite (before breaking)")]
    public Sprite mirrorNormalSprite;
    
    [Tooltip("Broken mirror sprite (after puzzle - shows passage)")]
    public Sprite mirrorBrokenSprite;
    
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
        // Set normal mirror sprite at start
        if (mirrorGameObject != null && mirrorNormalSprite != null)
        {
            SpriteRenderer sr = mirrorGameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = mirrorNormalSprite;
            }
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

    // Called when an evidence item is collected
    public void OnEvidenceCollected(string evidenceId)
    {
        evidenceCollected++;
        
        Debug.Log($"[Room08] Evidence collected: {evidenceId} ({evidenceCollected}/{totalEvidenceItems})");
        
        // Check if all evidence collected
        if (evidenceCollected >= totalEvidenceItems)
        {
            hasCollectedAllEvidence = true;
            Debug.Log("[Room08] All evidence collected!");
            
            // Show dialogue
            StartCoroutine(ShowDialogueSequence(
                "I've collected all the evidence I can find.",
                "Now I should check the medicine cabinet."
            ));
        }
    }
    
    // Check if ready for mirror interaction
    public bool IsReadyForMirror()
    {
        return hasCollectedAllEvidence && hasFoundHammer && hasInteractedWithBathtub;
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
        
        // Change mirror sprite to broken (shows passage)
        if (mirrorGameObject != null && mirrorBrokenSprite != null)
        {
            SpriteRenderer sr = mirrorGameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = mirrorBrokenSprite;
                Debug.Log("[Room08] Mirror changed to broken sprite (passage visible)");
            }
        }
        
        StartCoroutine(MirrorBrokenSequence());
    }
    
    System.Collections.IEnumerator MirrorBrokenSequence()
    {
        // Show passage found dialogue
        DialogueSystemV2.Instance?.StartDialogue("The mirror... it's shattered!", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        DialogueSystemV2.Instance?.StartDialogue("There's a passage behind it. I can climb through.", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
    }

    // Called when player interacts with broken mirror (passage)
    public void ClimbThroughPassage()
    {
        if (!canClimbThrough) return;
        
        StartCoroutine(TransitionToNextRoom());
    }
    
    System.Collections.IEnumerator TransitionToNextRoom()
    {
        // Show climb through dialogue
        DialogueSystemV2.Instance?.StartDialogue("Time to see what's on the other side...", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Fade out
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(0.8f);
            yield return new WaitForSeconds(0.8f);
        }
        
        // Load next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
