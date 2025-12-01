using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Placed in a Scene to trigger the ambient sound when the scene loads.
/// Will wait for any "PlayOnStart" cutscenes to finish before playing.
/// Also resumes ambient sound after any mid-game cutscene finishes.
/// </summary>
public class SceneAmbientPlayer : MonoBehaviour
{
    [Tooltip("The ScriptableObject holding the ambient sound data for this scene.")]
    public SceneAmbientConfig sceneAmbientConfig;

    void OnEnable()
    {
        // Subscribe to global cutscene event so we can restore ambient sound 
        // whenever ANY cutscene finishes (intro or mid-game).
        CutsceneManager.OnAnyCutsceneComplete += OnCutsceneEnded;
    }

    void OnDisable()
    {
        CutsceneManager.OnAnyCutsceneComplete -= OnCutsceneEnded;
    }

    void Start()
    {
        // Start the master coroutine to check for cutscenes at boot
        StartCoroutine(CheckForCutsceneAndPlay());
    }

    IEnumerator CheckForCutsceneAndPlay()
    {
        // STEP 1: ONLY wait for the AudioManager.
        while (AudioManager.Instance == null)
        {
            yield return null;
        }

        bool isCutsceneBusy = false;

        // STEP 2: Check for CutsceneManager *without* looping.
        // If it's not in the scene (e.g., testing a scene), we don't wait.
        if (CutsceneManager.Instance != null)
        {
            CutsceneTrigger trigger = FindFirstObjectByType<CutsceneTrigger>();

            // Check if a cutscene is either 1) already playing, or 2) a trigger is set to play on start
            if (CutsceneManager.Instance.IsPlaying() || (trigger != null && trigger.playOnStart))
            {
                isCutsceneBusy = true;
            }
        }

        // STEP 3: If no cutscene is blocking us, play immediately.
        // If a cutscene IS blocking, we do nothing here. The OnAnyCutsceneComplete event 
        // (handled by OnCutsceneEnded) will trigger the ambient sound when it finishes.
        if (!isCutsceneBusy)
        {
            StartCoroutine(PlayAmbientLogic());
        }
    }

    /// <summary>
    /// Event handler for when any cutscene finishes.
    /// </summary>
    void OnCutsceneEnded()
    {
        // Restore the ambient sound for this scene
        StartCoroutine(PlayAmbientLogic());
    }

    /// <summary>
    /// This is your original logic for playing the ambient sound.
    /// </summary>
    IEnumerator PlayAmbientLogic()
    {
        // We already know AudioManager.Instance is not null from the first check/event.
        if (sceneAmbientConfig == null)
        {
            Debug.LogWarning("[SceneAmbientPlayer] No SceneAmbientConfig assigned.", this);
            yield break;
        }

        // Ensure we don't try to play if audio manager is missing (safety)
        if (AudioManager.Instance == null) yield break;

        // A. Logic for the dedicated MUSIC source (usually only for the first scene).
        // If music is playing (e.g. from a cutscene that transitioned into this scene with music),
        // we might want to crossfade it to ambient.
        if (AudioManager.Instance.musicSource.isPlaying)
        {
            AudioManager.Instance.PlayMusic(
                sceneAmbientConfig.ambientClip,
                true,
                sceneAmbientConfig.fadeTime
            );
        }
        // B. Logic for the AMBIENT sources (used for all ambient sound transitions).
        else
        {
            AudioManager.Instance.PlayAmbient(
                sceneAmbientConfig.ambientClip,
                true,
                sceneAmbientConfig.fadeTime
            );
        }
    }
}