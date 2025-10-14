using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Spawn")]
    public string currentSpawnPointName = "DefaultSpawn";

    void Awake()
    {
        // Simple singleton - no DontDestroyOnLoad needed
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameManagers detected! Check your persistent scene setup.");
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, string spawnPointName)
    {
        currentSpawnPointName = spawnPointName;
        SceneManager.LoadScene(sceneName);
    }

    public string GetSpawnPointName()
    {
        return currentSpawnPointName;
    }

    // Optional: Reset spawn point when starting new game
    public void ResetSpawnPoint()
    {
        currentSpawnPointName = "DefaultSpawn";
    }
}