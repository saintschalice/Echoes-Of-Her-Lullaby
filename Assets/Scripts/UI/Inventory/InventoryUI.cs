using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Essential References")]
    public GameObject inventoryPanel;
    public Transform slotParent;
    public GameObject slotPrefab;
    public Button toggleButton;

    [Header("Settings")]
    public int maxSlots = 20;
    public bool startOpen = false;
    public KeyCode toggleKey = KeyCode.I;

    [Header("Tooltip (Optional)")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipItemName;
    public TextMeshProUGUI tooltipDescription;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventoryManager inventoryManager;
    private bool isOpen = false;
    private bool hasNotifiedTutorial = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            inventoryManager = FindFirstObjectByType<InventoryManager>();
        }

        SetupInventory();
        CreateSlots();

        SetVisible(startOpen);
        isOpen = startOpen;

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(() => {
                ToggleInventory();
            });
        }

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    void SetupInventory()
    {
        if (inventoryPanel == null)
        {
            return;
        }

        inventoryPanel.SetActive(true);

        CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = inventoryPanel.AddComponent<CanvasGroup>();
        }

        SetupScrollRect();
    }

    void SetupScrollRect()
    {
        ScrollRect scrollRect = inventoryPanel.GetComponentInChildren<ScrollRect>();
        if (scrollRect == null)
        {
            return;
        }

        if (scrollRect.content == null || scrollRect.content != slotParent)
        {
            RectTransform slotParentRect = slotParent.GetComponent<RectTransform>();
            if (slotParentRect != null)
            {
                scrollRect.content = slotParentRect;
            }
        }

        scrollRect.horizontal = true;
        scrollRect.vertical = false;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        Transform viewport = scrollRect.transform.Find("Viewport");
        if (viewport != null)
        {
            Mask mask = viewport.GetComponent<Mask>();
            RectMask2D rectMask = viewport.GetComponent<RectMask2D>();

            if (mask == null && rectMask == null)
            {
                viewport.gameObject.AddComponent<RectMask2D>();
            }
        }
    }

    void CreateSlots()
    {
        if (slotPrefab == null || slotParent == null)
        {
            return;
        }

        foreach (Transform child in slotParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        slots.Clear();

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();

            if (slot != null)
            {
                slots.Add(slot);
            }
        }
    }

    void SetVisible(bool visible)
    {
        if (inventoryPanel == null) return;

        CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
        else
        {
            inventoryPanel.SetActive(visible);
        }
    }

    public void ToggleInventory()
    {
        // --- 1. HANDLE EXCLUSIVITY RULES (Pause / Save UI) ---
        // This ensures clicking the button works the same as pressing the key

        // If Pause Menu is open -> Close it, Open Inventory
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused())
        {
            PauseMenuManager.Instance.ResumeGame();
            OpenInventory();
            return;
        }

        // If Save UI is open -> Close it, Open Inventory
        if (SaveUIManager.Instance != null && SaveUIManager.Instance.saveLoadPanel != null && SaveUIManager.Instance.saveLoadPanel.activeSelf)
        {
            SaveUIManager.Instance.CloseSaveLoadPanel(false); // false = Don't return to pause menu
            OpenInventory();
            return;
        }
        // -----------------------------------------------------

        if (ShouldBlockInventory()) return;

        isOpen = !isOpen;
        SetVisible(isOpen);
        RefreshInventory();

        if (isOpen && !hasNotifiedTutorial && TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnInventoryOpened();
            hasNotifiedTutorial = true;
        }
    }

    bool ShouldBlockInventory()
    {
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            return true;
        }

        // Note: PauseMenu and SaveUI checks were removed here because 
        // they are now handled in ToggleInventory() to perform the "Swap" behavior.

        return false;
    }

    public void ForceCloseInventory()
    {
        if (isOpen)
        {
            isOpen = false;
            SetVisible(false);
            HideItemTooltip();
        }
    }

    public void OpenInventory()
    {
        isOpen = true;
        SetVisible(true);
        RefreshInventory();

        if (!hasNotifiedTutorial && TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnInventoryOpened();
            hasNotifiedTutorial = true;
        }
    }

    public void CloseInventory()
    {
        isOpen = false;
        SetVisible(false);
        HideItemTooltip();
    }

    public void RefreshInventory()
    {
        if (inventoryManager == null)
        {
            return;
        }

        if (slots.Count == 0)
        {
            return;
        }

        List<InventoryItem> items = inventoryManager.GetAllItems();

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].SetItem(null);
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty) return;

        InventoryItem item = slot.CurrentItem;

        if (inventoryManager != null)
        {
            inventoryManager.UseItem(item.itemId);
        }

        RefreshInventory();
    }

    public void ShowItemTooltip(InventoryItem item, Vector3 position)
    {
        if (tooltipPanel == null || item == null) return;

        if (tooltipItemName != null)
            tooltipItemName.text = item.itemName;

        if (tooltipDescription != null)
            tooltipDescription.text = item.description;

        tooltipPanel.SetActive(true);
    }

    public void HideItemTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Now we just call ToggleInventory, because the exclusivity logic
            // has been moved inside that method.
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inventoryManager != null)
            {
                inventoryManager.AddItem("house_key");
                RefreshInventory();
            }
        }
    }

    public bool IsOpen => isOpen;
    public bool IsAnimating => false;

    public void OnInventoryChanged()
    {
        RefreshInventory();
    }

    [ContextMenu("Force Open")]
    void ForceOpen()
    {
        OpenInventory();
    }

    [ContextMenu("Force Close")]
    void ForceClose()
    {
        CloseInventory();
    }

    [ContextMenu("Add Test Item")]
    void AddTestItem()
    {
        if (inventoryManager != null)
        {
            inventoryManager.AddItem("house_key");
            RefreshInventory();
        }
    }
}