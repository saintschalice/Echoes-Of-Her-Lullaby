using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontDoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public string nextSceneName = "Room02_LivingRoom";
    public string requiredItemId = "house_key";

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Visual Indicators")]
    public GameObject lockedIndicator;

    [Header("Audio")]
    public AudioClip lockedDoorSound;
    public AudioClip unlockDoorSound;
    public AudioClip doorOpenSound;

    [Header("Animation (Optional)")]
    public Animator doorAnimator;
    public string openTrigger = "Open";

    private AudioSource audioSource;
    private bool playerInRange = false;
    private bool doorUnlocked = false;
    private bool isOpening = false;
    private DialogueSystemV2 dialogueSystem;

    private const string DOOR_UNLOCKED_ID = "FrontDoor_Unlocked";

    void Start()
    {
        Debug.LogError("[FrontDoor] SCRIPT IS RUNNING - INITIALIZATION COMPLETE!");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();

        if (lockedIndicator != null)
            lockedIndicator.SetActive(true);

        CheckSaveState();

        // Verify setup
        Collider2D col = GetComponent<Collider2D>();
        Debug.Log($"[FrontDoor] GameObject: {gameObject.name}");
        Debug.Log($"[FrontDoor] Layer: {LayerMask.LayerToName(gameObject.layer)}");
        Debug.Log($"[FrontDoor] Has Collider2D: {col != null}");
        if (col != null)
        {
            Debug.Log($"[FrontDoor] Collider is Trigger: {col.isTrigger}");
        }
    }

    void Update()
    {
        CheckPlayerDistance();
        // REMOVED: Input.GetMouseButtonDown(0) check - only OnMouseDown should handle clicks
    }

    void OnMouseDown()
    {
        Debug.LogError("[FrontDoor] =============== MOUSE CLICKED ON DOOR! ===============");

        if (!playerInRange)
        {
            Debug.LogError("[FrontDoor] Player not in range - need to be within " + interactionRadius + " units");
            return;
        }

        if (isOpening)
        {
            Debug.Log("[FrontDoor] Door is already opening");
            return;
        }

        AttemptOpenDoor();
    }

    void CheckPlayerDistance()
    {
        GameObject player = GameObject.FindGameObjectWithTag(requiredTag);

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            bool wasInRange = playerInRange;
            playerInRange = distance <= interactionRadius;

            // Log when range status changes
            if (wasInRange != playerInRange)
            {
                Debug.Log($"[FrontDoor] Player range changed: {playerInRange} (distance: {distance:F2})");
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    void AttemptOpenDoor()
    {
        Debug.Log("[FrontDoor] Attempting to open door...");

        // Check if we need a key
        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(requiredItemId))
        {
            ShowLockedMessage();
            return;
        }

        UnlockAndOpenDoor();
    }

    void ShowLockedMessage()
    {
        Debug.Log("[FrontDoor] Door is locked!");

        if (lockedDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lockedDoorSound);
        }

        ShowDialogue("The door is locked. I need to find a key.");
    }

    void UnlockAndOpenDoor()
    {
        Debug.Log("[FrontDoor] Unlocking and opening door!");

        isOpening = true;
        doorUnlocked = true;

        if (lockedIndicator != null)
            lockedIndicator.SetActive(false);

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(DOOR_UNLOCKED_ID);
        }

        if (unlockDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(unlockDoorSound);
        }

        ShowDialogue("The key fits! The door is unlocked.");

        Invoke(nameof(OpenDoor), 2f);
    }

    void OpenDoor()
    {
        Debug.Log("[FrontDoor] Opening door...");

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
        Debug.Log($"[FrontDoor] Loading scene: {nextSceneName}");

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnRoomEntered(nextSceneName);
        }

        SceneManager.LoadScene(nextSceneName);
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

        if (SaveSystem.Instance.WasObjectExamined(DOOR_UNLOCKED_ID))
        {
            doorUnlocked = true;

            if (lockedIndicator != null)
                lockedIndicator.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = doorUnlocked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}