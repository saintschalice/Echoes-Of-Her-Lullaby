using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [Header("Door Identity")]
    public string doorName = "Front Door";

    [Header("Scene Transition")]
    public string nextSceneName = "Room02_LivingRoom";
    public string spawnPointName = "FromHallway"; // Where player spawns in next room

    [Header("Lock Requirements")]
    public bool requiresItem = true;
    public string requiredItemId = "house_key";

    [Header("Alternative Unlock (Puzzle/Event)")]
    public bool unlockedByEvent = false;
    public string unlockEventId = ""; // For puzzle-based unlocks

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;

    [Header("Visual Indicators")]
    public GameObject lockedIndicator;
    public GameObject unlockedIndicator;

    [Header("Audio")]
    public AudioClip lockedDoorSound;
    public AudioClip unlockDoorSound;
    public AudioClip doorOpenSound;

    [Header("Animation")]
    public Animator doorAnimator;
    public string openTrigger = "Open";

    [Header("Messages")]
    public string lockedMessage = "The door is locked.";
    public string needItemMessage = "I need to find a key.";
    public string unlockMessage = "The door is unlocked!";

    private AudioSource audioSource;
    private bool playerInRange = false;
    private bool doorUnlocked = false;
    private bool isOpening = false;
    private DialogueSystemV2 dialogueSystem;

    void Start()
    {
        Debug.Log($"[{doorName}] Initialized");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();

        UpdateVisuals();
        CheckSaveState();
    }

    public void OnInteract(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);

        if (!playerInRange)
        {
            ShowDialogue("I need to get closer.");
            return;
        }

        if (isOpening) return;

        AttemptOpenDoor();
    }

    public void OnFocus(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
    }

    bool IsPlayerInRange(Transform playerTransform)
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= interactionRadius;
    }

    void AttemptOpenDoor()
    {
        Debug.Log($"[{doorName}] Attempting to open...");

        // Check if already unlocked
        if (doorUnlocked)
        {
            OpenDoor();
            return;
        }

        // Check if unlocked by event/puzzle
        if (unlockedByEvent && !string.IsNullOrEmpty(unlockEventId))
        {
            if (SaveSystem.Instance != null && SaveSystem.Instance.WasObjectExamined(unlockEventId))
            {
                UnlockDoor();
                return;
            }
        }

        // Check if requires item
        if (requiresItem)
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId))
            {
                UnlockDoor();
                return;
            }
            else
            {
                ShowLockedMessage();
                return;
            }
        }

        // Door is locked with no way to open yet
        ShowLockedMessage();
    }

    void ShowLockedMessage()
    {
        Debug.Log($"[{doorName}] Locked!");

        if (lockedDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lockedDoorSound);
        }

        string message = lockedMessage;
        if (requiresItem)
        {
            message += " " + needItemMessage;
        }

        ShowDialogue(message);
    }

    void UnlockDoor()
    {
        Debug.Log($"[{doorName}] Unlocking!");

        doorUnlocked = true;
        UpdateVisuals();

        // Save unlock state
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(GetDoorUnlockId());
        }

        if (unlockDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(unlockDoorSound);
        }

        ShowDialogue(unlockMessage);

        // Auto-open after brief delay
        Invoke(nameof(OpenDoor), 1.5f);
    }

    void OpenDoor()
    {
        Debug.Log($"[{doorName}] Opening...");

        isOpening = true;

        if (doorOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }

        Invoke(nameof(LoadNextScene), 1.5f);
    }

    void LoadNextScene()
    {
        Debug.Log($"[{doorName}] Loading scene: {nextSceneName}");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadScene(nextSceneName, spawnPointName);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnRoomEntered(nextSceneName);
        }
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

    void CheckSaveState()
    {
        if (SaveSystem.Instance == null) return;

        if (SaveSystem.Instance.WasObjectExamined(GetDoorUnlockId()))
        {
            doorUnlocked = true;
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
        if (lockedIndicator != null)
            lockedIndicator.SetActive(!doorUnlocked);

        if (unlockedIndicator != null)
            unlockedIndicator.SetActive(doorUnlocked);
    }

    string GetDoorUnlockId()
    {
        return $"{gameObject.name}_Unlocked";
    }

    // PUBLIC METHOD: For puzzle systems to unlock doors
    public void UnlockDoorByPuzzle()
    {
        if (!doorUnlocked)
        {
            UnlockDoor();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = doorUnlocked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}