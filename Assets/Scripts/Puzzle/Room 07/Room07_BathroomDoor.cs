using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Bathroom door in Lisa's Bedroom (Room 07)
/// Locked until all puzzles are complete
/// Based on RoomExit script but with puzzle completion check
/// </summary>
public class Room07_BathroomDoor : MonoBehaviour
{
    [Header("Scene Settings")]
    public string bathroomSceneName = "Room08_Lisa'sBathroom";
    
    [Header("Spawn Settings")]
    [Tooltip("Leave empty to use default spawn point in bathroom")]
    public string targetSpawnPointID = "";
    
    [Header("Transition Settings")]
    public float fadeOutDuration = 0.8f;
    public float fadeInDuration = 0.8f;
    public bool disablePlayerDuringTransition = true;
    
    [Header("Lock Settings")]
    [Tooltip("Dialogue when door is locked")]
    public string lockedDialogue = "The door is locked. I need to finish what I came here for first.";
    
    [Tooltip("Dialogue when door unlocks")]
    public string unlockedDialogue = "The door... it's open now. The bathroom. Where it all ended.";
    
    [Header("Audio")]
    [Tooltip("Sound when trying locked door")]
    public AudioClip lockedSound;
    
    [Tooltip("Sound when door unlocks")]
    public AudioClip unlockSound;
    
    [Tooltip("Sound when door opens")]
    public AudioClip doorOpenSound;
    
    [Header("Debug")]
    public bool debugMode = true;
    
    private bool hasTriggered = false;
    private bool hasShownUnlockDialogue = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        // Check if Player entered
        if (other.CompareTag("Player"))
        {
            if (debugMode) Debug.Log("[BathroomDoor] Player approached door");
            
            // Check if all puzzles are complete
            if (IsDoorUnlocked())
            {
                if (debugMode) Debug.Log("[BathroomDoor] Door is unlocked! Starting transition");
                
                // Show unlock dialogue first time
                if (!hasShownUnlockDialogue)
                {
                    hasShownUnlockDialogue = true;
                    StartCoroutine(ShowUnlockDialogueAndTransition(other.gameObject));
                }
                else
                {
                    // Already shown dialogue, just transition
                    hasTriggered = true;
                    StartCoroutine(TransitionToScene(other.gameObject));
                }
            }
            else
            {
                // Door is locked
                if (debugMode) Debug.Log("[BathroomDoor] Door is locked!");
                ShowLockedDialogue();
                
                // Play locked sound
                if (lockedSound != null)
                {
                    AudioManager.Instance?.PlaySFX(lockedSound);
                }
            }
        }
    }
    
    /// <summary>
    /// Check if all puzzles in Lisa's Bedroom are complete
    /// </summary>
    private bool IsDoorUnlocked()
    {
        Room07_FlowController flow = Room07_FlowController.Instance;
        
        if (flow == null)
        {
            Debug.LogError("[BathroomDoor] Room07_FlowController not found!");
            return false;
        }
        
        // Check if everything is complete
        bool isComplete = flow.IsEverythingComplete();
        
        if (debugMode)
        {
            Debug.Log($"[BathroomDoor] Puzzle completion check:");
            Debug.Log($"  - Bed: {flow.hasCheckedBed}");
            Debug.Log($"  - Wall: {flow.hasCheckedWall}");
            Debug.Log($"  - Diary: {flow.hasCheckedDiary}");
            Debug.Log($"  - Curtains: {flow.areCurtainsOpened}");
            Debug.Log($"  - Cup: {flow.hasEmilyCup}");
            Debug.Log($"  - Tea Party: {flow.isTeaPartyDone}");
            Debug.Log($"  - Chair: {flow.hasCheckedChair}");
            Debug.Log($"  - Closet: {flow.hasCheckedCloset}");
            Debug.Log($"  - Toybox: {flow.isToyboxSolved}");
            Debug.Log($"  - Doll: {flow.hasEmilyDoll}");
            Debug.Log($"  - Dollhouse: {flow.isDollhouseDone}");
            Debug.Log($"  - Reading Table: {flow.hasCheckedReadingTable}");
            Debug.Log($"  - ALL COMPLETE: {isComplete}");
        }
        
        return isComplete;
    }
    
    /// <summary>
    /// Show locked dialogue
    /// </summary>
    private void ShowLockedDialogue()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(lockedDialogue, "Lisa");
        }
    }
    
    /// <summary>
    /// Show unlock dialogue then transition
    /// </summary>
    private IEnumerator ShowUnlockDialogueAndTransition(GameObject player)
    {
        // Play unlock sound
        if (unlockSound != null)
        {
            AudioManager.Instance?.PlaySFX(unlockSound);
        }
        
        // Show unlock dialogue
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue(unlockedDialogue, "Lisa");
            
            // Wait for dialogue to finish
            while (DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Now transition
        hasTriggered = true;
        yield return StartCoroutine(TransitionToScene(player));
    }
    
    /// <summary>
    /// Transition to bathroom scene
    /// </summary>
    private IEnumerator TransitionToScene(GameObject player)
    {
        // 1. Disable player movement
        if (disablePlayerDuringTransition)
        {
            JoystickPlayerController playerController = player.GetComponent<JoystickPlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            
            // Stop player velocity
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        // Play door open sound
        if (doorOpenSound != null)
        {
            AudioManager.Instance?.PlaySFX(doorOpenSound);
        }
        
        // 2. Fade out
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }
        else
        {
            Debug.LogWarning("[BathroomDoor] ScreenFader not found! Transitioning without fade.");
            yield return new WaitForSeconds(0.5f);
        }
        
        // 3. Save player position before transition
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.GetCurrentSaveData().playerPosition = player.transform.position;
        }
        
        // 4. Set target spawn point for bathroom (if specified)
        if (!string.IsNullOrEmpty(targetSpawnPointID))
        {
            PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnPointID);
            PlayerPrefs.Save();
            if (debugMode) Debug.Log($"[BathroomDoor] Set target spawn point: {targetSpawnPointID}");
        }
        else
        {
            // Clear any previous spawn point to use default
            PlayerPrefs.SetString("TargetSpawnPoint", "");
            PlayerPrefs.Save();
            if (debugMode) Debug.Log("[BathroomDoor] Using default spawn point in bathroom");
        }
        
        // 5. Notify save system about room change
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnRoomEntered(bathroomSceneName);
        }
        
        // 6. Load bathroom scene
        if (debugMode) Debug.Log($"[BathroomDoor] Loading scene: {bathroomSceneName}");
        SceneManager.LoadScene(bathroomSceneName);
        
        // Note: Fade in will happen automatically in ScreenFader's Start() method
    }
    
    // Visualize trigger area in editor
    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            // Check if door is unlocked
            bool isUnlocked = false;
            if (Application.isPlaying && Room07_FlowController.Instance != null)
            {
                isUnlocked = IsDoorUnlocked();
            }
            
            // Green if unlocked, red if locked
            Gizmos.color = isUnlocked ? Color.green : Color.red;
            
            if (col is BoxCollider2D box)
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)box.offset, box.size);
            }
            else if (col is CircleCollider2D circle)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)circle.offset, circle.radius);
            }
        }
    }
}
