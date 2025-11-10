using UnityEngine;
using System.Collections;

/// <summary>
/// Manages Emily's scripted sequence events in the hallway scene
/// Integrates with SaveSystem and DialogueSystem
/// </summary>
public class EmilySceneSequence : MonoBehaviour
{
    [Header("Sequence Settings")]
    public string sequenceId = "emily_hallway_sequence";

    [Header("Positions")]
    public Transform staircaseBlockPosition;
    public Vector3 playerKnockbackPosition = new Vector3(-11.5f, 0, 0);
    public Transform ventPosition;

    [Header("Objects")]
    public GameObject table;
    public GameObject tableNotes;
    public GameObject closet;
    public GameObject vent;

    [Header("Triggers")]
    public BoxCollider2D staircaseTrigger;
    public BoxCollider2D secondConfrontationTrigger;

    private bool sequenceStarted = false;
    private bool firstConfrontationTriggered = false;
    private bool secondConfrontationTriggered = false;

    private void Start()
    {
        // Check if sequence already completed
        if (SaveSystem.Instance != null && SaveSystem.Instance.WasDialogueTriggered(sequenceId))
        {
            // Skip sequence, already played
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(HallwaySequence());
    }

    IEnumerator HallwaySequence()
    {
        sequenceStarted = true;

        // 1. Initial hallway dialogue
        yield return StartCoroutine(ShowDialogue(
            "This hallway feels endless... and so cold. These prints... they're so small. What happened here?",
            "Lisa"
        ));

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!firstConfrontationTriggered && other.gameObject == staircaseTrigger.gameObject)
        {
            StartCoroutine(FirstConfrontation());
        }

        if (!secondConfrontationTriggered && other.gameObject == secondConfrontationTrigger.gameObject)
        {
            StartCoroutine(SecondConfrontation());
        }
    }

    IEnumerator FirstConfrontation()
    {
        firstConfrontationTriggered = true;

        // Spawn Emily at staircase
        EmilyAIController.Instance.TeleportTo(staircaseBlockPosition.position);
        EmilyAIController.Instance.ActivateEmily();

        // Wind push effect
        yield return new WaitForSeconds(0.5f);
        EmilyAIController.Instance.audioController.PlayWindPushSound();

        // Knockback player
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = playerKnockbackPosition;

        // Emily becomes aggressive
        EmilyAIController.Instance.ForceState(EmilyState.HUNT);

        SaveSystem.Instance?.TriggerDialogue("emily_first_confrontation");
    }

    IEnumerator SecondConfrontation()
    {
        secondConfrontationTriggered = true;

        // Emily intercepts player
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        EmilyAIController.Instance.TeleportTo(player.position);

        // Throw animation and sound
        EmilyAIController.Instance.emilyAnimator.PlayHitAnimation();
        EmilyAIController.Instance.audioController.PlayWindPushSound();

        yield return new WaitForSeconds(1f);

        // Open vent access
        if (vent != null) vent.SetActive(true);

        // Emily moves to kitchen
        UnityEngine.SceneManagement.SceneManager.LoadScene("Room_04_Kitchen_Dining");

        SaveSystem.Instance?.TriggerDialogue("emily_second_confrontation");
    }

    public void OnTableNotesPickup()
    {
        if (tableNotes != null)
        {
            tableNotes.SetActive(false);
        }
        // Table stays visible

        // Add diary pages through existing system
        GlobalDiaryManager.Instance?.AddDiaryPage("diary_page_1");
    }

    public void OnClosetInteract()
    {
        StartCoroutine(ShowDialogue(
            "Someone was trying to get out...",
            "Lisa"
        ));
        // Play closet scratch animation if available
    }

    IEnumerator ShowDialogue(string text, string speaker)
    {
        DialogueSystemV2 dialogue = FindFirstObjectByType<DialogueSystemV2>();
        if (dialogue != null)
        {
            dialogue.StartDialogue(text, speaker);

            // Wait for dialogue to complete
            while (dialogue.IsDialogueActive())
            {
                yield return null;
            }
        }
    }
}