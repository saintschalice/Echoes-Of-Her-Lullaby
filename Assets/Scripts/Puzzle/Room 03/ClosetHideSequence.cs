using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ClosetHideSequence : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Animator closetAnimator;

    [Header("Interaction Settings")]
    public float interactionRadius = 2.0f;

    [Header("Hiding Visuals")]
    public float hideZoomSize = 3.5f;
    public float zoomDuration = 0.5f;

    [Tooltip("Very subtle shake magnitude in world units. Reduced for Hallway.")]
    public float shakeMagnitude = 0.0003f; // REDUCED INTENSITY

    [Tooltip("How quickly the camera follows the shake offset (0–1).")]
    [Range(0f, 1f)] public float shakeSmoothing = 0.15f;

    [Tooltip("How fast the shake moves (frequency).")]
    public float shakeFrequency = 0.75f;

    [Header("Audio")]
    public AudioClip muffledLoop; // Heartbeat or ambience
    [Range(0f, 1f)] public float audioVolume = 1.0f;
    public float audioFadeDuration = 0.5f;

    [Header("Exit Settings")]
    public Vector2 exitOffset = new Vector2(0f, -1.5f);
    [TextArea] public string exitWhisper = "Not ready... never ready... too much pain...";

    // State
    // Made public getter to match Island pattern if needed
    public bool IsHiding => isHiding;
    private bool isHiding = false;

    // Internal References
    private Camera mainCamera;
    private JoystickPlayerController playerController;
    private SpriteRenderer[] playerRenderers;
    private int originalPlayerLayer;
    private float originalOrthoSize;
    private Vector3 originalCameraPos;

    private AudioSource audioSource;
    private Coroutine activeSequence;
    private Coroutine shakeCoroutine;
    private Coroutine audioFadeCoroutine;

    void Start()
    {
        // 1. Setup Camera
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalOrthoSize = mainCamera.orthographicSize;

        // 2. Setup Audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = muffledLoop;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 0f; // 2D Sound

        // 3. Find Player (Cache references)
        FindPlayerReferences();
    }

    void FindPlayerReferences()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerController = p.GetComponent<JoystickPlayerController>();
            playerRenderers = p.GetComponentsInChildren<SpriteRenderer>();
        }
    }

    void OnDisable()
    {
        if (isHiding) ResetHidingStateInstant();
    }

    // =================================================================================
    // INTERACTABLE IMPLEMENTATION (Matches IslandHideAndRecipeInteractable)
    // =================================================================================

    public void Interact()
    {
        // 1. Ensure references are set up
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<JoystickPlayerController>();
            if (playerController != null)
            {
                playerRenderers = playerController.GetComponentsInChildren<SpriteRenderer>();
            }
        }

        if (activeSequence != null) return;

        // Toggle Hiding State
        if (isHiding) ExitHiding();
        else StartCoroutine(EnterHidingSequence());
    }

    public void OnInteract(PlayerContext context)
    {
        if (playerController == null)
        {
            GameObject p = context.PlayerObject ?? GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                playerController = p.GetComponent<JoystickPlayerController>();
                playerRenderers = p.GetComponentsInChildren<SpriteRenderer>();
            }
        }

        if (playerController == null) return;

        float dist = Vector2.Distance(transform.position, playerController.transform.position);
        if (dist > interactionRadius)
        {
            return;
        }

        Interact();
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }

    // =================================================================================

    // --- PUBLIC METHODS FOR EXTERNAL SCRIPTS ---

    // Acts as a TOGGLE: Call once to Hide, call again to Exit.
    public void HideInCloset()
    {
        Interact();
    }

    public void GetOutOfCloset()
    {
        ExitHiding();
    }

    // -------------------------------------------------------------

    IEnumerator EnterHidingSequence()
    {
        isHiding = true;

        // 1. Capture Camera Anchor
        if (mainCamera != null) originalCameraPos = mainCamera.transform.position;

        // 2. Disable Player
        if (playerController != null)
        {
            playerController.enabled = false;
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // Visuals
            if (playerRenderers != null)
                foreach (var sr in playerRenderers) sr.enabled = false;

            // Layer (Invisible to Enemy Raycasts)
            originalPlayerLayer = playerController.gameObject.layer;
            playerController.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        // 3. Visuals (Animator)
        if (closetAnimator != null) closetAnimator.SetTrigger("Hide");

        // 4. Camera & Audio Effects
        activeSequence = StartCoroutine(CameraZoom(hideZoomSize));
        shakeCoroutine = StartCoroutine(CameraShakeRoutine());
        FadeAudio(true);

        yield return activeSequence;
        activeSequence = null;
    }

    private void ExitHiding()
    {
        if (!isHiding) return;
        if (activeSequence != null) StopCoroutine(activeSequence);
        StartCoroutine(ExitHidingRoutine());
    }

    IEnumerator ExitHidingRoutine()
    {
        isHiding = false;

        // 1. Reset Effects
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        // Snap back to anchor to prevent drift
        if (mainCamera != null) mainCamera.transform.position = originalCameraPos;
        FadeAudio(false);

        // 2. Zoom Out
        activeSequence = StartCoroutine(CameraZoom(originalOrthoSize));

        // 3. Animator
        if (closetAnimator != null)
        {
            closetAnimator.SetTrigger("Open");
            yield return new WaitForSeconds(0.2f);
        }

        // 4. Re-enable Player
        if (playerController != null)
        {
            // Position Offset
            playerController.transform.position = transform.position + (Vector3)exitOffset;

            // Restore Layer
            playerController.gameObject.layer = originalPlayerLayer;

            // Restore Visuals
            if (playerRenderers != null)
                foreach (var sr in playerRenderers) sr.enabled = true;

            // Restore Input
            playerController.enabled = true;
        }

        yield return activeSequence;
        activeSequence = null;

        // 5. Close Door Animation
        if (closetAnimator != null) closetAnimator.SetTrigger("Close");
    }

    private void ResetHidingStateInstant()
    {
        isHiding = false;

        if (mainCamera != null) mainCamera.orthographicSize = originalOrthoSize;
        if (audioSource != null) audioSource.Stop();

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.gameObject.layer = originalPlayerLayer;
            if (playerRenderers != null) foreach (var sr in playerRenderers) sr.enabled = true;
        }
    }

    IEnumerator CameraZoom(float targetSize)
    {
        if (mainCamera == null) yield break;
        float startSize = mainCamera.orthographicSize;
        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / zoomDuration);
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }

    IEnumerator CameraShakeRoutine()
    {
        if (mainCamera == null) yield break;

        float time = 0f;
        mainCamera.transform.position = originalCameraPos;

        while (isHiding)
        {
            time += Time.deltaTime * shakeFrequency;

            // Smooth Perlin Noise Shake
            float noiseX = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f;

            Vector3 targetOffset = new Vector3(
                noiseX * shakeMagnitude,
                noiseY * shakeMagnitude,
                0f
            );

            Vector3 targetPos = originalCameraPos + targetOffset;

            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPos,
                shakeSmoothing
            );

            yield return null;
        }

        if (mainCamera != null) mainCamera.transform.position = originalCameraPos;
    }

    void FadeAudio(bool fadeIn)
    {
        if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
        audioFadeCoroutine = StartCoroutine(FadeAudioRoutine(fadeIn));
    }

    IEnumerator FadeAudioRoutine(bool fadeIn)
    {
        if (audioSource == null) yield break;
        float target = fadeIn ? audioVolume : 0f;
        float start = audioSource.volume;

        if (fadeIn && !audioSource.isPlaying) audioSource.Play();

        float elapsed = 0f;
        while (elapsed < audioFadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, target, elapsed / audioFadeDuration);
            yield return null;
        }
        audioSource.volume = target;
        if (!fadeIn) audioSource.Stop();
    }
}