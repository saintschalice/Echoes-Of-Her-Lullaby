using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

/// <summary>
/// Centralized audio manager that routes all audio through the Audio Mixer
/// and manages both one-shot and looping sounds (via LoopingSoundManager integration)
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

    // CHANGED: Made musicSource public so SceneAmbientPlayer can access it directly
    public AudioSource musicSource;

    private AudioSource ambientSource;
    private AudioSource ambientSource2;
    private AudioSource currentAmbientSource;

    [Header("Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float dialogueVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 1f;

    private Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();

    void Awake()
    {
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

        // Dialogue pool
        for (int i = 0; i < 3; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = dialogueGroup;
            source.playOnAwake = false;
            dialoguePool.Add(source);
        }

        // Music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // Ambient
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.outputAudioMixerGroup = ambientGroup;
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;

        // Ambient 2
        ambientSource2 = gameObject.AddComponent<AudioSource>();
        ambientSource2.outputAudioMixerGroup = ambientGroup;
        ambientSource2.loop = true;
        ambientSource2.playOnAwake = false;

        Debug.Log($"[AudioManager] Initialized with {sfxPool.Count} SFX sources and {dialoguePool.Count} dialogue sources");
    }

    #region SFX Methods
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource src = GetAvailableSFXSource();
        src.transform.position = position;
        src.volume = volume * sfxVolume;
        src.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource src = GetAvailableSFXSource();
        src.volume = volume * sfxVolume;
        src.PlayOneShot(clip);
    }

    public void PlaySFXWithRandomPitch(AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource src = GetAvailableSFXSource();
        src.pitch = Random.Range(minPitch, maxPitch);
        src.volume = volume * sfxVolume;
        src.PlayOneShot(clip);
        src.pitch = 1f;
    }

    AudioSource GetAvailableSFXSource()
    {
        foreach (var s in sfxPool)
            if (!s.isPlaying) return s;

        Debug.LogWarning("[AudioManager] All SFX sources busy, reusing first one.");
        return sfxPool[0];
    }
    #endregion

    #region Dialogue
    public void PlayDialogue(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource src = GetAvailableDialogueSource();
        src.volume = volume * dialogueVolume;
        src.PlayOneShot(clip);
    }

    public void StopAllDialogue()
    {
        foreach (var s in dialoguePool)
            s.Stop();
    }

    AudioSource GetAvailableDialogueSource()
    {
        foreach (var s in dialoguePool)
            if (!s.isPlaying) return s;
        return dialoguePool[0];
    }
    #endregion

    #region Music
    public void PlayMusic(AudioClip clip, bool loop = true, float fadeTime = 1f)
    {
        if (clip == null) return;
        if (musicSource.isPlaying)
            StartCoroutine(CrossfadeMusic(clip, fadeTime));
        else
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = 0;
            musicSource.Play();
            StartCoroutine(FadeAudioSource(musicSource, musicVolume, fadeTime));
        }
    }

    public void StopMusic(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutAndStop(musicSource, fadeTime));
    }

    System.Collections.IEnumerator CrossfadeMusic(AudioClip newClip, float fadeTime)
    {
        float half = fadeTime / 2f;
        yield return StartCoroutine(FadeAudioSource(musicSource, 0f, half));
        musicSource.clip = newClip;
        musicSource.Play();
        yield return StartCoroutine(FadeAudioSource(musicSource, musicVolume, half));
    }
    #endregion

    #region Ambient
    public void PlayAmbient(AudioClip clip, bool loop = true, float fadeTime = 1f)
    {
        if (clip == null) return;
        if (ambientSource.isPlaying && ambientSource.clip == clip) return;

        if (ambientSource.isPlaying)
            StartCoroutine(CrossfadeAmbient(clip, fadeTime));
        else
        {
            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.volume = 0;
            ambientSource.Play();
            StartCoroutine(FadeAudioSource(ambientSource, ambientVolume, fadeTime));
        }
    }

    public void StopAmbient(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutAndStop(ambientSource, fadeTime));
    }

    System.Collections.IEnumerator CrossfadeAmbient(AudioClip newClip, float fadeTime)
    {
        // 1. Determine the new source and the old source
        AudioSource newSource = (currentAmbientSource == ambientSource) ? ambientSource2 : ambientSource;
        AudioSource oldSource = currentAmbientSource;

        // 2. Setup the new source for seamless transition
        newSource.clip = newClip;
        newSource.loop = true;
        newSource.volume = 0f; // Start silent

        // Ensure the new source starts playing right away
        if (!newSource.isPlaying)
        {
            newSource.Play();
        }

        // Set the new source as the current source
        currentAmbientSource = newSource;

        // 3. Perform the concurrent fade
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime < startTime + fadeTime)
        {
            float t = (Time.unscaledTime - startTime) / fadeTime;

            // Fade in the new source
            newSource.volume = Mathf.Lerp(0f, ambientVolume, t);

            // Fade out the old source (if one exists)
            if (oldSource != null)
            {
                oldSource.volume = Mathf.Lerp(ambientVolume, 0f, t);
            }
            yield return null;
        }

        // 4. Cleanup and final volume setting
        newSource.volume = ambientVolume;
        if (oldSource != null)
        {
            oldSource.Stop();
            oldSource.volume = ambientVolume; // Reset volume for next use
        }
    }
    #endregion

    #region Volume
    public float GetSFXVolume() => sfxVolume;
    public void SetMasterVolume(float v) { masterVolume = Mathf.Clamp01(v); mainAudioMixer.SetFloat("MasterVolume", VolumeToDecibels(v)); SaveVolumeSettings(); }
    public void SetSFXVolume(float v) { sfxVolume = Mathf.Clamp01(v); mainAudioMixer.SetFloat("SFXVolume", VolumeToDecibels(v)); SaveVolumeSettings(); LoopingSoundManager.Instance.UpdateLoopingSoundVolumes(); }
    public void SetDialogueVolume(float v) { dialogueVolume = Mathf.Clamp01(v); mainAudioMixer.SetFloat("DialogueVolume", VolumeToDecibels(v)); SaveVolumeSettings(); }
    public void SetMusicVolume(float v) { musicVolume = Mathf.Clamp01(v); mainAudioMixer.SetFloat("MusicVolume", VolumeToDecibels(v)); SaveVolumeSettings(); }
    public void SetAmbientVolume(float v) { ambientVolume = Mathf.Clamp01(v); mainAudioMixer.SetFloat("AmbientVolume", VolumeToDecibels(v)); SaveVolumeSettings(); }

    float VolumeToDecibels(float v) => v > 0 ? 20f * Mathf.Log10(v) : -80f;

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
    // CHANGED: Made FadeAudioSource public so SceneAmbientPlayer can access it
    public System.Collections.IEnumerator FadeAudioSource(AudioSource src, float target, float time)
    {
        float start = src.volume; float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(start, target, elapsed / time);
            yield return null;
        }
        src.volume = target;
    }

    System.Collections.IEnumerator FadeOutAndStop(AudioSource src, float time)
    {
        yield return StartCoroutine(FadeAudioSource(src, 0f, time));
        src.Stop();
    }
    #endregion
}

#region Looping Sound Manager Integration
public class LoopingSoundManager : MonoBehaviour
{
    private static LoopingSoundManager instance;
    private Dictionary<string, AudioSource> loopingSounds = new Dictionary<string, AudioSource>();

    public static LoopingSoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("LoopingSoundManager");
                instance = go.AddComponent<LoopingSoundManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) Destroy(gameObject);
    }

    public void PlayLoopingSound(AudioClip clip, string id, float volume = 1f)
    {
        if (clip == null || string.IsNullOrEmpty(id)) return;
        StopLoopingSound(id);

        var go = new GameObject($"LoopingSound_{id}");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.loop = true;
        src.outputAudioMixerGroup = AudioManager.Instance?.sfxGroup;
        src.volume = volume * (AudioManager.Instance?.GetSFXVolume() ?? 1f);
        src.Play();
        loopingSounds[id] = src;
        Debug.Log($"[LoopingSoundManager] Started looping sound: {id}");
    }

    public void StopLoopingSound(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (loopingSounds.TryGetValue(id, out var src) && src != null)
        {
            src.Stop();
            Destroy(src.gameObject);
            loopingSounds.Remove(id);
            Debug.Log($"[LoopingSoundManager] Stopped looping sound: {id}");
        }
    }

    public void StopAllLoopingSounds()
    {
        foreach (var s in loopingSounds.Values)
        {
            if (s != null) { s.Stop(); Destroy(s.gameObject); }
        }
        loopingSounds.Clear();
        Debug.Log("[LoopingSoundManager] Stopped all looping sounds");
    }

    public bool IsLoopingSoundPlaying(string id)
    {
        return loopingSounds.ContainsKey(id) && loopingSounds[id] != null && loopingSounds[id].isPlaying;
    }

    public void UpdateLoopingSoundVolumes()
    {
        float vol = AudioManager.Instance?.GetSFXVolume() ?? 1f;
        foreach (var s in loopingSounds.Values)
            if (s != null) s.volume = vol;
    }

    void OnDestroy() => StopAllLoopingSounds();
}
#endregion

#region AudioManager Extensions
public static class AudioManagerExtensions
{
    public static void PlayLoopingSFX(this AudioManager m, AudioClip clip, string id)
        => LoopingSoundManager.Instance.PlayLoopingSound(clip, id);

    public static void StopLoopingSFX(this AudioManager m, string id)
        => LoopingSoundManager.Instance.StopLoopingSound(id);

    public static void StopAllLoopingSFX(this AudioManager m)
        => LoopingSoundManager.Instance.StopAllLoopingSounds();
}
#endregion