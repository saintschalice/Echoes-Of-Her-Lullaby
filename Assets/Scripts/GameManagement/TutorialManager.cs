using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Image tutorialImage; // Optional: Show images for tutorials
    public Button continueButton;
    public GameObject fingerPointer; // Optional: Pointing finger animation

    [Header("Tutorial Steps")]
    public GameObject joystickHighlight; // Highlight joystick
    public GameObject mailboxHighlight; // Highlight mailbox
    public GameObject inventoryButtonHighlight; // Highlight inventory button

    [Header("Audio")]
    public AudioClip tutorialSound;

    private AudioSource audioSource;
    private bool tutorialActive = false;
    private bool tutorialCompleted = false;

    // Tutorial state tracking
    private bool hasMovedJoystick = false;
    private bool hasExaminedMailbox = false;
    private bool hasTakenMail = false;
    private bool hasOpenedInventory = false;

    // Save state identifier
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
        }
    }

    void Start()
    {
        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // Hide all highlights initially
        HideAllHighlights();

        // Hide tutorial panel
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // Setup continue button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        // Check if tutorial should start
        CheckTutorialStatus();
    }

    void CheckTutorialStatus()
    {
        if (SaveSystem.Instance != null)
        {
            tutorialCompleted = SaveSystem.Instance.WasObjectExamined(TUTORIAL_COMPLETED_ID);
        }

        // Start tutorial if not completed
        if (!tutorialCompleted)
        {
            StartCoroutine(StartTutorialSequence());
        }
    }

    IEnumerator StartTutorialSequence()
    {
        yield return new WaitForSeconds(1f); // Wait 1 second after scene loads

        // Step 1: Movement Tutorial
        ShowTutorialStep(
            "Welcome to Echoes of Her Lullaby!\n\nUse the joystick to move around.\nTap objects to examine them.",
            joystickHighlight
        );

        // Wait for player to move
        yield return new WaitUntil(() => hasMovedJoystick);

        yield return new WaitForSeconds(0.5f);

        // Step 2: Mailbox Tutorial
        ShowTutorialStep(
            "Great! Now approach the mailbox and tap on it to examine it.",
            mailboxHighlight
        );
    }

    void ShowTutorialStep(string message, GameObject highlight = null)
    {
        tutorialActive = true;

        // Show panel
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        // Set text
        if (tutorialText != null)
            tutorialText.text = message;

        // Show highlight
        HideAllHighlights();
        if (highlight != null)
            highlight.SetActive(true);

        // Play sound
        if (tutorialSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(tutorialSound);
        }

        // Pause game during tutorial
        Time.timeScale = 0f;

        Debug.Log($"[Tutorial] {message}");
    }

    void HideTutorialStep()
    {
        tutorialActive = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        HideAllHighlights();

        // Resume game
        Time.timeScale = 1f;
    }

    void HideAllHighlights()
    {
        if (joystickHighlight != null)
            joystickHighlight.SetActive(false);
        if (mailboxHighlight != null)
            mailboxHighlight.SetActive(false);
        if (inventoryButtonHighlight != null)
            inventoryButtonHighlight.SetActive(false);
    }

    void OnContinueClicked()
    {
        HideTutorialStep();
    }

    // Public methods called by other scripts
    public void OnPlayerMoved()
    {
        if (!hasMovedJoystick && !tutorialCompleted)
        {
            hasMovedJoystick = true;
            HideTutorialStep();
        }
    }

    public void OnMailboxExamined()
    {
        if (!hasExaminedMailbox && !tutorialCompleted)
        {
            hasExaminedMailbox = true;
            HideTutorialStep();
        }
    }

    public void OnMailTaken()
    {
        if (!hasTakenMail && !tutorialCompleted)
        {
            hasTakenMail = true;
            StartCoroutine(ShowInventoryTutorial());
        }
    }

    IEnumerator ShowInventoryTutorial()
    {
        yield return new WaitForSeconds(1f);

        ShowTutorialStep(
            "Mail added to inventory!\n\nTap the inventory button to view your items and read the letter.",
            inventoryButtonHighlight
        );
    }

    public void OnInventoryOpened()
    {
        if (!hasOpenedInventory && !tutorialCompleted)
        {
            hasOpenedInventory = true;
            HideTutorialStep();
            CompleteTutorial();
        }
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

    // Public accessors
    public bool IsTutorialActive()
    {
        return tutorialActive;
    }

    public bool IsTutorialCompleted()
    {
        return tutorialCompleted;
    }
}