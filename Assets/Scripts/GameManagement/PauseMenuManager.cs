using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("Main Pause Menu Buttons")]
    public Button resumeButton;
    public Button settingsButton;
    public Button saveGameButton;
    public Button mainMenuButton;

    [Header("Settings Buttons")]
    public Button backFromSettingsButton;
    public Button audioTabButton;
    public Button videoTabButton;

    [Header("Audio Settings")]
    public GameObject audioSettingsPanel;
    public Slider sfxVolumeSlider;
    public Slider dialogueVolumeSlider;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI sfxValueText;
    public TextMeshProUGUI dialogueValueText;
    public TextMeshProUGUI musicValueText;

    [Header("Video Settings")]
    public GameObject videoSettingsPanel;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public TextMeshProUGUI brightnessValueText;
    public TextMeshProUGUI contrastValueText;
    public CanvasGroup brightnessOverlay; // For brightness control
    public CanvasGroup contrastOverlay;   // Add separate overlay for contrast

    [Header("Audio Mixer (Optional)")]
    public AudioMixer audioMixer;

    [Header("References")]
    public SaveUIManager saveUIManager;
    public GameObject joystickUI; // Direct reference to joystick UI GameObject

    [Header("Pause Button")]
    public Button pauseButton; // On-screen pause button
    public GameObject pauseButtonObject; // The pause button GameObject to hide/show

    private bool isPaused = false;
    private bool isInSettings = false;
    private CanvasGroup canvasGroup;

    // Audio settings
    private float sfxVolume = 1f;
    private float dialogueVolume = 1f;
    private float musicVolume = 1f;

    // Video settings
    private float brightness = 0.5f;
    private float contrast = 0.5f;

    public static PauseMenuManager Instance { get; private set; }

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

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        SetupUI();
        LoadSettings();
        CreateContrastOverlayIfNeeded();

        // Initially hide all panels
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(true); // Default to audio tab
        videoSettingsPanel.SetActive(false);

        // Disable main menu button since title screen isn't ready
        if (mainMenuButton != null)
        {
            mainMenuButton.interactable = false;
            // Optional: Add tooltip or visual indicator
            var mainMenuText = mainMenuButton.GetComponentInChildren<TextMeshProUGUI>();
            if (mainMenuText != null)
            {
                mainMenuText.color = Color.gray;
                mainMenuText.text = "Main Menu (Coming Soon)";
            }
        }
    }

    void CreateContrastOverlayIfNeeded()
    {
        // If contrast overlay isn't set, create one dynamically
        if (contrastOverlay == null && brightnessOverlay != null)
        {
            // Create a sibling to the brightness overlay
            GameObject contrastObj = new GameObject("ContrastOverlay");
            contrastObj.transform.SetParent(brightnessOverlay.transform.parent);

            // Copy transform properties from brightness overlay
            RectTransform brightRect = brightnessOverlay.GetComponent<RectTransform>();
            RectTransform contrastRect = contrastObj.AddComponent<RectTransform>();
            contrastRect.anchorMin = Vector2.zero;
            contrastRect.anchorMax = Vector2.one;
            contrastRect.sizeDelta = Vector2.zero;
            contrastRect.anchoredPosition = Vector2.zero;

            // Add Image component
            Image contrastImage = contrastObj.AddComponent<Image>();
            contrastImage.color = Color.gray;
            contrastImage.raycastTarget = false;

            // Add CanvasGroup
            contrastOverlay = contrastObj.AddComponent<CanvasGroup>();
            contrastOverlay.alpha = 0f;
            contrastOverlay.interactable = false;
            contrastOverlay.blocksRaycasts = false;

            Debug.Log("Created ContrastOverlay dynamically");
        }
    }

    void Update()
    {
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                if (isInSettings)
                {
                    BackFromSettings();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    void SetupUI()
    {
        // Pause button
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners(); // Clear any existing listeners
            pauseButton.onClick.AddListener(() => {
                Debug.Log("Pause button clicked!");
                PauseGame();
            });
            Debug.Log("Pause button listener added successfully");
        }
        else
        {
            Debug.LogWarning("Pause button is null in SetupUI!");
        }

        // Main pause menu buttons
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (saveGameButton != null)
            saveGameButton.onClick.AddListener(OpenSaveMenu);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Settings navigation
        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(BackFromSettings);

        if (audioTabButton != null)
            audioTabButton.onClick.AddListener(ShowAudioSettings);

        if (videoTabButton != null)
            videoTabButton.onClick.AddListener(ShowVideoSettings);

        // Audio sliders
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxVolumeSlider.value = sfxVolume;
        }

        if (dialogueVolumeSlider != null)
        {
            dialogueVolumeSlider.onValueChanged.AddListener(SetDialogueVolume);
            dialogueVolumeSlider.value = dialogueVolume;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            musicVolumeSlider.value = musicVolume;
        }

        // Video sliders
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            brightnessSlider.value = brightness;
        }

        if (contrastSlider != null)
        {
            contrastSlider.onValueChanged.AddListener(SetContrast);
            contrastSlider.value = contrast;
        }

        UpdateAllDisplayTexts();
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        // For mobile/2D games, keep cursor visible
        Cursor.visible = true;

        // Hide joystick UI during pause
        if (joystickUI != null)
            joystickUI.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        isInSettings = false;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;

        // Keep cursor visible for mobile/2D gameplay
        Cursor.visible = true;

        // Show joystick UI when resumed
        if (joystickUI != null)
            joystickUI.SetActive(true);

        SaveSettings();
    }

    public void OpenSettings()
    {
        isInSettings = true;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        ShowAudioSettings(); // Default to audio tab
    }

    public void BackFromSettings()
    {
        isInSettings = false;
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        SaveSettings();
    }

    public void OpenSaveMenu()
    {
        if (saveUIManager != null)
        {
            // Hide pause menu temporarily
            pauseMenuPanel.SetActive(false);

            // Open save/load panel
            saveUIManager.OpenSaveLoadPanel();

            // The save UI manager handles time scale, so we don't change it here
        }
        else
        {
            Debug.LogWarning("SaveUIManager reference not set in PauseMenuManager!");
        }
    }

    public void GoToMainMenu()
    {
        // Placeholder for when main menu is ready
        Debug.Log("Main Menu not implemented yet");

        // When ready, this would be:
        // Time.timeScale = 1f;
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Settings Tab Management
    public void ShowAudioSettings()
    {
        audioSettingsPanel.SetActive(true);
        videoSettingsPanel.SetActive(false);

        // Update button states
        SetTabButtonState(audioTabButton, true);
        SetTabButtonState(videoTabButton, false);
    }

    public void ShowVideoSettings()
    {
        audioSettingsPanel.SetActive(false);
        videoSettingsPanel.SetActive(true);

        // Update button states
        SetTabButtonState(audioTabButton, false);
        SetTabButtonState(videoTabButton, true);
    }

    void SetTabButtonState(Button button, bool isActive)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = isActive ? Color.white : Color.gray;
        button.colors = colors;
    }

    // Audio Settings
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        if (audioMixer != null)
        {
            // Convert to logarithmic scale for audio mixer
            float dbValue = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("SFXVolume", dbValue);
        }
        else
        {
            // Fallback: adjust AudioSource volumes directly
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                if (source.gameObject.CompareTag("SFX"))
                {
                    source.volume = volume;
                }
            }
        }

        UpdateSFXValueText();
    }

    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = volume;

        if (audioMixer != null)
        {
            float dbValue = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("DialogueVolume", dbValue);
        }

        UpdateDialogueValueText();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        if (audioMixer != null)
        {
            float dbValue = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("MusicVolume", dbValue);
        }
        else
        {
            // Fallback: adjust music AudioSource volumes
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                if (source.gameObject.CompareTag("Music"))
                {
                    source.volume = volume;
                }
            }
        }

        UpdateMusicValueText();
    }

    // Video Settings
    public void SetBrightness(float value)
    {
        brightness = value;

        if (brightnessOverlay != null)
        {
            Image overlayImage = brightnessOverlay.GetComponent<Image>();

            if (brightness < 0.5f)
            {
                // Darken screen: black overlay
                overlayImage.color = Color.black;
                float alpha = (0.5f - brightness) * 1.6f; // 0 to 0.8 alpha
                brightnessOverlay.alpha = alpha;
            }
            else if (brightness > 0.5f)
            {
                // Brighten screen: white overlay
                overlayImage.color = Color.white;
                float alpha = (brightness - 0.5f) * 0.8f; // 0 to 0.4 alpha
                brightnessOverlay.alpha = alpha;
            }
            else
            {
                // Normal brightness: no overlay
                brightnessOverlay.alpha = 0f;
            }
        }

        UpdateBrightnessValueText();
    }

    public void SetContrast(float value)
    {
        contrast = value;

        // Use a separate contrast overlay system
        if (contrastOverlay != null)
        {
            Image overlayImage = contrastOverlay.GetComponent<Image>();

            if (contrast < 0.5f)
            {
                // Low contrast: gray overlay reduces color differences
                overlayImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Gray
                float alpha = (0.5f - contrast) * 0.6f; // 0 to 0.3 alpha for subtle effect
                contrastOverlay.alpha = alpha;
            }
            else if (contrast > 0.5f)
            {
                // High contrast: use multiply blend mode effect (simulated with darker edges)
                // For higher contrast, we darken the overlay slightly to increase contrast
                overlayImage.color = Color.black;
                float alpha = (contrast - 0.5f) * 0.2f; // Very subtle darkening at edges
                contrastOverlay.alpha = alpha;

                // Alternative: Apply a vignette-like effect for high contrast
                // You could also modify the overlay to have a gradient texture
            }
            else
            {
                // Normal contrast: no overlay
                contrastOverlay.alpha = 0f;
            }
        }
        else
        {
            Debug.LogWarning("ContrastOverlay not set! Contrast adjustments won't be visible.");
        }

        UpdateContrastValueText();
    }

    // Update display texts
    void UpdateAllDisplayTexts()
    {
        UpdateSFXValueText();
        UpdateDialogueValueText();
        UpdateMusicValueText();
        UpdateBrightnessValueText();
        UpdateContrastValueText();
    }

    void UpdateSFXValueText()
    {
        if (sfxValueText != null)
            sfxValueText.text = Mathf.RoundToInt(sfxVolume * 100) + "%";
    }

    void UpdateDialogueValueText()
    {
        if (dialogueValueText != null)
            dialogueValueText.text = Mathf.RoundToInt(dialogueVolume * 100) + "%";
    }

    void UpdateMusicValueText()
    {
        if (musicValueText != null)
            musicValueText.text = Mathf.RoundToInt(musicVolume * 100) + "%";
    }

    void UpdateBrightnessValueText()
    {
        if (brightnessValueText != null)
        {
            int displayValue = Mathf.RoundToInt(brightness * 100);
            brightnessValueText.text = displayValue + "%";
        }
    }

    void UpdateContrastValueText()
    {
        if (contrastValueText != null)
            contrastValueText.text = Mathf.RoundToInt(contrast * 100) + "%";
    }

    // Settings persistence
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("DialogueVolume", dialogueVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.SetFloat("Contrast", contrast);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        contrast = PlayerPrefs.GetFloat("Contrast", 0.5f);

        // Apply loaded settings
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
        if (dialogueVolumeSlider != null) dialogueVolumeSlider.value = dialogueVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (brightnessSlider != null) brightnessSlider.value = brightness;
        if (contrastSlider != null) contrastSlider.value = contrast;

        // Apply settings immediately
        SetSFXVolume(sfxVolume);
        SetDialogueVolume(dialogueVolume);
        SetMusicVolume(musicVolume);
        SetBrightness(brightness);
        SetContrast(contrast);
    }

    // Public getter for pause state
    public bool IsPaused()
    {
        return isPaused;
    }

    // Method to be called when save menu is closed
    public void OnSaveMenuClosed()
    {
        // Show pause menu again when save menu is closed
        if (isPaused)
        {
            pauseMenuPanel.SetActive(true);
        }
    }
}