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
    public GameObject mailboxHighlight;
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
    private bool cutsceneFinished = false; // NEW: Track if cutscene is done

    private bool hasMovedJoystick = false;
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

        // NEW: Subscribe to cutscene completion event
        if (waitForCutscene)
        {
            CutsceneManager.OnAnyCutsceneComplete += OnCutsceneFinished;
            Debug.Log("[Tutorial] Waiting for cutscene to complete before starting tutorial");
        }
        else
        {
            cutsceneFinished = true; // No need to wait
        }
    }

    void Start()
    {
        // FIND REFERENCES AT RUNTIME
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
        // NEW: Unsubscribe from event
        if (waitForCutscene)
        {
            CutsceneManager.OnAnyCutsceneComplete -= OnCutsceneFinished;
        }
    }

    // NEW: Called when cutscene completes
    void OnCutsceneFinished()
    {
        cutsceneFinished = true;
        Debug.Log("[Tutorial] Cutscene finished, starting tutorial now");

        // Start tutorial if we were waiting
        if (!tutorialCompleted)
        {
            StartCoroutine(StartTutorialSequence());
        }
    }

    // NEW: Find all required references at runtime
    void FindReferences()
    {
        // Find DialogueSystemV2
        if (dialogueSystem == null)
        {
            dialogueSystem = DialogueSystemV2.Instance;

            if (dialogueSystem == null)
            {
                dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
            }

            if (dialogueSystem == null)
            {
                Debug.LogError("[Tutorial] DialogueSystemV2 not found!");
            }
            else
            {
                Debug.Log("[Tutorial] DialogueSystemV2 found successfully!");
            }
        }

        // Find Tutorial Panel
        if (tutorialPanel == null)
        {
            tutorialPanel = GameObject.Find("TutorialPanel");

            if (tutorialPanel == null)
            {
                GameObject mainCanvas = GameObject.Find("MainCanvas");
                if (mainCanvas != null)
                {
                    Transform panelTransform = mainCanvas.transform.Find("TutorialPanel");
                    if (panelTransform != null)
                    {
                        tutorialPanel = panelTransform.gameObject;
                    }
                }
            }

            if (tutorialPanel == null)
            {
                Debug.LogError("[Tutorial] TutorialPanel not found!");
            }
            else
            {
                Debug.Log("[Tutorial] TutorialPanel found successfully!");
            }
        }

        // Find tutorial text if not set
        if (tutorialText == null && tutorialPanel != null)
        {
            tutorialText = tutorialPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        // Find continue button if not set
        if (continueButton == null && tutorialPanel != null)
        {
            continueButton = tutorialPanel.GetComponentInChildren<Button>();
        }
    }

    void CheckTutorialStatus()
    {
        if (SaveSystem.Instance != null)
        {
            tutorialCompleted = SaveSystem.Instance.WasObjectExamined(TUTORIAL_COMPLETED_ID);
        }

        // NEW: Only start tutorial if cutscene is finished (or not waiting for one)
        if (!tutorialCompleted && cutsceneFinished)
        {
            StartCoroutine(StartTutorialSequence());
        }
    }

    IEnumerator StartTutorialSequence()
    {
        // NEW: Double-check cutscene is done
        if (waitForCutscene && !cutsceneFinished)
        {
            Debug.Log("[Tutorial] Waiting for cutscene...");
            yield return new WaitUntil(() => cutsceneFinished);
        }

        yield return new WaitForSeconds(1f);

        ShowTutorialStep(
            "Use the D-pad to move around.\nTap objects to examine them.",
            joystickHighlight
        );

        yield return new WaitUntil(() => hasMovedJoystick);
        yield return new WaitForSeconds(0.5f);

        ShowTutorialStep(
            "Great! Now approach the mailbox and tap on it to examine it.",
            mailboxHighlight
        );
    }

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

    public bool IsTutorialActive()
    {
        return tutorialActive;
    }

    public bool IsTutorialCompleted()
    {
        return tutorialCompleted;
    }
}