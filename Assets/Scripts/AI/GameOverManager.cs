using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Manages game over state when Emily catches Lisa
/// </summary>
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Audio")]
    public AudioClip deathSound;

    private void Awake()
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

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        SetupButtons();
    }

    void SetupButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void TriggerGameOver(string message = "You were caught...")
    {
        if (gameOverPanel == null) return;

        // Freeze game
        Time.timeScale = 0f;

        // Show UI
        gameOverPanel.SetActive(true);
        if (gameOverText != null)
        {
            gameOverText.text = message;
        }

        // Play death sound
        if (deathSound != null)
        {
            AudioManager.Instance?.PlaySFX(deathSound);
        }

        // Disable player controls
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }

        Debug.Log("[GameOver] Player caught by Emily");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}