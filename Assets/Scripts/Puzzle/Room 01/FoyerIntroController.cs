using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FoyerIntroController : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The unique ID used in the SaveSystem to remember if this cutscene has been played.")]
    public string cutsceneSaveID = "IntroCutscene_Played";

    [Header("References")]
    [Tooltip("The GameObject that contains the Cutscene logic/director. This should be DISABLED in the Inspector by default.")]
    public GameObject cutsceneObject;

    [Tooltip("The CanvasGroup for the black screen overlay. This object must be ENABLED in the Inspector to prevent the 'split-second' flicker.")]
    public CanvasGroup blackoutCanvasGroup;

    // Internal flag to allow manual finishing via UnityEvents
    private bool manualFinishTriggered = false;

    void Awake()
    {
        // CRITICAL FIX FOR "SPLIT SECOND" FLICKER:
        // Ensure the black screen is strictly fully opaque and active the moment the scene loads.
        if (blackoutCanvasGroup != null)
        {
            blackoutCanvasGroup.alpha = 1f;
            blackoutCanvasGroup.gameObject.SetActive(true);
        }

        // Ensure cutscene object starts disabled so it doesn't auto-play before we check the save file
        if (cutsceneObject != null)
        {
            cutsceneObject.SetActive(false);
        }

        // Subscribe to scene loaded event to reset trigger on retry
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the cutscene trigger when the scene loads (for retry logic)
        manualFinishTriggered = false;
    }

    IEnumerator Start()
    {
        // Wait until SaveSystem is ready
        while (SaveSystem.Instance == null)
        {
            yield return null;
        }

        // We run the check inside the Coroutine now so we can wait for the cutscene to finish
        yield return StartCoroutine(CheckAndPlayCutsceneRoutine());
    }

    IEnumerator CheckAndPlayCutsceneRoutine()
    {
        // Check SaveSystem to see if we have already triggered this cutscene
        bool hasSeenCutscene = SaveSystem.Instance.WasDialogueTriggered(cutsceneSaveID);

        if (hasSeenCutscene)
        {
            // CASE 1: LOAD GAME (ALREADY SEEN)
            // Skip the cutscene completely and just reveal the room
            Debug.Log($"[FoyerIntro] Cutscene '{cutsceneSaveID}' already seen. Skipping and fading in.");

            if (cutsceneObject != null)
                cutsceneObject.SetActive(false);

            // Show all persistent objects (Lisa, UI, etc.)
            if (PersistentSceneHider.Instance != null)
            {
                PersistentSceneHider.Instance.ShowAllObjects();
            }

            // Start the fade in to reveal the Foyer
            yield return StartCoroutine(FadeInRoom());
        }
        else
        {
            // CASE 2: NEW GAME (NOT SEEN)
            // All persistent objects are already hidden by PersistentSceneHider
            Debug.Log($"[FoyerIntro] First time seeing '{cutsceneSaveID}'. Playing cutscene.");

            // Mark it as seen in the SaveSystem immediately so next load won't replay it
            SaveSystem.Instance.TriggerDialogue(cutsceneSaveID);

            if (cutsceneObject != null)
            {
                manualFinishTriggered = false;
                cutsceneObject.SetActive(true);

                // Wait for cutscene to finish
                // The cutscene should call FinishIntro() when done
                yield return new WaitForSeconds(2f);

                DisableBlackout();
                
                // Show all persistent objects after cutscene
                if (PersistentSceneHider.Instance != null)
                {
                    PersistentSceneHider.Instance.ShowAllObjects();
                    Debug.Log("[FoyerIntro] All persistent objects shown after cutscene");
                }
            }
            else
            {
                // Fallback if reference is missing
                if (PersistentSceneHider.Instance != null)
                {
                    PersistentSceneHider.Instance.ShowAllObjects();
                }
                yield return StartCoroutine(FadeInRoom());
            }
        }
    }

    // This runs only if we SKIP the cutscene (loading a save)
    IEnumerator FadeInRoom()
    {
        // Wait a small moment to ensure scene is stable
        yield return new WaitForSeconds(0.5f);

        float duration = 1.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (blackoutCanvasGroup != null)
            {
                // Fade from 1 (Black) to 0 (Transparent)
                blackoutCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            }
            yield return null;
        }

        DisableBlackout();
    }

    void DisableBlackout()
    {
        Debug.Log("[FoyerIntro] Disabling blackout panel.");
        if (blackoutCanvasGroup != null)
        {
            blackoutCanvasGroup.alpha = 0f;
            blackoutCanvasGroup.gameObject.SetActive(false);
        }
    }

    // --- PUBLIC METHODS ---

    /// <summary>
    /// Call this method from your Cutscene's "OnFinished" event if the Cutscene GameObject
    /// does not deactivate itself automatically.
    /// </summary>
    public void FinishIntro()
    {
        manualFinishTriggered = true;
        Debug.Log("[FoyerIntro] Manual finish triggered.");
        
        // Show all persistent objects when cutscene finishes
        if (PersistentSceneHider.Instance != null)
        {
            PersistentSceneHider.Instance.ShowAllObjects();
            Debug.Log("[FoyerIntro] All persistent objects shown via PersistentSceneHider");
        }
    }
}