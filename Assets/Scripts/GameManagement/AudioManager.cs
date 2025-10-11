using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

/// <summary>
/// Centralized audio manager that routes all audio through the Audio Mixer
/// No need to add AudioSource components to individual objects
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer mainAudioMixer;
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup dialogueGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup ambientGroup;

    [Header("Audio Sources (Pooled)")]
    [SerializeField] private int audioSourcePoolSize = 10;
    private List<AudioSource> sfxPool = new List<AudioSource>();
    private List<AudioSource> dialoguePool = new List<AudioSource>();
    private AudioSource musicSource;
    private AudioSource ambientSource;

    [Header("Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float dialogueVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 1f;

    private Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadVolumeSettings();
        ApplyVolumeSettings();
    }

    void InitializeAudioSources()
    {
        // Create pooled SFX sources
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxGroup;
            source.playOnAwake = false;
            sfxPool.Add(source);
        }

        // Create dedicated Dialogue source pool
        for (int i = 0; i < 3; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = dialogueGroup;
            source.playOnAwake = false;
            dialoguePool.Add(source);
        }

        // Create dedicated Music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        // Create dedicated Ambient source
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.outputAudioMixerGroup = ambientGroup;
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;

        Debug.Log($"[AudioManager] Initialized with {sfxPool.Count} SFX sources, {dialoguePool.Count} dialogue sources");
    }

    #region SFX Methods
    /// <summary>
    /// Play a one-shot SFX at a specific position
    /// </summary>
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        if (source != null)
        {
            source.transform.position = position;
            source.volume = volume * sfxVolume;
            source.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play a one-shot SFX without 3D positioning
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        if (source != null)
        {
            source.volume = volume * sfxVolume;
            source.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play SFX with random pitch variation (good for footsteps, impacts)
    /// </summary>
    public void PlaySFXWithRandomPitch(AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        if (source != null)
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.volume = volume * sfxVolume;
            source.PlayOneShot(clip);
            source.pitch = 1f; // Reset pitch after
        }
    }

    AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // All sources busy, return first one (will interrupt)
        Debug.LogWarning("[AudioManager] All SFX sources busy, reusing first source");
        return sfxPool[0];
    }
    #endregion

    #region Dialogue Methods
    /// <summary>
    /// Play dialogue audio (typing sounds, character voices)
    /// </summary>
    public void PlayDialogue(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableDialogueSource();
        if (source != null)
        {
            source.volume = volume * dialogueVolume;
            source.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Stop all dialogue sounds (useful when skipping dialogue)
    /// </summary>
    public void StopAllDialogue()
    {
        foreach (AudioSource source in dialoguePool)
        {
            source.Stop();
        }
    }

    AudioSource GetAvailableDialogueSource()
    {
        foreach (AudioSource source in dialoguePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return dialoguePool[0];
    }
    #endregion

    #region Music Methods
    /// <summary>
    /// Play background music with optional fade
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = true, float fadeTime = 1f)
    {
        if (clip == null) return;

        if (musicSource.isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(CrossfadeMusic(clip, fadeTime));
        }
        else
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeAudioSource(musicSource, musicVolume, fadeTime));
        }
    }

    /// <summary>
    /// Stop music with fade out
    /// </summary>
    public void StopMusic(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutAndStop(musicSource, fadeTime));
    }

    System.Collections.IEnumerator CrossfadeMusic(AudioClip newClip, float fadeTime)
    {
        float halfFade = fadeTime / 2f;

        // Fade out current music
        yield return StartCoroutine(FadeAudioSource(musicSource, 0f, halfFade));

        // Switch clip
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in new music
        yield return StartCoroutine(FadeAudioSource(musicSource, musicVolume, halfFade));
    }
    #endregion

    #region Ambient Methods
    /// <summary>
    /// Play ambient/atmospheric sound (room ambience, wind, rain, etc)
    /// </summary>
    public void PlayAmbient(AudioClip clip, bool loop = true, float fadeTime = 1f)
    {
        if (clip == null) return;

        if (ambientSource.isPlaying && ambientSource.clip == clip)
        {
            return; // Already playing this ambient sound
        }

        if (ambientSource.isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(CrossfadeAmbient(clip, fadeTime));
        }
        else
        {
            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.volume = 0f;
            ambientSource.Play();
            StartCoroutine(FadeAudioSource(ambientSource, ambientVolume, fadeTime));
        }
    }

    /// <summary>
    /// Stop ambient sound with fade out
    /// </summary>
    public void StopAmbient(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutAndStop(ambientSource, fadeTime));
    }

    System.Collections.IEnumerator CrossfadeAmbient(AudioClip newClip, float fadeTime)
    {
        float halfFade = fadeTime / 2f;

        yield return StartCoroutine(FadeAudioSource(ambientSource, 0f, halfFade));

        ambientSource.clip = newClip;
        ambientSource.Play();

        yield return StartCoroutine(FadeAudioSource(ambientSource, ambientVolume, halfFade));
    }
    #endregion

    #region Volume Control
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        mainAudioMixer.SetFloat("MasterVolume", VolumeToDecibels(masterVolume));
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        mainAudioMixer.SetFloat("SFXVolume", VolumeToDecibels(sfxVolume));
        SaveVolumeSettings();
    }

    public void SetDialogueVolume(float volume)
    {
        dialogueVolume = Mathf.Clamp01(volume);
        mainAudioMixer.SetFloat("DialogueVolume", VolumeToDecibels(dialogueVolume));
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        mainAudioMixer.SetFloat("MusicVolume", VolumeToDecibels(musicVolume));
        SaveVolumeSettings();
    }

    public void SetAmbientVolume(float volume)
    {
        ambientVolume = Mathf.Clamp01(volume);
        mainAudioMixer.SetFloat("AmbientVolume", VolumeToDecibels(ambientVolume));
        SaveVolumeSettings();
    }

    float VolumeToDecibels(float volume)
    {
        // Convert 0-1 range to -80 to 0 dB
        return volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
    }

    void ApplyVolumeSettings()
    {
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetDialogueVolume(dialogueVolume);
        SetMusicVolume(musicVolume);
        SetAmbientVolume(ambientVolume);
    }

    void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("DialogueVolume", dialogueVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
        PlayerPrefs.Save();
    }

    void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        dialogueVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);
    }
    #endregion

    #region Audio Fading
    System.Collections.IEnumerator FadeAudioSource(AudioSource source, float targetVolume, float fadeTime)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeTime);
            yield return null;
        }

        source.volume = targetVolume;
    }

    System.Collections.IEnumerator FadeOutAndStop(AudioSource source, float fadeTime)
    {
        yield return StartCoroutine(FadeAudioSource(source, 0f, fadeTime));
        source.Stop();
    }
    #endregion

    #region Debug
    [ContextMenu("Test SFX")]
    void TestSFX()
    {
        Debug.Log("[AudioManager] Testing SFX - make sure you have an AudioClip assigned!");
    }

    [ContextMenu("Test Music")]
    void TestMusic()
    {
        Debug.Log("[AudioManager] Testing Music - make sure you have an AudioClip assigned!");
    }

    [ContextMenu("Test Ambient")]
    void TestAmbient()
    {
        Debug.Log("[AudioManager] Testing Ambient - make sure you have an AudioClip assigned!");
    }
    #endregion
}