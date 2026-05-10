using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Main flow controller for Room 09 - Master Bedroom's Bathroom
/// Manages 4 mirror puzzles and Emily's manifestation
/// </summary>
public class Room09_FlowController : MonoBehaviour
{
    public static Room09_FlowController Instance;

    [Header("Story Milestones")]
    public bool isIntroDone = false;
    public bool isDoorLocked = true;
    
    [Header("Mirror Puzzle Progress")]
    public bool mirror1Complete = false; // Medicine Cabinet
    public bool mirror2Complete = false; // Bathtub Drain
    public bool mirror3Complete = false; // Vanity Terror
    public bool mirror4Complete = false; // Evidence Sequence
    
    [Header("Emily State")]
    public GameObject emilyManifestation; // Full power Emily
    public bool emilyHasCollapsed = false;
    
    [Header("Master Bedroom Door")]
    public GameObject masterBedroomDoor;
    public bool canEnterMasterBedroom = false;
    
    [Header("Scene Transition")]
    public string nextSceneName = "Room10_MasterBedroom";
    
    [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioClip tenseMusicClip;
    public AudioClip emilyScreamClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Show Emily at full power
        if (emilyManifestation != null) emilyManifestation.SetActive(true);
        
        // Play tense music
        if (ambientAudio != null && tenseMusicClip != null)
        {
            ambientAudio.clip = tenseMusicClip;
            ambientAudio.loop = true;
            ambientAudio.Play();
        }
        
        // Trigger intro sequence
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
        
        // Entry dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENTRY_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENTRY_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Door slams
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.DOOR_SLAMS, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.TRAPPED, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily manifestation
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_APPEARS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_APPEARS_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_WARNING, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player movement
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        isIntroDone = true;
    }

    // Check if all mirrors are complete
    public bool AreAllMirrorsComplete()
    {
        return mirror1Complete && mirror2Complete && mirror3Complete && mirror4Complete;
    }

    // Called when a mirror puzzle is completed
    public void OnMirrorComplete(int mirrorNumber)
    {
        switch (mirrorNumber)
        {
            case 1:
                mirror1Complete = true;
                break;
            case 2:
                mirror2Complete = true;
                break;
            case 3:
                mirror3Complete = true;
                break;
            case 4:
                mirror4Complete = true;
                break;
        }
        
        // Check if all mirrors complete
        if (AreAllMirrorsComplete())
        {
            StartCoroutine(AllMirrorsCompleteSequence());
        }
    }

    System.Collections.IEnumerator AllMirrorsCompleteSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        
        // All mirrors complete
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ALL_MIRRORS_COMPLETE, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Mother's voice
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.MOTHER_VOICE, "Mother");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily's breakdown
        StartCoroutine(EmilyBreakdownSequence());
    }

    System.Collections.IEnumerator EmilyBreakdownSequence()
    {
        // Emily breakdown dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_BREAKDOWN_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_BREAKDOWN_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_BREAKDOWN_3, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily collapses (fade out or animation)
        if (emilyManifestation != null)
        {
            // Fade out Emily
            SpriteRenderer sr = emilyManifestation.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float elapsed = 0f;
                Color c = sr.color;
                while (elapsed < 2f)
                {
                    elapsed += Time.deltaTime;
                    c.a = Mathf.Lerp(1f, 0.2f, elapsed / 2f);
                    sr.color = c;
                    yield return null;
                }
            }
        }
        
        emilyHasCollapsed = true;
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily's final words
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_WHISPER_1, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.EMILY_WHISPER_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Door unlocks
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.DOOR_UNLOCKS, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Unlock door
        isDoorLocked = false;
        canEnterMasterBedroom = true;
        
        // Re-enable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
    }

    // Called when player interacts with master bedroom door
    public void EnterMasterBedroom()
    {
        if (!canEnterMasterBedroom)
        {
            DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.NEED_SOLVE_MIRRORS, "Lisa");
            return;
        }
        
        StartCoroutine(TransitionToMasterBedroom());
    }

    System.Collections.IEnumerator TransitionToMasterBedroom()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Final approach dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.APPROACH_DOOR_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.APPROACH_DOOR_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.APPROACH_DOOR_3, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Save progress
        SaveSystem.Instance?.MarkPuzzleSolved("room09_all_mirrors");
        
        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
