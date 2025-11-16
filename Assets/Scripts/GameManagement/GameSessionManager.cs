using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game session and handles cleanup when returning to main menu.
/// Add this to your PersistentScene or a manager object.
/// </summary>
public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    [Header("Objects to Keep Between Sessions")]
    [Tooltip("Objects with these names won't be destroyed when returning to main menu")]
    public string[] persistentObjectNames = { "ScreenFader" };

    [Header("Auto Cleanup")]
    [Tooltip("Automatically clean up on Main Menu load")]
    public bool autoCleanupOnMainMenu = true;
    public string mainMenuSceneName = "MainMenu";

    private bool isCleaningUp = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to scene loading
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If main menu loaded and auto cleanup is enabled, clean up
        if (autoCleanupOnMainMenu && scene.name == mainMenuSceneName && !isCleaningUp)
        {
            Debug.Log("[GameSession] Main menu detected, cleaning up game session");
            CleanupGameSession();
        }
    }

    /// <summary>
    /// Call this before returning to main menu to clean up all persistent objects
    /// </summary>
    public void CleanupGameSession()
    {
        if (isCleaningUp) return;
        
        isCleaningUp = true;
        Debug.Log("[GameSession] Starting game session cleanup");

        // Get all objects in DontDestroyOnLoad
        GameObject tempObj = new GameObject("TempSceneFinder");
        DontDestroyOnLoad(tempObj);
        Scene dontDestroyScene = tempObj.scene;
        Destroy(tempObj);

        GameObject[] persistentObjects = dontDestroyScene.GetRootGameObjects();

        int destroyedCount = 0;
        int keptCount = 0;

        foreach (GameObject obj in persistentObjects)
        {
            // Check if this object should be kept
            bool shouldKeep = ShouldKeepObject(obj);

            if (shouldKeep)
            {
                Debug.Log($"[GameSession] Keeping: {obj.name}");
                keptCount++;
            }
            else
            {
                Debug.Log($"[GameSession] Destroying: {obj.name}");
                
                // Clear singleton instances if they exist
                ClearSingleton(obj);
                
                Destroy(obj);
                destroyedCount++;
            }
        }

        Debug.Log($"[GameSession] Cleanup complete - Destroyed: {destroyedCount}, Kept: {keptCount}");
        
        // Destroy this manager last
        if (Instance == this)
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    bool ShouldKeepObject(GameObject obj)
    {
        // Check against the keep list
        foreach (string keepName in persistentObjectNames)
        {
            if (obj.name.Contains(keepName))
            {
                return true;
            }
        }

        // Don't destroy this manager yet (it destroys itself last)
        if (obj == gameObject)
        {
            return true;
        }

        return false;
    }

    void ClearSingleton(GameObject obj)
    {
        // Try to clear common singleton patterns
        MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
        
        foreach (MonoBehaviour component in components)
        {
            if (component == null) continue;

            // Use reflection to find and clear Instance properties
            System.Type type = component.GetType();
            var instanceProperty = type.GetProperty("Instance", 
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.Static);

            if (instanceProperty != null && instanceProperty.CanWrite)
            {
                try
                {
                    instanceProperty.SetValue(null, null);
                    Debug.Log($"[GameSession] Cleared singleton: {type.Name}");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[GameSession] Could not clear singleton {type.Name}: {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Call this manually if you want to clean up at a specific time
    /// </summary>
    public static void CleanupNow()
    {
        if (Instance != null)
        {
            Instance.CleanupGameSession();
        }
        else
        {
            Debug.LogWarning("[GameSession] No GameSessionManager instance found!");
        }
    }
}
