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
    
    [Header("Ending Trigger")]
    public bool canTriggerEnding = false;
    
    [Header("Scene Transition")]
    public string mainMenuSceneName = "MainMenu";
    
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
        
        yield return new WaitForSeconds(1f);
        
        // Trigger ending cutscene automatically
        canTriggerEnding = true;
        StartCoroutine(EndingCutsceneSequence());
    }

    // ENDING CUTSCENE - Final revelation and game completion
    System.Collections.IEnumerator EndingCutsceneSequence()
    {
        yield return new WaitForSeconds(1f);
        
        // ENDING DIALOGUE SEQUENCE (20 lines)
        
        // 1-3: Final realization
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_3, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(1f);
        
        // 4-6: Understanding Emily
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_4, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_5, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_6, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(0.5f);
        
        // 7-9: Mother's plan revealed
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_7, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_8, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_9, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(1f);
        
        // 10-12: Emily's sacrifice
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_10, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_11, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_12, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(0.5f);
        
        // 13-15: Forgiveness
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_13, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_14, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_15, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(1f);
        
        // 16-18: Emily fades away
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_16, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        // Fade Emily completely
        if (emilyManifestation != null)
        {
            SpriteRenderer sr = emilyManifestation.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float elapsed = 0f;
                Color c = sr.color;
                while (elapsed < 3f)
                {
                    elapsed += Time.deltaTime;
                    c.a = Mathf.Lerp(0.2f, 0f, elapsed / 3f);
                    sr.color = c;
                    yield return null;
                }
                emilyManifestation.SetActive(false);
            }
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_17, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_18, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(2f);
        
        // 19-20: Final words
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_19, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        DialogueSystemV2.Instance?.StartDialogue(Room09_Dialogues.ENDING_20, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
        
        yield return new WaitForSeconds(2f);
        
        // Fade to black
        ScreenFader fader = ScreenFader.Instance;
        if (fader != null)
        {
            fader.FadeOut(2f);
            yield return new WaitForSeconds(2f);
        }
        
        // Save game completion
        SaveSystem.Instance?.MarkPuzzleSolved("game_complete");
        
        yield return new WaitForSeconds(1f);
        
        // Return to main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
