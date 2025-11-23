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
    [Tooltip("Subtle shake while hiding (e.g. 0.1)")]
    public float shakeMagnitude = 0.1f;

    [Header("Audio")]
    public AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1.0f;
    public float audioFadeDuration = 0.5f;

    [Header("State (Read Only)")]
    public bool isHiding = false;
    [SerializeField] private bool scratchesShown = false;
    [SerializeField] private bool recipeFound = false;

    // Internal References
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
        {
            originalOrthoSize = mainCamera.orthographicSize;
        }

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
        if (isHiding)
        {
            ResetHidingStateInstant();
        }
    }

    private void SyncWithRoomController()
    {
        if (KitchenRoomController.Instance != null)
        {
            if (KitchenRoomController.Instance.recipeRead)
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
            {
                DialogueSystemV2.Instance.StartDialogue("It's too far to reach.", "Lisa");
            }
            return;
        }

        // BUG 1 FIX: Check if intro is in progress OR Emily has already appeared
        bool emilyHasAppeared = false;
        bool introRunning = false;

        if (KitchenRoomController.Instance != null)
        {
            emilyHasAppeared = KitchenRoomController.Instance.emilyIntroDone;
            introRunning = KitchenRoomController.Instance.introInProgress;
        }

        // Allow hiding if Emily has appeared OR if the intro sequence is currently running
        bool canHide = emilyHasAppeared || introRunning;

        if (!canHide)
        {
            // PRE-EMILY: Comment only, NO hiding
            if (DialogueSystemV2.Instance != null && !DialogueSystemV2.Instance.IsDialogueActive())
            {
                DialogueSystemV2.Instance.StartDialogue("What a weird kitchen island... There's plenty of space beneath it.", "Lisa");
            }
            return;
        }

        // POST-EMILY: Hiding Allowed
        if (activeSequence != null) return;

        if (isHiding)
        {
            ExitHiding();
        }
        else
        {
            StartCoroutine(EnterHidingSequence());
        }
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
            {
                foreach (var sr in playerRenderers) sr.enabled = false;
            }

            originalPlayerLayer = playerController.gameObject.layer;
            playerController.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        if (KitchenRoomController.Instance != null)
        {
            KitchenRoomController.Instance.isPlayerHidden = true;
        }

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

            if (KitchenRoomController.Instance != null)
            {
                KitchenRoomController.Instance.OnRecipeBookRead();
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
        {
            KitchenRoomController.Instance.isPlayerHidden = false;
        }

        if (playerController != null)
        {
            playerController.gameObject.layer = originalPlayerLayer;

            if (playerRenderers != null)
            {
                foreach (var sr in playerRenderers) sr.enabled = true;
            }

            playerController.enabled = true;
        }

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
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

        while (isHiding)
        {
            Vector3 offset = (Vector3)Random.insideUnitCircle * shakeMagnitude;
            offset.z = 0;

            mainCamera.transform.position = originalCameraPos + offset;

            yield return null;
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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
#endif
}