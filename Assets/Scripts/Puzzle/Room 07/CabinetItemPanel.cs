using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Simple panel that shows an item - click the item to take it
/// No buttons needed, just click the item image
/// </summary>
public class CabinetItemPanel : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cabinetPanel;
    public Button closeButton; // Optional
    
    [Header("Scene References")]
    public GameObject cupInScene; // The actual cup GameObject in the scene (will be hidden after taking)
    
    [Header("Item Display")]
    public Image itemImage; // Click this to take item
    public Text itemNameText;
    public Text itemDescriptionText;
    
    [Header("Item to Give")]
    public string itemId = "emily_cup";
    
    [Header("Visual Feedback")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    
    [Header("Audio")]
    public AudioClip openSound;
    public AudioClip takeSound;
    
    private bool itemTaken = false;
    private EventTrigger itemTrigger;

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
        
        // Add click handler to item image
        SetupItemClickHandler();
    }

    void SetupItemClickHandler()
    {
        if (itemImage == null) return;
        
        // Make sure image can receive raycasts
        itemImage.raycastTarget = true;
        
        // Add EventTrigger component
        itemTrigger = itemImage.gameObject.GetComponent<EventTrigger>();
        if (itemTrigger == null)
            itemTrigger = itemImage.gameObject.AddComponent<EventTrigger>();
        
        // Add click event
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { OnItemClicked(); });
        itemTrigger.triggers.Add(clickEntry);
        
        // Add hover events for visual feedback
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { OnItemHoverEnter(); });
        itemTrigger.triggers.Add(enterEntry);
        
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnItemHoverExit(); });
        itemTrigger.triggers.Add(exitEntry);
    }

    void OnEnable()
    {
        PauseGame();
        PlaySound(openSound);
        
        Debug.Log($"[CabinetItemPanel] Panel opened. Cup In Scene assigned: {cupInScene != null}");
        
        // Load item data from database
        LoadItemData();
        
        // Check if already taken
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemId))
        {
            itemTaken = true;
            
            // Disable item image interaction
            if (itemImage != null)
            {
                itemImage.color = Color.gray;
                itemImage.raycastTarget = false;
            }
            
            // Show "Already taken" message
            if (itemDescriptionText != null)
            {
                itemDescriptionText.text = "Already taken.";
                itemDescriptionText.color = Color.gray;
            }
            
            // Hide cup in scene if already taken
            if (cupInScene != null)
            {
                cupInScene.SetActive(false);
            }
        }
        else
        {
            itemTaken = false;
            
            // Enable item image interaction
            if (itemImage != null)
            {
                itemImage.color = normalColor;
                itemImage.raycastTarget = true;
            }
            
            // Show cup in scene if not taken yet
            if (cupInScene != null)
            {
                cupInScene.SetActive(true);
            }
        }
    }

    void LoadItemData()
    {
        if (InventoryManager.Instance?.itemDatabase != null)
        {
            InventoryItem item = InventoryManager.Instance.itemDatabase.GetItem(itemId);
            
            if (item != null)
            {
                // Update UI with item data
                if (itemImage != null && item.itemIcon != null)
                    itemImage.sprite = item.itemIcon;
                
                if (itemNameText != null)
                    itemNameText.text = item.itemName;
                
                if (itemDescriptionText != null && !itemTaken)
                    itemDescriptionText.text = item.description;
            }
            else
            {
                Debug.LogError($"[CabinetItemPanel] Item not found in database: {itemId}");
            }
        }
    }

    void OnItemHoverEnter()
    {
        if (itemTaken) return;
        
        // Highlight item on hover
        if (itemImage != null)
            itemImage.color = hoverColor;
    }

    void OnItemHoverExit()
    {
        if (itemTaken) return;
        
        // Return to normal color
        if (itemImage != null)
            itemImage.color = normalColor;
    }

    void OnItemClicked()
    {
        if (itemTaken) return;
        
        Debug.Log($"[CabinetItemPanel] Item clicked: {itemId}");
        StartCoroutine(TakeItemSequence());
    }

    IEnumerator TakeItemSequence()
    {
        itemTaken = true;
        
        // Play sound
        PlaySound(takeSound);
        
        // Hide cup in scene immediately (if assigned)
        if (cupInScene != null)
        {
            cupInScene.SetActive(false);
            Debug.Log("[CabinetItemPanel] Cup in scene hidden!");
        }
        else
        {
            Debug.LogWarning("[CabinetItemPanel] Cup In Scene is not assigned! Cup will not be hidden.");
        }
        
        // Close panel immediately to hide the UI cup
        if (cabinetPanel != null)
            cabinetPanel.SetActive(false);
        
        ResumeGame();
        
        yield return new WaitForSeconds(0.3f);
        
        // Show dialogue (now 2 parts instead of 4)
        yield return StartCoroutine(ShowDialogueSequence(
            Room07_ShortDialogues_FINAL.CABINET_1,
            Room07_ShortDialogues_FINAL.CABINET_2
        ));
        
        // Wait for dialogue to finish
        while (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Add item with notification
        InventoryManager.Instance?.AddItemWithNotification(itemId);
        
        // Update flow controller
        Room07_FlowController flow = Room07_FlowController.Instance;
        if (flow != null)
        {
            flow.hasEmilyCup = true;
        }
    }

    void ClosePanel()
    {
        if (cabinetPanel != null)
            cabinetPanel.SetActive(false);
        
        ResumeGame();
    }

    void PauseGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = true;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);
    }

    void ResumeGame()
    {
        EmilyGhost emily = FindFirstObjectByType<EmilyGhost>();
        if (emily != null) emily.isPaused = false;

        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = true;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(true);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioManager.Instance?.PlaySFX(clip);
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
