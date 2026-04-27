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
        // Nakatago ang salamin hangga't hindi tapos ang puzzle
        if (mirrorTrigger != null) mirrorTrigger.SetActive(false);
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
        if (mirrorTrigger != null) mirrorTrigger.SetActive(true);
    }
}