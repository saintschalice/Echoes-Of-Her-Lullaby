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
    public bool waitForInput = true;
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

    [Header("Speaker Configurations")]
    public SpeakerData[] speakers;

    [Header("Audio")]
    public AudioSource audioSource;
    public float typingSoundVolume = 0.5f;
    public int charactersPerSound = 1; // Play sound every X characters (1 = every character, 2 = every other character)

    [Header("Player Controller")]
    public MonoBehaviour playerController; // Reference to your player controller
    public GameObject joystickUI; // Reference to the joystick UI GameObject

    // Private variables
    private List<DialogueLine> currentDialogue = new List<DialogueLine>();
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine typingCoroutine;
    private string fullText = "";
    private float baseTypingSoundVolume; // Store original volume

    public static DialogueSystemV2 Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
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

        // Setup audio source if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        // Store base volume
        baseTypingSoundVolume = typingSoundVolume;
    }

    void Start()
    {
        UpdateDialogueVolume();

        ConnectToAudioMixer();

        // Hide dialogue panel at start
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Find player controller if not assigned
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<JoystickPlayerController>();
            if (playerController == null)
            {
                playerController = FindFirstObjectByType<MonoBehaviour>();
            }
        }

        // Find joystick UI if not assigned
        if (joystickUI == null)
        {
            // Try to find the joystick by name or component
            GameObject foundJoystick = GameObject.Find("Joystick");
            Debug.Log("Found Joystick by name: " + (foundJoystick != null ? foundJoystick.name : "NULL"));

            if (foundJoystick == null)
            {
                foundJoystick = GameObject.Find("PlayerLight2D");
                Debug.Log("Found PlayerLight2D: " + (foundJoystick != null ? foundJoystick.name : "NULL"));

                if (foundJoystick != null)
                {
                    // Look for joystick in the player hierarchy
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
            // Check if already connected via Inspector
            if (audioSource.outputAudioMixerGroup != null)
            {
                Debug.Log("DialogueSystem AudioSource already connected to: " + audioSource.outputAudioMixerGroup.name);
                return;
            }

            // If not connected, try to find and connect automatically
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
        else
        {
            Debug.LogWarning("AudioSource is null in ConnectToAudioMixer!");
        }
    }

    public void UpdateDialogueVolume()
    {
        if (audioSource != null)
        {
            float dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);

            // If connected to audio mixer, don't modify audioSource.volume
            if (audioSource.outputAudioMixerGroup != null)
            {
                // Volume is controlled by the mixer
                Debug.Log($"Dialogue volume controlled by mixer: {dialogueVolume * 100}%");
            }
            else
            {
                // Fallback: control volume directly
                audioSource.volume = baseTypingSoundVolume * dialogueVolume;
                Debug.Log($"Dialogue volume set directly: {audioSource.volume}");
            }
        }
    }

    void Update()
    {
        // Handle input during dialogue
        if (isDialogueActive)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                HandleDialogueInput();
            }
        }

        // Test input
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestDialogue();
        }
    }

    void HandleDialogueInput()
    {
        if (isTyping)
        {
            // Skip typing animation and stop sounds immediately
            skipTyping = true;
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
        else
        {
            // Move to next line or end dialogue
            NextLine();
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines provided!");
            return;
        }

        // Setup dialogue
        currentDialogue.Clear();
        currentDialogue.AddRange(lines);
        currentLineIndex = 0;
        isDialogueActive = true;

        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Hide joystick UI
        if (joystickUI != null)
        {
            joystickUI.SetActive(false);
        }

        // Show dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // Ensure cursor is visible for dialogue interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Start first line
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
            typewriterSpeed = 0.05f,
            waitForInput = true
        };

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

        // Update speaker visuals
        UpdateSpeakerVisuals(currentLine.speakerName);

        // Start typing animation
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    void UpdateSpeakerVisuals(string speakerName)
    {
        SpeakerData speaker = GetSpeakerData(speakerName);

        if (speaker != null)
        {
            // Update dialogue box sprite
            if (dialogueBoxImage != null && speaker.dialogueBoxSprite != null)
            {
                dialogueBoxImage.sprite = speaker.dialogueBoxSprite;
            }

            // Update text color
            if (dialogueText != null)
            {
                dialogueText.color = speaker.textColor;
            }

            // Update speaker name
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

        // Return default (first speaker) if not found
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

        // Clear text
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        SpeakerData speaker = GetSpeakerData(line.speakerName);
        int soundCounter = 0;

        // Type each character
        for (int i = 0; i < fullText.Length; i++)
        {
            // Check if we should skip typing
            if (skipTyping)
            {
                break;
            }

            // Add next character
            if (dialogueText != null)
            {
                dialogueText.text = fullText.Substring(0, i + 1);
            }

            // Play typing sound based on character counter
            char currentChar = fullText[i];
            if (!char.IsWhiteSpace(currentChar)) // Don't play sound for spaces
            {
                soundCounter++;
                if (soundCounter >= charactersPerSound)
                {
                    PlayTypingSound(speaker);
                    soundCounter = 0;
                }
            }

            // Wait for next character
            yield return new WaitForSeconds(line.typewriterSpeed);
        }

        // Stop any lingering typing sounds
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Ensure full text is displayed
        if (dialogueText != null)
        {
            dialogueText.text = fullText;
        }

        isTyping = false;
        skipTyping = false;

        Debug.Log($"Finished typing: {fullText}");

        // Auto-advance if not waiting for input
        if (!line.waitForInput)
        {
            yield return new WaitForSeconds(2f);
            NextLine();
        }
    }

    void PlayTypingSound(SpeakerData speaker)
    {
        if (audioSource != null && speaker != null && speaker.typingSounds != null && speaker.typingSounds.Length > 0)
        {
            // Stop any currently playing sound to prevent overlap
            audioSource.Stop();

            // Reset pitch to normal (no distortion)
            audioSource.pitch = 1f;

            // Pick a random sound from the speaker's collection
            AudioClip soundToPlay = speaker.typingSounds[Random.Range(0, speaker.typingSounds.Length)];

            // Play the sound with appropriate volume
            float currentVolume = baseTypingSoundVolume;

            // If not connected to mixer, apply dialogue volume setting
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

        // Stop typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Stop any playing audio
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Hide dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Show joystick UI
        if (joystickUI != null)
        {
            joystickUI.SetActive(true);
        }

        // Re-enable player movement
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Restore cursor for mobile/touch gameplay
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Clear dialogue data
        currentDialogue.Clear();
        currentLineIndex = 0;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    // Test function
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