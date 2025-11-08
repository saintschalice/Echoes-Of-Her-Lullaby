using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Placed in a Scene to trigger the ambient sound when the scene loads.
/// Will wait for any "PlayOnStart" cutscenes to finish before playing.
/// </summary>
public class SceneAmbientPlayer : MonoBehaviour
{
    [Tooltip("The ScriptableObject holding the ambient sound data for this scene.")]
    public SceneAmbientConfig sceneAmbientConfig;

    private bool hasSubscribed = false; // Flag to prevent event errors

    void Start()
    {
        // Start the master coroutine to check for cutscenes
        StartCoroutine(CheckForCutsceneAndPlay());
    }

    IEnumerator CheckForCutsceneAndPlay()
    {
        // STEP 1: ONLY wait for the AudioManager.
        while (AudioManager.Instance == null)
        {
            yield return null;
        }

        // STEP 2: Check for CutsceneManager *without* looping.
        // If it's not in the scene (e.g., testing a scene), we don't wait.
        if (CutsceneManager.Instance != null)
        {
            CutsceneTrigger trigger = FindFirstObjectByType<CutsceneTrigger>();

            // Check if a cutscene is either 1) already playing, or 2) a trigger is set to play on start
            if (CutsceneManager.Instance.IsPlaying() || (trigger != null && trigger.playOnStart))
            {
                // A cutscene is active or will be. We must wait.
                // Subscribe to the static event that fires when *any* cutscene finishes.
                CutsceneManager.OnAnyCutsceneComplete += PlayAmbientAfterCutscene;
                hasSubscribed = true;

                // Exit coroutine; the event will call PlayAmbientAfterCutscene
                yield break;
            }
        }

        // STEP 3: If no CutsceneManager was found, or if no cutscene was playing,
        // play the ambient sound immediately.
        StartCoroutine(PlayAmbientLogic());
    }

    /// <summary>
    /// This method is called by the CutsceneManager.OnAnyCutsceneComplete event
    /// </summary>
    void PlayAmbientAfterCutscene()
    {
        // Unsubscribe from the event immediately to prevent future calls
        if (hasSubscribed)
        {
            CutsceneManager.OnAnyCutsceneComplete -= PlayAmbientAfterCutscene;
            hasSubscribed = false;
        }

        // Now that the cutscene is over, play the ambient sound
        StartCoroutine(PlayAmbientLogic());
    }

    /// <summary>
    /// This is your original logic for playing the ambient sound.
    /// </summary>
    IEnumerator PlayAmbientLogic()
    {
        // We already know AudioManager.Instance is not null from the first check.
        if (sceneAmbientConfig == null)
        {
            Debug.LogWarning("[SceneAmbientPlayer] No SceneAmbientConfig assigned.", this);
            yield break;
        }

        // A. Logic for the dedicated MUSIC source (usually only for the first scene).
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

    /// <summary>
    /// Clean up the event subscription if this object is destroyed
    /// </summary>
    void OnDestroy()
    {
        if (hasSubscribed && CutsceneManager.Instance != null)
        {
            CutsceneManager.OnAnyCutsceneComplete -= PlayAmbientAfterCutscene;
            hasSubscribed = false;
        }
    }
}