using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Rug that allows transition to next room
/// Only accessible after completing ALL tasks including mirror interaction
/// </summary>
public class Room07_RugTransition : MonoBehaviour, IInteractable
{
    [Header("Scene Transition")]
    public string nextSceneName = "Room08_Lisa'sBathroom"; // Next room scene name
    public float transitionDelay = 1f;
    
    [Header("Visual Feedback")]
    public GameObject interactionPrompt; // Optional UI prompt
    public AudioClip rugMoveSound;
    public AudioClip trapdoorOpenSound;
    
    [Header("Animation (Optional)")]
    public Animator rugAnimator; // If you have rug animation
    public string rugMoveAnimationTrigger = "Move";
    
    private bool isTransitioning = false;

    // IInteractable implementation
    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    public void OnFocus(PlayerContext context)
    {
        // Show interaction prompt if available
        if (interactionPrompt != null && CanUseRug())
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void OnBlur(PlayerContext context)
    {
        // Hide interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    // Main interaction method
    public void Interact()
    {
        if (isTransitioning) return;
        
        Room07_FlowController flow = Room07_FlowController.Instance;
        
        if (!CanUseRug())
        {
            // Not ready yet
            DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.RUG_LOCKED, "Lisa");
            return;
        }
        
        // Ready to transition
        StartCoroutine(TransitionSequence());
    }

    // Check if player can use the rug
    private bool CanUseRug()
    {
        Room07_FlowController flow = Room07_FlowController.Instance;
        
        if (flow == null) return false;
        
        // Must complete EVERYTHING including mirror
        return flow.IsEverythingComplete() && flow.hasInteractedWithMirror;
    }

    // Transition sequence with dialogue and effects
    IEnumerator TransitionSequence()
    {
        isTransitioning = true;
        
        // 1. Show initial dialogue (now 2 parts instead of 3)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.RUG_READY_1,
            Room07_ShortDialogues_FINAL.RUG_READY_2
        ));
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // 2. Play rug move sound
        if (rugMoveSound != null)
        {
            AudioManager.Instance?.PlaySFX(rugMoveSound);
        }
        
        // 3. Trigger rug animation if available
        if (rugAnimator != null)
        {
            rugAnimator.SetTrigger(rugMoveAnimationTrigger);
            yield return new WaitForSeconds(1f);
        }
        
        // 4. Play trapdoor open sound
        if (trapdoorOpenSound != null)
        {
            AudioManager.Instance?.PlaySFX(trapdoorOpenSound);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // 5. Show transition dialogue
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.RUG_TRANSITION_1,
            Room07_ShortDialogues_FINAL.RUG_TRANSITION_2,
            Room07_ShortDialogues_FINAL.RUG_TRANSITION_3
        ));
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(transitionDelay);
        
        // 6. Fade out and load next scene
        StartCoroutine(LoadNextScene());
    }

    // Load next scene with fade effect
    IEnumerator LoadNextScene()
    {
        // Optional: Trigger fade out effect
        // FadeManager.Instance?.FadeOut();
        
        yield return new WaitForSeconds(1f);
        
        // Load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("[Room07_RugTransition] Next scene name is not set!");
        }
    }

    // Optional: Visual indicator when rug becomes usable
    void Update()
    {
        // You can add visual effects here when rug becomes available
        // For example, glowing effect, particle system, etc.
        
        if (CanUseRug() && !isTransitioning)
        {
            // Add visual feedback that rug is now usable
            // Example: rugGlowEffect.SetActive(true);
        }
    }
    
    // Helper method to show multiple dialogues in sequence
    // Player is stopped during ALL dialogues - no movement between them
    System.Collections.IEnumerator ShowDialogueSequence(params string[] dialogues)
    {
        // Disable player movement at the START of sequence
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        bool wasPlayerEnabled = player != null && player.enabled;
        bool wasJoystickActive = joystick != null && joystick.activeSelf;
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        foreach (string dialogue in dialogues)
        {
            DialogueSystemV2.Instance?.StartDialogue(dialogue, "Lisa");
            
            while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
            {
                yield return null;
            }
            
            // NO DELAY between dialogues - keep player stopped
        }
        
        // Re-enable player movement at the END of sequence
        if (player != null && wasPlayerEnabled) player.enabled = true;
        if (joystick != null && wasJoystickActive) joystick.SetActive(true);
    }
}
