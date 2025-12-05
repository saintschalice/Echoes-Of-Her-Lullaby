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

    private int lastFoundSlot = -1;
    private bool isTransitioning = false;

    void Start()
    {
        SetupButtons();
        ShowMainMenu();
        UpdateContinueButton();
    }

    void SetupButtons()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);

        if (continueYesButton != null)
            continueYesButton.onClick.AddListener(OnContinueYes);

        if (continueNoButton != null)
            continueNoButton.onClick.AddListener(OnContinueNo);

        if (settingsBackButton != null)
            settingsBackButton.onClick.AddListener(OnSettingsBack);

        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(OnCreditsBack);
    }

    void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (continueConfirmationPanel != null) continueConfirmationPanel.SetActive(false);
        if (saveSlotSelectionPanel != null) saveSlotSelectionPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    void UpdateContinueButton()
    {
        if (continueButton == null) return;

        bool hasSaveFile = CheckForAnySaveFile();
        continueButton.interactable = hasSaveFile;
    }

    bool CheckForAnySaveFile()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");

        if (!Directory.Exists(savePath))
            return false;

        for (int i = 0; i <= 3; i++)
        {
            string filePath = Path.Combine(savePath, $"save_slot_{i}.json");
            if (File.Exists(filePath))
                return true;
        }

        return false;
    }

    void OnNewGameClicked()
    {
        if (isTransitioning) return;

        Debug.Log("[MainMenu] New Game clicked");
        StartCoroutine(StartNewGameWithFade());
    }

    IEnumerator StartNewGameWithFade()
    {
        isTransitioning = true;

        // FIXED: Tell SaveSystem to create a new game via PlayerPrefs
        // This ensures the logic runs AFTER the scene loads in SaveSystem.Start()
        PlayerPrefs.SetInt("LoadSlotOnStart", -1);
        PlayerPrefs.Save();
        Debug.Log("[MainMenu] Queued New Game (-1) for SaveSystem");

        // Use ScreenFader if available
        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;

            // Fade out to black
            ScreenFader.Instance.FadeOut(-1, () => {
                fadeComplete = true;
            });

            // Wait for fade to complete
            while (!fadeComplete)
            {
                yield return null;
            }

            Debug.Log("[MainMenu] Loading PersistentScene");
            SceneManager.LoadScene("PersistentScene");
        }
        else
        {
            // No ScreenFader, load directly
            Debug.LogWarning("[MainMenu] ScreenFader not found, loading without fade");
            SceneManager.LoadScene("PersistentScene");
        }
    }

    void OnContinueClicked()
    {
        Debug.Log("[MainMenu] Continue clicked");

        int mostRecentSlot = GetMostRecentSaveSlot();

        if (mostRecentSlot == -1)
        {
            Debug.LogWarning("[MainMenu] No save files found!");
            return;
        }

        lastFoundSlot = mostRecentSlot;

        GameSaveData saveData = LoadSaveData(mostRecentSlot);

        if (saveData != null && lastSaveInfoText != null)
        {
            string roomName = SaveUIManager.GetRoomDisplayName(saveData.currentScene);
            int hours = Mathf.FloorToInt(saveData.playtimeSeconds / 3600f);
            int minutes = Mathf.FloorToInt((saveData.playtimeSeconds % 3600f) / 60f);

            lastSaveInfoText.text = $"<b>{saveData.saveName}</b>\n" +
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

        Debug.Log("[MainMenu] Continue YES - Loading last save");
        StartCoroutine(ContinueGameWithFade());
    }

    IEnumerator ContinueGameWithFade()
    {
        isTransitioning = true;

        if (lastFoundSlot != -1)
        {
            // FIXED: Do not call SaveSystem.LoadGame here directly.
            // Instead, set the preference so SaveSystem.Start() picks it up in the next scene.
            PlayerPrefs.SetInt("LoadSlotOnStart", lastFoundSlot);
            PlayerPrefs.Save();
            Debug.Log($"[MainMenu] Queued Slot {lastFoundSlot} for SaveSystem");

            // Use ScreenFader if available
            if (ScreenFader.Instance != null)
            {
                bool fadeComplete = false;

                // Fade out to black
                ScreenFader.Instance.FadeOut(-1, () => {
                    fadeComplete = true;
                });

                // Wait for fade to complete
                while (!fadeComplete)
                {
                    yield return null;
                }

                Debug.Log("[MainMenu] Loading PersistentScene");
                SceneManager.LoadScene("PersistentScene");
            }
            else
            {
                // No ScreenFader, load directly
                Debug.LogWarning("[MainMenu] ScreenFader not found, loading without fade");
                SceneManager.LoadScene("PersistentScene");
            }
        }
    }

    void OnContinueNo()
    {
        Debug.Log("[MainMenu] Continue NO - Showing save slot selection");

        if (continueConfirmationPanel != null)
        {
            continueConfirmationPanel.SetActive(false);
            Debug.Log("[MainMenu] ContinueConfirmationPanel hidden");
        }

        if (saveSlotSelectionPanel != null)
        {
            saveSlotSelectionPanel.SetActive(true);
            Debug.Log("[MainMenu] SaveSlotSelectionPanel shown");
        }

        if (SaveUIManager.Instance != null)
        {
            Debug.Log("[MainMenu] Calling SaveUIManager.RefreshSlots()");
            SaveUIManager.Instance.RefreshSlots();
            Debug.Log("[MainMenu] RefreshSlots() completed");
        }
        else
        {
            Debug.LogWarning("[MainMenu] SaveUIManager.Instance is null!");
        }
    }

    void OnSettingsClicked()
    {
        Debug.Log("[MainMenu] Settings clicked");

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OnSettingsBack()
    {
        Debug.Log("[MainMenu] Settings back clicked");
        ShowMainMenu();
    }

    void OnCreditsClicked()
    {
        Debug.Log("[MainMenu] Credits clicked");

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    void OnCreditsBack()
    {
        Debug.Log("[MainMenu] Credits back clicked");
        ShowMainMenu();
    }

    void OnExitClicked()
    {
        if (isTransitioning) return;

        Debug.Log("[MainMenu] Exit clicked");
        StartCoroutine(ExitGameWithFade());
    }

    IEnumerator ExitGameWithFade()
    {
        isTransitioning = true;

        // Use ScreenFader if available
        if (ScreenFader.Instance != null)
        {
            bool fadeComplete = false;

            // Fade out to black
            ScreenFader.Instance.FadeOut(-1, () => {
                fadeComplete = true;
            });

            // Wait for fade to complete
            while (!fadeComplete)
            {
                yield return null;
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    int GetMostRecentSaveSlot()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");

        if (!Directory.Exists(savePath))
            return -1;

        int mostRecentSlot = -1;
        System.DateTime mostRecentTime = System.DateTime.MinValue;

        for (int i = 0; i <= 3; i++)
        {
            string filePath = Path.Combine(savePath, $"save_slot_{i}.json");

            if (File.Exists(filePath))
            {
                GameSaveData saveData = LoadSaveData(i);

                if (saveData != null)
                {
                    System.DateTime saveTime;
                    if (System.DateTime.TryParse(saveData.saveDate, out saveTime))
                    {
                        if (saveTime > mostRecentTime)
                        {
                            mostRecentTime = saveTime;
                            mostRecentSlot = i;
                        }
                    }
                }
            }
        }

        return mostRecentSlot;
    }

    GameSaveData LoadSaveData(int slot)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");
        string filePath = Path.Combine(savePath, $"save_slot_{slot}.json");

        if (!File.Exists(filePath))
            return null;

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[MainMenu] Failed to load save data: {e.Message}");
            return null;
        }
    }

    public void OnSaveSlotClosed()
    {
        Debug.Log("[MainMenu] OnSaveSlotClosed called");

        if (saveSlotSelectionPanel != null)
        {
            saveSlotSelectionPanel.SetActive(false);
            Debug.Log("[MainMenu] SaveSlotSelectionPanel hidden");
        }

        ShowMainMenu();
        Debug.Log("[MainMenu] ShowMainMenu() called");
    }
}