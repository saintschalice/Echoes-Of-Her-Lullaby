using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Simple room exit trigger with fade transition
/// Automatically fades out when player enters, loads scene, then fades in
/// </summary>
public class RoomExit : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "Room05_DiningRoom";
    
    [Header("Spawn Settings")]
    [Tooltip("Leave empty to use default spawn point in next scene")]
    public string targetSpawnPointID = "";
    
    [Header("Transition Settings")]
    public float fadeOutDuration = 0.8f;
    public float fadeInDuration = 0.8f;
    public bool disablePlayerDuringTransition = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[RoomExit] Something entered the trigger: " + other.name);

        if (hasTriggered) return;

        // Check if Player entered
        if (other.CompareTag("Player"))
        {
            Debug.Log("[RoomExit] DETECTED PLAYER! Starting transition to: " + nextSceneName);
            hasTriggered = true;
            
            // Start fade transition
            StartCoroutine(TransitionToScene(other.gameObject));
        }
        else
        {
            Debug.LogWarning("[RoomExit] Object " + other.name + " is NOT tagged as 'Player'!");
        }
    }
    
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
        
        // 2. Fade out
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeOut(fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }
        else
        {
            Debug.LogWarning("[RoomExit] ScreenFader not found! Transitioning without fade.");
            yield return new WaitForSeconds(0.5f);
        }
        
        // 3. Save player position before transition
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.GetCurrentSaveData().playerPosition = player.transform.position;
        }
        
        // 4. Set target spawn point for next scene (if specified)
        if (!string.IsNullOrEmpty(targetSpawnPointID))
        {
            PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnPointID);
            PlayerPrefs.Save();
            Debug.Log($"[RoomExit] Set target spawn point: {targetSpawnPointID}");
        }
        else
        {
            // Clear any previous spawn point to use default
            PlayerPrefs.SetString("TargetSpawnPoint", "");
            PlayerPrefs.Save();
            Debug.Log("[RoomExit] Using default spawn point in next scene");
        }
        
        // 5. Notify save system about room change
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnRoomEntered(nextSceneName);
        }
        
        // 6. Load scene
        Debug.Log("[RoomExit] Loading scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
        
        // Note: Fade in will happen automatically in ScreenFader's Start() method
    }
}