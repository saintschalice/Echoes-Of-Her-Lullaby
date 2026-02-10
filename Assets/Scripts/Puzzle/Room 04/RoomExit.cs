using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExit : MonoBehaviour
{
    public string nextSceneName = "Room05_DiningRoom"; //
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("[RoomExit] Moving Lisa to Dining Room...");
            hasTriggered = true;

            // Lilipat na ng Scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}