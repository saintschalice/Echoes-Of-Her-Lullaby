using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Quick-Time Event for breaking the bathroom mirror
/// Player must tap 15 targets within 2 minutes (3 seconds per tap)
/// </summary>
public class Room08_MirrorQTE : MonoBehaviour
{
    [Header("QTE Settings")]
    public int totalTaps = 15; // 15 taps total (changed from 50)
    public float totalTimeLimit = 25f; // 25 seconds total
    public int maxFailures = 0; // No failures allowed - just time limit
    
    [Header("UI References")]
    public Image fullScreenTapArea; // Full screen tap area within panel - will fill with color
    public Image fillImage; // Progress fill image (child of fullScreenTapArea)
    public Color fillColor = new Color(0.8f, 0.2f, 0.2f, 0.5f); // Red-ish color with transparency
    
    // Support both Text and TextMeshProUGUI
    public Text timerText; // Shows total time remaining (legacy)
    public TextMeshProUGUI timerTextTMP; // Shows total time remaining (TMP)
    
    public Text progressText; // "10/15" (legacy)
    public TextMeshProUGUI progressTextTMP; // "10/15" (TMP)
    
    [Header("Visual Effects")]
    public Image mirrorImage; // The mirror sprite
    public Sprite mirrorPhase1; // Clean mirror
    public Sprite mirrorPhase2; // First cracks (after ~4 taps)
    public Sprite mirrorPhase3; // More cracks (after ~8 taps)
    public Sprite mirrorPhase4; // Almost shattered (after ~12 taps)
    public GameObject shatterEffect; // Particle effect when mirror breaks
    
    [Header("Audio")]
    public AudioClip tapSound;
    public AudioClip crackSound;
    public AudioClip shatterSound;
    public AudioClip failSound;
    public AudioClip[] glassStressSounds; // Escalating stress sounds
    
    [Header("Camera Shake")]
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;
    
    // Runtime state
    private int currentTap = 0;
    private float totalTimeRemaining;
    private bool isQTEActive = false;
    private Coroutine qteCoroutine; // Track the QTE coroutine

    public void StartQTE()
    {
        // Disable player controls
        DisablePlayer();
        
        // Setup full screen tap area
        if (fullScreenTapArea != null)
        {
            Button tapButton = fullScreenTapArea.GetComponent<Button>();
            if (tapButton == null)
            {
                tapButton = fullScreenTapArea.gameObject.AddComponent<Button>();
            }
            tapButton.onClick.RemoveAllListeners();
            tapButton.onClick.AddListener(OnScreenTapped);
            
            // Set fill color
            fullScreenTapArea.color = fillColor;
        }
        
        // Setup fill image
        if (fillImage != null)
        {
            fillImage.fillAmount = 0f; // Start empty
            fillImage.color = fillColor;
        }
        
        // Reset state
        currentTap = 0;
        totalTimeRemaining = totalTimeLimit;
        isQTEActive = true;
        
        // Set initial mirror sprite
        if (mirrorImage != null && mirrorPhase1 != null)
        {
            mirrorImage.sprite = mirrorPhase1;
        }
        
        // Start QTE sequence
        qteCoroutine = StartCoroutine(QTESequence());
    }

    System.Collections.IEnumerator QTESequence()
    {
        // Main timer runs for entire QTE
        while (currentTap < totalTaps && isQTEActive && totalTimeRemaining > 0)
        {
            // Update total timer
            totalTimeRemaining -= Time.unscaledDeltaTime;
            
            // Update timer text (support both Text and TMP)
            string timeString = totalTimeRemaining.ToString("F1") + "s";
            
            if (timerText != null)
            {
                timerText.text = timeString;
                
                // Change color as time runs out
                if (totalTimeRemaining < 10f)
                    timerText.color = Color.red;
                else if (totalTimeRemaining < 15f)
                    timerText.color = Color.yellow;
                else
                    timerText.color = Color.white;
            }
            
            if (timerTextTMP != null)
            {
                timerTextTMP.text = timeString;
                
                // Change color as time runs out
                if (totalTimeRemaining < 10f)
                    timerTextTMP.color = Color.red;
                else if (totalTimeRemaining < 15f)
                    timerTextTMP.color = Color.yellow;
                else
                    timerTextTMP.color = Color.white;
            }
            
            // Update progress text (support both Text and TMP)
            string progressString = $"{currentTap}/{totalTaps}";
            if (progressText != null)
            {
                progressText.text = progressString;
            }
            if (progressTextTMP != null)
            {
                progressTextTMP.text = progressString;
            }
            
            yield return null;
        }
        
        // Check end conditions
        if (totalTimeRemaining <= 0)
        {
            // Time ran out
            yield return StartCoroutine(GameOver());
        }
        else if (currentTap >= totalTaps)
        {
            // All taps successful - mirror breaks!
            yield return StartCoroutine(MirrorShatter());
        }
    }

    void OnScreenTapped()
    {
        if (!isQTEActive) return;
        
        // Play tap sound
        if (tapSound != null)
        {
            AudioManager.Instance?.PlaySFX(tapSound);
        }
        
        // Success!
        OnTapSuccess(currentTap);
        currentTap++;
        
        // Update fill amount
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)currentTap / totalTaps;
        }
    }

    void OnTapSuccess(int tapIndex)
    {
        // Play crack sound
        if (crackSound != null)
        {
            AudioManager.Instance?.PlaySFX(crackSound);
        }
        
        // Play escalating glass stress sound
        if (glassStressSounds != null && glassStressSounds.Length > 0)
        {
            int soundIndex = Mathf.FloorToInt((float)tapIndex / totalTaps * glassStressSounds.Length);
            soundIndex = Mathf.Min(soundIndex, glassStressSounds.Length - 1);
            if (glassStressSounds[soundIndex] != null)
            {
                AudioManager.Instance?.PlaySFX(glassStressSounds[soundIndex]);
            }
        }
        
        // Update mirror sprite based on progress
        UpdateMirrorSprite(tapIndex);
        
        // Camera shake
        StartCoroutine(ShakeCamera());
    }

    void UpdateMirrorSprite(int tapIndex)
    {
        if (mirrorImage == null) return;
        
        // Calculate which phase based on tap progress
        float progress = (float)tapIndex / totalTaps;
        
        if (progress < 0.25f && mirrorPhase1 != null)
        {
            mirrorImage.sprite = mirrorPhase1; // 0-25% (0-3 taps)
        }
        else if (progress < 0.5f && mirrorPhase2 != null)
        {
            mirrorImage.sprite = mirrorPhase2; // 25-50% (4-7 taps)
        }
        else if (progress < 0.75f && mirrorPhase3 != null)
        {
            mirrorImage.sprite = mirrorPhase3; // 50-75% (8-11 taps)
        }
        else if (mirrorPhase4 != null)
        {
            mirrorImage.sprite = mirrorPhase4; // 75-100% (12-15 taps)
        }
    }

    void OnTapFailed()
    {
        // No longer used - removed failure system
    }

    System.Collections.IEnumerator ResetProgressTextColor()
    {
        // No longer used - removed failure system
        yield break;
    }

    System.Collections.IEnumerator GameOver()
    {
        isQTEActive = false;
        
        // Show failure dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_FAILED, "Lisa");
        
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Close panel
        Room08UIManager uiManager = FindFirstObjectByType<Room08UIManager>();
        if (uiManager != null)
        {
            uiManager.HideAllPanels();
        }
        
        // DON'T re-enable player - jumpscare will handle it
        
        // Trigger jumpscare + game over
        if (JumpscareManager.Instance != null)
        {
            JumpscareManager.Instance.TriggerJumpscare("Time ran out...");
        }
        else
        {
            // Fallback to direct game over if jumpscare not available
            EnablePlayer();
            GameOverManager.Instance?.TriggerGameOver("Time ran out...");
        }
    }

    System.Collections.IEnumerator MirrorShatter()
    {
        isQTEActive = false;
        
        // Play shatter sound
        if (shatterSound != null)
        {
            AudioManager.Instance?.PlaySFX(shatterSound);
        }
        
        // Show shatter effect
        if (shatterEffect != null)
        {
            shatterEffect.SetActive(true);
        }
        
        // Big camera shake
        StartCoroutine(ShakeCamera(shakeIntensity * 3, shakeDuration * 2));
        
        yield return new WaitForSeconds(1f);
        
        // Hide shatter effect
        if (shatterEffect != null)
        {
            shatterEffect.SetActive(false);
        }
        
        // Close panel
        Room08UIManager uiManager = FindFirstObjectByType<Room08UIManager>();
        if (uiManager != null)
        {
            uiManager.OnMirrorPuzzleComplete();
        }
        
        // Re-enable player
        EnablePlayer();
    }

    System.Collections.IEnumerator ShakeCamera(float intensity = -1, float duration = -1)
    {
        if (intensity < 0) intensity = shakeIntensity;
        if (duration < 0) duration = shakeDuration;
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    void DisablePlayer()
    {
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void EnablePlayer()
    {
        JoystickPlayerController player = JoystickPlayerController.Instance;
        if (player != null) player.enabled = true;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }
}
