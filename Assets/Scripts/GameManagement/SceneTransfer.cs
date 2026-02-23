using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Exact name of the scene file (e.g., Room04_Kitchen)")]
    public string sceneToLoad;

    [Tooltip("ID of the spawn point to land on (e.g., FromDining)")]
    public string targetSpawnID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. ISULAT SA MEMORY: "Sa susunod na scene, hanapin mo ang ID na 'FromDining'"
            PlayerPrefs.SetString("TargetSpawnPoint", targetSpawnID);
            PlayerPrefs.Save();

            // 2. LOAD SCENE
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}