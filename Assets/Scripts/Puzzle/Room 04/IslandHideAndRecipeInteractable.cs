using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // Required for UnityAction
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class IslandHideAndRecipeInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public Vector2 interactionBoxSize = new Vector2(3f, 1.5f);
    public Vector2 interactionOffset = Vector2.zero;

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

    // Public Getter
    public bool IsHiding => isHiding;

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

    // Reference to the button to lock it
    private OnScreenInteractButton cachedButton;
    private Button cachedUnityButton; // Cache the actual Unity Button component

    // Cached action to ensure Add/Remove listener works reliably
    private UnityAction onHiddenInteractAction;

    private void Reset()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null) box.isTrigger = true;
    }

    private void Awake()
    {
        // Cache the delegate once to ensure identity equality for Add/Remove Listener
        onHiddenInteractAction = new UnityAction(OnHiddenInteract);
    }

    void Start()
    {
        // //debug Warning for Layers
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            //debug.LogWarning($"[IslandInteractable] '{name}' is on the 'Default' layer. Ensure your PlayerInteractionTracker includes the Default layer mask, or move this object to 'Interactable'.", this);
        }

        mainCamera = Camera.main;
        if (mainCamera != null)
            originalOrthoSize = mainCamera.orthographicSize;

        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.clip = heartbeatClip;
        heartbeatSource.loop = true;
        heartbeatSource.playOnAwake = false;
        heartbeatSource.volume = 0f;
        heartbeatSource.spatialBlend = 0f;

        FindPlayerReferences();

        RefreshButtonReference();

        SyncWithRoomController();
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

    void RefreshButtonReference()
    {
        if (cachedButton == null)
        {
            cachedButton = FindFirstObjectByType<OnScreenInteractButton>();
            if (cachedButton != null)
            {
                cachedUnityButton = cachedButton.GetComponent<Button>();
            }
        }
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

    // =================================================================================
    // INTERACTABLE IMPLEMENTATION
    // =================================================================================

    public void Interact()
    {
        //debug.Log($"[Island] Interact called. IsHiding: {isHiding}, ActiveSequence: {activeSequence != null}");

        // 1. Ensure references are set up
        if (playerController == null)
        {
            FindPlayerReferences();
            if (playerController == null)
            {
                playerController = FindFirstObjectByType<JoystickPlayerController>();
                if (playerController != null)
                    playerRenderers = playerController.GetComponentsInChildren<SpriteRenderer>();
            }
        }

        // 2. Conditions
        bool emilyHasAppeared = false;
        bool introRunning = false;

        if (KitchenRoomController.Instance != null)
        {
            emilyHasAppeared = KitchenRoomController.Instance.emilyIntroDone;
            introRunning = KitchenRoomController.Instance.introInProgress;
        }

        bool canHide = emilyHasAppeared || introRunning;

        // Check conditions - only restrict entry (hiding), allow exit
        if (!isHiding && !canHide)
        {
            if (DialogueSystemV2.Instance != null && !DialogueSystemV2.Instance.IsDialogueActive())
                DialogueSystemV2.Instance.StartDialogue("What a weird kitchen island... There's plenty of space beneath it.", "Lisa");
            return;
        }

        RefreshButtonReference();

        if (activeSequence != null)
        {
            //debug.Log("[Island] Interact blocked because a sequence is running.");
            return;
        }

        if (isHiding)
        {
            ExitHiding();
        }
        else
        {
            // MOVED: Set interaction lock and listener HERE synchronously to guarantee it works
            if (cachedButton != null && cachedUnityButton != null)
            {
                cachedButton.SetInteractionLock(true);
                try
                {
                    // Remove first to avoid duplicates (safe to call even if not present)
                    cachedUnityButton.onClick.RemoveListener(onHiddenInteractAction);
                    cachedUnityButton.onClick.AddListener(onHiddenInteractAction);
                    //debug.Log("[Island] Added OnHiddenInteract listener to button.");
                }
                catch (System.Exception e)
                {
                    //debug.LogError($"[Island] Error adding listener: {e.Message}");
                }
            }
            else
            {
                //debug.LogWarning("[Island] Cached Button is null when trying to hide!");
            }

            StartCoroutine(EnterHidingSequence());
        }
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

        if (!IsPlayerInBox(playerController.transform.position))
        {
            if (DialogueSystemV2.Instance != null && !DialogueSystemV2.Instance.IsDialogueActive())
                DialogueSystemV2.Instance.StartDialogue("It's too far to reach.", "Lisa");
            return;
        }

        Interact();
    }

    private bool IsPlayerInBox(Vector3 playerPos)
    {
        Vector2 center = (Vector2)transform.position + interactionOffset;
        float halfWidth = interactionBoxSize.x * 0.5f;
        float halfHeight = interactionBoxSize.y * 0.5f;

        bool insideX = playerPos.x >= center.x - halfWidth && playerPos.x <= center.x + halfWidth;
        bool insideY = playerPos.y >= center.y - halfHeight && playerPos.y <= center.y + halfHeight;

        return insideX && insideY;
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }

    // =================================================================================
    // PUBLIC HELPERS
    // =================================================================================

    public void HideUnderIsland()
    {
        Interact();
    }

    public void GetOutFromUnderIsland()
    {
        ExitHiding();
    }

    // =================================================================================
    // LOGIC
    // =================================================================================

    IEnumerator EnterHidingSequence()
    {
        //debug.Log("[Island] Entering hiding sequence...");
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
        //debug.Log("[Island] Hiding sequence complete.");

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
        //debug.Log($"[Island] ExitHiding called. IsHiding: {isHiding}");
        if (!isHiding) return;

        if (activeSequence != null)
        {
            //debug.Log("[Island] Stopping active sequence for Exit.");
            StopCoroutine(activeSequence);
        }

        // Ensure activeSequence is cleared so we don't get stuck in "running" state
        activeSequence = null;

        StartCoroutine(ExitHidingRoutine());
    }

    private void OnHiddenInteract()
    {
        //debug.Log($"[Island] OnHiddenInteract triggered by Button listener. IsHiding: {isHiding}");

        // Safety: If an animation is playing, don't interrupt aggressively unless stuck
        if (activeSequence != null) return;

        if (isHiding) ExitHiding();
    }

    IEnumerator ExitHidingRoutine()
    {
        //debug.Log("[Island] Starting Exit Hiding Routine...");
        isHiding = false;

        if (KitchenRoomController.Instance != null)
            KitchenRoomController.Instance.isPlayerHidden = false;

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        if (mainCamera != null) mainCamera.transform.position = originalCameraPos;
        FadeHeartbeat(false);

        activeSequence = StartCoroutine(CameraZoom(originalOrthoSize));

        if (playerController != null)
        {
            playerController.gameObject.layer = originalPlayerLayer;
            if (playerRenderers != null)
                foreach (var sr in playerRenderers) sr.enabled = true;
            playerController.enabled = true;
        }

        // Cleanup listener using cached action
        if (cachedButton != null && cachedUnityButton != null)
        {
            try
            {
                cachedUnityButton.onClick.RemoveListener(onHiddenInteractAction);
                cachedButton.SetInteractionLock(false);
                //debug.Log("[Island] Removed OnHiddenInteract listener.");
            }
            catch (System.Exception e)
            {
                //debug.LogError($"[Island] Error removing listener: {e.Message}");
            }
        }

        yield return activeSequence;
        activeSequence = null;
        //debug.Log("[Island] Exit sequence complete.");
    }

    private void ResetHidingStateInstant()
    {
        //debug.Log("[Island] Resetting hiding state instant.");
        isHiding = false;
        if (cachedButton != null && cachedUnityButton != null)
        {
            cachedUnityButton.onClick.RemoveListener(onHiddenInteractAction);
            cachedButton.SetInteractionLock(false);
        }
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
        mainCamera.transform.position = originalCameraPos;

        while (isHiding)
        {
            time += Time.deltaTime * shakeFrequency;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + (Vector3)interactionOffset;
        Gizmos.DrawWireCube(center, new Vector3(interactionBoxSize.x, interactionBoxSize.y, 1f));
    }
}