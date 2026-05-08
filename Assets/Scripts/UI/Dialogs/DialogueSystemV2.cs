using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 4)]
    public string text;
    public string speakerName;
    public float typewriterSpeed = 0.05f;
}

[System.Serializable]
public class SpeakerData
{
    public string speakerName;
    public Sprite dialogueBoxSprite;
    public Color textColor = Color.white;
    public AudioClip[] typingSounds;
}

public class DialogueSystemV2 : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;
    public Image dialogueBoxImage;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject tapToContinueIndicator;

    [Header("Speaker Configurations")]
    public SpeakerData[] speakers;

    [Header("Audio Settings")]
    public float typingSoundVolume = 0.5f;
    public int charactersPerSound = 1;

    [Header("Player Controller")]
    public MonoBehaviour playerController;
    public GameObject joystickUI;

    [Header("Choice System")]
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;
    public Transform choiceButtonParent;

    private List<DialogueLine> currentDialogue = new List<DialogueLine>();
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine typingCoroutine;
    private string fullText = "";
    private System.Action[] currentChoiceCallbacks;

    public System.Action OnDialogueStarted;
    public System.Action OnDialogueEnded;

    public static DialogueSystemV2 Instance { get; private set; }
    private InventoryUI cachedInventoryUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
            {
                transform.SetParent(null, true);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        JoystickPlayerController.OnInstanceChanged += HandlePlayerControllerChanged;
        HandlePlayerControllerChanged(JoystickPlayerController.Instance);
    }

    void OnDisable()
    {
        JoystickPlayerController.OnInstanceChanged -= HandlePlayerControllerChanged;
    }

    void Start()
    {
        FindReferences();

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }

        Debug.Log("[Dialogue] DialogueSystem initialized with AudioManager integration");
    }

    void FindReferences()
    {
        EnsurePlayerControllerReference();

        if (joystickUI == null)
        {
            joystickUI = GameObject.Find("Joystick");

            if (joystickUI == null)
            {
                GameObject persistentUI = GameObject.Find("PersistentUI");
                if (persistentUI != null)
                {
                    Transform joystickTransform = persistentUI.transform.Find("Joystick");
                    if (joystickTransform != null)
                    {
                        joystickUI = joystickTransform.gameObject;
                    }
                }
            }
        }

        EnsureInventoryReference();
    }

    void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleDialogueInput();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestDialogue();
        }
    }

    void HandleDialogueInput()
    {
        if (isTyping)
        {
            skipTyping = true;
            // Stop dialogue typing sounds through AudioManager
            AudioManager.Instance?.StopAllDialogue();
        }
        else
        {
            NextLine();
        }
    }

    public void ShowChoices(string[] choices, System.Action[] callbacks)
    {
        if (choicePanel == null || choiceButtonPrefab == null || choiceButtonParent == null)
        {
            Debug.LogWarning("[Dialogue] Choice system not set up properly!");
            return;
        }

        Debug.Log($"[Dialogue] ShowChoices called with {choices.Length} choices");

        currentChoiceCallbacks = callbacks;

        // Clear existing buttons
        foreach (Transform child in choiceButtonParent)
        {
            Destroy(child.gameObject);
        }

        // Create new buttons
        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceButtonParent);

            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = choices[i];
            }

            if (button != null)
            {
                button.interactable = true; // Ensure button is interactable
                button.onClick.RemoveAllListeners(); // Clear any existing listeners
                button.onClick.AddListener(() => OnChoiceSelected(index));
                Debug.Log($"[Dialogue] Created button {index}: {choices[index]}, interactable: {button.interactable}");
            }
        }

        // Hide dialogue panel first
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            
            // Also disable its CanvasGroup if it has one
            CanvasGroup dialogueCG = dialoguePanel.GetComponent<CanvasGroup>();
            if (dialogueCG != null)
            {
                dialogueCG.blocksRaycasts = false;
            }
        }

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }

        // Show choice panel
        choicePanel.SetActive(true);

        // Ensure CanvasGroup allows interaction
        CanvasGroup choicePanelCG = choicePanel.GetComponent<CanvasGroup>();
        if (choicePanelCG != null)
        {
            choicePanelCG.alpha = 1f;
            choicePanelCG.interactable = true;
            choicePanelCG.blocksRaycasts = true;
            Debug.Log($"[Dialogue] ChoicePanel CanvasGroup set: alpha={choicePanelCG.alpha}, interactable={choicePanelCG.interactable}, blocksRaycasts={choicePanelCG.blocksRaycasts}");
        }
        else
        {
            Debug.LogWarning("[Dialogue] ChoicePanel has no CanvasGroup!");
        }

        // DON'T disable player controller - keep it active so input works
        // Just pause Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        Debug.Log("[Dialogue] ShowChoices complete - choice panel should be visible and clickable");
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"[Dialogue] OnChoiceSelected called with index: {choiceIndex}");

        // Resume Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = false;

        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        if (currentChoiceCallbacks != null && choiceIndex < currentChoiceCallbacks.Length)
        {
            Debug.Log($"[Dialogue] Invoking callback for choice {choiceIndex}");
            currentChoiceCallbacks[choiceIndex]?.Invoke();
        }

        currentChoiceCallbacks = null;
        EndDialogue();
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines provided!");
            return;
        }

        InventoryManager.Instance?.NotifyActionStarted();

        EnsurePlayerControllerReference();

        // Notify UIStateManager
        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.PrepareForDialogue();
        }

        OnDialogueStarted?.Invoke();

        currentDialogue.Clear();
        currentDialogue.AddRange(lines);
        currentLineIndex = 0;
        isDialogueActive = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (joystickUI != null)
        {
            joystickUI.SetActive(false);
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // Pause Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        EnsureInventoryReference();
        if (cachedInventoryUI != null)
        {
            cachedInventoryUI.ForceCloseInventory();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        DisplayLine();

        Debug.Log($"[Dialogue] Started dialogue with {lines.Length} lines");
    }

    public void StartDialogue(string text, string speaker = "Lisa")
    {
        DialogueLine[] lines = new DialogueLine[1];
        lines[0] = new DialogueLine
        {
            text = text,
            speakerName = speaker,
            typewriterSpeed = 0.05f
        };

        StartDialogue(lines);
    }

    public void StartDialogue(string[] texts, string speaker = "Lisa")
    {
        DialogueLine[] lines = new DialogueLine[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            lines[i] = new DialogueLine
            {
                text = texts[i],
                speakerName = speaker,
                typewriterSpeed = 0.05f
            };
        }

        StartDialogue(lines);
    }

    void DisplayLine()
    {
        if (currentLineIndex >= currentDialogue.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = currentDialogue[currentLineIndex];

        UpdateSpeakerVisuals(currentLine.speakerName);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }

        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    void UpdateSpeakerVisuals(string speakerName)
    {
        SpeakerData speaker = GetSpeakerData(speakerName);

        if (speaker != null)
        {
            if (dialogueBoxImage != null && speaker.dialogueBoxSprite != null)
            {
                dialogueBoxImage.sprite = speaker.dialogueBoxSprite;
            }

            if (dialogueText != null)
            {
                dialogueText.color = speaker.textColor;
            }

            if (speakerNameText != null)
            {
                speakerNameText.text = speakerName;
            }
        }

        Debug.Log($"[Dialogue] Updated visuals for speaker: {speakerName}");
    }

    SpeakerData GetSpeakerData(string speakerName)
    {
        foreach (SpeakerData speaker in speakers)
        {
            if (speaker.speakerName.Equals(speakerName, System.StringComparison.OrdinalIgnoreCase))
            {
                return speaker;
            }
        }

        if (speakers.Length > 0)
        {
            return speakers[0];
        }

        return null;
    }

    IEnumerator TypeText(DialogueLine line)
    {
        isTyping = true;
        skipTyping = false;
        fullText = line.text;

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        SpeakerData speaker = GetSpeakerData(line.speakerName);
        int soundCounter = 0;

        for (int i = 0; i < fullText.Length; i++)
        {
            if (skipTyping)
            {
                break;
            }

            if (dialogueText != null)
            {
                dialogueText.text = fullText.Substring(0, i + 1);
            }

            char currentChar = fullText[i];
            if (!char.IsWhiteSpace(currentChar))
            {
                soundCounter++;
                if (soundCounter >= charactersPerSound)
                {
                    PlayTypingSound(speaker);
                    soundCounter = 0;
                }
            }

            yield return new WaitForSeconds(line.typewriterSpeed);
        }

        // Stop all dialogue sounds when typing finishes
        AudioManager.Instance?.StopAllDialogue();

        if (dialogueText != null)
        {
            dialogueText.text = fullText;
        }

        isTyping = false;
        skipTyping = false;

        Debug.Log($"[Dialogue] Finished typing: {fullText}");

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(true);
            StartCoroutine(AnimateTapIndicator());
        }
    }

    IEnumerator AnimateTapIndicator()
    {
        if (tapToContinueIndicator == null) yield break;

        RectTransform indicator = tapToContinueIndicator.GetComponent<RectTransform>();
        if (indicator == null) yield break;

        Vector3 originalScale = indicator.localScale;
        float time = 0f;

        while (tapToContinueIndicator.activeSelf)
        {
            time += Time.unscaledDeltaTime * 2f;
            float scale = 1f + Mathf.Sin(time) * 0.1f;
            indicator.localScale = originalScale * scale;
            yield return null;
        }

        indicator.localScale = originalScale;
    }

    void PlayTypingSound(SpeakerData speaker)
    {
        if (speaker == null || speaker.typingSounds == null || speaker.typingSounds.Length == 0)
            return;

        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("[Dialogue] AudioManager not found!");
            return;
        }

        AudioClip soundToPlay = speaker.typingSounds[Random.Range(0, speaker.typingSounds.Length)];

        // Play through AudioManager - automatically routed to Dialogue mixer group
        AudioManager.Instance.PlayDialogue(soundToPlay, typingSoundVolume);
    }

    void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogue.Count)
        {
            DisplayLine();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        Debug.Log("[Dialogue] Ending dialogue");

        isDialogueActive = false;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Stop all dialogue sounds
        AudioManager.Instance?.StopAllDialogue();

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentDialogue.Clear();
        currentLineIndex = 0;

        // Notify UIStateManager AFTER cleaning up
        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.DialogueComplete();
        }

        OnDialogueEnded?.Invoke();
        InventoryManager.Instance?.NotifyActionEnded();

        // Resume Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = false;

        // CRITICAL FIX: Immediately re-enable controls to prevent stuck state
        // Re-enable joystick immediately - try multiple names
        if (joystickUI != null)
        {
            joystickUI.SetActive(true);
            Debug.Log("[Dialogue] Joystick re-enabled immediately after dialogue");
        }
        else
        {
            // Fallback: try to find joystick with multiple possible names
            joystickUI = GameObject.Find("Joystick");
            if (joystickUI == null)
            {
                joystickUI = GameObject.Find("FloatingJoystick");
            }
            if (joystickUI == null)
            {
                joystickUI = GameObject.Find("VariableJoystick");
            }
            
            if (joystickUI != null)
            {
                joystickUI.SetActive(true);
                Debug.Log($"[Dialogue] Joystick found and re-enabled: {joystickUI.name} (fallback)");
            }
            else
            {
                Debug.LogWarning("[Dialogue] Joystick not found! Player may be stuck. Tried: Joystick, FloatingJoystick, VariableJoystick");
            }
        }

        // Re-enable player controller immediately
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("[Dialogue] Player controller re-enabled immediately");
        }
        else
        {
            // Fallback: try to find player controller
            EnsurePlayerControllerReference(false);
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("[Dialogue] Player controller found and re-enabled (fallback)");
            }
            else
            {
                Debug.LogWarning("[Dialogue] Player controller not found! Player may be stuck.");
            }
        }

        Debug.Log("[Dialogue] EndDialogue complete - controls should be restored");
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    void TestDialogue()
    {
        DialogueLine[] testLines = new DialogueLine[]
        {
            new DialogueLine { text = "This is a test message from Lisa.", speakerName = "Lisa" },
            new DialogueLine { text = "And this is a mysterious voice...", speakerName = "???" },
            new DialogueLine { text = "Finally, this is Emily speaking.", speakerName = "Emily" }
        };

        StartDialogue(testLines);
    }

    void HandlePlayerControllerChanged(JoystickPlayerController controller)
    {
        playerController = controller;
    }

    void EnsurePlayerControllerReference(bool logIfMissing = true)
    {
        if (playerController != null)
            return;

        if (JoystickPlayerController.Instance != null)
        {
            playerController = JoystickPlayerController.Instance;
            return;
        }

        if (PersistentSpawnManager.Instance != null && PersistentSpawnManager.Instance.player != null)
        {
            playerController = PersistentSpawnManager.Instance.player.GetComponent<JoystickPlayerController>();
            if (playerController != null)
                return;
        }

        if (logIfMissing)
        {
            Debug.LogWarning("[Dialogue] JoystickPlayerController not found!");
        }
    }

    void EnsureInventoryReference()
    {
        if (cachedInventoryUI != null)
            return;

        cachedInventoryUI = InventoryUI.Instance;

        if (cachedInventoryUI == null)
        {
            Debug.LogWarning("[Dialogue] InventoryUI not found!");
        }
    }
}