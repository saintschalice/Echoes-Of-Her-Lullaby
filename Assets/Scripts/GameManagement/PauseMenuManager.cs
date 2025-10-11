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

    [Header("Video Settings")]
    public GameObject videoSettingsPanel;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public TextMeshProUGUI brightnessValueText;
    public TextMeshProUGUI contrastValueText;
    public CanvasGroup brightnessOverlay;
    public CanvasGroup contrastOverlay;

    [Header("References")]
    public SaveUIManager saveUIManager;
    public GameObject joystickUI;

    [Header("Pause Button")]
    public Button pauseButton;
    public GameObject pauseButtonObject;

    private bool isPaused = false;
    private bool isInSettings = false;
    private CanvasGroup canvasGroup;

    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float dialogueVolume = 1f;
    private float musicVolume = 1f;
    private float ambientVolume = 1f;
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
        FindReferences();
        SetupUI();
        LoadSettings();
        CreateContrastOverlayIfNeeded();

        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(true);
        videoSettingsPanel.SetActive(false);

        if (mainMenuButton != null)
        {
            mainMenuButton.interactable = false;
            var mainMenuText = mainMenuButton.GetComponentInChildren<TextMeshProUGUI>();
            if (mainMenuText != null)
            {
                mainMenuText.color = Color.gray;
                mainMenuText.text = "Main Menu (Coming Soon)";
            }
        }

        Debug.Log("[PauseMenu] PauseMenuManager initialized with AudioManager integration");
    }

    void FindReferences()
    {
        // Find SaveUIManager
        if (saveUIManager == null)
        {
            saveUIManager = SaveUIManager.Instance;

            if (saveUIManager == null)
            {
                saveUIManager = FindFirstObjectByType<SaveUIManager>();
            }

            if (saveUIManager == null)
            {
                GameObject saveUIObj = GameObject.Find("SaveUIManager");
                if (saveUIObj != null)
                {
                    saveUIManager = saveUIObj.GetComponent<SaveUIManager>();
                }
            }

            if (saveUIManager == null)
            {
                Debug.LogError("[PauseMenu] SaveUIManager not found!");
            }
            else
            {
                Debug.Log("[PauseMenu] SaveUIManager found: " + saveUIManager.gameObject.name);
            }
        }

        // Find Joystick UI
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

            if (joystickUI != null)
            {
                Debug.Log("[PauseMenu] Joystick UI found successfully!");
            }
        }
    }

    void CreateContrastOverlayIfNeeded()
    {
        if (contrastOverlay == null && brightnessOverlay != null)
        {
            GameObject contrastObj = new GameObject("ContrastOverlay");
            contrastObj.transform.SetParent(brightnessOverlay.transform.parent);

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

            Debug.Log("[PauseMenu] Created ContrastOverlay dynamically");
        }
    }

    void Update()
    {
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
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(() => {
                Debug.Log("[PauseMenu] Pause button clicked!");
                PauseGame();
            });
        }

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (saveGameButton != null)
            saveGameButton.onClick.AddListener(OpenSaveMenu);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(BackFromSettings);

        if (audioTabButton != null)
            audioTabButton.onClick.AddListener(ShowAudioSettings);

        if (videoTabButton != null)
            videoTabButton.onClick.AddListener(ShowVideoSettings);

        // Setup audio sliders
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

        // Setup video sliders
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
        Cursor.visible = true;

        if (joystickUI != null)
            joystickUI.SetActive(false);

        // Force close inventory when pause menu opens
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.ForceCloseInventory();
        }

        Debug.Log("[PauseMenu] Game paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        isInSettings = false;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = true;

        if (joystickUI != null)
            joystickUI.SetActive(true);

        SaveSettings();
        Debug.Log("[PauseMenu] Game resumed");
    }

    public void OpenSettings()
    {
        isInSettings = true;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        ShowAudioSettings();
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
        if (saveUIManager == null)
        {
            Debug.LogWarning("[PauseMenu] SaveUIManager was null, attempting to find it...");
            FindReferences();
        }

        if (saveUIManager != null)
        {
            pauseMenuPanel.SetActive(false);
            saveUIManager.OpenSaveLoadPanel();
            Debug.Log("[PauseMenu] Opened save menu successfully");
        }
        else
        {
            Debug.LogError("[PauseMenu] SaveUIManager reference not set!");

            DialogueSystemV2 dialogueSystem = DialogueSystemV2.Instance;
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue("Save system not available.", "System");
            }
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("[PauseMenu] Main Menu not implemented yet");
    }

    public void ShowAudioSettings()
    {
        audioSettingsPanel.SetActive(true);
        videoSettingsPanel.SetActive(false);
        SetTabButtonState(audioTabButton, true);
        SetTabButtonState(videoTabButton, false);
    }

    public void ShowVideoSettings()
    {
        audioSettingsPanel.SetActive(false);
        videoSettingsPanel.SetActive(true);
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

    // NEW: Master Volume Control
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(volume);
        }

        UpdateMasterValueText();
    }

    // NEW: Updated to use AudioManager
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }

        UpdateSFXValueText();
    }

    // NEW: Updated to use AudioManager
    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = volume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetDialogueVolume(volume);
        }

        UpdateDialogueValueText();
    }

    // NEW: Updated to use AudioManager
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }

        UpdateMusicValueText();
    }

    // NEW: Ambient Volume Control
    public void SetAmbientVolume(float volume)
    {
        ambientVolume = volume;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetAmbientVolume(volume);
        }

        UpdateAmbientValueText();
    }

    public void SetBrightness(float value)
    {
        brightness = value;

        if (brightnessOverlay != null)
        {
            Image overlayImage = brightnessOverlay.GetComponent<Image>();

            if (brightness < 0.5f)
            {
                overlayImage.color = Color.black;
                float alpha = (0.5f - brightness) * 1.6f;
                brightnessOverlay.alpha = alpha;
            }
            else if (brightness > 0.5f)
            {
                overlayImage.color = Color.white;
                float alpha = (brightness - 0.5f) * 0.8f;
                brightnessOverlay.alpha = alpha;
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
                float alpha = (0.5f - contrast) * 0.6f;
                contrastOverlay.alpha = alpha;
            }
            else if (contrast > 0.5f)
            {
                overlayImage.color = Color.black;
                float alpha = (contrast - 0.5f) * 0.2f;
                contrastOverlay.alpha = alpha;
            }
            else
            {
                contrastOverlay.alpha = 0f;
            }
        }

        UpdateContrastValueText();
    }

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

    void UpdateMasterValueText()
    {
        if (masterValueText != null)
            masterValueText.text = Mathf.RoundToInt(masterVolume * 100) + "%";
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

    void UpdateAmbientValueText()
    {
        if (ambientValueText != null)
            ambientValueText.text = Mathf.RoundToInt(ambientVolume * 100) + "%";
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

        Debug.Log("[PauseMenu] Settings saved");
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

        if (masterVolumeSlider != null) masterVolumeSlider.value = masterVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = sfxVolume;
        if (dialogueVolumeSlider != null) dialogueVolumeSlider.value = dialogueVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = musicVolume;
        if (ambientVolumeSlider != null) ambientVolumeSlider.value = ambientVolume;
        if (brightnessSlider != null) brightnessSlider.value = brightness;
        if (contrastSlider != null) contrastSlider.value = contrast;

        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetDialogueVolume(dialogueVolume);
        SetMusicVolume(musicVolume);
        SetAmbientVolume(ambientVolume);
        SetBrightness(brightness);
        SetContrast(contrast);

        Debug.Log("[PauseMenu] Settings loaded");
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void OnSaveMenuClosed()
    {
        if (isPaused)
        {
            pauseMenuPanel.SetActive(true);
        }
    }
}