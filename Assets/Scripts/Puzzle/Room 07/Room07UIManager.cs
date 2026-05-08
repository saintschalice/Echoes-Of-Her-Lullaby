using UnityEngine;

public class Room07UIManager : MonoBehaviour
{
    [Header("Puzzle Panels")]
    public GameObject curtainPanel;
    public GameObject cabinetPanel; // NEW: Panel for getting Emily's Cup
    public GameObject teaPartyPanel;
    public GameObject toyboxPanel;
    public GameObject dollhousePanel;
    public GameObject blackScreenCutscene;

    public void HideAllPanels()
    {
        if (curtainPanel != null) curtainPanel.SetActive(false);
        if (cabinetPanel != null) cabinetPanel.SetActive(false);
        if (teaPartyPanel != null) teaPartyPanel.SetActive(false);
        if (toyboxPanel != null) toyboxPanel.SetActive(false);
        if (dollhousePanel != null) dollhousePanel.SetActive(false);
    }

    public void ShowCurtainPanel() 
    { 
        HideAllPanels(); 
        if (curtainPanel != null)
        {
            curtainPanel.SetActive(true);
            Debug.Log("[Room07] Showing Curtain Panel");
        }
        else
        {
            Debug.LogError("[Room07] Curtain Panel is NULL! Assign it in Room07UIManager.");
        }
    }
    
    public void ShowCabinetPanel()
    {
        HideAllPanels();
        if (cabinetPanel != null)
        {
            cabinetPanel.SetActive(true);
            Debug.Log("[Room07] Showing Cabinet Panel");
        }
        else
        {
            Debug.LogError("[Room07] Cabinet Panel is NULL! Assign it in Room07UIManager.");
        }
    }
    
    public void ShowTeaPartyPanel() 
    { 
        HideAllPanels(); 
        if (teaPartyPanel != null)
        {
            teaPartyPanel.SetActive(true);
            Debug.Log("[Room07] Showing Tea Party Panel");
        }
        else
        {
            Debug.LogError("[Room07] Tea Party Panel is NULL! Assign it in Room07UIManager.");
        }
    }
    
    public void ShowToyboxPanel() 
    { 
        HideAllPanels(); 
        if (toyboxPanel != null)
        {
            toyboxPanel.SetActive(true);
            Debug.Log("[Room07] Showing Toybox Panel");
        }
        else
        {
            Debug.LogError("[Room07] Toybox Panel is NULL! Assign it in Room07UIManager.");
        }
    }
    
    public void ShowDollhousePanel() 
    { 
        HideAllPanels(); 
        if (dollhousePanel != null)
        {
            dollhousePanel.SetActive(true);
            Debug.Log("[Room07] Showing Dollhouse Panel");
        }
        else
        {
            Debug.LogError("[Room07] Dollhouse Panel is NULL! Assign it in Room07UIManager.");
        }
    }

    // TATAWAGIN ITO NG MGA UI BUTTONS/LOGIC MO KAPAG NA-SOLVE NA ANG PUZZLE
    public void OnCurtainsOpened()
    {
        HideAllPanels();
        Room07_FlowController.Instance.areCurtainsOpened = true;
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.CURTAINS_COMPLETION, "Lisa");
    }

    public void OnTeaPartySolved()
    {
        HideAllPanels();
        Room07_FlowController.Instance.isTeaPartyDone = true;
        InventoryManager.Instance?.RemoveItem("emily_cup");
        
        Debug.Log("[Room07] Tea Party Solved! isTeaPartyDone = true");
        
        // Start the cutscene sequence
        StartCoroutine(TeaPartyCutsceneSequence());
    }
    
    System.Collections.IEnumerator TeaPartyCutsceneSequence()
    {
        // 1. Show cutscene
        if (blackScreenCutscene != null)
        {
            blackScreenCutscene.SetActive(true);
            yield return new WaitForSeconds(3f); // Cutscene duration
            blackScreenCutscene.SetActive(false);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // 2. Show dialogue after cutscene (now 3 parts instead of 4)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_1,
            Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_2,
            Room07_ShortDialogues_FINAL.TEA_PARTY_MEMORY_3
        ));
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // 3. Show progress message
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TEA_PARTY_COMPLETE, "Lisa");
    }

    public void OnToyboxSolved()
    {
        HideAllPanels();
        Room07_FlowController.Instance.isToyboxSolved = true;
        // Pagkasara nito, pwede ulit i-interact ang toybox para makuha ang doll
        DialogueSystemV2.Instance?.StartDialogue(Room07_ShortDialogues_FINAL.TOYBOX_SOLVED, "Lisa");
    }

    public void OnDollhouseSolved()
    {
        HideAllPanels();
        StartCoroutine(DollhouseCompletionSequence());
    }
    
    System.Collections.IEnumerator DollhouseCompletionSequence()
    {
        Room07_FlowController.Instance.isDollhouseDone = true;
        InventoryManager.Instance?.RemoveItem("emily_doll");
        
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.DOLLHOUSE_COMPLETE_1,
            Room07_ShortDialogues_FINAL.DOLLHOUSE_COMPLETE_2
        ));
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

    public void PlayCutscene()
    {
        if (blackScreenCutscene != null)
        {
            blackScreenCutscene.SetActive(true);
            Invoke(nameof(EndCutscene), 2f);
        }
    }

    private void EndCutscene()
    {
        if (blackScreenCutscene != null) blackScreenCutscene.SetActive(false);
    }
}