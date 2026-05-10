using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Main flow controller for Room 10 - Master Bedroom
/// The final revelation room where all truths are revealed
/// </summary>
public class Room10_FlowController : MonoBehaviour
{
    public static Room10_FlowController Instance;

    [Header("Story Milestones")]
    public bool isIntroDone = false;
    public bool hasExaminedRoom = false;
    public bool hasFoundLullaby = false;
    public bool hasApproachedMirror = false;
    public bool hasSeenFlashback = false;
    public bool hasForgiven = false;
    
    [Header("Emily State")]
    public GameObject emilyManifestation; // Solid Emily blocking mirror
    public bool emilyHasFaded = false;
    
    [Header("Mirror")]
    public GameObject truthMirror;
    public GameObject mirrorGlowEffect;
    public bool canAccessMirror = false;
    
    [Header("Flashback")]
    public GameObject flashbackPanel; // Full-screen flashback sequence
    public FlashbackImage[] flashbackImages; // Sequence of images
    
    [Header("Music Box")]
    public GameObject musicBox;
    public AudioClip lullabyClip;
    
    [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioClip tenseMusicClip;
    public AudioClip peacefulMusicClip;
    
    [Header("Scene Transition")]
    public string nextSceneName = "EndingScene"; // Or main menu

    [System.Serializable]
    public class FlashbackImage
    {
        public Sprite image;
        public string dialogue;
        public float displayDuration = 3f;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Show Emily blocking mirror
        if (emilyManifestation != null) emilyManifestation.SetActive(true);
        
        // Hide flashback panel
        if (flashbackPanel != null) flashbackPanel.SetActive(false);
        
        // Play tense music
        if (ambientAudio != null && tenseMusicClip != null)
        {
            ambientAudio.clip = tenseMusicClip;
            ambientAudio.loop = true;
            ambientAudio.Play();
        }
        
        // Trigger intro
        Invoke(nameof(PlayIntro), 1f);
    }

    private void PlayIntro()
    {
        StartCoroutine(PlayIntroSequence());
    }
    
    System.Collections.IEnumerator PlayIntroSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Entry dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.ENTRY_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.ENTRY_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Mirror magnetism
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.MIRROR_CALL_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.MIRROR_CALL_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily blocks
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_BLOCKS_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_BLOCKS_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_BLOCKS_3, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        isIntroDone = true;
    }

    // Called when player examines room evidence
    public void OnRoomExamined()
    {
        hasExaminedRoom = true;
        CheckProgression();
    }

    // Called when player finds music box
    public void OnLullabyFound()
    {
        hasFoundLullaby = true;
        
        // Play lullaby
        if (lullabyClip != null && ambientAudio != null)
        {
            ambientAudio.Stop();
            ambientAudio.clip = lullabyClip;
            ambientAudio.loop = true;
            ambientAudio.Play();
        }
        
        CheckProgression();
    }

    // Check if player can approach mirror
    void CheckProgression()
    {
        // Must examine room and find lullaby
        if (hasExaminedRoom && hasFoundLullaby && !canAccessMirror)
        {
            StartCoroutine(UnlockMirrorAccess());
        }
    }

    System.Collections.IEnumerator UnlockMirrorAccess()
    {
        canAccessMirror = true;
        
        // Reality distortion
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.REALITY_WARP_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.REALITY_WARP_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily's breakdown
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_TRUTH_1, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_TRUTH_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_TRUTH_3, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Mirror glow effect
        if (mirrorGlowEffect != null)
        {
            mirrorGlowEffect.SetActive(true);
        }
    }

    // Called when player interacts with mirror
    public void ApproachMirror()
    {
        if (!canAccessMirror)
        {
            DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.HINT_EXAMINE_ROOM, "Lisa");
            return;
        }
        
        if (hasApproachedMirror) return;
        
        hasApproachedMirror = true;
        StartCoroutine(MirrorApproachSequence());
    }

    System.Collections.IEnumerator MirrorApproachSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Approach dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.APPROACH_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.APPROACH_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily desperate
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_DESPERATE_1, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_DESPERATE_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily accepts
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_ACCEPTS_1, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_ACCEPTS_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_ACCEPTS_3, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Mirror activates
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.MIRROR_ACTIVATES, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.FLASHBACK_BEGINS, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Start flashback
        yield return StartCoroutine(PlayFlashbackSequence());
        
        // After flashback
        yield return StartCoroutine(FinalUnderstandingSequence());
    }

    System.Collections.IEnumerator PlayFlashbackSequence()
    {
        hasSeenFlashback = true;
        
        // Show flashback panel
        if (flashbackPanel != null)
        {
            flashbackPanel.SetActive(true);
        }
        
        // Play flashback dialogues with images
        string[] flashbackDialogues = new string[]
        {
            Room10_Dialogues.FLASHBACK_1,
            Room10_Dialogues.FLASHBACK_2,
            Room10_Dialogues.FLASHBACK_3,
            Room10_Dialogues.FLASHBACK_4,
            Room10_Dialogues.FLASHBACK_5,
            Room10_Dialogues.FLASHBACK_6,
            Room10_Dialogues.FLASHBACK_7,
            Room10_Dialogues.FLASHBACK_8,
            Room10_Dialogues.FLASHBACK_9
        };
        
        for (int i = 0; i < flashbackDialogues.Length; i++)
        {
            // Show image if available
            if (flashbackImages != null && i < flashbackImages.Length)
            {
                // TODO: Display flashback image
            }
            
            // Show dialogue
            DialogueSystemV2.Instance?.StartDialogue(flashbackDialogues[i], "Lisa");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
        // Hide flashback panel
        if (flashbackPanel != null)
        {
            flashbackPanel.SetActive(false);
        }
    }

    System.Collections.IEnumerator FinalUnderstandingSequence()
    {
        // After vision
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.AFTER_VISION_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.AFTER_VISION_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Understanding
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.UNDERSTANDING_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.UNDERSTANDING_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily explains (5 parts)
        string[] emilyExplanations = new string[]
        {
            Room10_Dialogues.EMILY_EXPLAINS_1,
            Room10_Dialogues.EMILY_EXPLAINS_2,
            Room10_Dialogues.EMILY_EXPLAINS_3,
            Room10_Dialogues.EMILY_EXPLAINS_4,
            Room10_Dialogues.EMILY_EXPLAINS_5
        };
        
        foreach (string explanation in emilyExplanations)
        {
            DialogueSystemV2.Instance?.StartDialogue(explanation, "Emily");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        // Lisa's response (4 parts)
        string[] lisaResponses = new string[]
        {
            Room10_Dialogues.LISA_RESPONSE_1,
            Room10_Dialogues.LISA_RESPONSE_2,
            Room10_Dialogues.LISA_RESPONSE_3,
            Room10_Dialogues.LISA_RESPONSE_4
        };
        
        foreach (string response in lisaResponses)
        {
            DialogueSystemV2.Instance?.StartDialogue(response, "Lisa");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily's apology (4 parts)
        string[] emilyApologies = new string[]
        {
            Room10_Dialogues.EMILY_APOLOGY_1,
            Room10_Dialogues.EMILY_APOLOGY_2,
            Room10_Dialogues.EMILY_APOLOGY_3,
            Room10_Dialogues.EMILY_APOLOGY_4
        };
        
        foreach (string apology in emilyApologies)
        {
            DialogueSystemV2.Instance?.StartDialogue(apology, "Emily");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        // Forgiveness
        yield return StartCoroutine(ForgivenessSequence());
    }

    System.Collections.IEnumerator ForgivenessSequence()
    {
        hasForgiven = true;
        
        // Lisa forgives (3 parts)
        string[] forgiveness = new string[]
        {
            Room10_Dialogues.LISA_FORGIVES_1,
            Room10_Dialogues.LISA_FORGIVES_2,
            Room10_Dialogues.LISA_FORGIVES_3
        };
        
        foreach (string line in forgiveness)
        {
            DialogueSystemV2.Instance?.StartDialogue(line, "Lisa");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Emily's relief
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_RELIEF_1, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_RELIEF_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // Emily fades
        yield return StartCoroutine(EmilyDepartureSequence());
    }

    System.Collections.IEnumerator EmilyDepartureSequence()
    {
        // Change music to peaceful
        if (ambientAudio != null && peacefulMusicClip != null)
        {
            ambientAudio.Stop();
            ambientAudio.clip = peacefulMusicClip;
            ambientAudio.loop = true;
            ambientAudio.Play();
        }
        
        // Emily fades dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_FADES_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_FADES_2, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EMILY_FADES_3, "Emily");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Fade out Emily
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
                    c.a = Mathf.Lerp(1f, 0f, elapsed / 3f);
                    sr.color = c;
                    yield return null;
                }
            }
            emilyManifestation.SetActive(false);
        }
        
        emilyHasFaded = true;
        
        yield return new WaitForSeconds(1f);
        
        // Final goodbye
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.FINAL_GOODBYE_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.FINAL_GOODBYE_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
        
        // Epilogue
        yield return StartCoroutine(EpilogueSequence());
    }

    System.Collections.IEnumerator EpilogueSequence()
    {
        // Epilogue dialogue
        string[] epilogue = new string[]
        {
            Room10_Dialogues.EPILOGUE_1,
            Room10_Dialogues.EPILOGUE_2,
            Room10_Dialogues.EPILOGUE_3
        };
        
        foreach (string line in epilogue)
        {
            DialogueSystemV2.Instance?.StartDialogue(line, "Lisa");
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
        
        yield return new WaitForSeconds(2f);
        
        // Fade to black and end
        // TODO: Add fade effect
        
        yield return new WaitForSeconds(2f);
        
        // Save completion
        SaveSystem.Instance?.MarkPuzzleSolved("game_complete");
        
        // Load ending scene or credits
        SceneManager.LoadScene(nextSceneName);
    }
}
