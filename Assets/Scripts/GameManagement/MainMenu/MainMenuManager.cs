using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject continueConfirmationPanel;
    public GameObject saveSlotSelectionPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Main Menu Buttons")]
    public Button newGameButton;
    public Button continueButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitButton;

    [Header("Continue Confirmation Buttons")]
    public Button continueYesButton;
    public Button continueNoButton;
    public TextMeshProUGUI lastSaveInfoText;

    [Header("Settings/Credits Close Buttons")]
    public Button settingsBackButton;
    public Button creditsBackButton;

    [Header("Settings Integration")]
    public Button audioTabButton;
    public Button videoTabButton;
    public GameObject audioSettingsPanel;
    public GameObject videoSettingsPanel;

    [Header("Audio Sliders")]
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

    [Header("Video Sliders")]
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public TextMeshProUGUI brightnessValueText;
    public TextMeshProUGUI contrastValueText;
    public CanvasGroup brightnessOverlay;
    public CanvasGroup contrastOverlay;

    private int lastFoundSlot = -1;
    private bool isTransitioning = false;

    // Settings Data
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float dialogueVolume = 1f;
    private float musicVolume = 1f;
    private float ambientVolume = 1f;
    private float brightness = 0.5f;
    private float contrast = 0.5f;

    void Start()
    {
        // SAFETY CHECK: Ensure SaveSystem exists. 
        // If we started directly in Scene 2, the persistent SaveSystem might not exist yet.
        if (SaveSystem.Instance == null)
        {
            Debug.LogWarning("[MainMenu] SaveSystem not found! Creating temporary instance...");
            GameObject saveSys = new GameObject("SaveSystem_AutoCreated");
            saveSys.AddComponent<SaveSystem>();
        }

        SetupButtons();
        ShowMainMenu();
        UpdateContinueButton();

        // Initialize Settings
        LoadSettings();
        SetupSettingsUI();
    }

    void SetupButtons()
    {
        if (newGameButton != null) newGameButton.onClick.AddListener(OnNewGameClicked);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinueClicked);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsClicked);
        if (creditsButton != null) creditsButton.onClick.AddListener(OnCreditsClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);

        if (continueYesButton != null) continueYesButton.onClick.AddListener(OnContinueYes);
        if (continueNoButton != null) continueNoButton.onClick.AddListener(OnContinueNo);

        if (settingsBackButton != null) settingsBackButton.onClick.AddListener(OnSettingsBack);
        if (creditsBackButton != null) creditsBackButton.onClick.AddListener(OnCreditsBack);
    }

    // --- NEW SETTINGS LOGIC ---
    void SetupSettingsUI()
    {
        if (audioTabButton != null) audioTabButton.onClick.AddListener(ShowAudioSettings);
        if (videoTabButton != null) videoTabButton.onClick.AddListener(ShowVideoSettings);

        if (masterVolumeSlider != null) { masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume); masterVolumeSlider.value = masterVolume; }
        if (sfxVolumeSlider != null) { sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); sfxVolumeSlider.value = sfxVolume; }
        if (dialogueVolumeSlider != null) { dialogueVolumeSlider.onValueChanged.AddListener(SetDialogueVolume); dialogueVolumeSlider.value = dialogueVolume; }
        if (musicVolumeSlider != null) { musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume); musicVolumeSlider.value = musicVolume; }
        if (ambientVolumeSlider != null) { ambientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume); ambientVolumeSlider.value = ambientVolume; }

        if (brightnessSlider != null) { brightnessSlider.onValueChanged.AddListener(SetBrightness); brightnessSlider.value = brightness; }
        if (contrastSlider != null) { contrastSlider.onValueChanged.AddListener(SetContrast); contrastSlider.value = contrast; }

        UpdateAllDisplayTexts();
    }

    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (continueConfirmationPanel != null) continueConfirmationPanel.SetActive(false);
        if (saveSlotSelectionPanel != null) saveSlotSelectionPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        UpdateContinueButton();
    }

    void UpdateContinueButton()
    {
        if (continueButton == null) return;

        // Use SaveSystem to check for files instead of local logic
        bool hasSaveFile = false;
        if (SaveSystem.Instance != null)
        {
            hasSaveFile = SaveSystem.Instance.HasAnySaveFile();
        }

        continueButton.interactable = hasSaveFile;

        // Optional: Make button look transparent if disabled
        CanvasGroup btnGroup = continueButton.GetComponent<CanvasGroup>();
        if (btnGroup != null)
        {
            btnGroup.alpha = hasSaveFile ? 1f : 0.5f;
        }
    }

    void OnNewGameClicked()
    {
        if (isTransitioning) return;
        StartCoroutine(StartNewGameWithFade());
    }

    IEnumerator StartNewGameWithFade()
    {
        isTransitioning = true;

        // Signal to SaveSystem (via PlayerPrefs or direct call) that we want a new game
        // We can use the same method SaveUIManager uses:
        PlayerPrefs.SetInt("LoadSlotOnStart", -1);

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.CreateNewGame();
        }

        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;
            ScreenFader.Instance.FadeOut(-1, () => { fadeComplete = true; });
            while (!fadeComplete) yield return null;
            SceneManager.LoadScene("PersistentScene");
        }
        else
        {
            SceneManager.LoadScene("PersistentScene");
        }
    }

    void OnContinueClicked()
    {
        Debug.Log("[MainMenu] Continue clicked");

        if (SaveSystem.Instance == null) return;

        // 1. Ask SaveSystem for the newest slot directly
        int mostRecentSlot = SaveSystem.Instance.GetMostRecentSaveSlot();

        if (mostRecentSlot == -1)
        {
            Debug.LogWarning("[MainMenu] No save files found!");
            return;
        }

        lastFoundSlot = mostRecentSlot;

        // 2. Get the data for that slot to display info
        GameSaveData saveData = SaveSystem.Instance.GetSaveInfo(mostRecentSlot);

        if (saveData != null && lastSaveInfoText != null)
        {
            string roomName = SaveUIManager.GetRoomDisplayName(saveData.currentScene);

            int hours = Mathf.FloorToInt(saveData.playtimeSeconds / 3600f);
            int minutes = Mathf.FloorToInt((saveData.playtimeSeconds % 3600f) / 60f);

            lastSaveInfoText.text = $"<b>CONTINUE FROM LAST SAVE?</b>\n\n" +
                                   $"<size=80%>{saveData.saveName}</size>\n" +
                                   $"Location: {roomName}\n" +
                                   $"Playtime: {hours:00}:{minutes:00}\n" +
                                   $"Saved: {saveData.saveDate}";
        }

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (continueConfirmationPanel != null) continueConfirmationPanel.SetActive(true);
    }

    void OnContinueYes()
    {
        if (isTransitioning) return;

        Debug.Log($"[MainMenu] Continue YES - Loading slot {lastFoundSlot}");
        StartCoroutine(ContinueGameWithFade());
    }

    IEnumerator ContinueGameWithFade()
    {
        isTransitioning = true;

        if (lastFoundSlot != -1)
        {
            // Set the flag so PersistentScene knows which slot to load
            PlayerPrefs.SetInt("LoadSlotOnStart", lastFoundSlot);

            if (ScreenFader.Instance != null)
            {
                bool fadeComplete = false;
                ScreenFader.Instance.FadeOut(-1, () => { fadeComplete = true; });
                while (!fadeComplete) yield return null;
                SceneManager.LoadScene("PersistentScene");
            }
            else
            {
                SceneManager.LoadScene("PersistentScene");
            }
        }
        else
        {
            isTransitioning = false;
        }
    }

    void OnContinueNo()
    {
        if (continueConfirmationPanel != null) continueConfirmationPanel.SetActive(false);
        if (saveSlotSelectionPanel != null)
        {
            saveSlotSelectionPanel.SetActive(true);

            // Refresh slots via SaveUIManager if it exists
            if (SaveUIManager.Instance != null)
            {
                SaveUIManager.Instance.RefreshSlots();
            }
        }
    }

    // --- SETTINGS NAVIGATION ---

    void OnSettingsClicked()
    {
        Debug.Log("[MainMenu] Settings clicked");
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            ShowAudioSettings();
        }
    }

    public void OnSettingsBack()
    {
        SaveSettings();
        Debug.Log("[MainMenu] Settings back clicked");
        ShowMainMenu();
    }

    void OnCreditsClicked()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    void OnCreditsBack()
    {
        ShowMainMenu();
    }

    void OnExitClicked()
    {
        if (isTransitioning) return;
        StartCoroutine(ExitGameWithFade());
    }

    IEnumerator ExitGameWithFade()
    {
        isTransitioning = true;
        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;
            ScreenFader.Instance.FadeOut(-1, () => { fadeComplete = true; });
            while (!fadeComplete) yield return null;
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnSaveSlotClosed()
    {
        if (saveSlotSelectionPanel != null) saveSlotSelectionPanel.SetActive(false);
        ShowMainMenu();
    }

    // --- SETTINGS IMPLEMENTATION ---

    public void ShowAudioSettings()
    {
        if (audioSettingsPanel) audioSettingsPanel.SetActive(true);
        if (videoSettingsPanel) videoSettingsPanel.SetActive(false);
        SetTabButtonState(audioTabButton, true);
        SetTabButtonState(videoTabButton, false);
    }

    public void ShowVideoSettings()
    {
        if (audioSettingsPanel) audioSettingsPanel.SetActive(false);
        if (videoSettingsPanel) videoSettingsPanel.SetActive(true);
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

    // Volume Setters
    public void SetMasterVolume(float volume) { masterVolume = volume; if (AudioManager.Instance != null) AudioManager.Instance.SetMasterVolume(volume); UpdateMasterValueText(); }
    public void SetSFXVolume(float volume) { sfxVolume = volume; if (AudioManager.Instance != null) AudioManager.Instance.SetSFXVolume(volume); UpdateSFXValueText(); }
    public void SetDialogueVolume(float volume) { dialogueVolume = volume; if (AudioManager.Instance != null) AudioManager.Instance.SetDialogueVolume(volume); UpdateDialogueValueText(); }
    public void SetMusicVolume(float volume) { musicVolume = volume; if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(volume); UpdateMusicValueText(); }
    public void SetAmbientVolume(float volume) { ambientVolume = volume; if (AudioManager.Instance != null) AudioManager.Instance.SetAmbientVolume(volume); UpdateAmbientValueText(); }

    // Video Setters
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
            else brightnessOverlay.alpha = 0f;
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
            else contrastOverlay.alpha = 0f;
        }
        UpdateContrastValueText();
    }

    // Text Updaters
    void UpdateAllDisplayTexts()
    {
        UpdateMasterValueText(); UpdateSFXValueText(); UpdateDialogueValueText();
        UpdateMusicValueText(); UpdateAmbientValueText();
        UpdateBrightnessValueText(); UpdateContrastValueText();
    }

    void UpdateMasterValueText() { if (masterValueText != null) masterValueText.text = Mathf.RoundToInt(masterVolume * 100) + "%"; }
    void UpdateSFXValueText() { if (sfxValueText != null) sfxValueText.text = Mathf.RoundToInt(sfxVolume * 100) + "%"; }
    void UpdateDialogueValueText() { if (dialogueValueText != null) dialogueValueText.text = Mathf.RoundToInt(dialogueVolume * 100) + "%"; }
    void UpdateMusicValueText() { if (musicValueText != null) musicValueText.text = Mathf.RoundToInt(musicVolume * 100) + "%"; }
    void UpdateAmbientValueText() { if (ambientValueText != null) ambientValueText.text = Mathf.RoundToInt(ambientVolume * 100) + "%"; }
    void UpdateBrightnessValueText() { if (brightnessValueText != null) brightnessValueText.text = Mathf.RoundToInt(brightness * 100) + "%"; }
    void UpdateContrastValueText() { if (contrastValueText != null) contrastValueText.text = Mathf.RoundToInt(contrast * 100) + "%"; }

    // Save/Load
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
    }
}