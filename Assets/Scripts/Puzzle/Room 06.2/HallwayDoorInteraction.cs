// ==========================================================
// Developer: Jhon Jellar Z. Miranda
// Project: Echoes of Her Lullaby
// Description: Handles locked hallway door interactions, 
// dialogue checks, and scene transitions.
// ==========================================================

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Kailangan para sa Coroutines
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

    [Header("DEBUG & TESTING")]
    [Tooltip("Check this to ignore key and photo requirements during testing.")]
    public bool debug_BypassRequirements = false;

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

    void Update()
    {
        // DEBUG SHORTCUT: Pindutin ang F8 kapag nasa tapat ng pinto para mag-force unlock
        if (playerInRange && Input.GetKeyDown(KeyCode.F8))
        {
            Debug.Log("[DEBUG] Force Unlocking Door via F8!");
            UnlockAndOpenDoor();
        }
    }

    public void Interact()
    {
        if (isTransitioning) return;
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
        if (doorUnlocked)
        {
            OpenDoor();
            return;
        }

        // 0. DEBUG BYPASS CHECK
        if (debug_BypassRequirements)
        {
            Debug.Log("[DEBUG] Requirements Bypassed!");
            UnlockAndOpenDoor();
            return;
        }

        // 1. CHECK INVENTORY FOR KEY
        bool hasKey = false;
        if (SaveSystem.Instance != null && SaveSystem.Instance.HasItem(requiredItemId)) hasKey = true;
        else if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(requiredItemId)) hasKey = true;

        if (!hasKey)
        {
            ShowLockedMessage(lockedDialogue);
            return;
        }

        // 2. BULLETPROOF PUZZLE CHECK
        if (requirePhotoInteraction)
        {
            bool isEmilyChasing = false;
            // Note: Siguraduhing existing ang Room06_HallwayController script mo
            if (Room06_HallwayController.Instance != null)
            {
                isEmilyChasing = Room06_HallwayController.Instance.isEmilyHunting;
            }

            bool hasCheckedPhoto = PlayerPrefs.GetInt(photoPrefsKey, 0) == 1;

            if (!hasCheckedPhoto && !isEmilyChasing)
            {
                ShowLockedMessage(missingPhotoDialogue);
                return;
            }
        }

        // TAMA LAHAT! May susi at tapos na ang jumpscare/hunting.
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

        if (consumeKeyOnUse && !string.IsNullOrEmpty(requiredItemId) && !debug_BypassRequirements)
        {
            if (SaveSystem.Instance != null) SaveSystem.Instance.RemoveInventoryItem(requiredItemId);
            if (InventoryManager.Instance != null) InventoryManager.Instance.RemoveItem(requiredItemId);
        }

        UpdateVisuals();

        if (unlockDoorSound != null && audioSource != null)
            audioSource.PlayOneShot(unlockDoorSound);

        ShowDialogue(successDialogue);

        // Gumagamit na ng Coroutine para hindi maapektuhan ng Time.timeScale = 0 (Dialogue Pause)
        StartCoroutine(OpenDoorRoutine(1.5f));
    }

    IEnumerator OpenDoorRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        OpenDoor();
    }

    void OpenDoor()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (doorOpenSound != null && audioSource != null) audioSource.PlayOneShot(doorOpenSound);

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
            StartCoroutine(TransitionRoutine(transitionDelay));
        }
    }

    IEnumerator TransitionRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        TransitionToScene();
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
        Debug.Log("[Dialogue Check] Lisa says: " + message);

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