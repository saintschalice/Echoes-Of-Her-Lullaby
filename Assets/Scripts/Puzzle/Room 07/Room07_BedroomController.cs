using UnityEngine;

public class Room07_BedroomController : MonoBehaviour
{
    public static Room07_BedroomController Instance;

    [Header("Puzzle Status")]
    public bool isWindowTied = false;
    public bool isTeaSetPlaced = false;
    public bool isDollPlaced = false;
    public bool puzzleCompleted = false;

    [Header("Jumpscare Elements")]
    public GameObject mirrorTrigger;
    public AudioSource roomAudio;
    public AudioClip lullabyClip;

    void Awake() { Instance = this; }

    void Start()
    {
        // DISABLED: Now using Room07_FlowController instead
        // Mirror should be visible from the start
        // if (mirrorTrigger != null) mirrorTrigger.SetActive(false);
    }

    public void CheckPuzzleProgress()
    {
        if (!puzzleCompleted && isWindowTied && isTeaSetPlaced && isDollPlaced)
        {
            puzzleCompleted = true;
            TriggerCreepyPhase();
        }
    }

    void TriggerCreepyPhase()
    {
        Debug.Log("Puzzle Complete! Check the mirror...");
        if (roomAudio != null && lullabyClip != null) roomAudio.PlayOneShot(lullabyClip);
        // DISABLED: Mirror is always visible now
        // if (mirrorTrigger != null) mirrorTrigger.SetActive(true);
    }
}