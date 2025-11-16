using UnityEngine;

/// <summary>
/// Broadcasts noise events to Emily AI when player performs actions
/// Compatible with your existing AudioManager
/// </summary>
public class NoiseEventSystem : MonoBehaviour
{
    public static NoiseEventSystem Instance { get; private set; }

    [Header("Noise Sounds (Optional)")]
    public AudioClip runningSound;
    public AudioClip interactSound;
    public AudioClip doorSound;

    [Header("Settings")]
    public bool playNoiseAudio = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Pre-warm the dialogue system
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("", "");
            Debug.Log("[NoiseEventSystem] Pre-warmed Dialogue System.");
        }
    }

    /// <summary>
    /// Broadcast a noise event to Emily AI
    /// </summary>
    public static void BroadcastNoise(Vector3 position, float strength = 1f)
    {
        if (EmilyAIController.Instance != null && EmilyAIController.Instance.perception != null)
        {
            EmilyAIController.Instance.perception.OnNoiseHeard(position, strength);
            Debug.Log($"[NoiseEvent] Broadcast at {position} with strength {strength}");
        }
    }

    // Call these from player controller or interaction system

    public void OnPlayerRun(Vector3 position)
    {
        BroadcastNoise(position, 0.8f);

        // Optionally play running sound
        if (playNoiseAudio && runningSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(runningSound, position, 0.5f);
        }
    }

    public void OnPlayerInteract(Vector3 position)
    {
        BroadcastNoise(position, 0.6f);

        // Optionally play interact sound
        if (playNoiseAudio && interactSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(interactSound, position, 0.7f);
        }
    }

    public void OnPlayerOpenDoor(Vector3 position)
    {
        BroadcastNoise(position, 1f);

        // Optionally play door sound
        if (playNoiseAudio && doorSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(doorSound, position, 0.8f);
        }
    }

    /// <summary>
    /// Integration point: Add this to your JoystickPlayerController
    /// Call in Update() when player is moving fast
    /// </summary>
    public static void OnPlayerMovement(Vector3 position, float movementSpeed)
    {
        // Only broadcast noise if moving fast (running)
        if (movementSpeed > 4f) // Adjust threshold as needed
        {
            BroadcastNoise(position, 0.5f);
        }
    }

    /// <summary>
    /// Generic noise broadcast for any custom events
    /// </summary>
    public static void MakeNoise(Vector3 position, float strength, AudioClip soundEffect = null)
    {
        BroadcastNoise(position, strength);

        if (soundEffect != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(soundEffect, position);
        }
    }
}