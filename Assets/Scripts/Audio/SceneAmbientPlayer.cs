using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Import required for Coroutines

/// <summary>
/// Placed in a Scene to trigger the ambient sound when the scene loads.
/// </summary>
public class SceneAmbientPlayer : MonoBehaviour
{
    [Tooltip("The ScriptableObject holding the ambient sound data for this scene.")]
    public SceneAmbientConfig sceneAmbientConfig;

    void Start()
    {
        // Start the coroutine to safely wait for the AudioManager to be ready.
        StartCoroutine(AttemptPlayAmbient());
    }

    IEnumerator AttemptPlayAmbient()
    {
        // Wait until the AudioManager Singleton is ready.
        while (AudioManager.Instance == null)
        {
            yield return null;
        }

        if (sceneAmbientConfig == null)
        {
            yield break;
        }

        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // A. Logic for the dedicated MUSIC source (usually only for the first scene).
        // Use this IF the current music source is currently playing.
        if (AudioManager.Instance.musicSource.isPlaying)
        {
            // If music is playing, use the crossfade logic for MUSIC to switch tracks.
            // NOTE: This assumes subsequent game scenes use the PlayMusic method if they need music.
            AudioManager.Instance.PlayMusic(
                sceneAmbientConfig.ambientClip,
                true,
                sceneAmbientConfig.fadeTime
            );
        }
        // B. Logic for the AMBIENT sources (used for all ambient sound transitions).
        else
        {
            // If music is NOT playing, or if we want to switch to the AMBIENT layer, 
            // use the dual-source crossfade logic. This is generally preferred 
            // for seamless ambient transitions between game areas.
            AudioManager.Instance.PlayAmbient(
                sceneAmbientConfig.ambientClip,
                true,
                sceneAmbientConfig.fadeTime
            );
        }
    }
}