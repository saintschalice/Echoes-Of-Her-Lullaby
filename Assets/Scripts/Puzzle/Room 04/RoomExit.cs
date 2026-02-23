using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExit : MonoBehaviour
{
    public string nextSceneName = "Room05_DiningRoom"; //
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check kung may bumangga sa trigger
        Debug.Log("[RoomExit] Something entered the trigger: " + other.name);

        if (hasTriggered) return;

        // 2. Check kung Player ba ang bumangga
        if (other.CompareTag("Player"))
        {
            Debug.Log("[RoomExit] DETECTED PLAYER! Loading: " + nextSceneName);
            hasTriggered = true;
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("[RoomExit] Object " + other.name + " is NOT tagged as 'Player'!");
        }
    }
}