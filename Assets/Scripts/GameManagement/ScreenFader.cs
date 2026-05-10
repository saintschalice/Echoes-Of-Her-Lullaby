using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float defaultFadeDuration = 1f;
    public Color fadeColor = Color.black;

    [Header("Auto Fade In on Start")]
    public bool fadeInOnStart = true;
    public float startDelay = 0.2f;

    [Header("Auto Fade on Scene Load")]
    public bool fadeInOnSceneLoad = true;

    private bool isFading = false;
    private Coroutine currentFadeCoroutine;

    public static ScreenFader Instance { get; private set; }

    void Awake()
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

        // Ensure fade image exists
        if (fadeImage == null)
        {
            fadeImage = GetComponentInChildren<Image>();
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f); // Start fully black
            fadeImage.raycastTarget = false; // Don't block raycasts when invisible
        }

        // CRITICAL FIX: Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (fadeInOnStart)
        {
            Invoke(nameof(FadeInOnStart), startDelay);
        }
    }

    // CRITICAL FIX: Fade in automatically when a new scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeInOnSceneLoad && fadeImage != null)
        {
            Debug.Log($"[ScreenFader] Scene loaded: {scene.name}, fading in...");
            
            // Make sure we start from black
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
            
            // Fade in after a short delay
            Invoke(nameof(FadeInOnStart), startDelay);
        }
    }

    void FadeInOnStart()
    {
        FadeIn();
    }

    public void FadeOut(float duration = -1, System.Action onComplete = null)
    {
        if (duration < 0) duration = defaultFadeDuration;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeCoroutine(0, 1, duration, onComplete));
    }

    public void FadeIn(float duration = -1, System.Action onComplete = null)
    {
        if (duration < 0) duration = defaultFadeDuration;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeCoroutine(1, 0, duration, onComplete));
    }

    public void FadeTo(float targetAlpha, float duration = -1, System.Action onComplete = null)
    {
        if (duration < 0) duration = defaultFadeDuration;

        float currentAlpha = fadeImage != null ? fadeImage.color.a : 0;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeCoroutine(currentAlpha, targetAlpha, duration, onComplete));
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration, System.Action onComplete)
    {
        if (fadeImage == null)
        {
            Debug.LogError("[ScreenFader] Fade image is not assigned!");
            onComplete?.Invoke();
            yield break;
        }

        isFading = true;
        fadeImage.raycastTarget = true; // Block interactions during fade

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time so fades work when paused
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, endAlpha);

        // If fading to transparent, disable raycast blocking
        if (endAlpha <= 0.01f)
        {
            fadeImage.raycastTarget = false;
        }

        isFading = false;
        currentFadeCoroutine = null;
        onComplete?.Invoke();
    }

    public bool IsFading()
    {
        return isFading;
    }

    public void SetInstantBlack()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
            fadeImage.raycastTarget = true;
        }
    }

    public void SetInstantClear()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
            fadeImage.raycastTarget = false;
        }
    }
}