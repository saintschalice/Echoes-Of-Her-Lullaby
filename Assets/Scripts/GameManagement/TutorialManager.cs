using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Image tutorialImage;
    public Button continueButton;
    public GameObject fingerPointer;

    [Header("Tutorial Steps")]
    public GameObject joystickHighlight;
    public GameObject interactionButtonHighlight; // NEW: Highlight for the interaction button
    public GameObject mailboxHighlight; // Optional: Highlight for the object itself
    public GameObject inventoryButtonHighlight;

    [Header("References")]
    public DialogueSystemV2 dialogueSystem;

    [Header("Audio")]
    public AudioClip tutorialSound;

    [Header("Cutscene Settings")]
    [Tooltip("Wait for opening cutscene to complete before starting tutorial")]
    public bool waitForCutscene = true;

    private AudioSource audioSource;
    private bool tutorialActive = false;
    private bool tutorialCompleted = false;
    private bool cutsceneFinished = false;

    // State Tracking
    private bool hasMovedJoystick = false;
    private bool hasEnteredSensor = false; // NEW
    private bool hasExaminedMailbox = false;
    private bool hasTakenMail = false;
    private bool hasOpenedInventory = false;

    private const string TUTORIAL_COMPLETED_ID = "Tutorial_Completed";

    public static TutorialManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (waitForCutscene)
        {
            CutsceneManager.OnAnyCutsceneComplete += OnCutsceneFinished;
        }
        else
        {
            cutsceneFinished = true;
        }
    }

    void Start()
    {
        FindReferences();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        HideAllHighlights();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        CheckTutorialStatus();
    }

    void OnDestroy()
    {
        if (waitForCutscene)
        {
            CutsceneManager.OnAnyCutsceneComplete -= OnCutsceneFinished;
        }
    }

    void OnCutsceneFinished()
    {
        cutsceneFinished = true;
        if (!tutorialCompleted)
        {
            StartCoroutine(StartTutorialSequence());
        }
    }

    void FindReferences()
    {
        if (dialogueSystem == null)
            dialogueSystem = DialogueSystemV2.Instance ?? FindFirstObjectByType<DialogueSystemV2>();

        if (tutorialPanel == null)
        {
            tutorialPanel = GameObject.Find("TutorialPanel");
            // Fallback search in MainCanvas
            if (tutorialPanel == null)
            {
                Transform t = GameObject.Find("MainCanvas")?.transform.Find("TutorialPanel");
                if (t != null) tutorialPanel = t.gameObject;
            }
        }

        if (tutorialText == null && tutorialPanel != null)
            tutorialText = tutorialPanel.GetComponentInChildren<TextMeshProUGUI>();

        if (continueButton == null && tutorialPanel != null)
            continueButton = tutorialPanel.GetComponentInChildren<Button>();
    }

    void CheckTutorialStatus()
    {
        if (SaveSystem.Instance != null)
        {
            tutorialCompleted = SaveSystem.Instance.WasObjectExamined(TUTORIAL_COMPLETED_ID);
        }

        if (!tutorialCompleted && cutsceneFinished)
        {
            StartCoroutine(StartTutorialSequence());
        }
    }

    // 1. Start Sequence: Movement
    IEnumerator StartTutorialSequence()
    {
        if (waitForCutscene && !cutsceneFinished)
        {
            yield return new WaitUntil(() => cutsceneFinished);
        }

        yield return new WaitForSeconds(1f);

        ShowTutorialStep(
            "Use the D-pad to move around.",
            joystickHighlight
        );
    }

    // 2. Called by Player Movement Input
    public void OnPlayerMoved()
    {
        if (!hasMovedJoystick && !tutorialCompleted)
        {
            hasMovedJoystick = true;
            HideTutorialStep();
            // We now wait for them to hit the sensor
        }
    }

    // 3. NEW: Triggered by TutorialAreaSensor
    public void TriggerInteractionTutorial()
    {
        if (tutorialCompleted || hasEnteredSensor) return;

        hasEnteredSensor = true;

        // Show specific instruction for the interaction button
        ShowTutorialStep(
            "Press this button to interact with objects.",
            interactionButtonHighlight // The circle highlight for the button
        );
    }

    // 4. Called by MailboxInteraction when examined/interacted
    public void OnMailboxExamined()
    {
        // If they interacted, we hide the "Press Button" tutorial
        if (!hasExaminedMailbox && !tutorialCompleted)
        {
            hasExaminedMailbox = true;
            HideTutorialStep();
        }
    }

    // 5. Called by MailboxInteraction when mail is added
    public void OnMailTaken()
    {
        if (!hasTakenMail && !tutorialCompleted)
        {
            hasTakenMail = true;
            StartCoroutine(ShowInventoryTutorial());
        }
    }

    // 6. Shows prompt to open inventory
    IEnumerator ShowInventoryTutorial()
    {
        // Small delay to allow the "Item Added" dialogue to finish or settle
        yield return new WaitForSeconds(0.5f);

        // Combined instructions here
        ShowTutorialStep(
            "Mail added to inventory!\n\nTap the inventory button to view your items.\n\nTap once for the item description. Tap twice to interact with the item.",
            inventoryButtonHighlight
        );
    }

    // 7. Called by InventoryUI (or InventoryManager) when opened
    public void OnInventoryOpened()
    {
        if (!hasOpenedInventory && !tutorialCompleted)
        {
            hasOpenedInventory = true;

            // We hide the tutorial step immediately when inventory opens
            // because the instructions were already shown in the previous step.
            HideTutorialStep();
            CompleteTutorial();
        }
    }

    // Helper to show step
    void ShowTutorialStep(string message, GameObject highlight = null)
    {
        tutorialActive = true;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        if (tutorialText != null)
            tutorialText.text = message;

        HideAllHighlights();

        if (highlight != null)
            highlight.SetActive(true);

        if (tutorialSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(tutorialSound);
        }

        // Pause time for the tutorial box so they don't miss it
        Time.timeScale = 0f;

        Debug.Log($"[Tutorial] {message}");
    }

    void HideTutorialStep()
    {
        tutorialActive = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        HideAllHighlights();
        Time.timeScale = 1f;

        // If we just finished the inventory explanation, mark as complete
        if (hasOpenedInventory && !tutorialCompleted)
        {
            CompleteTutorial();
        }
    }

    void HideAllHighlights()
    {
        if (joystickHighlight != null) joystickHighlight.SetActive(false);
        if (mailboxHighlight != null) mailboxHighlight.SetActive(false);
        if (inventoryButtonHighlight != null) inventoryButtonHighlight.SetActive(false);
        if (interactionButtonHighlight != null) interactionButtonHighlight.SetActive(false);
    }

    void OnContinueClicked()
    {
        HideTutorialStep();
    }

    void CompleteTutorial()
    {
        tutorialCompleted = true;

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(TUTORIAL_COMPLETED_ID);
        }

        Debug.Log("[Tutorial] Tutorial completed!");
    }

    public bool IsTutorialActive()
    {
        return tutorialActive;
    }

    public bool IsTutorialCompleted()
    {
        return tutorialCompleted;
    }
}