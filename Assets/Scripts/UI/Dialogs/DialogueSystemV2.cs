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
    // REMOVED: waitForInput - now ALWAYS waits for player tap
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
    public GameObject tapToContinueIndicator; // NEW: "Tap to continue" icon/text

    [Header("Speaker Configurations")]
    public SpeakerData[] speakers;

    [Header("Audio")]
    public AudioSource audioSource;
    public float typingSoundVolume = 0.5f;
    public int charactersPerSound = 1;

    [Header("Player Controller")]
    public MonoBehaviour playerController;
    public GameObject joystickUI;

    [Header("Choice System")]
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;
    public Transform choiceButtonParent;

    // Private variables
    private List<DialogueLine> currentDialogue = new List<DialogueLine>();
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine typingCoroutine;
    private string fullText = "";
    private float baseTypingSoundVolume;

    // Choice system variables
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

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        baseTypingSoundVolume = typingSoundVolume;
    }

    void Start()
    {
        UpdateDialogueVolume();
        ConnectToAudioMixer();

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        // Hide tap to continue indicator initially
        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<JoystickPlayerController>();
            if (playerController == null)
            {
                playerController = FindFirstObjectByType<MonoBehaviour>();
            }
        }

        if (joystickUI == null)
        {
            GameObject foundJoystick = GameObject.Find("Joystick");
            Debug.Log("Found Joystick by name: " + (foundJoystick != null ? foundJoystick.name : "NULL"));

            if (foundJoystick == null)
            {
                foundJoystick = GameObject.Find("PlayerLight2D");
                Debug.Log("Found PlayerLight2D: " + (foundJoystick != null ? foundJoystick.name : "NULL"));

                if (foundJoystick != null)
                {
                    Transform parent = foundJoystick.transform.parent;
                    if (parent != null)
                    {
                        foundJoystick = parent.Find("Joystick")?.gameObject;
                        Debug.Log("Found Joystick in parent: " + (foundJoystick != null ? foundJoystick.name : "NULL"));
                    }
                }
            }
            joystickUI = foundJoystick;
        }

        Debug.Log("DialogueSystem initialized with joystick: " + (joystickUI != null ? joystickUI.name : "NULL"));
    }

    void ConnectToAudioMixer()
    {
        if (audioSource != null)
        {
            if (audioSource.outputAudioMixerGroup != null)
            {
                Debug.Log("DialogueSystem AudioSource already connected to: " + audioSource.outputAudioMixerGroup.name);
                return;
            }

            UnityEngine.Audio.AudioMixer[] mixers = Resources.FindObjectsOfTypeAll<UnityEngine.Audio.AudioMixer>();

            foreach (var mixer in mixers)
            {
                if (mixer.name == "MainAudioMixer")
                {
                    UnityEngine.Audio.AudioMixerGroup[] groups = mixer.FindMatchingGroups("Dialogue");
                    if (groups.Length > 0)
                    {
                        audioSource.outputAudioMixerGroup = groups[0];
                        Debug.Log("Connected DialogueSystem AudioSource to MainAudioMixer -> Dialogue group");
                        return;
                    }
                }
            }

            Debug.LogWarning("Could not find Dialogue mixer group. Audio volume will be controlled locally.");
        }
    }

    public void UpdateDialogueVolume()
    {
        if (audioSource != null)
        {
            float dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);

            if (audioSource.outputAudioMixerGroup != null)
            {
                Debug.Log($"Dialogue volume controlled by mixer: {dialogueVolume * 100}%");
            }
            else
            {
                audioSource.volume = baseTypingSoundVolume * dialogueVolume;
                Debug.Log($"Dialogue volume set directly: {audioSource.volume}");
            }
        }
    }

    void Update()
    {
        if (isDialogueActive)
        {
            // MODIFIED: Only tap/click advances dialogue
            if (Input.GetMouseButtonDown(0)) // Tap or left click
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
            // Skip typing animation
            skipTyping = true;
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
        else
        {
            // Move to next line
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

        // Store callbacks
        currentChoiceCallbacks = callbacks;

        // Clear existing buttons
        foreach (Transform child in choiceButtonParent)
        {
            Destroy(child.gameObject);
        }

        // Create choice buttons
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

        // Show choice panel
        choicePanel.SetActive(true);

        // Hide main dialogue while showing choices
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Hide tap to continue indicator during choices
        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(false);
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        // Hide choice panel
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        // Execute callback
        if (currentChoiceCallbacks != null && choiceIndex < currentChoiceCallbacks.Length)
        {
            currentChoiceCallbacks[choiceIndex]?.Invoke();
        }

        // Clear callbacks
        currentChoiceCallbacks = null;

        // Close dialogue
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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        DisplayLine();

        Debug.Log($"Started dialogue with {lines.Length} lines");
    }

    public void StartDialogue(string text, string speaker = "Lisa")
    {
        DialogueLine[] lines = new DialogueLine[1];
        lines[0] = new DialogueLine
        {
            text = text,
            speakerName = speaker,
            typewriterSpeed = 0.05f
            // No waitForInput needed - script now ALWAYS waits for tap
        };

        StartDialogue(lines);
    }

    // NEW: Convenience method for multiple single-line dialogues
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

        // Hide tap to continue while typing
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

        Debug.Log($"Updated visuals for speaker: {speakerName}");
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

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (dialogueText != null)
        {
            dialogueText.text = fullText;
        }

        isTyping = false;
        skipTyping = false;

        Debug.Log($"Finished typing: {fullText}");

        // MODIFIED: Show tap to continue indicator after typing finishes
        // Dialogue NEVER auto-closes - player MUST tap to continue
        if (tapToContinueIndicator != null)
        {
            tapToContinueIndicator.SetActive(true);

            // Optional: Animate the indicator
            StartCoroutine(AnimateTapIndicator());
        }
    }

    // NEW: Animate the "tap to continue" indicator
    IEnumerator AnimateTapIndicator()
    {
        if (tapToContinueIndicator == null) yield break;

        RectTransform indicator = tapToContinueIndicator.GetComponent<RectTransform>();
        if (indicator == null) yield break;

        Vector3 originalScale = indicator.localScale;
        float time = 0f;

        while (tapToContinueIndicator.activeSelf)
        {
            time += Time.unscaledDeltaTime * 2f; // Speed of pulse
            float scale = 1f + Mathf.Sin(time) * 0.1f; // Pulse between 0.9 and 1.1
            indicator.localScale = originalScale * scale;
            yield return null;
        }

        indicator.localScale = originalScale;
    }

    void PlayTypingSound(SpeakerData speaker)
    {
        if (audioSource != null && speaker != null && speaker.typingSounds != null && speaker.typingSounds.Length > 0)
        {
            audioSource.Stop();
            audioSource.pitch = 1f;

            AudioClip soundToPlay = speaker.typingSounds[Random.Range(0, speaker.typingSounds.Length)];

            float currentVolume = baseTypingSoundVolume;

            if (audioSource.outputAudioMixerGroup == null)
            {
                float dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);
                currentVolume *= dialogueVolume;
            }

            audioSource.PlayOneShot(soundToPlay, currentVolume);
        }
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
        Debug.Log("Ending dialogue");

        isDialogueActive = false;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Hide tap to continue indicator
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