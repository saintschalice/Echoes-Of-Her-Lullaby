using UnityEngine;

public sealed class EmilyAudio : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip catchClip;
    public AudioClip huntClip;
    public AudioClip searchClip;
    public AudioClip investigateClip;
    public AudioClip patrolClip;
    public AudioClip cooldownClip;

    AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();

        if (_source == null)
        {
            Debug.LogError("[EMILY AUDIO] Missing AudioSource on Emily!");
        }
    }

    public void PlayCatch()
    {
        if (catchClip == null)
        {
            Debug.LogError("[EMILY AUDIO] catchClip is NOT assigned!");
            return;
        }

        AudioSource.PlayClipAtPoint(catchClip, transform.position, 1f);
    }

    public void ToHunt() => PlayIfAssigned(huntClip);
    public void ToSearch() => PlayIfAssigned(searchClip);
    public void ToInvestigate() => PlayIfAssigned(investigateClip);
    public void ToPatrol() => PlayIfAssigned(patrolClip);
    public void ToCooldown() => PlayIfAssigned(cooldownClip);

    void PlayIfAssigned(AudioClip clip)
    {
        if (clip == null) return;
        _source.PlayOneShot(clip);
    }
}
