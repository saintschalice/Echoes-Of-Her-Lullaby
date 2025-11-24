using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class IslandHideAndRecipeInteractable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string recipeItemId = "recipe_book_kitchen";

    [Header("Hiding Visuals")]
    public float hideZoomSize = 3.5f;
    public float zoomDuration = 0.5f;

    [Tooltip("Very subtle shake magnitude in world units (try 0.0003–0.001).")]
    public float shakeMagnitude = 0.0005f;

    [Tooltip("How quickly the camera follows the shake offset (0–1).")]
    [Range(0f, 1f)]
    public float shakeSmoothing = 0.15f;

    [Tooltip("How fast the shake moves (frequency).")]
    public float shakeFrequency = 0.75f;

    [Header("Audio")]
    public AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1.0f;
    public float audioFadeDuration = 0.5f;

    [Header("State (Read Only)")]
    public bool isHiding = false;
    [SerializeField] private bool scratchesShown = false;
    [SerializeField] private bool recipeFound = false;

    private Camera mainCamera;
    private JoystickPlayerController playerController;
    private SpriteRenderer[] playerRenderers;
    private int originalPlayerLayer;
    private float originalOrthoSize;
    private Vector3 originalCameraPos;

    private AudioSource heartbeatSource;
    private Coroutine activeSequence;
    private Coroutine shakeCoroutine;
    private Coroutine audioFadeCoroutine;

    public bool IsHiding => isHiding;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalOrthoSize = mainCamera.orthographicSize;

        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.clip = heartbeatClip;
        heartbeatSource.loop = true;
        heartbeatSource.playOnAwake = false;
        heartbeatSource.volume = 0f;
        heartbeatSource.spatialBlend = 0f;

        SyncWithRoomController();
    }

    void OnDisable()
    {
        if (isHiding) ResetHidingStateInstant();
    }

    private void SyncWithRoomController()
    {
        if (KitchenRoomController.Instance != null)
        {
            if (KitchenRoomController.Instance.recipeRead || InventoryManager.Instance.HasItem(recipeItemId))
            {
                scratchesShown = true;
                recipeFound = true;
            }
        }
    }

    void OnMouseDown()
    {
        if (playerController == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
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
            if (DialogueSystemV2.Instance != null && !DialogueSystemV2.Instance.IsDialogueActive())
                DialogueSystemV2.Instance.StartDialogue("It's too far to reach.", "Lisa");
            return;
        }

        bool emilyHasAppeared = false;
        bool introRunning = false;

        if (KitchenRoomController.Instance != null)
        {
            emilyHasAppeared = KitchenRoomController.Instance.emilyIntroDone;
            introRunning = KitchenRoomController.Instance.introInProgress;
        }

        bool canHide = emilyHasAppeared || introRunning;

        if (!canHide)
        {
            if (DialogueSystemV2.Instance != null && !DialogueSystemV2.Instance.IsDialogueActive())
                DialogueSystemV2.Instance.StartDialogue("What a weird kitchen island... There's plenty of space beneath it.", "Lisa");
            return;
        }

        if (activeSequence != null) return;

        if (isHiding) ExitHiding();
        else StartCoroutine(EnterHidingSequence());
    }

    IEnumerator EnterHidingSequence()
    {
        isHiding = true;

        if (mainCamera != null) originalCameraPos = mainCamera.transform.position;

        if (playerController != null)
        {
            playerController.enabled = false;
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            if (playerRenderers != null)
                foreach (var sr in playerRenderers) sr.enabled = false;

            originalPlayerLayer = playerController.gameObject.layer;
            playerController.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        if (KitchenRoomController.Instance != null)
            KitchenRoomController.Instance.isPlayerHidden = true;

        activeSequence = StartCoroutine(CameraZoom(hideZoomSize));
        shakeCoroutine = StartCoroutine(CameraShakeRoutine());
        FadeHeartbeat(true);

        yield return activeSequence;
        activeSequence = null;

        if (!scratchesShown)
        {
            if (DialogueSystemV2.Instance != null)
            {
                DialogueSystemV2.Instance.StartDialogue("There are scratches here, as if made with someone's fingernails.", "Lisa");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
                yield return new WaitForSeconds(0.2f);
                DialogueSystemV2.Instance.StartDialogue("Oh, a recipe book. That might be important.", "Lisa");
                while (DialogueSystemV2.Instance.IsDialogueActive()) yield return null;
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(recipeItemId);
            }

            scratchesShown = true;
            recipeFound = true;
        }
    }

    public void ExitHiding()
    {
        if (!isHiding) return;
        if (activeSequence != null) StopCoroutine(activeSequence);
        StartCoroutine(ExitHidingRoutine());
    }

    IEnumerator ExitHidingRoutine()
    {
        isHiding = false;
        if (KitchenRoomController.Instance != null)
            KitchenRoomController.Instance.isPlayerHidden = false;

        if (playerController != null)
        {
            playerController.gameObject.layer = originalPlayerLayer;
            if (playerRenderers != null)
                foreach (var sr in playerRenderers) sr.enabled = true;
            playerController.enabled = true;
        }

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        // Reset camera position to prevent drift before zooming out
        if (mainCamera != null) mainCamera.transform.position = originalCameraPos;

        FadeHeartbeat(false);

        activeSequence = StartCoroutine(CameraZoom(originalOrthoSize));
        yield return activeSequence;
        activeSequence = null;
    }

    private void ResetHidingStateInstant()
    {
        isHiding = false;
        if (mainCamera != null) mainCamera.orthographicSize = originalOrthoSize;
        if (heartbeatSource != null) heartbeatSource.Stop();
        if (KitchenRoomController.Instance != null) KitchenRoomController.Instance.isPlayerHidden = false;

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

        // Make sure we start exactly at the stored position
        mainCamera.transform.position = originalCameraPos;

        while (isHiding)
        {
            time += Time.deltaTime * shakeFrequency;

            // Smooth Perlin-based offsets between -1 and 1
            // We use different seeds (0 vs time) for X and Y to ensure they don't move diagonally
            float noiseX = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f;

            Vector3 targetOffset = new Vector3(
                noiseX * shakeMagnitude,
                noiseY * shakeMagnitude,
                0f
            );

            Vector3 targetPos = originalCameraPos + targetOffset;

            // Smoothly interpolate toward the target position
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPos,
                shakeSmoothing
            );

            yield return null;
        }

        // When hiding ends, snap back to the original position
        if (mainCamera != null)
        {
            mainCamera.transform.position = originalCameraPos;
        }
    }

    void FadeHeartbeat(bool fadeIn)
    {
        if (audioFadeCoroutine != null) StopCoroutine(audioFadeCoroutine);
        audioFadeCoroutine = StartCoroutine(FadeHeartbeatRoutine(fadeIn));
    }

    IEnumerator FadeHeartbeatRoutine(bool fadeIn)
    {
        if (heartbeatSource == null) yield break;
        float target = fadeIn ? heartbeatVolume : 0f;
        float start = heartbeatSource.volume;
        if (fadeIn && !heartbeatSource.isPlaying) heartbeatSource.Play();
        float elapsed = 0f;
        while (elapsed < audioFadeDuration)
        {
            elapsed += Time.deltaTime;
            heartbeatSource.volume = Mathf.Lerp(start, target, elapsed / audioFadeDuration);
            yield return null;
        }
        heartbeatSource.volume = target;
        if (!fadeIn) heartbeatSource.Stop();
    }
}