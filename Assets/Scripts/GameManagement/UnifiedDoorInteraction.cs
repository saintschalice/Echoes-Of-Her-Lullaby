using UnityEngine;
using UnityEngine.SceneManagement;

public class UnifiedDoorInteraction : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public string targetSceneName = "Room02_LivingRoom";
    public string doorID = "Door_Foyer_To_LivingRoom"; // Unique ID for save system

    [Header("Lock Settings")]
    public bool startsLocked = true;
    public string requiredItemId = "house_key";
    public bool consumeKeyOnUse = false; // Should key disappear after use?
    public bool unlockPermanently = true; // Save unlocked state?

    [Header("Spawn Settings")]
    public string targetSpawnPointID = "Main"; // Which spawn point to use in target scene
    public Vector3 spawnOffset = Vector3.zero; // Optional offset from spawn point

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";
    public bool usePhysicalCollider = true; // NEW: Set false if door shouldn't block player

    [Header("Visual Indicators")]
    public GameObject lockedIndicator;
    public GameObject interactionPrompt; // "Press E to interact" UI

    [Header("Audio")]
    public AudioClip lockedDoorSound;
    public AudioClip unlockDoorSound;
    public AudioClip doorOpenSound;

    [Header("Animation")]
    public Animator doorAnimator;
    public string openTrigger = "Open";

    [Header("Transition")]
    public float transitionDelay = 1.5f;
    public bool useFadeTransition = true;

    private AudioSource audioSource;
    private bool playerInRange = false;
    private bool doorUnlocked = false;
    private bool isTransitioning = false;
    private DialogueSystemV2 dialogueSystem;

    void Start()
    {
        Debug.Log($"[UnifiedDoor] Initializing door: {doorID}");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();

        // Load saved door state
        LoadDoorState();

        UpdateVisuals();

        // Verify collider setup
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Debug.Log($"[UnifiedDoor] {doorID} - Collider is trigger: {col.isTrigger}");
        }
        else
        {
            Debug.LogWarning($"[UnifiedDoor] {doorID} - No Collider2D found!");
        }
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        // Tracker handles range, so we just check state
        if (isTransitioning) return;

        AttemptOpenDoor();
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);

        if (!playerInRange)
        {
            ShowDialogue($"I need to get closer to the door.");
            return;
        }

        if (isTransitioning) return;

        AttemptOpenDoor();
    }

    public void OnFocus(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(playerInRange && !isTransitioning);
        }
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    bool IsPlayerInRange(Transform playerTransform)
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= interactionRadius;
    }

    void LoadDoorState()
    {
        if (SaveSystem.Instance == null) return;

        // Check if door was previously unlocked
        string currentScene = SceneManager.GetActiveScene().name;
        RoomState roomState = SaveSystem.Instance.GetRoomState(currentScene);

        if (roomState.openedDoors.Contains(doorID))
        {
            doorUnlocked = true;
            Debug.Log($"[UnifiedDoor] {doorID} loaded as unlocked from save");
        }
        else
        {
            doorUnlocked = !startsLocked;
        }
    }

    void SaveDoorState()
    {
        if (SaveSystem.Instance == null || !unlockPermanently) return;

        string currentScene = SceneManager.GetActiveScene().name;
        RoomState roomState = SaveSystem.Instance.GetRoomState(currentScene);

        if (!roomState.openedDoors.Contains(doorID))
        {
            roomState.openedDoors.Add(doorID);
            SaveSystem.Instance.UpdateRoomState(currentScene, roomState);
            Debug.Log($"[UnifiedDoor] {doorID} saved as unlocked");
        }
    }

    void AttemptOpenDoor()
    {
        Debug.Log($"[UnifiedDoor] Attempting to open {doorID}...");

        // If door is already unlocked, just open it
        if (doorUnlocked)
        {
            OpenDoor();
            return;
        }

        // Check if player has required item
        if (string.IsNullOrEmpty(requiredItemId))
        {
            // No key required, unlock and open
            UnlockAndOpenDoor();
            return;
        }

        // Check inventory for key
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(requiredItemId))
        {
            UnlockAndOpenDoor();
        }
        else if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId))
        {
            UnlockAndOpenDoor();
        }
        else
        {
            ShowLockedMessage();
        }
    }

    void ShowLockedMessage()
    {
        Debug.Log($"[UnifiedDoor] {doorID} is locked!");

        if (lockedDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lockedDoorSound);
        }

        string message = string.IsNullOrEmpty(requiredItemId)
            ? "The door is locked."
            : $"The door is locked. I need to find a {requiredItemId}.";

        ShowDialogue(message);
    }

    void UnlockAndOpenDoor()
    {
        Debug.Log($"[UnifiedDoor] Unlocking {doorID}!");

        doorUnlocked = true;

        // Save unlocked state
        if (unlockPermanently)
        {
            SaveDoorState();
        }

        // Consume key if required
        if (consumeKeyOnUse && !string.IsNullOrEmpty(requiredItemId))
        {
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.RemoveInventoryItem(requiredItemId);
            }
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RemoveItem(requiredItemId);
            }
        }

        UpdateVisuals();

        if (unlockDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(unlockDoorSound);
        }

        ShowDialogue("The key fits! The door is unlocked.");

        // Open after short delay
        Invoke(nameof(OpenDoor), 1.5f);
    }

    void OpenDoor()
    {
        if (isTransitioning) return;

        Debug.Log($"[UnifiedDoor] Opening {doorID}...");
        isTransitioning = true;

        if (doorOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }

        // Save current position before transitioning
        if (SaveSystem.Instance != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(requiredTag);
            if (player != null)
            {
                SaveSystem.Instance.GetCurrentSaveData().playerPosition = player.transform.position;
            }
        }

        // Use fade transition if available
        if (useFadeTransition && ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(transitionDelay * 0.6f, TransitionToScene);
        }
        else
        {
            Invoke(nameof(TransitionToScene), transitionDelay);
        }
    }

    void TransitionToScene()
    {
        Debug.Log($"[UnifiedDoor] Transitioning to scene: {targetSceneName}");

        // Store spawn point info for target scene
        PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnPointID);
        PlayerPrefs.Save();

        // Notify save system about room change
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnRoomEntered(targetSceneName);
        }

        // Load scene
        SceneManager.LoadScene(targetSceneName);

        // Fade in will happen automatically in ScreenFader's Start()
    }

    void ShowDialogue(string message)
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue(message, "Lisa");
        }
        else
        {
            Debug.Log($"[Dialogue] Lisa: {message}");
        }
    }

    void UpdateVisuals()
    {
        if (lockedIndicator != null)
        {
            lockedIndicator.SetActive(!doorUnlocked);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = doorUnlocked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        // Draw direction arrow
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.up * 1f);
    }

    // Public methods for external control
    public void UnlockDoor()
    {
        doorUnlocked = true;
        SaveDoorState();
        UpdateVisuals();
    }

    public void LockDoor()
    {
        doorUnlocked = false;
        UpdateVisuals();
    }

    public bool IsUnlocked()
    {
        return doorUnlocked;
    }
}