using UnityEngine;

/// <summary>
/// Handles all interactable objects in Room 10 - Master Bedroom
/// Bed, Diary, Music Box, Mirror
/// </summary>
public class Room10_Interactable : MonoBehaviour
{
    [Header("Interactable Type")]
    public InteractableType type;
    
    [Header("State")]
    public bool hasBeenExamined = false;
    
    [Header("Music Box")]
    public AudioClip lullabyClip;
    
    public enum InteractableType
    {
        Bed,
        Diary,
        MusicBox,
        Mirror
    }
    
    private void OnMouseDown()
    {
        HandleInteraction();
    }
    
    public void HandleInteraction()
    {
        Room10_FlowController controller = Room10_FlowController.Instance;
        if (controller == null) return;
        
        // Check if intro is done
        if (!controller.isIntroDone)
        {
            return; // Can't interact during intro
        }
        
        switch (type)
        {
            case InteractableType.Bed:
                ExamineBed();
                break;
                
            case InteractableType.Diary:
                ExamineDiary();
                break;
                
            case InteractableType.MusicBox:
                ExamineMusicBox();
                break;
                
            case InteractableType.Mirror:
                ApproachMirror();
                break;
        }
    }
    
    void ExamineBed()
    {
        if (hasBeenExamined) return;
        
        hasBeenExamined = true;
        
        // Show bed dialogue
        StartCoroutine(BedExaminationSequence());
    }
    
    System.Collections.IEnumerator BedExaminationSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EXAMINE_ROOM_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EXAMINE_ROOM_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EXAMINE_BED, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        // Mark as examined
        Room10_FlowController.Instance?.OnRoomExamined();
    }
    
    void ExamineDiary()
    {
        if (hasBeenExamined) return;
        
        hasBeenExamined = true;
        
        // Show diary dialogue
        StartCoroutine(DiaryExaminationSequence());
    }
    
    System.Collections.IEnumerator DiaryExaminationSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EXAMINE_DIARY_1, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.EXAMINE_DIARY_2, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        // Mark as examined
        Room10_FlowController.Instance?.OnRoomExamined();
    }
    
    void ExamineMusicBox()
    {
        if (hasBeenExamined) return;
        
        hasBeenExamined = true;
        
        // Show music box dialogue and play lullaby
        StartCoroutine(MusicBoxSequence());
    }
    
    System.Collections.IEnumerator MusicBoxSequence()
    {
        // Disable player
        JoystickPlayerController player = JoystickPlayerController.Instance;
        GameObject joystick = GameObject.Find("Joystick");
        
        if (player != null) player.enabled = false;
        if (joystick != null) joystick.SetActive(false);
        
        // Show dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.LULLABY_FOUND, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Play lullaby
        if (lullabyClip != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio == null) audio = gameObject.AddComponent<AudioSource>();
            audio.clip = lullabyClip;
            audio.Play();
        }
        
        yield return new WaitForSeconds(2f);
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.LULLABY_PLAYS, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.LULLABY_MEMORY, "Lisa");
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Re-enable player
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);
        
        // Add lullaby fragment to inventory
        InventoryManager.Instance?.AddItemWithNotification("Lullaby Fragment #4");
        
        // Mark as found
        Room10_FlowController.Instance?.OnLullabyFound();
    }
    
    void ApproachMirror()
    {
        Room10_FlowController controller = Room10_FlowController.Instance;
        if (controller == null) return;
        
        // Check if can access mirror
        if (!controller.canAccessMirror)
        {
            // Show hint
            DialogueSystemV2.Instance?.StartDialogue(Room10_Dialogues.HINT_EXAMINE_ROOM, "Lisa");
            return;
        }
        
        // Trigger mirror sequence
        controller.ApproachMirror();
    }
}
