using UnityEngine;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("Persistent UI References")]
    public GameObject cutscenePanel; // Black panel container
    public TextMeshProUGUI dialogueText;
    public CanvasGroup panelCanvasGroup;
    public GameObject skipButton; // NEW: Skip button (bottom right)

    [Header("Default Settings")]
    public float defaultFadeDuration = 1.5f;
    public Color backgroundColor = Color.black;

    private AudioSource audioSource;
    private bool isPlaying = false;
    private bool skipRequested = false; // NEW: Track skip request
    private Coroutine cutsceneCoroutine;

    private MonoBehaviour playerController;
    private GameObject joystickUI;

    public System.Action OnCutsceneComplete;

    // NEW: Static event that tutorial can listen to
    public static System.Action OnAnyCutsceneComplete;

    public static CutsceneManager Instance { get; private set; }

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

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Find canvas group if not assigned
        if (panelCanvasGroup == null && cutscenePanel != null)
        {
            panelCanvasGroup = cutscenePanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = cutscenePanel.AddComponent<CanvasGroup>();
            }
        }

        // --- Setup skip button ---
        // Moved from Start() to fix race condition
        if (skipButton != null)
        {
            // Add click listener
            UnityEngine.UI.Button btnComponent = skipButton.GetComponent<UnityEngine.UI.Button>();
            if (btnComponent != null)
            {
                btnComponent.onClick.AddListener(RequestSkip);
            }
        }
        // --- END OF MOVED BLOCK ---
    }

    void Start()
    {
        // Hide cutscene UI initially
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        // Ensure skip button is hidden on Start
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }

        Debug.Log("[CutsceneManager] Initialized and ready");
    }

    /// <summary>
    /// Called when skip button is pressed
    /// </summary>
    public void RequestSkip()
    {
        if (isPlaying)
        {
            skipRequested = true;

            // Stop audio immediately
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopAllDialogue();
            }

            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            Debug.Log("[CutsceneManager] Skip requested - stopping audio");
        }
    }

    /// <summary>
    /// Play a cutscene using the provided data
    /// </summary>
    public void PlayCutscene(VoiceOverCutsceneData cutsceneData, System.Action onComplete = null)
    {
        if (isPlaying)
        {
            Debug.LogWarning("[CutsceneManager] Already playing a cutscene!");
            return;
        }

        if (cutsceneData == null)
        {
            Debug.LogError("[CutsceneManager] No cutscene data provided!");
            return;
        }

        if (cutsceneCoroutine != null)
        {
            StopCoroutine(cutsceneCoroutine);
        }

        // Store callback
        OnCutsceneComplete = onComplete;

        cutsceneCoroutine = StartCoroutine(CutsceneSequence(cutsceneData));
    }

    IEnumerator CutsceneSequence(VoiceOverCutsceneData data)
    {
        isPlaying = true;
        skipRequested = false; // Reset skip flag

        // Get fade duration from data or use default
        float fadeDuration = data.fadeDuration > 0 ? data.fadeDuration : defaultFadeDuration;

        // Find player references (refresh each time in case scene changed)
        FindPlayerReferences();

        // Disable player controls
        DisablePlayerControls();

        // Start with instant black screen - NO fade in at the beginning
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.SetInstantBlack();
        }

        // Show cutscene panel immediately (no fade in)
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(true);
        }

        // Set panel to fully visible immediately
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 1f;
        }

        // Show skip button
        if (skipButton != null)
        {
            skipButton.SetActive(true);
        }

        // --- MUSIC LOGIC (BEFORE 0.3S DELAY) ---
        bool musicWasPlaying = false;
        AudioClip previousMusic = null;

        // Check if music is playing right now
        if (AudioManager.Instance != null &&
            AudioManager.Instance.musicSource != null &&
            AudioManager.Instance.musicSource.isPlaying)
        {
            musicWasPlaying = true;
            previousMusic = AudioManager.Instance.musicSource.clip;
        }

        // Now, decide what to do
        if (data.backgroundMusic != null && AudioManager.Instance != null)
        {
            // Case 1: Cutscene HAS its own music. Play it.
            // This will crossfade from any old music.
            AudioManager.Instance.PlayMusic(data.backgroundMusic, true, fadeDuration * 0.5f);

            // *** ADDED: Also explicitly stop any lingering ambient sound. ***
            AudioManager.Instance.StopAmbient(fadeDuration * 0.5f);

            // Adjust volume if specified
            if (AudioManager.Instance.musicSource != null)
            {
                AudioManager.Instance.musicSource.volume = data.musicVolume * AudioManager.Instance.musicVolume;
            }
            Debug.Log($"[CutsceneManager] Playing cutscene background music: {data.backgroundMusic.name}");
        }
        else if (AudioManager.Instance != null) // Removed 'musicWasPlaying' check
        {
            // Case 2: Cutscene has NO music. Fade out BOTH lingering music AND ambient.
            if (musicWasPlaying) // Keep this check for the log message
            {
                Debug.Log($"[CutsceneManager] Fading out lingering music for silent cutscene.");
                AudioManager.Instance.StopMusic(0.2f); // Stop music
            }

            // *** ADDED: Always stop ambient sound for a silent cutscene. ***
            Debug.Log($"[CutsceneManager] Fading out lingering ambient for silent cutscene.");
            AudioManager.Instance.StopAmbient(0.2f);
        }
        // Case 3: AudioManager is null, do nothing.
        // --- END OF MOVED MUSIC LOGIC ---

        // Small delay before starting voiceover
        yield return new WaitForSeconds(0.3f);


        // Play voiceover and display synchronized text
        if (data.voiceOverAudio != null)
        {
            // Route voiceover through Dialogue system (same as regular dialogue)
            if (AudioManager.Instance != null)
            {
                // Use PlayDialogue instead of PlayVoiceover - routes to dialogue mixer group
                AudioManager.Instance.PlayDialogue(data.voiceOverAudio, data.voiceoverVolume);
            }
            else
            {
                // Fallback to local audio source
                audioSource.clip = data.voiceOverAudio;
                audioSource.volume = data.voiceoverVolume;
                audioSource.Play();
            }

            yield return StartCoroutine(DisplaySynchronizedText(data));
        }

        // Wait a bit after audio finishes (unless skipped)
        if (!skipRequested)
        {
            yield return new WaitForSeconds(1f);
        }

        // Hide skip button
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }

        // Restore/stop music logic
        if (data.backgroundMusic != null && AudioManager.Instance != null)
        {
            // This cutscene HAD its own music. We must fade it out 
            // to make way for the new SceneAmbientPlayer.
            Debug.Log($"[CutsceneManager] Fading out cutscene music: {data.backgroundMusic.name}");
            AudioManager.Instance.StopMusic(fadeDuration * 0.5f);
        }
        else if (musicWasPlaying && AudioManager.Instance != null)
        {
            // This cutscene had NO music (it faded out the main menu music).
            // Do NOT restore the old music.
            // The SceneAmbientPlayer is about to take over.
            Debug.Log("[CutsceneManager] Cutscene complete. Letting SceneAmbientPlayer take over.");
        }
        // *** NOTE: We do NOT stop ambient here, because the SceneAmbientPlayer will
        // fade it out when it starts the *new* scene's ambient sound.

        // Fade out cutscene panel (unless skipped)
        float fadeOutDuration = skipRequested ? 0.5f : fadeDuration;
        if (panelCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(panelCanvasGroup, 1f, 0f, fadeOutDuration));
        }

        // Hide cutscene panel
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(false);
        }

        // Clear text
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        // Fade screen back in using ScreenFader
        if (ScreenFader.Instance != null)
        {
            // USE THE 'fadeOutDuration' INSTEAD OF 'fadeDuration' (This is the skip fix)
            ScreenFader.Instance.FadeIn(fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }

        // Re-enable player controls
        EnablePlayerControls();

        isPlaying = false;

        // Invoke completion callback
        OnCutsceneComplete?.Invoke();
        OnCutsceneComplete = null; // Clear callback

        // Invoke static event for systems like TutorialManager
        OnAnyCutsceneComplete?.Invoke();

        Debug.Log("[CutsceneManager] Cutscene completed!");
    }

    IEnumerator DisplaySynchronizedText(VoiceOverCutsceneData data)
    {
        if (dialogueText == null || data.lines == null || data.lines.Count == 0)
        {
            Debug.LogWarning("[CutsceneManager] Missing dialogue text or cutscene lines!");
            yield break;
        }

        float startTime = Time.time;

        foreach (CutsceneLine line in data.lines)
        {
            // Check for skip
            if (skipRequested)
            {
                break;
            }

            if (line.sentences == null || line.sentences.Count == 0)
            {
                Debug.LogWarning("[CutsceneManager] Line has no sentences!");
                continue;
            }

            // Display each sentence individually
            for (int i = 0; i < line.sentences.Count; i++)
            {
                // Check for skip
                if (skipRequested)
                {
                    break;
                }

                SentenceTimestamp sentence = line.sentences[i];

                // Wait until it's time to show this sentence
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < sentence.startTime)
                {
                    // We must check for skip WHILE waiting
                    float waitStart = Time.time;
                    while (Time.time - waitStart < (sentence.startTime - elapsedTime))
                    {
                        if (skipRequested) break;
                        yield return null;
                    }
                    if (skipRequested) break;
                }

                // Check for skip AGAIN after waiting
                if (skipRequested) break;

                // Set the sentence text
                dialogueText.text = sentence.sentence;

                // Fade in the sentence
                yield return StartCoroutine(FadeTextAlpha(0f, 1f, sentence.fadeInDuration));

                // Determine when to fade out this sentence
                float fadeOutTime;

                if (sentence.endTime > 0)
                {
                    // Use explicit end time if set
                    fadeOutTime = sentence.endTime;
                }
                else if (i < line.sentences.Count - 1)
                {
                    // Fade out just before next sentence starts
                    SentenceTimestamp nextSentence = line.sentences[i + 1];
                    fadeOutTime = nextSentence.startTime - 0.3f; // Start fading 0.3s before next sentence
                }
                else
                {
                    // Last sentence in line - keep visible until line fade out
                    fadeOutTime = -1; // Will be handled by line fade out
                }

                // Wait and fade out if needed
                if (fadeOutTime > 0)
                {
                    float currentTime = Time.time - startTime;
                    float waitTime = fadeOutTime - currentTime;

                    if (waitTime > 0)
                    {
                        // We must check for skip WHILE waiting
                        float waitStart = Time.time;
                        while (Time.time - waitStart < waitTime)
                        {
                            if (skipRequested) break;
                            yield return null;
                        }
                        if (skipRequested) break;
                    }

                    if (skipRequested) break;

                    // Fade out this sentence
                    yield return StartCoroutine(FadeTextAlpha(1f, 0f, 0.3f));
                    dialogueText.text = "";
                }
            } // end for loop (sentences)

            if (skipRequested) break;

            // Fade out the last sentence if it's still visible
            if (dialogueText.color.a > 0.1f && line.fadeOutDuration > 0)
            {
                yield return StartCoroutine(FadeTextAlpha(1f, 0f, line.fadeOutDuration));
            }

            // Clear text after fade out
            dialogueText.text = "";

            // Additional pause after line
            if (line.pauseAfterLine > 0)
            {
                // We must check for skip WHILE waiting
                float waitStart = Time.time;
                while (Time.time - waitStart < line.pauseAfterLine)
                {
                    if (skipRequested) break;
                    yield return null;
                }
                if (skipRequested) break;
            }
        } // end foreach loop (lines)

        // Wait for audio to finish if still playing (unless we skipped)
        // This is the skip fix
        if (!skipRequested && data.voiceOverAudio != null)
        {
            float audioLength = data.voiceOverAudio.length;
            float totalElapsed = Time.time - startTime;

            if (totalElapsed < audioLength)
            {
                // We must check for skip WHILE waiting
                float waitStart = Time.time;
                float waitDuration = audioLength - totalElapsed;
                while (Time.time - waitStart < waitDuration)
                {
                    if (skipRequested) break;
                    yield return null;
                }
            }
        }
    }

    IEnumerator FadeTextAlpha(float startAlpha, float endAlpha, float duration)
    {
        if (dialogueText == null) yield break;

        Color color = dialogueText.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (skipRequested && endAlpha == 0f) // Allow skip to hurry up fade-outs
            {
                elapsed = duration;
            }

            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            dialogueText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        dialogueText.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // No skip check here, we WANT this fade to complete
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    void FindPlayerReferences()
    {
        // Added a null check for safety
        if (playerController == null)
        {
            // Use the non-obsolete version of this method
            playerController = FindFirstObjectByType<JoystickPlayerController>();
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

    void DisablePlayerControls()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (joystickUI != null)
        {
            joystickUI.SetActive(false);
        }

        // Notify systems
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.NotifyActionStarted();
        }

        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.PrepareForDialogue();
        }

        Debug.Log("[CutsceneManager] Player controls disabled");
    }

    void EnablePlayerControls()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (joystickUI != null)
        {
            joystickUI.SetActive(true);
        }

        // Notify systems
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.NotifyActionEnded();
        }

        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.DialogueComplete();
        }

        Debug.Log("[CutsceneManager] Player controls enabled");
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    void OnDestroy()
    {
        // Stop dialogue audio if playing (voiceovers use dialogue system)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllDialogue();
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Cleanup
        if (cutsceneCoroutine != null)
        {
            StopCoroutine(cutsceneCoroutine);
        }
    }
}