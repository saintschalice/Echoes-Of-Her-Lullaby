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

    public static DialogueSystemV2 Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<JoystickPlayerController>();

            if (playerController == null)
            {
                Debug.LogWarning("[Dialogue] JoystickPlayerController not found!");
            }
        }

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
            Debug.LogWarning("Choice system not set up properly!");
            return;
        }

        currentChoiceCallbacks = callbacks;

        foreach (Transform child in choiceButtonParent)
        {
            Destroy(child.gameObject);
        }

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
                button.onClick.AddListener(() => OnChoiceSelected(index));
            }
        }

        choicePanel.SetActive(true);

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        if (currentChoiceCallbacks != null && choiceIndex < currentChoiceCallbacks.Length)
        {
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

        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.ForceCloseInventory();
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

    /// <summary>
    /// NEW: Play typing sound through AudioManager (categorized as Dialogue)
    /// </summary>
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

        if (joystickUI != null)
        {
            joystickUI.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentDialogue.Clear();
        currentLineIndex = 0;
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
}