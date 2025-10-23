using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float defaultFadeDuration = 1f;

    public static FadeScreen Instance { get; private set; }

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
    }

    void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.raycastTarget = false; // Prevent blocking input
        }
    }

    public void FadeOut(float duration = -1f)
    {
        if (duration < 0) duration = defaultFadeDuration;
        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(0f, 1f, duration));
    }

    public void FadeIn(float duration = -1f)
    {
        if (duration < 0) duration = defaultFadeDuration;
        StopAllCoroutines();
        StartCoroutine(FadeCoroutine(1f, 0f, duration));
    }

    public void FadeOutAndIn(float fadeOutDuration = 1f, float fadeInDuration = 1f, float holdDuration = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndInCoroutine(fadeOutDuration, fadeInDuration, holdDuration));
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        if (fadeImage == null) yield break;

        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    IEnumerator FadeOutAndInCoroutine(float fadeOutDuration, float fadeInDuration, float holdDuration)
    {
        // Fade out
        yield return StartCoroutine(FadeCoroutine(0f, 1f, fadeOutDuration));

        // Hold black screen
        if (holdDuration > 0f)
        {
            yield return new WaitForSecondsRealtime(holdDuration);
        }

        // Fade in
        yield return StartCoroutine(FadeCoroutine(1f, 0f, fadeInDuration));
    }

    public bool IsFading()
    {
        return fadeImage != null && fadeImage.color.a > 0f && fadeImage.color.a < 1f;
    }

    public void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(alpha);
            fadeImage.color = color;
        }
    }
}