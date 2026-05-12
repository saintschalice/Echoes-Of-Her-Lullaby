using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Room 06 - Hallway Upstairs Controller
/// Flow: Intro dialogue → Photo frame interaction → Photo scratches out → Emily spawns and hunts
/// </summary>
public class Room06_HallwayController : MonoBehaviour
{
    public static Room06_HallwayController Instance { get; private set; }

    [Header("Photo Frame")]
    [Tooltip("Photo frame GameObject (sprite)")]
    public GameObject photoFrame;
    
    [Tooltip("Normal photo sprite (before scratch)")]
    public Sprite normalPhotoSprite;
    
    [Tooltip("Scratched photo sprite (after interaction)")]
    public Sprite scratchedPhotoSprite;
    
    [Header("Photo Panel UI")]
    [Tooltip("UI Panel to show full photo")]
    public GameObject photoPanel;
    
    [Tooltip("Image component in panel to show photo")]
    public UnityEngine.UI.Image photoPanelImage;
    
    [Header("Emily Configuration")]
    [Tooltip("Emily GameObject (must have NavMeshAgent and EmilyGhost)")]
    public GameObject emilyGameObject;
    
    [Tooltip("Where Emily spawns after photo interaction")]
    public Transform emilySpawnPoint;
    
    [Tooltip("Emily's chase speed")]
    [Range(1f, 10f)]
    public float emilyChaseSpeed = 4.5f;
    
    [Tooltip("Distance for Game Over")]
    [Range(0.5f, 3f)]
    public float catchDistance = 1.0f;
    
    [Header("Audio")]
    [Tooltip("Sound when photo gets scratched")]
    public AudioClip scratchSound;
    
    [Tooltip("Emily spawn/jumpscare sound")]
    public AudioClip emilySpawnSound;
    
    [Tooltip("Chase music loop")]
    public AudioClip chaseMusicLoop;
    
    public AudioSource roomAudioSource;
    
    [Header("Timing")]
    [Tooltip("Delay after scratch before Emily spawns")]
    [Range(0f, 3f)]
    public float spawnDelay = 1.5f;
    
    [Header("Persistence")]
    [Tooltip("Save flag for intro dialogue")]
    public string introDialogueFlag = "Room06_Intro";
    
    [Tooltip("Save flag for photo interaction")]
    public string photoInteractedFlag = "Room06_PhotoInteracted";
    
    [Header("Debug")]
    public bool debugMode = true;

    // State tracking
    private bool hasPlayedIntro = false;
    private bool hasInteractedWithPhoto = false;
    private bool isEmilyHunting = false;
    
    private NavMeshAgent emilyAgent;
    private EmilyGhost emilyScript;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        // CRITICAL: Force close photo panel immediately on awake
        // This prevents it from showing if it was left open from previous game over
        if (photoPanel != null)
        {
            photoPanel.SetActive(false);
            if (debugMode) Debug.Log("[Room06] Photo panel force closed in Awake");
        }
    }

    private void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        
        // Get Emily components
        if (emilyGameObject != null)
        {
            emilyAgent = emilyGameObject.GetComponent<NavMeshAgent>();
            emilyScript = emilyGameObject.GetComponent<EmilyGhost>();
            
            // Disable Emily initially
            emilyGameObject.SetActive(false);
        }
        
        // CRITICAL: Force hide photo panel on start
        if (photoPanel != null)
        {
            photoPanel.SetActive(false);
            if (debugMode) Debug.Log("[Room06] Photo panel hidden on start");
        }
        
        // Load saved state
        LoadState();
        
        // Play intro if first time
        if (!hasPlayedIntro)
        {
            StartCoroutine(PlayIntroSequence());
        }
        else
        {
            if (debugMode) Debug.Log("[Room06] Intro already played, skipping");
        }
        
        // If photo already interacted, show scratched version
        if (hasInteractedWithPhoto && photoFrame != null)
        {
            SpriteRenderer sr = photoFrame.GetComponent<SpriteRenderer>();
            if (sr != null && scratchedPhotoSprite != null)
            {
                sr.sprite = scratchedPhotoSprite;
            }
        }
    }

    private void Update()
    {
        // Update Emily's target during hunt
        if (isEmilyHunting && emilyAgent != null && playerTransform != null)
        {
            if (emilyAgent.isActiveAndEnabled && emilyAgent.isOnNavMesh)
            {
                emilyAgent.SetDestination(playerTransform.position);
            }
        }
        
        // Check for catch
        if (isEmilyHunting && emilyGameObject != null && playerTransform != null)
        {
            float distance = Vector2.Distance(emilyGameObject.transform.position, playerTransform.position);
            
            if (distance <= catchDistance)
            {
                TriggerGameOver();
            }
        }
    }

    private void LoadState()
    {
        if (SaveSystem.Instance != null)
        {
            hasPlayedIntro = SaveSystem.Instance.WasDialogueTriggered(introDialogueFlag);
            hasInteractedWithPhoto = SaveSystem.Instance.WasDialogueTriggered(photoInteractedFlag);
        }
    }

    private void SaveState(string flag)
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.TriggerDialogue(flag);
        }
    }

    /// <summary>
    /// Intro dialogue when entering room
    /// </summary>
    private IEnumerator PlayIntroSequence()
    {
        if (debugMode) Debug.Log("[Room06] Playing intro sequence");
        
        // Disable player controls
        JoystickPlayerController playerController = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (playerController != null) playerController.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        yield return new WaitForSeconds(0.5f);
        
        // Intro dialogue
        DialogueSystemV2.Instance?.StartDialogue("The upstairs hallway... it feels colder here.", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        DialogueSystemV2.Instance?.StartDialogue("There's a photo frame on the wall. I should take a closer look.", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player controls
        if (playerController != null) playerController.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        hasPlayedIntro = true;
        SaveState(introDialogueFlag);
        
        if (debugMode) Debug.Log("[Room06] Intro sequence complete");
    }

    /// <summary>
    /// Called when player interacts with photo frame
    /// </summary>
    public void OnPhotoFrameInteract()
    {
        if (hasInteractedWithPhoto)
        {
            // Already interacted
            DialogueSystemV2.Instance?.StartDialogue("The faces are scratched out... just like the others.", "Lisa");
            return;
        }
        
        if (debugMode) Debug.Log("[Room06] Photo frame interacted");
        
        StartCoroutine(PhotoInteractionSequence());
    }

    private IEnumerator PhotoInteractionSequence()
    {
        hasInteractedWithPhoto = true;
        SaveState(photoInteractedFlag);
        
        // Disable player controls
        JoystickPlayerController playerController = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (playerController != null) playerController.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Initial examination
        DialogueSystemV2.Instance?.StartDialogue("A family photo... they look happy.", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // SHOW PHOTO PANEL with normal photo
        if (photoPanel != null && photoPanelImage != null && normalPhotoSprite != null)
        {
            photoPanelImage.sprite = normalPhotoSprite;
            photoPanel.SetActive(true);
            
            if (debugMode) Debug.Log("[Room06] Photo panel opened - showing normal photo");
        }
        
        yield return new WaitForSeconds(1.5f);
        
        // SCRATCH EFFECT - Transition to scratched photo!
        if (photoPanelImage != null && scratchedPhotoSprite != null)
        {
            // Play scratch sound
            if (scratchSound != null)
            {
                if (roomAudioSource != null)
                {
                    roomAudioSource.PlayOneShot(scratchSound);
                }
                else
                {
                    AudioManager.Instance?.PlaySFX(scratchSound);
                }
            }
            
            // Change panel image to scratched sprite
            photoPanelImage.sprite = scratchedPhotoSprite;
            
            if (debugMode) Debug.Log("[Room06] Photo scratched in panel!");
        }
        
        yield return new WaitForSeconds(1.0f);
        
        // Close photo panel (auto close)
        if (photoPanel != null)
        {
            photoPanel.SetActive(false);
            if (debugMode) Debug.Log("[Room06] Photo panel closed automatically");
        }
        
        // NOW update the world photo frame GameObject to bloody/scratched version
        if (photoFrame != null)
        {
            SpriteRenderer sr = photoFrame.GetComponent<SpriteRenderer>();
            if (sr != null && scratchedPhotoSprite != null)
            {
                sr.sprite = scratchedPhotoSprite;
                if (debugMode) Debug.Log("[Room06] World photo frame changed to bloody version");
            }
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Lisa's reaction
        DialogueSystemV2.Instance?.StartDialogue("What?! The faces... they're scratched out!", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        DialogueSystemV2.Instance?.StartDialogue("No... she's here!", "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player controls
        if (playerController != null) playerController.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        // Wait before spawning Emily
        yield return new WaitForSeconds(spawnDelay);
        
        // SPAWN EMILY!
        SpawnEmily();
    }

    private void SpawnEmily()
    {
        if (emilyGameObject == null || emilySpawnPoint == null)
        {
            Debug.LogError("[Room06] Missing Emily GameObject or spawn point!");
            return;
        }
        
        if (debugMode) Debug.Log("[Room06] Spawning Emily!");
        
        // Position Emily
        emilyGameObject.transform.position = emilySpawnPoint.position;
        emilyGameObject.SetActive(true);
        
        // Play spawn sound
        if (emilySpawnSound != null)
        {
            if (roomAudioSource != null)
            {
                roomAudioSource.PlayOneShot(emilySpawnSound);
            }
            else
            {
                AudioManager.Instance?.PlaySFX(emilySpawnSound);
            }
        }
        
        // Configure Emily AI
        if (emilyScript != null)
        {
            emilyScript.huntSpeed = emilyChaseSpeed;
            emilyScript.SetStateExternal(EmilyGhost.State.Hunt);
        }
        
        // Configure NavMeshAgent
        if (emilyAgent != null)
        {
            emilyAgent.enabled = true;
            emilyAgent.speed = emilyChaseSpeed;
            emilyAgent.Warp(emilySpawnPoint.position);
        }
        
        // Start chase music
        if (chaseMusicLoop != null)
        {
            if (roomAudioSource != null)
            {
                roomAudioSource.clip = chaseMusicLoop;
                roomAudioSource.loop = true;
                roomAudioSource.Play();
            }
            else
            {
                AudioManager.Instance?.PlayLoopingSFX(chaseMusicLoop, "room06_chase");
            }
        }
        
        isEmilyHunting = true;
        
        if (debugMode) Debug.Log($"[Room06] Emily hunting! Speed: {emilyChaseSpeed}");
    }

    private void TriggerGameOver()
    {
        if (!isEmilyHunting) return;
        
        isEmilyHunting = false;
        
        if (debugMode) Debug.Log("[Room06] Emily caught player - Game Over!");
        
        // CRITICAL: Close photo panel before game over
        if (photoPanel != null && photoPanel.activeSelf)
        {
            photoPanel.SetActive(false);
            if (debugMode) Debug.Log("[Room06] Photo panel closed before game over");
        }
        
        // Stop Emily
        if (emilyAgent != null && emilyAgent.isActiveAndEnabled)
        {
            emilyAgent.isStopped = true;
            emilyAgent.velocity = Vector3.zero;
        }
        
        // Stop chase music
        if (roomAudioSource != null && roomAudioSource.isPlaying)
        {
            roomAudioSource.Stop();
        }
        
        // Trigger Game Over
        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver("Emily caught you...");
        }
    }
    
    /// <summary>
    /// Public method to close photo panel (can be called by close button or externally)
    /// </summary>
    public void ClosePhotoPanel()
    {
        if (photoPanel != null)
        {
            photoPanel.SetActive(false);
            if (debugMode) Debug.Log("[Room06] Photo panel closed");
        }
        
        // Re-enable player controls if they were disabled
        JoystickPlayerController playerController = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (playerController != null) playerController.enabled = true;
        if (joystick != null) joystick.SetActive(true);
    }

    // Visualize spawn point in editor
    private void OnDrawGizmos()
    {
        if (emilySpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(emilySpawnPoint.position, 0.5f);
            Gizmos.DrawLine(transform.position, emilySpawnPoint.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (emilySpawnPoint != null)
        {
            // Draw catch distance
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawWireSphere(emilySpawnPoint.position, catchDistance);
        }
    }
}
