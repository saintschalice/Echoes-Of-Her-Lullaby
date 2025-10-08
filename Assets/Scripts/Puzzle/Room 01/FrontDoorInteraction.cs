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
    public GameObject lockedIndicator; // Optional: Show lock icon

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
    private Camera mainCamera;

    // Save state identifier
    private const string DOOR_UNLOCKED_ID = "FrontDoor_Unlocked";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        dialogueSystem = FindFirstObjectByType<DialogueSystemV2>();
        mainCamera = Camera.main;

        if (lockedIndicator != null)
            lockedIndicator.SetActive(true);

        CheckSaveState();
    }

    void Update()
    {
        CheckPlayerDistance();

        // Handle TOUCH/CLICK interaction ONLY
        if (playerInRange && !isOpening)
        {
            if (Input.GetMouseButtonDown(0)) // Left click or touch
            {
                if (IsTappedOn())
                {
                    AttemptOpenDoor();
                }
            }
        }
    }

    bool IsTappedOn()
    {
        if (mainCamera == null) return false;

        Vector2 touchPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            return hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform);
        }

        return false;
    }

    void CheckPlayerDistance()
    {
        GameObject player = GameObject.FindGameObjectWithTag(requiredTag);

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            playerInRange = distance <= interactionRadius;
        }
    }

    void AttemptOpenDoor()
    {
        // Check if player has the key
        if (InventoryManager.Instance == null || !InventoryManager.Instance.HasItem(requiredItemId))
        {
            ShowLockedMessage();
            return;
        }

        // Player has the key - unlock and open door
        UnlockAndOpenDoor();
    }

    void ShowLockedMessage()
    {
        if (lockedDoorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lockedDoorSound);
        }

        ShowDialogue("The door is locked. I need to find a key.");
    }

    void UnlockAndOpenDoor()
    {
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
            Debug.Log($"Lisa: {message}");
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