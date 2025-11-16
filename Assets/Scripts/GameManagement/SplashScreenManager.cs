using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    [Header("Splash Settings")]
    [Tooltip("Duration to show splash screen in seconds")]
    public float splashDuration = 3f;

    [Tooltip("Fade in/out duration")]
    public float fadeDuration = 1f;

    [Header("Optional: Tap to Skip")]
    public bool allowTapToSkip = true;
    public GameObject tapToSkipText;

    [Header("UI References")]
    public Image splashImage;
    public CanvasGroup canvasGroup;

    private bool isTransitioning = false;

    void Start()
    {
        // Initialize
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (tapToSkipText != null)
        {
            tapToSkipText.SetActive(allowTapToSkip);
        }

        // Start splash sequence
        StartCoroutine(SplashSequence());
    }

    void Update()
    {
        // Handle tap/click to skip
        if (allowTapToSkip && !isTransitioning)
        {
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                StopAllCoroutines();
                StartCoroutine(TransitionToMainMenu());
            }
        }
    }

    IEnumerator SplashSequence()
    {
        // Fade in
        yield return StartCoroutine(FadeIn());

        // Hold splash screen
        yield return new WaitForSeconds(splashDuration);

        // Transition to main menu
        yield return StartCoroutine(TransitionToMainMenu());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator TransitionToMainMenu()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        // Fade out
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }
}