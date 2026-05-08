using UnityEngine;

public class Room07_FlowController : MonoBehaviour
{
    public static Room07_FlowController Instance;

    [Header("Story Milestones")]
    public bool isIntroDone = false;
    
    [Header("Environmental Checks")]
    public bool hasCheckedBed = false;
    public bool hasCheckedWall = false;
    public bool hasCheckedDiary = false;
    public bool hasCheckedChair = false;
    public bool hasCheckedCloset = false;
    public bool hasCheckedReadingTable = false;
    
    [Header("Puzzle Progress")]
    public bool areCurtainsOpened = false;
    public bool hasEmilyCup = false;
    public bool isTeaPartyDone = false;
    public bool isToyboxSolved = false;
    public bool hasEmilyDoll = false;
    public bool isDollhouseDone = false;
    public bool hasInteractedWithMirror = false; // NEW: Track mirror interaction

    [Header("Climax & Chase Sequences")]
    public GameObject emilyAI;           // Ang kalaban
    public GameObject bedroomDoorCollider; // Para i-lock ang pinto
    public AudioSource toyboxMusicBox;
    public AudioClip lullabyFragment3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // I-hide si Emily sa umpisa
        if (emilyAI != null) emilyAI.SetActive(false);

        // Trigger Intro Dialogue pagpasok
        Invoke(nameof(PlayIntro), 1f);
    }

    private void PlayIntro()
    {
        StartCoroutine(PlayIntroSequence());
    }
    
    System.Collections.IEnumerator PlayIntroSequence()
    {
        // Part 1 (now 2 parts instead of 3)
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.INTRO_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Part 2
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.INTRO_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        isIntroDone = true;
    }

    // Check if everything has been completed before mirror interaction
    public bool IsEverythingComplete()
    {
        // All environmental checks
        bool allEnvironmentalChecked = hasCheckedBed && hasCheckedWall && hasCheckedDiary &&
                                       hasCheckedChair && hasCheckedCloset && hasCheckedReadingTable;
        
        // All puzzles completed
        bool allPuzzlesComplete = areCurtainsOpened && isTeaPartyDone && 
                                 isToyboxSolved && isDollhouseDone;
        
        return allEnvironmentalChecked && allPuzzlesComplete;
    }

    // Tatawagin ito ng salamin (Mirror)
    public void CheckFinalCondition()
    {
        if (IsEverythingComplete())
        {
            TriggerClimax();
        }
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("I feel like I'm still missing something in here...", "Lisa");
        }
    }

    private void TriggerClimax()
    {
        Debug.Log("Climax Triggered! Emily appears.");

        // 1. Play Music Box
        if (toyboxMusicBox != null && lullabyFragment3 != null)
        {
            toyboxMusicBox.PlayOneShot(lullabyFragment3);
        }

        // 2. Lock the door
        if (bedroomDoorCollider != null)
        {
            bedroomDoorCollider.tag = "Untagged"; // O palitan ang logic mo ng locked door
        }

        // 3. Spawn Emily and Start Chase
        if (emilyAI != null)
        {
            emilyAI.SetActive(true);
            // I-activate mo rito ang NavMeshAgent/Chase script ni Emily
        }
    }
}