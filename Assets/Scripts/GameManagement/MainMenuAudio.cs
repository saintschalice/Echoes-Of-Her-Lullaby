using UnityEngine;
using System.Collections;

/// <summary>
/// Dedicated audio controller for the Main Menu.
/// Ensures the menu music plays and game ambience stops whenever this scene loads.
/// </summary>
public class MainMenuAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The music clip to play for the main menu.")]
    public AudioClip menuMusic;

    [Tooltip("How long the crossfade should take.")]
    public float fadeTime = 1.5f;

    [Tooltip("If true, it will force stop any 'Ambient' sources (wind, rain) from the previous scene.")]
    public bool stopGameAmbience = true;

    void Start()
    {
        StartCoroutine(PlayMenuAudioSequence());
    }

    IEnumerator PlayMenuAudioSequence()
    {
        // 1. Wait until the AudioManager is initialized and ready
        while (AudioManager.Instance == null)
        {
            yield return null;
        }

        // 2. (Optional) Fade out any environmental ambience from the game level (Wind, Cave noises, etc.)
        if (stopGameAmbience)
        {
            AudioManager.Instance.StopAmbient(fadeTime);
        }

        // 3. Play the Main Menu music
        if (menuMusic != null)
        {
            // Check if this specific clip is already playing to avoid restarting it 
            // (useful if you reload the menu or if the music persisted correctly)
            if (AudioManager.Instance.musicSource.isPlaying && AudioManager.Instance.musicSource.clip == menuMusic)
            {
                // It is technically playing, but we must ensure the volume is audible.
                // (Sometimes returning from a pause menu might leave the volume faded out)
                float targetVolume = AudioManager.Instance.musicVolume; // Get current setting
                StartCoroutine(AudioManager.Instance.FadeAudioSource(AudioManager.Instance.musicSource, targetVolume, 0.5f));
            }
            else
            {
                // Standard Play: Crossfades from whatever was playing before
                AudioManager.Instance.PlayMusic(menuMusic, true, fadeTime);
            }
        }
    }
}