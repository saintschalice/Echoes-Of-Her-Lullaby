using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class EmilyAudio : MonoBehaviour
{
    public AudioClip idleLoop, huntLoop, catchClip;

    AudioSource _src; void Awake()
    {
        _src = GetComponent<AudioSource>();
        _src.spatialBlend = 1; _src.loop = true; _src.playOnAwake = false;
    }

    public void ToPatrol() => Play(idleLoop);
    public void ToInvestigate() => Play(idleLoop);
    public void ToSearch() => Play(idleLoop);
    public void ToCooldown() => Play(idleLoop);
    public void ToHunt() => Play(huntLoop);

    void Play(AudioClip c) { if (!_src || c == null) return; _src.clip = c; _src.Play(); }
    public void PlayCatch() { AudioSource.PlayClipAtPoint(catchClip, transform.position); }
}
