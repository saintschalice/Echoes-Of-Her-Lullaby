using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMainMenuManager : MonoBehaviour
{
    [Header("Main Menu Panels")]
    [Tooltip("The panel containing buttons like New Game, Load, etc.")]
    public GameObject mainMenuPanel;
    [Tooltip("The panel containing the settings UI.")]
    public GameObject settingsPanel;

    [Header("Navigation Buttons")]
    public Button backButton; // The button inside settings to return to main menu
    public Button audioTabButton;
    public Button videoTabButton;

    [Header("Audio Settings UI")]
    public GameObject audioSettingsPanel;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider dialogueVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambientVolumeSlider;
    public TextMeshProUGUI masterValueText;
    public TextMeshProUGUI sfxValueText;
    public TextMeshProUGUI dialogueValueText;
    public TextMeshProUGUI musicValueText;
    public TextMeshProUGUI ambientValueText;

    [Header("Video Settings UI")]
    public GameObject videoSettingsPanel;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public TextMeshProUGUI brightnessValueText;
    public TextMeshProUGUI contrastValueText;

    [Header("Visual Overlays")]
    [Tooltip("Reference to the UI Image/Panel acting as brightness overlay")]
    public CanvasGroup brightnessOverlay;
    [Tooltip("Reference to the UI Image/Panel acting as contrast overlay (Optional - can be auto-generated)")]
    public CanvasGroup contrastOverlay;

    // Internal state
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float dialogueVolume = 1f;
    private float musicVolume = 1f;
    private float ambientVolume = 1f;
    private float brightness = 0.5f;
    private float contrast = 0.5f;

    void Start()
    {
        SetupUI();
        LoadSettings();
        CreateContrastOverlayIfNeeded();

        // Ensure correct initial state
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    void SetupUI()
    {
        // Navigation
        if (backButton != null)
            backButton.onClick.AddListener(CloseSettings);

        if (audioTabButton != null)
            audioTabButton.onClick.AddListener(ShowAudioSettings);

        if (videoTabButton != null)
            videoTabButton.onClick.AddListener(ShowVideoSettings);

        // Audio Sliders
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            masterVolumeSlider.value = masterVolume;
        }

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

        if (ambientVolumeSlider != null)
        {
            ambientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume);
            ambientVolumeSlider.value = ambientVolume;
        }

        // Video Sliders
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
    }

    // --- Public Methods for Main Menu Buttons ---

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        ShowAudioSettings(); // Default to Audio tab
        UpdateAllDisplayTexts();
    }

    public void CloseSettings()
    {
        SaveSettings(); // Save when leaving the menu

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // --- Tabs ---

    public void ShowAudioSettings()
    {
        if (audioSettingsPanel != null) audioSettingsPanel.SetActive(true);
        if (videoSettingsPanel != null) videoSettingsPanel.SetActive(false);
        SetTabButtonState(audioTabButton, true);
        SetTabButtonState(videoTabButton, false);
    }

    public void ShowVideoSettings()
    {
        if (audioSettingsPanel != null) audioSettingsPanel.SetActive(false);
        if (videoSettingsPanel != null) videoSettingsPanel.SetActive(true);
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

    // --- Audio Logic (Mirrors PauseMenuManager) ---

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        if (AudioManager.Instance != null) AudioManager.Instance.SetMasterVolume(volume);
        UpdateMasterValueText();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (AudioManager.Instance != null) AudioManager.Instance.SetSFXVolume(volume);
        UpdateSFXValueText();
    }

    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = volume;
        if (AudioManager.Instance != null) AudioManager.Instance.SetDialogueVolume(volume);
        UpdateDialogueValueText();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(volume);
        UpdateMusicValueText();
    }

    public void SetAmbientVolume(float volume)
    {
        ambientVolume = volume;
        if (AudioManager.Instance != null) AudioManager.Instance.SetAmbientVolume(volume);
        UpdateAmbientValueText();
    }

    // --- Video Logic (Mirrors PauseMenuManager) ---

    public void SetBrightness(float value)
    {
        brightness = value;
        if (brightnessOverlay != null)
        {
            Image overlayImage = brightnessOverlay.GetComponent<Image>();
            if (brightness < 0.5f)
            {
                overlayImage.color = Color.black;
                brightnessOverlay.alpha = (0.5f - brightness) * 1.6f;
            }
            else if (brightness > 0.5f)
            {
                overlayImage.color = Color.white;
                brightnessOverlay.alpha = (brightness - 0.5f) * 0.8f;
            }
            else
            {
                brightnessOverlay.alpha = 0f;
            }
        }
        UpdateBrightnessValueText();
    }

    public void SetContrast(float value)
    {
        contrast = value;
        if (contrastOverlay != null)
        {
            Image overlayImage = contrastOverlay.GetComponent<Image>();
            if (contrast < 0.5f)
            {
                overlayImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                contrastOverlay.alpha = (0.5f - contrast) * 0.6f;
            }
            else if (contrast > 0.5f)
            {
                overlayImage.color = Color.black;
                contrastOverlay.alpha = (contrast - 0.5f) * 0.2f;
            }
            else
            {
                contrastOverlay.alpha = 0f;
            }
        }
        UpdateContrastValueText();
    }

    void CreateContrastOverlayIfNeeded()
    {
        // If we have a brightness overlay but no contrast overlay, create one automatically
        if (contrastOverlay == null && brightnessOverlay != null)
        {
            GameObject contrastObj = new GameObject("ContrastOverlay_Generated");
            contrastObj.transform.SetParent(brightnessOverlay.transform.parent);
            // Ensure it renders on top or below based on hierarchy preference
            contrastObj.transform.SetSiblingIndex(brightnessOverlay.transform.GetSiblingIndex() + 1);

            RectTransform contrastRect = contrastObj.AddComponent<RectTransform>();
            contrastRect.anchorMin = Vector2.zero;
            contrastRect.anchorMax = Vector2.one;
            contrastRect.sizeDelta = Vector2.zero;
            contrastRect.anchoredPosition = Vector2.zero;

            Image contrastImage = contrastObj.AddComponent<Image>();
            contrastImage.color = Color.gray;
            contrastImage.raycastTarget = false;

            contrastOverlay = contrastObj.AddComponent<CanvasGroup>();
            contrastOverlay.alpha = 0f;
            contrastOverlay.interactable = false;
            contrastOverlay.blocksRaycasts = false;
        }
    }

    // --- UI Updates ---

    void UpdateAllDisplayTexts()
    {
        UpdateMasterValueText();
        UpdateSFXValueText();
        UpdateDialogueValueText();
        UpdateMusicValueText();
        UpdateAmbientValueText();
        UpdateBrightnessValueText();
        UpdateContrastValueText();
    }

    void UpdateMasterValueText() { if (masterValueText != null) masterValueText.text = Mathf.RoundToInt(masterVolume * 100) + "%"; }
    void UpdateSFXValueText() { if (sfxValueText != null) sfxValueText.text = Mathf.RoundToInt(sfxVolume * 100) + "%"; }
    void UpdateDialogueValueText() { if (dialogueValueText != null) dialogueValueText.text = Mathf.RoundToInt(dialogueVolume * 100) + "%"; }
    void UpdateMusicValueText() { if (musicValueText != null) musicValueText.text = Mathf.RoundToInt(musicVolume * 100) + "%"; }
    void UpdateAmbientValueText() { if (ambientValueText != null) ambientValueText.text = Mathf.RoundToInt(ambientVolume * 100) + "%"; }
    void UpdateBrightnessValueText() { if (brightnessValueText != null) brightnessValueText.text = Mathf.RoundToInt(brightness * 100) + "%"; }
    void UpdateContrastValueText() { if (contrastValueText != null) contrastValueText.text = Mathf.RoundToInt(contrast * 100) + "%"; }

    // --- Save/Load System ---

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("DialogueVolume", dialogueVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.SetFloat("Contrast", contrast);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);
        brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        contrast = PlayerPrefs.GetFloat("Contrast", 0.5f);

        // Update Sliders
        if (masterVolumeSlider != null) masterVolumeSlider.value = masterVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
        if (dialogueVolumeSlider != null) dialogueVolumeSlider.value = dialogueVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (ambientVolumeSlider != null) ambientVolumeSlider.value = ambientVolume;
        if (brightnessSlider != null) brightnessSlider.value = brightness;
        if (contrastSlider != null) contrastSlider.value = contrast;

        // Apply to Systems
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetDialogueVolume(dialogueVolume);
        SetMusicVolume(musicVolume);
        SetAmbientVolume(ambientVolume);
        SetBrightness(brightness);
        SetContrast(contrast);

        UpdateAllDisplayTexts();
    }
}