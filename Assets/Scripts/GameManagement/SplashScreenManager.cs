using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SplashScreenManager : MonoBehaviour
{
    [System.Serializable]
    public class SplashStep
    {
        [Tooltip("The UI Panel or Object for this specific splash screen")]
        public GameObject screenObject;

        [Tooltip("How long this screen stays visible (ignored if Wait For Input is true)")]
        public float duration = 3f;

        [Tooltip("If checked, the screen pauses indefinitely until the user taps/clicks.")]
        public bool waitForInput = false;
    }

    [Header("Splash Sequence Configuration")]
    public List<SplashStep> splashSequence;

    [Tooltip("Duration for fading in and out")]
    public float fadeDuration = 1f;

    [Header("Settings")]
    public string nextSceneName = "MainMenu";
    public bool allowTapToSkip = true;

    [Header("Component References")]
    [Tooltip("The Image component of your black overlay panel.")]
    public Image fadeImage;

    private bool inputDetected = false;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("SplashScreenManager: Missing 'Fade Overlay Image'! Please assign a black UI Image.");
            return;
        }

        // --- CRITICAL FIX ---
        // Force the Black Overlay to be drawn ON TOP of everything else.
        // In Unity UI, the last child is drawn last (on top).
        fadeImage.transform.SetAsLastSibling();
        fadeImage.gameObject.SetActive(true);

        // 1. Initialize Fade Image to BLACK (Alpha 1) so the scene starts dark
        SetImageAlpha(1f);
        fadeImage.raycastTarget = true; // Block input while black

        // 2. Hide all screen objects initially
        foreach (var step in splashSequence)
        {
            if (step.screenObject != null)
                step.screenObject.SetActive(false);
        }

        // 3. Start the sequence
        StartCoroutine(PlaySequence());
    }

    void Update()
    {
        // Detect Input
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            inputDetected = true;
        }
    }

    IEnumerator PlaySequence()
    {
        // Short delay to ensure Unity is ready
        yield return new WaitForEndOfFrame();

        foreach (var step in splashSequence)
        {
            if (step.screenObject == null) continue;

            // --- A. SETUP CONTENT ---
            step.screenObject.SetActive(true);
            inputDetected = false;

            // --- B. FADE IN (Black -> Clear) ---
            // This reveals the Logo/Warning
            yield return StartCoroutine(FadeRoutine(1f, 0f));

            // --- C. WAIT LOGIC ---
            if (step.waitForInput)
            {
                // Wait indefinitely for touch
                while (!inputDetected)
                {
                    yield return null;
                }
            }
            else
            {
                // Wait for Timer (or Skip)
                float timer = 0f;
                while (timer < step.duration)
                {
                    if (allowTapToSkip && inputDetected) break;
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // --- D. FADE OUT (Clear -> Black) ---
            // This covers the Logo/Warning with black
            yield return StartCoroutine(FadeRoutine(0f, 1f));

            // --- E. CLEANUP ---
            step.screenObject.SetActive(false);
        }

        // --- SEQUENCE COMPLETE ---
        // Load the next scene (Screen is currently Black)
        SceneManager.LoadScene(nextSceneName);
    }

    // This matches the logic from your FadeScreen.cs
    IEnumerator FadeRoutine(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = startAlpha;
        fadeImage.color = c;

        // Block clicks if we are fading to black, allow if fading to clear
        // (Optional: keep true always to prevent clicking during transitions)
        fadeImage.raycastTarget = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            c.a = newAlpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure final value is set
        c.a = endAlpha;
        fadeImage.color = c;

        // If we just faded to clear (0), allow clicks strictly on the content behind
        // If we faded to black (1), block clicks
        fadeImage.raycastTarget = (endAlpha > 0.9f);
    }

    void SetImageAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}