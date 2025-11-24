using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DemoEndUI : MonoBehaviour
{
    public static DemoEndUI Instance { get; private set; }

    [Header("UI Components")]
    [Tooltip("The full panel containing the black background, text, and buttons.")]
    public GameObject endScreenPanel;
    public CanvasGroup contentCanvasGroup; // For fading in the whole screen

    [Header("Buttons")]
    public Button mainMenuButton;
    public Button exitButton;

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Ensure screen is hidden on start
        if (endScreenPanel != null) endScreenPanel.SetActive(false);
        if (contentCanvasGroup != null) contentCanvasGroup.alpha = 0f;

        // Setup Buttons
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);
    }

    public void ShowDemoEnd()
    {
        StartCoroutine(EndSequenceRoutine());
    }

    private IEnumerator EndSequenceRoutine()
    {
        // 1. Disable Player Control
        if (JoystickPlayerController.Instance != null)
        {
            JoystickPlayerController.Instance.enabled = false;

            // Stop physics movement immediately
            Rigidbody2D rb = JoystickPlayerController.Instance.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        // 2. Close other UIs
        if (InventoryManager.Instance != null) InventoryManager.Instance.CloseInventoryUI();
        if (RecipeBookUI.Instance != null) RecipeBookUI.Instance.CloseBook();

        // 3. Trigger Fade Screen (Fade to Black)
        if (FadeScreen.Instance != null)
        {
            FadeScreen.Instance.FadeOut(fadeDuration);
            // Wait for the screen to turn completely black
            yield return new WaitForSeconds(fadeDuration);
        }
        else
        {
            // Fallback if FadeScreen is missing
            Debug.LogWarning("FadeScreen not found, using fallback delay.");
            yield return new WaitForSeconds(0.5f);
        }

        // 4. Show End Screen Content (Text/Buttons)
        // NOTE: Ensure your DemoEndUI Canvas has a higher Sorting Order than FadeScreen's Canvas
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);

            // Fade in the text/buttons smoothly over the black screen
            if (contentCanvasGroup != null)
            {
                float timer = 0f;
                // Fade text in slightly faster than the screen fade
                float textFadeDuration = 1.0f;

                while (timer < textFadeDuration)
                {
                    timer += Time.unscaledDeltaTime; // Use unscaled time in case of pauses
                    contentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / textFadeDuration);
                    yield return null;
                }
                contentCanvasGroup.alpha = 1f;
            }
        }
    }

    void OnMainMenuClicked()
    {
        Time.timeScale = 1f; // Ensure time is running
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnExitClicked()
    {
        Debug.Log("[DemoEndUI] Quitting Game...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}