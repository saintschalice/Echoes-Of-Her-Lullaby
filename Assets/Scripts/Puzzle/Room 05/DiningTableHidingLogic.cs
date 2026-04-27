using UnityEngine;
using System.Collections;

public class DiningTableHidingLogic : MonoBehaviour, IInteractable
{
    public static DiningTableHidingLogic Instance;

    [Header("Hiding Settings")]
    public float hideZoomSize = 3.5f;
    public float zoomDuration = 0.5f;
    public float shakeMagnitude = 0.0005f;
    public float shakeFrequency = 0.75f;
    public float shakeSmoothing = 0.15f;

    [Header("Audio")]
    public AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1.0f;
    public float audioFadeDuration = 0.5f;
    private AudioSource heartbeatSource;

    [Header("State")]
    public bool isHiding = false;
    private bool keyFound = false;
    private float originalOrthoSize;
    private Vector3 originalCameraPos;

    private Camera mainCamera;
    private JoystickPlayerController playerController;
    private SpriteRenderer[] playerRenderers;

    void Awake() { Instance = this; }

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

        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            playerController = p.GetComponent<JoystickPlayerController>();
            playerRenderers = p.GetComponentsInChildren<SpriteRenderer>();
        }
    }

    // ITO ANG TATAWAGIN PAG NAG-CLICK SA TABLE
    public void Interact()
    {
        var controller = Room05_DiningRoomController.Instance;

        // 1. Normal interaction kung wala pang hunt mode o hindi pa tapos ang puzzle
        if (!controller.isSpoonPlaced)
        {
            controller.OnTableInteract();
            return;
        }

        // 2. Manual Hiding Decision (Dito papasok si Lisa sa ilalim)
        if (controller.isEmilyHunting && !isHiding)
        {
            StartCoroutine(StartHidingSequence());
        }
        else if (isHiding)
        {
            // Manual decision para lumabas pagkatapos makuha ang key
            StartCoroutine(ExitHidingSequence());
        }
    }

    public IEnumerator StartHidingSequence()
    {
        isHiding = true;
        originalCameraPos = mainCamera.transform.position;

        // Lock Player Movement & Hide Sprite
        if (playerController != null)
        {
            playerController.enabled = false;
            foreach (var sr in playerRenderers) sr.enabled = false;
        }

        // Start Effects (Zoom, Shake, Heartbeat)
        StartCoroutine(CameraZoom(hideZoomSize));
        StartCoroutine(CameraShakeRoutine());
        StartCoroutine(FadeHeartbeat(true));

        yield return new WaitForSeconds(1f);

        // AUTO PROGRESSION: Pagkapasok, automatic na mag-da-dialogue at makukuha ang key
        if (!keyFound)
        {
            yield return new WaitForSeconds(0.5f);

            // Gamitin ang TryShowDialogue (Auto-Lisa speaker)
            Room05_DiningRoomController.Instance.TryShowDialogue("It's so dark down here... wait, there's a key taped under the table frame!"); 

            yield return new WaitForSeconds(1.5f);

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.AddItem("bedroom_key");

            keyFound = true;
        }
    }

    public IEnumerator ExitHidingSequence()
    {
        isHiding = false;

        // Start Visuals/Audio Restore
        StartCoroutine(CameraZoom(originalOrthoSize));
        StartCoroutine(FadeHeartbeat(false));
        mainCamera.transform.position = originalCameraPos;

        yield return new WaitForSeconds(zoomDuration);

        // Restore Player Movement & Sprite
        if (playerController != null)
        {
            foreach (var sr in playerRenderers) sr.enabled = true;
            playerController.enabled = true;
        }

        Room05_DiningRoomController.Instance.TryShowDialogue("I need to get out of this room before she comes back.");
    }

    // --- LOGIC FUNCTIONS ---

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
        while (isHiding)
        {
            time += Time.deltaTime * shakeFrequency;
            float noiseX = (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f;

            Vector3 targetPos = originalCameraPos + new Vector3(noiseX * shakeMagnitude, noiseY * shakeMagnitude, 0f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, shakeSmoothing);
            yield return null;
        }
        mainCamera.transform.position = originalCameraPos;
    }

    IEnumerator FadeHeartbeat(bool fadeIn)
    {
        if (heartbeatSource == null) yield break;
        float targetVol = fadeIn ? heartbeatVolume : 0f;
        float startVol = heartbeatSource.volume;
        if (fadeIn) heartbeatSource.Play();

        float elapsed = 0f;
        while (elapsed < audioFadeDuration)
        {
            elapsed += Time.deltaTime;
            heartbeatSource.volume = Mathf.Lerp(startVol, targetVol, elapsed / audioFadeDuration);
            yield return null;
        }
        heartbeatSource.volume = targetVol;
        if (!fadeIn) heartbeatSource.Stop();
    }

    // Interface Implementation
    public void OnInteract(PlayerContext context) => Interact();
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }
}