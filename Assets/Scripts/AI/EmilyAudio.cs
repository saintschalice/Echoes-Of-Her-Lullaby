using UnityEngine;
using System.Collections;

public sealed class EmilyAudio : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip catchClip;
    public AudioClip huntClip;
    public AudioClip searchClip;
    public AudioClip investigateClip;
    public AudioClip patrolClip;
    public AudioClip cooldownClip;

    [Header("Settings")]
    [Tooltip("Time in seconds for the fade out/in effect.")]
    public float fadeDuration = 0.5f;

    AudioSource _source;
    Coroutine _currentFade;
    float _targetVolume = 1f;

    void Awake()
    {
        _source = GetComponent<AudioSource>();

        if (_source == null)
        {
            Debug.LogError("[EMILY AUDIO] Missing AudioSource on Emily!");
        }
        else
        {
            // IMPORTANT: Set loop to true so state music (like Hunt/Patrol) loops continuously
            _source.loop = true;
            // Capture the initial volume from the Inspector so we can return to it
            _targetVolume = _source.volume;
        }
    }

    public void PlayCatch()
    {
        if (catchClip == null)
        {
            Debug.LogError("[EMILY AUDIO] catchClip is NOT assigned!");
            return;
        }

        // Use PlayClipAtPoint for catch so it plays even if this object gets disabled immediately after
        AudioSource.PlayClipAtPoint(catchClip, transform.position, 1f);
    }

    public void ToHunt() => SwitchTrack(huntClip);
    public void ToSearch() => SwitchTrack(searchClip);
    public void ToInvestigate() => SwitchTrack(investigateClip);
    public void ToPatrol() => SwitchTrack(patrolClip);
    public void ToCooldown() => SwitchTrack(cooldownClip);

    void SwitchTrack(AudioClip nextClip)
    {
        if (nextClip == null) return;
        if (_source == null) return;

        // If we are already playing this exact clip, do nothing
        if (_source.clip == nextClip && _source.isPlaying) return;

        // Stop any currently running fade so they don't fight
        if (_currentFade != null) StopCoroutine(_currentFade);

        // Start the new fade sequence
        _currentFade = StartCoroutine(FadeRoutine(nextClip));
    }

    IEnumerator FadeRoutine(AudioClip nextClip)
    {
        // 1. Fade Out (only if audio is currently playing)
        if (_source.isPlaying)
        {
            float startVol = _source.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                _source.volume = Mathf.Lerp(startVol, 0f, timer / fadeDuration);
                yield return null;
            }
            _source.volume = 0f;
            _source.Stop();
        }

        // 2. Swap Clip
        _source.clip = nextClip;
        _source.Play();

        // 3. Fade In
        float inTimer = 0f;
        while (inTimer < fadeDuration)
        {
            inTimer += Time.deltaTime;
            _source.volume = Mathf.Lerp(0f, _targetVolume, inTimer / fadeDuration);
            yield return null;
        }

        _source.volume = _targetVolume;
        _currentFade = null;
    }
}