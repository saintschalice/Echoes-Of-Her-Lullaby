using UnityEngine;

public class Room07_Interactable : MonoBehaviour, IInteractable
{
    public enum ObjectType
    {
        Bed, WallDrawings, Bookshelf, Diary, WindowCurtains, Cabinet_Cup,
        TeaParty, Chair, Closet, Toybox, Dollhouse, ReadingTable, Mirror
    }

    public ObjectType myType;

    [Header("UI & Inventory Reference")]
    public Room07UIManager uiManager;
    public string requiredItemID; // e.g., "emily_cup" or "emily_doll"

    [Header("Interaction Prompt (Optional)")]
    public string interactionPrompt = "Press E to interact";

    // IInteractable implementation
    public void OnInteract(PlayerContext context)
    {
        Interact();
    }

    public void OnFocus(PlayerContext context)
    {
        // Optional: Show interaction prompt
        Debug.Log($"[Room07] Focused on {myType}");
    }

    public void OnBlur(PlayerContext context)
    {
        // Optional: Hide interaction prompt
        Debug.Log($"[Room07] Blurred from {myType}");
    }

    // Main interaction method - called by mobile button
    public void Interact()
    {
        DoInteract();
    }

    // Core interaction logic
    private void DoInteract()
    {
        Room07_FlowController flow = Room07_FlowController.Instance;
        
        Debug.Log($"[Room07] Interacting with: {myType}");

        switch (myType)
        {
            case ObjectType.Bed:
                // Prerequisite: Intro must be done
                if (!flow.isIntroDone)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.BED_PREREQUISITE, "Lisa");
                    return;
                }
                StartCoroutine(ShowDialogueSequence(
                    Room07_ShortDialogues_FINAL.BED_1,
                    Room07_ShortDialogues_FINAL.BED_2
                ));
                flow.hasCheckedBed = true;
                break;

            case ObjectType.WallDrawings:
                // Prerequisite: Bed must be checked
                if (!flow.hasCheckedBed)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.WALL_PREREQUISITE, "Lisa");
                    return;
                }
                // Show wall drawings image/preview
                StartCoroutine(ShowDialogueSequence(
                    Room07_ShortDialogues_FINAL.WALL_1,
                    Room07_ShortDialogues_FINAL.WALL_2,
                    Room07_ShortDialogues_FINAL.WALL_3
                ));
                flow.hasCheckedWall = true;
                break;

            case ObjectType.Bookshelf:
                // Prerequisite: Wall must be checked
                if (!flow.hasCheckedWall)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.DIARY_PREREQUISITE, "Lisa");
                    return;
                }
                // Interact with bookshelf to discover diary
                if (!flow.hasCheckedDiary)
                {
                    StartCoroutine(DiscoverDiarySequence());
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.DIARY_ALREADY_READ, "Lisa");
                }
                break;

            case ObjectType.Diary:
                // This is for if diary is a separate pickup
                if (!flow.hasCheckedDiary)
                {
                    DialogueSystemV2.Instance?.StartDialogue("Child's diary: 'Emily came to me again last night. She sang the pretty song and made the scary dreams go away.'", "Lisa");
                    flow.hasCheckedDiary = true;
                }
                break;

            case ObjectType.WindowCurtains:
                // Prerequisite: Diary must be checked
                if (!flow.hasCheckedDiary)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CURTAINS_PREREQUISITE, "Lisa");
                    return;
                }
                if (!flow.areCurtainsOpened)
                {
                    StartCoroutine(OpenCurtainSequence());
                }
                else
                {
                    StartCoroutine(ShowDialogueSequence(
                        Room07_ShortDialogues_FINAL.CURTAINS_OPENED_1,
                        Room07_ShortDialogues_FINAL.CURTAINS_OPENED_2
                    ));
                }
                break;

            case ObjectType.Cabinet_Cup:
                // Prerequisite: Curtains must be opened
                if (!flow.areCurtainsOpened)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CABINET_LOCKED, "Lisa");
                    return;
                }
                if (!flow.hasEmilyCup)
                {
                    // Open cabinet panel to show and take the cup
                    if (uiManager != null)
                    {
                        uiManager.ShowCabinetPanel();
                    }
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CABINET_EMPTY, "Lisa");
                }
                break;

            case ObjectType.TeaParty:
                // Prerequisite: Must have Emily's cup
                if (!flow.hasEmilyCup)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_INCOMPLETE, "Lisa");
                    return;
                }
                if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("emily_cup"))
                {
                    StartCoroutine(OpenTeaPartySequence());
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_DONE, "Lisa");
                }
                break;

            case ObjectType.Chair:
                // Prerequisite: Tea party must be done
                if (!flow.isTeaPartyDone)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CHAIR_PREREQUISITE, "Lisa");
                    return;
                }
                StartCoroutine(ShowDialogueSequence(
                    Room07_ShortDialogues_FINAL.CHAIR_1,
                    Room07_ShortDialogues_FINAL.CHAIR_2,
                    Room07_ShortDialogues_FINAL.CHAIR_3
                ));
                flow.hasCheckedChair = true;
                break;

            case ObjectType.Closet:
                // Prerequisite: Chair must be checked
                if (!flow.hasCheckedChair)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CLOSET_PREREQUISITE, "Lisa");
                    return;
                }
                StartCoroutine(ShowDialogueSequence(
                    Room07_ShortDialogues_FINAL.CLOSET_1,
                    Room07_ShortDialogues_FINAL.CLOSET_2,
                    Room07_ShortDialogues_FINAL.CLOSET_3,
                    Room07_ShortDialogues_FINAL.CLOSET_4
                ));
                flow.hasCheckedCloset = true;
                break;

            case ObjectType.Toybox:
                // Prerequisite: Closet must be checked
                if (!flow.hasCheckedCloset)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_PREREQUISITE, "Lisa");
                    return;
                }
                if (!flow.isToyboxSolved)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_LOCKED, "Lisa");
                    // Open puzzle directly
                    uiManager.ShowToyboxPanel();
                }
                else if (!flow.hasEmilyDoll)
                {
                    // After puzzle solved, get doll
                    flow.hasEmilyDoll = true;
                    StartCoroutine(PickupDollSequence());
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_EMPTY, "Lisa");
                }
                break;

            case ObjectType.Dollhouse:
                // Prerequisite: Must have Emily's doll
                if (!flow.hasEmilyDoll)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.DOLLHOUSE_PREREQUISITE, "Lisa");
                    return;
                }
                if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("emily_doll"))
                {
                    StartCoroutine(OpenDollhouseSequence());
                }
                else
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.DOLLHOUSE_DONE, "Lisa");
                }
                break;

            case ObjectType.ReadingTable:
                // Prerequisite: Dollhouse must be done
                if (!flow.isDollhouseDone)
                {
                    DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.READING_TABLE_PREREQUISITE, "Lisa");
                    return;
                }
                StartCoroutine(ShowDialogueSequence(
                    Room07_ShortDialogues_FINAL.READING_TABLE_1,
                    Room07_ShortDialogues_FINAL.READING_TABLE_2,
                    Room07_ShortDialogues_FINAL.READING_TABLE_3
                ));
                flow.hasCheckedReadingTable = true;
                break;

            case ObjectType.Mirror:
                // Prerequisite: EVERYTHING must be complete
                if (!flow.IsEverythingComplete())
                {
                    // Give specific hint about what's missing
                    string hint = GetMissingStepHint(flow);
                    DialogueSystemV2.Instance?.StartDialogue(hint, "Lisa");
                    return;
                }
                
                // Mark mirror as interacted
                flow.hasInteractedWithMirror = true;
                
                // Trigger the jumpscare sequence
                StartCoroutine(TriggerMirrorSequence());
                break;
        }
    }
    
    // Helper method to give specific hints about missing steps
    private string GetMissingStepHint(Room07_FlowController flow)
    {
        // Check in order of sequence
        if (!flow.hasCheckedBed) return Room07_ShortDialogues_FINAL.MIRROR_HINT_BED;
        if (!flow.hasCheckedWall) return Room07_ShortDialogues_FINAL.MIRROR_HINT_WALL;
        if (!flow.hasCheckedDiary) return Room07_ShortDialogues_FINAL.MIRROR_HINT_DIARY;
        if (!flow.areCurtainsOpened) return Room07_ShortDialogues_FINAL.MIRROR_HINT_CURTAINS;
        if (!flow.hasEmilyCup) return Room07_ShortDialogues_FINAL.MIRROR_HINT_CABINET;
        if (!flow.isTeaPartyDone) return Room07_ShortDialogues_FINAL.MIRROR_HINT_TEA_PARTY;
        if (!flow.hasCheckedChair) return Room07_ShortDialogues_FINAL.MIRROR_HINT_CHAIR;
        if (!flow.hasCheckedCloset) return Room07_ShortDialogues_FINAL.MIRROR_HINT_CLOSET;
        if (!flow.isToyboxSolved) return Room07_ShortDialogues_FINAL.MIRROR_HINT_TOYBOX;
        if (!flow.hasEmilyDoll) return Room07_ShortDialogues_FINAL.MIRROR_HINT_DOLL;
        if (!flow.isDollhouseDone) return Room07_ShortDialogues_FINAL.MIRROR_HINT_DOLLHOUSE;
        if (!flow.hasCheckedReadingTable) return Room07_ShortDialogues_FINAL.MIRROR_HINT_READING_TABLE;
        
        return Room07_ShortDialogues_FINAL.MIRROR_NOT_READY;
    }
    
    // Coroutine to handle mirror jumpscare sequence
    System.Collections.IEnumerator TriggerMirrorSequence()
    {
        // Show ready dialogues
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.MIRROR_READY_1,
            Room07_ShortDialogues_FINAL.MIRROR_READY_2,
            Room07_ShortDialogues_FINAL.MIRROR_READY_3
        ));
        
        yield return new WaitForSeconds(0.5f);
        
        // Show jumpscare dialogues (now 2 parts instead of 3)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.MIRROR_JUMPSCARE_1,
            Room07_ShortDialogues_FINAL.MIRROR_JUMPSCARE_2
        ));
        
        yield return new WaitForSeconds(0.3f);
        
        // Show chase dialogue
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.MIRROR_CHASE, "Lisa");
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        // Trigger jumpscare
        MirrorJumpscareSequence jumpscareSequence = FindFirstObjectByType<MirrorJumpscareSequence>();
        if (jumpscareSequence != null)
        {
            jumpscareSequence.TriggerJumpscare();
        }
        else
        {
            // Fallback
            Room07_FlowController.Instance.CheckFinalCondition();
        }
    }

    // Coroutine to discover diary from bookshelf
    System.Collections.IEnumerator DiscoverDiarySequence()
    {
        // Show dialogue about finding diary
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.DIARY_FIND_1,
            Room07_ShortDialogues_FINAL.DIARY_FIND_2
        ));

        yield return new WaitForSeconds(0.3f);

        // Show diary content (now 3 parts instead of 5)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.DIARY_1,
            Room07_ShortDialogues_FINAL.DIARY_2,
            Room07_ShortDialogues_FINAL.DIARY_3
        ));

        Room07_FlowController.Instance.hasCheckedDiary = true;
    }

    // Coroutine to handle doll pickup - NOTIFICATION ONLY, no duplicate dialogue
    System.Collections.IEnumerator PickupDollSequence()
    {
        // Add item with notification (notification will show automatically)
        InventoryManager.Instance?.AddItemWithNotification("emily_doll");

        // Wait for notification to finish (player must click to continue)
        while (ItemNotificationUI.Instance != null && ItemNotificationUI.Instance.IsShowing())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Play cutscene after notification
        if (uiManager != null)
        {
            uiManager.PlayCutscene(); // Play Cutscene 2 (Doll Memory)
        }
    }

    // Coroutine to handle curtain panel opening with proper dialogue first
    System.Collections.IEnumerator OpenCurtainSequence()
    {
        // 1. Show dialogue first (now 2 parts instead of 4)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.CURTAINS_1,
            Room07_ShortDialogues_FINAL.CURTAINS_2
        ));

        yield return new WaitForSeconds(0.3f);

        // 2. Open panel
        if (uiManager != null)
        {
            uiManager.ShowCurtainPanel();
        }
    }

    // Coroutine to handle tea party panel opening
    System.Collections.IEnumerator OpenTeaPartySequence()
    {
        // Show dialogue first (now 2 parts instead of 3)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.TEA_PARTY_READY_1,
            Room07_ShortDialogues_FINAL.TEA_PARTY_READY_2
        ));

        yield return new WaitForSeconds(0.3f);

        if (uiManager != null)
        {
            uiManager.ShowTeaPartyPanel();
        }
    }

    // Coroutine to handle dollhouse panel opening
    System.Collections.IEnumerator OpenDollhouseSequence()
    {
        // Show dialogue first (now 1 part instead of 2)
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.DOLLHOUSE_READY_1, "Lisa");
        
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (uiManager != null)
        {
            uiManager.ShowDollhousePanel();
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