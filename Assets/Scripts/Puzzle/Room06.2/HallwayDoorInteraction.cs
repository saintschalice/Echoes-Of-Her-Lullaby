using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;

public class HallwayDoorInteraction : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public string targetSceneName = "Room07_LisasBedroom";
    public string doorID = "Door_Hallway_To_LisaBed"; // Unique ID for save system

    [Header("Lock Settings")]
    public bool startsLocked = true;
    public string requiredItemId = "bedroom_key";
    public bool consumeKeyOnUse = true;
    public bool unlockPermanently = true;

    [Header("Hallway Puzzle Requirements")]
    public bool requirePhotoInteraction = true;
    public string photoPrefsKey = "R06_PhotoInteracted";

    [Header("Custom Door Dialogue")]
    [TextArea] public string lockedDialogue = "Lisa's Bedroom? It's locked...";
    [TextArea] public string missingPhotoDialogue = "It's locked. But I feel like I should look around this hallway first...";
    [TextArea] public string successDialogue = "The key fits! The door is unlocked.";

    [Header("Spawn Settings")]
    public string targetSpawnPointID = "Main";
    public Vector3 spawnOffset = Vector3.zero;

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public string requiredTag = "Player";

    [Header("Visual Indicators")]
    public GameObject lockedIndicator;
    public GameObject interactionPrompt;

    [Header("Audio")]
    public AudioClip lockedDoorSound;
    public AudioClip unlockDoorSound;
    public AudioClip doorOpenSound;

    [Header("Transition")]
    public float transitionDelay = 1.5f;
    public bool useFadeTransition = true;

    private AudioSource audioSource;
    private bool playerInRange = false;
    private bool doorUnlocked = false;
    private bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        LoadDoorState();
        UpdateVisuals();
    }

    public void Interact()
    {
        if (isTransitioning) return;

        // Tinanggal muna natin ang IsDialogueActive check dito para siguradong papasok
        AttemptOpenDoor();
    }

    public void OnInteract(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);
        if (!playerInRange)
        {
            ShowDialogue("I need to get closer to the door.");
            return;
        }

        if (isTransitioning) return;
        AttemptOpenDoor();
    }

    public void OnFocus(PlayerContext context)
    {
        playerInRange = IsPlayerInRange(context.Transform);
        if (interactionPrompt != null) interactionPrompt.SetActive(playerInRange && !isTransitioning);
    }

    public void OnBlur(PlayerContext context)
    {
        playerInRange = false;
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    bool IsPlayerInRange(Transform playerTransform)
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= interactionRadius;
    }

    void LoadDoorState()
    {
        if (SaveSystem.Instance == null) return;

        string currentScene = SceneManager.GetActiveScene().name;
        RoomState roomState = SaveSystem.Instance.GetRoomState(currentScene);

        if (roomState.openedDoors.Contains(doorID)) doorUnlocked = true;
        else doorUnlocked = !startsLocked;
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
        }
    }

    void AttemptOpenDoor()
    {
        Debug.Log($"[DoorLogic] Checking door: {doorID}");

        if (doorUnlocked)
        {
            Debug.Log("[DoorLogic] Door is already unlocked! Opening...");
            OpenDoor();
            return;
        }

        // 1. CHECK INVENTORY FOR KEY
        bool hasKey = false;
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(requiredItemId)) hasKey = true;
        else if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId)) hasKey = true;

        if (!hasKey)
        {
            Debug.Log("[DoorLogic] Result: Missing Key.");
            ShowLockedMessage(lockedDialogue);
            return;
        }

        // 2. CHECK PHOTO FRAME PROGRESS
        if (requirePhotoInteraction)
        {
            bool hasCheckedPhoto = PlayerPrefs.GetInt(photoPrefsKey, 0) == 1;
            if (!hasCheckedPhoto)
            {
                Debug.Log("[DoorLogic] Result: Has Key, but Photo Frame NOT checked.");
                ShowLockedMessage(missingPhotoDialogue);
                return;
            }
        }

        // TAMA LAHAT! May susi at nakita ang picture.
        Debug.Log("[DoorLogic] Result: ALL REQUIREMENTS MET! Unlocking door.");
        UnlockAndOpenDoor();
    }

    void ShowLockedMessage(string message)
    {
        if (lockedDoorSound != null && audioSource != null)
            audioSource.PlayOneShot(lockedDoorSound);

        ShowDialogue(message);
    }

    void UnlockAndOpenDoor()
    {
        doorUnlocked = true;

        if (unlockPermanently) SaveDoorState();

        if (consumeKeyOnUse && !string.IsNullOrEmpty(requiredItemId))
        {
            if (SaveSystem.Instance != null) SaveSystem.Instance.RemoveInventoryItem(requiredItemId);
            if (InventoryManager.Instance != null) InventoryManager.Instance.RemoveItem(requiredItemId);
        }

        UpdateVisuals();

        if (unlockDoorSound != null && audioSource != null)
            audioSource.PlayOneShot(unlockDoorSound);

        ShowDialogue(successDialogue);

        Invoke(nameof(OpenDoor), 1.5f);
    }

    void OpenDoor()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (doorOpenSound != null && audioSource != null) audioSource.PlayOneShot(doorOpenSound);

        // Tinanggal muna ang Animator requirement baka nagko-cause ng error kung wala
        // if (doorAnimator != null) doorAnimator.SetTrigger(openTrigger);

        if (SaveSystem.Instance != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(requiredTag);
            if (player != null) SaveSystem.Instance.GetCurrentSaveData().playerPosition = player.transform.position;
        }

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
        if (string.IsNullOrEmpty(targetSceneName))
        {
            isTransitioning = false;
            return;
        }

        PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnPointID);
        PlayerPrefs.Save();

        if (SaveSystem.Instance != null) SaveSystem.Instance.OnRoomEntered(targetSceneName);

        SceneManager.LoadScene(targetSceneName);
    }

    void ShowDialogue(string message)
    {
        // DEBUG LOG para kahit hindi lumabas sa screen, makikita mo sa Unity Console
        Debug.Log("[Dialogue Check] Lisa says: " + message);

        // BALIK SA INSTANCE (Yung tested and working sa Photo Frame)
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(message, "Lisa");
        }
        else
        {
            Debug.LogWarning("[DoorLogic] ERROR: Walang DialogueSystemV2 na nahanap sa scene!");
        }
    }

    void UpdateVisuals()
    {
        if (lockedIndicator != null) lockedIndicator.SetActive(!doorUnlocked);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = doorUnlocked ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}