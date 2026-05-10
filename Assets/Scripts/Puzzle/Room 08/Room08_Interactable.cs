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
        
        // Show bathtub dialogue
        yield return StartCoroutine(ShowDialogueSequence(
            Room08_Dialogues.BATHTUB_1,
            Room08_Dialogues.BATHTUB_2
        ));
        
        flow.hasCheckedBathtub = true;
    }

    System.Collections.IEnumerator ExamineMedicineCabinet()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
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
            
            // Check if all evidence collected - Emily enters!
            if (flow.IsAllEvidenceFound())
            {
                flow.OnAllEvidenceCollected();
            }
        }
    }

    void ExamineEvidence()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        // Show evidence dialogue based on type
        switch (evidenceId)
        {
            case "torn_clothes":
                StartCoroutine(ExamineTornClothes());
                flow.hasFoundTornClothes = true;
                break;
                
            case "apology_note":
                StartCoroutine(ExamineApologyNote());
                flow.hasFoundApologyNote = true;
                break;
        }
        
        // Hide this evidence object after examining
        gameObject.SetActive(false);
        
        // Check if all evidence collected - Emily enters!
        if (flow.IsAllEvidenceFound())
        {
            flow.OnAllEvidenceCollected();
        }
    }
    
    System.Collections.IEnumerator ExamineTornClothes()
    {
        yield return StartCoroutine(ShowDialogueSequence(
            Room08_Dialogues.TORN_CLOTHES_1,
            Room08_Dialogues.TORN_CLOTHES_2
        ));
    }
    
    System.Collections.IEnumerator ExamineApologyNote()
    {
        yield return StartCoroutine(ShowDialogueSequence(
            Room08_Dialogues.APOLOGY_NOTE_1,
            Room08_Dialogues.APOLOGY_NOTE_2
        ));
    }

    void ExamineMirror()
    {
        Room08_FlowController flow = Room08_FlowController.Instance;
        
        // Check if all evidence is collected
        if (!flow.IsAllEvidenceFound())
        {
            DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.NEED_ALL_EVIDENCE, "Lisa");
            return;
        }
        
        // Check if Emily is hunting
        if (!flow.isEmilyHunting)
        {
            DialogueSystemV2.Instance?.StartDialogue("I should look around more...", "Lisa");
            return;
        }
        
        // Ready to break mirror - start QTE immediately
        if (!flow.hasBrokenMirror)
        {
            StartCoroutine(StartMirrorQTE());
        }
        // Already broken
        else
        {
            DialogueSystemV2.Instance?.StartDialogue("The mirror is shattered. I can see the passage behind it.", "Lisa");
        }
    }

    System.Collections.IEnumerator StartMirrorQTE()
    {
        // Show QTE start dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room08_Dialogues.QTE_START, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Find and start QTE
        Room08_MirrorQTE qte = FindFirstObjectByType<Room08_MirrorQTE>();
        if (qte != null)
        {
            qte.StartQTE();
        }
        else
        {
            Debug.LogError("[Room08] Room08_MirrorQTE not found!");
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
