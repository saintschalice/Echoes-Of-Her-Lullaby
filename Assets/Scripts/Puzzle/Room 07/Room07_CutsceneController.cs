using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Centralized cutscene management for Room 07
/// Handles fade transitions, black screens, and lullaby playback
/// </summary>
public class Room07_CutsceneController : MonoBehaviour
{
    public static Room07_CutsceneController Instance { get; private set; }

    [Header("Fade System")]
    [Tooltip("Black screen panel for fades")]
    public Image fadePanel;
    
    [Tooltip("Fade duration in seconds")]
    public float fadeDuration = 0.5f;

    [Header("Cutscene Images")]
    [Tooltip("Image component to show cutscene")]
    public Image cutsceneImage;
    
    [Tooltip("Tea party cutscene sprite")]
    public Sprite teaPartyCutscene;
    
    [Tooltip("Doll cutscene sprite")]
    public Sprite dollCutscene;

    [Header("Lullaby Audio")]
    [Tooltip("Lullaby fragment 1 (after tea party)")]
    public AudioClip lullabyFragment1;
    
    [Tooltip("Lullaby fragment 2 (after doll pickup)")]
    public AudioClip lullabyFragment2;
    
    [Tooltip("Lullaby fragment 3 (at mirror climax)")]
    public AudioClip lullabyFragment3;
    
    [Tooltip("Audio source for lullaby playback")]
    public AudioSource lullabyAudioSource;

    [Header("Debug")]
    public bool debugMode = true;

    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure fade panel starts transparent
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(false);
        }

        // Ensure cutscene image starts hidden
        if (cutsceneImage != null)
        {
            cutsceneImage.gameObject.SetActive(false);
        }
    }

    // ==================== FADE METHODS ====================

    /// <summary>
    /// Fade to black
    /// </summary>
    public IEnumerator FadeOut(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;
        
        if (fadePanel == null)
        {
            Debug.LogError("[Cutscene] Fade panel is null!");
            yield break;
        }

        isFading = true;
        fadePanel.gameObject.SetActive(true);

        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;
        isFading = false;

        if (debugMode) Debug.Log("[Cutscene] Faded to black");
    }

    /// <summary>
    /// Fade from black
    /// </summary>
    public IEnumerator FadeIn(float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;
        
        if (fadePanel == null)
        {
            Debug.LogError("[Cutscene] Fade panel is null!");
            yield break;
        }

        isFading = true;

        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
        isFading = false;

        if (debugMode) Debug.Log("[Cutscene] Faded from black");
    }

    // ==================== CUTSCENE METHODS ====================

    /// <summary>
    /// Play tea party cutscene with fade transitions
    /// </summary>
    public IEnumerator PlayTeaPartyCutscene()
    {
        if (debugMode) Debug.Log("[Cutscene] Starting tea party cutscene");

        // Disable player
        DisablePlayer();

        // 1. Fade to black
        yield return StartCoroutine(FadeOut(0.5f));

        // 2. Show cutscene image
        if (cutsceneImage != null && teaPartyCutscene != null)
        {
            cutsceneImage.sprite = teaPartyCutscene;
            cutsceneImage.gameObject.SetActive(true);
        }

        // 3. Wait for cutscene duration
        yield return new WaitForSeconds(3f);

        // 4. Hide cutscene image
        if (cutsceneImage != null)
        {
            cutsceneImage.gameObject.SetActive(false);
        }

        // 5. Fade from black
        yield return StartCoroutine(FadeIn(0.5f));

        // 6. Play lullaby fragment 1 with black screen
        yield return StartCoroutine(PlayLullabyWithBlackScreen(lullabyFragment1));

        // Re-enable player
        EnablePlayer();

        if (debugMode) Debug.Log("[Cutscene] Tea party cutscene complete");
    }

    /// <summary>
    /// Play doll cutscene with fade transitions
    /// </summary>
    public IEnumerator PlayDollCutscene()
    {
        if (debugMode) Debug.Log("[Cutscene] Starting doll cutscene");

        // Disable player
        DisablePlayer();

        // 1. Fade to black
        yield return StartCoroutine(FadeOut(0.5f));

        // 2. Show cutscene image
        if (cutsceneImage != null && dollCutscene != null)
        {
            cutsceneImage.sprite = dollCutscene;
            cutsceneImage.gameObject.SetActive(true);
        }

        // 3. Wait for cutscene duration
        yield return new WaitForSeconds(2f);

        // 4. Hide cutscene image
        if (cutsceneImage != null)
        {
            cutsceneImage.gameObject.SetActive(false);
        }

        // 5. Fade from black
        yield return StartCoroutine(FadeIn(0.5f));

        // 6. Play lullaby fragment 2 with black screen
        yield return StartCoroutine(PlayLullabyWithBlackScreen(lullabyFragment2));

        // Re-enable player
        EnablePlayer();

        if (debugMode) Debug.Log("[Cutscene] Doll cutscene complete");
    }

    // ==================== LULLABY METHODS ====================

    /// <summary>
    /// Play lullaby fragment with black screen and fade transitions
    /// </summary>
    public IEnumerator PlayLullabyWithBlackScreen(AudioClip lullabyClip)
    {
        if (lullabyClip == null)
        {
            Debug.LogWarning("[Cutscene] Lullaby clip is null!");
            yield break;
        }

        if (debugMode) Debug.Log($"[Cutscene] Playing lullaby: {lullabyClip.name}");

        // Disable player
        DisablePlayer();

        // 1. Fade to black
        yield return StartCoroutine(FadeOut(1.0f));

        // 2. Play lullaby
        if (lullabyAudioSource != null)
        {
            lullabyAudioSource.clip = lullabyClip;
            lullabyAudioSource.Play();

            // Wait for audio to finish
            yield return new WaitForSeconds(lullabyClip.length);
        }
        else
        {
            Debug.LogWarning("[Cutscene] Lullaby audio source is null!");
            yield return new WaitForSeconds(3f); // Fallback duration
        }

        // 3. Fade from black
        yield return StartCoroutine(FadeIn(1.0f));

        // Re-enable player
        EnablePlayer();

        if (debugMode) Debug.Log("[Cutscene] Lullaby playback complete");
    }

    /// <summary>
    /// Play lullaby fragment 3 (for mirror climax)
    /// </summary>
    public IEnumerator PlayMirrorLullaby()
    {
        yield return StartCoroutine(PlayLullabyWithBlackScreen(lullabyFragment3));
    }

    // ==================== PLAYER CONTROL ====================

    private void DisablePlayer()
    {
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");

        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);

        if (debugMode) Debug.Log("[Cutscene] Player disabled");
    }

    private void EnablePlayer()
    {
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");

        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);

        if (debugMode) Debug.Log("[Cutscene] Player enabled");
    }

    // ==================== PUBLIC HELPERS ====================

    public bool IsFading()
    {
        return isFading;
    }
}
