using UnityEngine;

/// <summary>
/// Place this on a GameObject in any scene to trigger a cutscene.
/// Can be triggered on Start, manually via code, or by specific game events.
/// </summary>
public class CutsceneTrigger : MonoBehaviour, IInteractable
{
    [Header("Cutscene Data")]
    [Tooltip("The cutscene data asset to play")]
    public VoiceOverCutsceneData cutsceneData;

    [Header("Trigger Settings")]
    [Tooltip("Play this cutscene when the scene starts")]
    public bool playOnStart = false;

    [Tooltip("Delay before playing (if playOnStart is true)")]
    public float startDelay = 0.5f;

    [Tooltip("If true, this cutscene plays BEFORE any other scene initialization (like tutorials)")]
    public bool priorityPlayback = false;

    [Header("Events")]
    [Tooltip("Called when cutscene completes")]
    public UnityEngine.Events.UnityEvent OnCutsceneCompleted;

    private bool hasPlayed = false;

    void Awake()
    {
        // If priority playback, execute immediately in Awake (before other Start methods)
        if (playOnStart && priorityPlayback && cutsceneData != null)
        {
            Invoke(nameof(PlayCutscene), startDelay);
        }
    }

    void Start()
    {
        // Normal playback in Start (after Awake)
        if (playOnStart && !priorityPlayback && cutsceneData != null)
        {
            Invoke(nameof(PlayCutscene), startDelay);
        }
    }

    /// <summary>
    /// Play the assigned cutscene. Can be called from other scripts or Unity Events.
    /// </summary>
    public void PlayCutscene()
    {
        if (cutsceneData == null)
        {
            Debug.LogError("[CutsceneTrigger] No cutscene data assigned!");
            return;
        }

        if (CutsceneManager.Instance == null)
        {
            Debug.LogError("[CutsceneTrigger] CutsceneManager not found! Make sure it's in the persistent scene.");
            return;
        }

        if (CutsceneManager.Instance.IsPlaying())
        {
            Debug.LogWarning("[CutsceneTrigger] A cutscene is already playing!");
            return;
        }

        Debug.Log($"[CutsceneTrigger] Playing cutscene: {cutsceneData.name}");

        CutsceneManager.Instance.PlayCutscene(cutsceneData, OnCutsceneFinished);
        hasPlayed = true;
    }

    /// <summary>
    /// Play the cutscene only if it hasn't been played yet in this scene
    /// </summary>
    public void PlayCutsceneOnce()
    {
        if (!hasPlayed)
        {
            PlayCutscene();
        }
    }

    void OnCutsceneFinished()
    {
        Debug.Log($"[CutsceneTrigger] Cutscene completed: {cutsceneData.name}");
        OnCutsceneCompleted?.Invoke();
    }

    /// <summary>
    /// Reset the "has played" flag - useful if you want to replay the cutscene
    /// </summary>
    public void ResetPlayedFlag()
    {
        hasPlayed = false;
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        PlayCutsceneOnce();
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        PlayCutsceneOnce();
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }
}