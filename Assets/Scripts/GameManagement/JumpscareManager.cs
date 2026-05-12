using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Universal Jumpscare System for all game over scenarios
/// Shows 3-sprite sequence (tilt left, tilt right, center) with audio
/// Then transitions to GameOverManager
/// </summary>
public class JumpscareManager : MonoBehaviour
{
    public static JumpscareManager Instance { get; private set; }

    [Header("Jumpscare UI")]
    [Tooltip("Full screen panel for jumpscare (should be above everything)")]
    public GameObject jumpscarePanel;
    
    [Tooltip("Image component that shows the jumpscare sprites")]
    public Image jumpscareImage;
    
    [Header("Jumpscare Sprites")]
    [Tooltip("First sprite: Emily tilted left")]
    public Sprite tiltLeftSprite;
    
    [Tooltip("Second sprite: Emily tilted right")]
    public Sprite tiltRightSprite;
    
    [Tooltip("Third sprite: Emily centered (final scare)")]
    public Sprite centerSprite;
    
    [Header("Timing")]
    [Tooltip("How long to show tilt left sprite (seconds)")]
    public float tiltLeftDuration = 0.3f;
    
    [Tooltip("How long to show tilt right sprite (seconds)")]
    public float tiltRightDuration = 0.3f;
    
    [Tooltip("How long to show center sprite (seconds)")]
    public float centerDuration = 2.0f;
    
    [Tooltip("Total jumpscare duration (should match audio length, e.g., 11 seconds)")]
    public float totalJumpscareDuration = 11f;
    
    [Header("Audio")]
    [Tooltip("Jumpscare sound effect (11 seconds)")]
    public AudioClip jumpscareSound;
    
    [Header("Visual Effects")]
    [Tooltip("Enable screen shake during jumpscare")]
    public bool enableScreenShake = true;
    
    [Tooltip("Screen shake intensity")]
    public float shakeIntensity = 0.5f;
    
    [Tooltip("Enable flash effect")]
    public bool enableFlash = true;
    
    [Tooltip("Flash color (usually white or red)")]
    public Color flashColor = Color.white;
    
    [Tooltip("Flash image for effect")]
    public Image flashImage;
    
    [Header("Fade Settings")]
    [Tooltip("Fade in duration at start of jumpscare")]
    public float fadeInDuration = 0.2f;
    
    [Tooltip("Fade out duration at end of jumpscare")]
    public float fadeOutDuration = 0.5f;
    
    // State
    private bool isPlayingJumpscare = false;
    private Coroutine jumpscareCoroutine;
    private string pendingGameOverMessage = "GAME OVER";

    private void Awake()
    {
        Debug.Log("=== JUMPSCARE MANAGER AWAKE ===");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[Jumpscare] ✅ Instance created and set to DontDestroyOnLoad");
        }
        else
        {
            Debug.LogWarning("[Jumpscare] ⚠️ Duplicate instance found, destroying this one");
            Destroy(gameObject);
            return;
        }
        
        InitializeJumpscare();
    }
    
    private void InitializeJumpscare()
    {
        Debug.Log("=== JUMPSCARE INITIALIZATION ===");
        
        // Hide jumpscare panel at start
        if (jumpscarePanel != null)
        {
            jumpscarePanel.SetActive(false);
            Debug.Log("[Jumpscare] ✅ Panel hidden at start");
        }
        else
        {
            Debug.LogError("[Jumpscare] ❌ jumpscarePanel is NULL! Assign it in Inspector!");
        }
        
        // Setup flash image
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(false);
            flashImage.color = flashColor;
            Debug.Log("[Jumpscare] ✅ Flash image configured");
        }
        else
        {
            Debug.LogWarning("[Jumpscare] ⚠️ flashImage is NULL (optional, but recommended)");
        }
        
        // Verify all critical references
        Debug.Log("=== REFERENCE CHECK ===");
        Debug.Log($"Jumpscare Panel: {(jumpscarePanel != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Jumpscare Image: {(jumpscareImage != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Tilt Left Sprite: {(tiltLeftSprite != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Tilt Right Sprite: {(tiltRightSprite != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Center Sprite: {(centerSprite != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Jumpscare Sound: {(jumpscareSound != null ? "✅ Assigned" : "❌ NULL")}");
        Debug.Log($"Flash Image: {(flashImage != null ? "✅ Assigned" : "⚠️ NULL (optional)")}");
        
        // Count missing references
        int missingCount = 0;
        if (jumpscarePanel == null) missingCount++;
        if (jumpscareImage == null) missingCount++;
        if (tiltLeftSprite == null) missingCount++;
        if (tiltRightSprite == null) missingCount++;
        if (centerSprite == null) missingCount++;
        
        if (missingCount > 0)
        {
            Debug.LogError($"[Jumpscare] ❌ {missingCount} CRITICAL references are missing!");
            Debug.LogError("[Jumpscare] Jumpscare will NOT work until all references are assigned!");
        }
        else
        {
            Debug.Log("[Jumpscare] ✅ All critical references assigned - Ready to use!");
        }
        
        Debug.Log("=== INITIALIZATION COMPLETE ===");
    }
    
    /// <summary>
    /// Trigger jumpscare sequence, then show game over
    /// Call this instead of GameOverManager.TriggerGameOver()
    /// </summary>
    public void TriggerJumpscare(string gameOverMessage = "GAME OVER")
    {
        Debug.Log("=== JUMPSCARE TRIGGER CALLED ===");
        Debug.Log($"[Jumpscare] Message: {gameOverMessage}");
        
        if (isPlayingJumpscare)
        {
            Debug.LogWarning("[Jumpscare] Already playing jumpscare!");
            return;
        }
        
        // CRITICAL DEBUG: Check all references
        Debug.Log($"[Jumpscare] Panel: {(jumpscarePanel != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Jumpscare] Image: {(jumpscareImage != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Jumpscare] Tilt Left: {(tiltLeftSprite != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Jumpscare] Tilt Right: {(tiltRightSprite != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Jumpscare] Center: {(centerSprite != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Jumpscare] Audio: {(jumpscareSound != null ? "✅" : "❌ NULL")}");
        
        // Check for missing critical references
        if (jumpscarePanel == null)
        {
            Debug.LogError("[Jumpscare] ❌ CRITICAL: jumpscarePanel is NULL! Cannot show jumpscare!");
            Debug.LogError("[Jumpscare] Falling back to direct game over...");
            GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
            return;
        }
        
        if (jumpscareImage == null)
        {
            Debug.LogError("[Jumpscare] ❌ CRITICAL: jumpscareImage is NULL! Cannot show jumpscare!");
            Debug.LogError("[Jumpscare] Falling back to direct game over...");
            GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
            return;
        }
        
        if (tiltLeftSprite == null || tiltRightSprite == null || centerSprite == null)
        {
            Debug.LogError("[Jumpscare] ❌ CRITICAL: One or more sprites are NULL! Cannot show jumpscare!");
            Debug.LogError("[Jumpscare] Falling back to direct game over...");
            GameOverManager.Instance?.TriggerGameOver(gameOverMessage);
            return;
        }
        
        Debug.Log("[Jumpscare] ✅ All references OK! Starting jumpscare sequence...");
        
        pendingGameOverMessage = gameOverMessage;
        
        if (jumpscareCoroutine != null)
        {
            StopCoroutine(jumpscareCoroutine);
        }
        
        jumpscareCoroutine = StartCoroutine(JumpscareSequence());
    }
    
    private IEnumerator JumpscareSequence()
    {
        isPlayingJumpscare = true;
        
        Debug.Log("[Jumpscare] Starting jumpscare sequence");
        
        // 1. Freeze game immediately
        Time.timeScale = 0f;
        
        // 2. Disable player controls
        DisablePlayerControls();
        
        // 3. Stop all ambient audio
        StopAllAudio();
        
        // 4. Show jumpscare panel
        if (jumpscarePanel != null)
        {
            jumpscarePanel.SetActive(true);
            
            // Setup canvas group for fading
            CanvasGroup cg = jumpscarePanel.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = jumpscarePanel.AddComponent<CanvasGroup>();
            }
            cg.alpha = 0f;
            
            // Fade in
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                timer += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
                yield return null;
            }
            cg.alpha = 1f;
        }
        
        // 5. Play jumpscare sound
        if (jumpscareSound != null)
        {
            AudioManager.Instance?.PlaySFX(jumpscareSound);
        }
        
        // 6. Flash effect at start
        if (enableFlash && flashImage != null)
        {
            yield return StartCoroutine(FlashEffect(0.1f));
        }
        
        // 7. Show sprite sequence with screen shake
        Coroutine shakeCoroutine = null;
        if (enableScreenShake)
        {
            shakeCoroutine = StartCoroutine(ContinuousScreenShake());
        }
        
        // Sprite 1: Tilt Left
        if (jumpscareImage != null && tiltLeftSprite != null)
        {
            jumpscareImage.sprite = tiltLeftSprite;
            jumpscareImage.enabled = true;
        }
        yield return new WaitForSecondsRealtime(tiltLeftDuration);
        
        // Sprite 2: Tilt Right
        if (jumpscareImage != null && tiltRightSprite != null)
        {
            jumpscareImage.sprite = tiltRightSprite;
        }
        yield return new WaitForSecondsRealtime(tiltRightDuration);
        
        // Sprite 3: Center (final scare)
        if (jumpscareImage != null && centerSprite != null)
        {
            jumpscareImage.sprite = centerSprite;
        }
        
        // Flash again on center sprite
        if (enableFlash && flashImage != null)
        {
            yield return StartCoroutine(FlashEffect(0.15f));
        }
        
        yield return new WaitForSecondsRealtime(centerDuration);
        
        // 8. Calculate remaining time to match audio duration
        float elapsedTime = fadeInDuration + tiltLeftDuration + tiltRightDuration + centerDuration;
        float remainingTime = totalJumpscareDuration - elapsedTime - fadeOutDuration;
        
        if (remainingTime > 0)
        {
            // Hold center sprite for remaining time
            yield return new WaitForSecondsRealtime(remainingTime);
        }
        
        // 9. Stop screen shake
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            ResetCameraPosition();
        }
        
        // 10. Fade out jumpscare
        if (jumpscarePanel != null)
        {
            CanvasGroup cg = jumpscarePanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                float timer = 0f;
                while (timer < fadeOutDuration)
                {
                    timer += Time.unscaledDeltaTime;
                    cg.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
                    yield return null;
                }
                cg.alpha = 0f;
            }
            
            jumpscarePanel.SetActive(false);
        }
        
        // 11. Trigger game over screen
        Debug.Log("[Jumpscare] Jumpscare complete, showing game over screen");
        
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver(pendingGameOverMessage);
        }
        else
        {
            Debug.LogError("[Jumpscare] GameOverManager not found!");
        }
        
        isPlayingJumpscare = false;
    }
    
    private IEnumerator FlashEffect(float duration)
    {
        if (flashImage == null) yield break;
        
        flashImage.gameObject.SetActive(true);
        
        Color startColor = flashColor;
        startColor.a = 0f;
        Color endColor = flashColor;
        endColor.a = 0.8f;
        
        // Flash in
        float timer = 0f;
        while (timer < duration / 2f)
        {
            timer += Time.unscaledDeltaTime;
            flashImage.color = Color.Lerp(startColor, endColor, timer / (duration / 2f));
            yield return null;
        }
        
        // Flash out
        timer = 0f;
        while (timer < duration / 2f)
        {
            timer += Time.unscaledDeltaTime;
            flashImage.color = Color.Lerp(endColor, startColor, timer / (duration / 2f));
            yield return null;
        }
        
        flashImage.gameObject.SetActive(false);
    }
    
    private IEnumerator ContinuousScreenShake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;
        
        Vector3 originalPos = mainCamera.transform.localPosition;
        
        while (true)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            
            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            
            yield return null;
        }
    }
    
    private void ResetCameraPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.localPosition = new Vector3(0, 0, -10);
        }
    }
    
    private void DisablePlayerControls()
    {
        // Disable player
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null)
        {
            player.enabled = false;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
        
        // Disable joystick
        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null)
        {
            joystick.SetActive(false);
        }
        
        // Pause Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null)
        {
            emilyAI.isPaused = true;
        }
    }
    
    private void StopAllAudio()
    {
        // Stop ambient and music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient(0f);
            AudioManager.Instance.StopMusic(0f);
        }
        
        // Stop Emily audio
        EmilyAudio emilyAudio = FindFirstObjectByType<EmilyAudio>();
        if (emilyAudio != null)
        {
            AudioSource source = emilyAudio.GetComponent<AudioSource>();
            if (source != null)
            {
                source.Stop();
            }
        }
    }
}
