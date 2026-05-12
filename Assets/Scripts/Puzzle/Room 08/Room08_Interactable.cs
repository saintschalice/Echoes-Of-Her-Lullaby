using UnityEngine;
using System.Collections;

/// <summary>
/// Handles interactions with objects in Room 08 - Lisa's Bathroom
/// </summary>
public class Room08_Interactable : MonoBehaviour, IInteractable
{
    public enum ObjectType
    {
        Bathtub, MedicineCabinet, Mirror, Door, Passage, Evidence
    }

    public ObjectType myType;
    
    [Header("Evidence Type (if ObjectType is Evidence)")]
    public string evidenceId; // "torn_clothes", "apology_note"

    // IInteractable implementation
    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    public void OnFocus(PlayerContext context)
    {
        Debug.Log($"[Room08] Focused on {myType}");
    }

    public void OnBlur(PlayerContext context)
    {
        Debug.Log($"[Room08] Blurred from {myType}");
    }

    // Main interaction method
    public void Interact()
    {
        DoInteract();
    }

    private void DoInteract()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        Debug.Log($"[Room08] Interacting with: {myType}");

        switch (myType)
        {
            case ObjectType.Bathtub:
                StartCoroutine(ExamineBathtub());
                break;

            case ObjectType.MedicineCabinet:
                StartCoroutine(ExamineMedicineCabinet());
                break;

            case ObjectType.Evidence:
                ExamineEvidence();
                break;

            case ObjectType.Mirror:
                ExamineMirror();
                break;

            case ObjectType.Door:
                DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.DOOR_LOCKED, "Lisa");
                break;

            case ObjectType.Passage:
                if (flow.canClimbThrough)
                {
                    flow.ClimbThroughPassage();
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue("The passage is blocked by the mirror.", "Lisa");
                }
                break;
        }
    }

    System.Collections.IEnumerator ExamineBathtub()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        // Check if hammer is obtained first
        if (!flow.hasFoundHammer)
        {
            DialogueSystemV2.Instance?.StartDialogue("I should look around more before examining the bathtub.", "Lisa");
            yield break;
        }
        
        // Show bathtub dialogue
        yield return StartCoroutine(ShowDialogueSequence(
            Room08_Dialogues.BATHTUB_1,
            Room08_Dialogues.BATHTUB_2
        ));
        
        flow.hasInteractedWithBathtub = true;
        flow.hasCheckedBathtub = true;
    }

    System.Collections.IEnumerator ExamineMedicineCabinet()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        // Check if evidence is collected first
        if (!flow.hasCollectedAllEvidence)
        {
            DialogueSystemV2.Instance?.StartDialogue("I should collect the evidence first.", "Lisa");
            yield break;
        }
        
        // Show medicine cabinet dialogue
        yield return StartCoroutine(ShowDialogueSequence(
            Room08_Dialogues.MEDICINE_1,
            Room08_Dialogues.MEDICINE_2
        ));
        
        flow.hasCheckedMedicine = true;
        
        // Auto-obtain hammer if not already obtained
        if (!flow.hasFoundHammer)
        {
            // Add hammer to inventory with notification
            InventoryManager.Instance?.AddItemWithNotification("hammer");
            
            // Wait for notification to finish
            yield return new WaitForSeconds(2f);
            
            // Show hammer dialogue
            yield return StartCoroutine(ShowDialogueSequence(
                Room08_Dialogues.HAMMER_FOUND_1,
                Room08_Dialogues.HAMMER_FOUND_2
            ));
            
            flow.hasFoundHammer = true;
        }
    }

    void ExamineEvidence()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        // Show evidence dialogue
        StartCoroutine(ShowDialogueSequence(
            "Evidence found. This might be important.",
            "I should collect everything I can find."
        ));
        
        // Hide this evidence object after examining
        gameObject.SetActive(false);
        
        // Mark evidence as collected (you can track individual items if needed)
        // For now, we'll use a simple flag
        flow.hasCollectedAllEvidence = true;
    }

    void ExamineMirror()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        Room08UIManager uiManager = FindFirstObjectByType<Room08UIManager>();
        
        // Check if ready for mirror interaction
        if (!flow.IsReadyForMirror())
        {
            DialogueSystemV2.Instance?.StartDialogue("I need to finish examining everything first.", "Lisa");
            return;
        }
        
        // Already broken - this is now the passage!
        if (flow.hasBrokenMirror)
        {
            // Transition to next room
            flow.ClimbThroughPassage();
            return;
        }
        
        // Ready to break mirror - show panel
        if (uiManager != null)
        {
            // Disable this interactable during puzzle to prevent double-interaction
            enabled = false;
            uiManager.ShowMirrorPanel();
        }
        else
        {
            Debug.LogError("[Room08] Room08UIManager not found!");
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
