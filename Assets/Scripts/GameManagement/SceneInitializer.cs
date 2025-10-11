using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneInitializer : MonoBehaviour
{
    [Header("Persistent Scene")]
    public string persistentSceneName = "PersistentScene";

    [Header("Starting Scene")]
    public string startingSceneName = "Room01_Foyer";

    void Awake()
    {
        // Check if PersistentScene is already loaded
        bool persistentSceneLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == persistentSceneName)
            {
                persistentSceneLoaded = true;
                break;
            }
        }

        // Load persistent scene if not already loaded
        if (!persistentSceneLoaded)
        {
            SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
        }
    }

    void Start()
    {
        // Load starting scene if we're only in PersistentScene
        if (SceneManager.sceneCount == 1)
        {
            StartCoroutine(LoadStartingScene());
        }
    }

    IEnumerator LoadStartingScene()
    {
        yield return new WaitForSeconds(0.5f);

        // Load the starting scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(startingSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Set the starting scene as active
        Scene startingScene = SceneManager.GetSceneByName(startingSceneName);
        SceneManager.SetActiveScene(startingScene);
    }
}