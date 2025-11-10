using UnityEngine;

public enum EmilyAudioState
{
    Patrol,
    Investigate,
    Hunt,
    Search,
    Cooldown
}

/// <summary>
/// Manages Emily's audio with 3D spatialization and occlusion
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EmilyAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip presenceAmbient;
    public AudioClip huntChaseMusic;
    public AudioClip catchSound;
    public AudioClip windPushSound;
    public AudioClip voiceLine;

    [Header("Settings")]
    public float minDistance = 3f;
    public float maxDistance = 15f;
    public AnimationCurve falloffCurve = AnimationCurve.Linear(0, 1, 1, 0);

    [Header("Occlusion")]
    public bool useOcclusion = true;
    public LayerMask occlusionMask;
    public float occludedVolume = 0.3f;

    private AudioSource audioSource;
    private EmilyAIController controller;
    private Transform player;
    private EmilyAudioState currentAudioState;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ConfigureAudioSource();
    }

    private void Start()
    {
        controller = GetComponent<EmilyAIController>();
        player = controller.player;
    }

    void ConfigureAudioSource()
    {
        audioSource.spatialBlend = 1f; // Full 3D
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        UpdateSpatialAudio();
    }

    void UpdateSpatialAudio()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float volumeMultiplier = falloffCurve.Evaluate(distance / maxDistance);

        // Check for occlusion
        if (useOcclusion)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                player.position - transform.position,
                distance,
                occlusionMask
            );

            if (hit.collider != null)
            {
                volumeMultiplier *= occludedVolume;
            }
        }

        audioSource.volume = volumeMultiplier * AudioManager.Instance.sfxVolume;
    }

    public void SetAudioState(EmilyAudioState newState)
    {
        if (currentAudioState == newState) return;
        currentAudioState = newState;

        switch (newState)
        {
            case EmilyAudioState.Hunt:
                PlaySound(huntChaseMusic);
                break;
            case EmilyAudioState.Patrol:
                PlaySound(presenceAmbient);
                break;
            default:
                PlaySound(presenceAmbient);
                break;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayPresenceSound()
    {
        PlaySound(presenceAmbient);
    }

    public void PlayCatchSound()
    {
        AudioManager.Instance?.PlaySFX(catchSound, transform.position);
    }

    public void PlayWindPushSound()
    {
        AudioManager.Instance?.PlaySFX(windPushSound, transform.position);
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
    }
}