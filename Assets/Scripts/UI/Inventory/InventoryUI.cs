using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
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
    private bool hasNotifiedTutorial = false; // NEW: Track tutorial notification

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
                Debug.Log("Toggle button clicked!");
                ToggleInventory();
            });
        }

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);

        Debug.Log("[InventoryUI] Simple inventory system initialized");
    }

    void SetupInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("[InventoryUI] No inventory panel assigned!");
            return;
        }

        inventoryPanel.SetActive(true);

        CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = inventoryPanel.AddComponent<CanvasGroup>();
        }

        SetupScrollRect();

        RectTransform panelRect = inventoryPanel.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.5f, 0f);
            panelRect.anchorMax = new Vector2(0.5f, 0f);
            panelRect.pivot = new Vector2(0.5f, 0f);
            panelRect.anchoredPosition = new Vector2(0, 20);

            if (panelRect.sizeDelta.x < 100 || panelRect.sizeDelta.y < 50)
            {
                panelRect.sizeDelta = new Vector2(400, 80);
            }
        }
    }

    void SetupScrollRect()
    {
        ScrollRect scrollRect = inventoryPanel.GetComponentInChildren<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogWarning("[InventoryUI] No ScrollRect found in inventory panel");
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

        if (scrollRect.content != null)
        {
            HorizontalLayoutGroup layout = slotParent.GetComponent<HorizontalLayoutGroup>();
            float spacing = layout != null ? layout.spacing : 5f;
            float slotWidth = 60f;
            float contentWidth = (maxSlots * slotWidth) + ((maxSlots - 1) * spacing);

            scrollRect.content.anchorMin = new Vector2(0, 0);
            scrollRect.content.anchorMax = new Vector2(0, 1);
            scrollRect.content.pivot = new Vector2(0, 0.5f);
            scrollRect.content.anchoredPosition = new Vector2(0, 0);
            scrollRect.content.sizeDelta = new Vector2(contentWidth, 0);
        }

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
            Debug.LogError("[InventoryUI] Missing slot prefab or slot parent!");
            return;
        }

        // Clear existing slots
        foreach (Transform child in slotParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        slots.Clear();

        // Create slots
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();

            if (slot != null)
            {
                slots.Add(slot);

                RectTransform slotRect = slotObj.GetComponent<RectTransform>();
                if (slotRect != null)
                {
                    slotRect.sizeDelta = new Vector2(60, 60);
                }
            }
            else
            {
                Debug.LogWarning($"[InventoryUI] Slot prefab doesn't have InventorySlot component!");
            }
        }

        Debug.Log($"[InventoryUI] Created {slots.Count} inventory slots");
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

        Debug.Log($"[InventoryUI] Set inventory visible: {visible}");
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        SetVisible(isOpen);
        RefreshInventory();

        // NEW: Notify tutorial when inventory is opened (Step 4 completion)
        if (isOpen && !hasNotifiedTutorial && TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnInventoryOpened();
            hasNotifiedTutorial = true;
        }

        Debug.Log($"[InventoryUI] Toggled inventory - now open: {isOpen}");
    }

    bool ShouldBlockInventory()
    {
        // Block if dialogue is active
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive())
        {
            return true;
        }

        // Block if pause menu is open
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused())
        {
            return true;
        }

        // Block if save/load menu is open
        if (SaveUIManager.Instance != null && SaveUIManager.Instance.saveLoadPanel != null
            && SaveUIManager.Instance.saveLoadPanel.activeSelf)
        {
            return true;
        }

        return false;
    }

    // NEW: Force close inventory (called by other systems)
    public void ForceCloseInventory()
    {
        if (isOpen)
        {
            isOpen = false;
            SetVisible(false);
            HideItemTooltip();
            Debug.Log("[InventoryUI] Inventory force closed");
        }
    }

    public void OpenInventory()
    {
        isOpen = true;
        SetVisible(true);
        RefreshInventory();

        // NEW: Notify tutorial
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
            Debug.LogWarning("[InventoryUI] InventoryManager is null!");
            return;
        }

        if (slots.Count == 0)
        {
            Debug.LogWarning("[InventoryUI] No slots created!");
            return;
        }

        List<InventoryItem> items = inventoryManager.GetAllItems();
        Debug.Log($"[InventoryUI] Found {items.Count} items in inventory");

        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log($"[InventoryUI] Item {i}: {items[i].itemName} ({items[i].itemId})");
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                Debug.Log($"[InventoryUI] Setting slot {i} to item: {items[i].itemName}");
                slots[i].SetItem(items[i]);
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                Debug.Log($"[InventoryUI] Setting slot {i} to empty");
                slots[i].SetItem(null);
                slots[i].gameObject.SetActive(true);
            }
        }

        Debug.Log($"[InventoryUI] Refreshed {items.Count} items in {slots.Count} slots");
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty) return;

        InventoryItem item = slot.CurrentItem;
        Debug.Log($"[InventoryUI] Clicked item: {item.itemName}");

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

        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        if (tooltipRect != null)
        {
            tooltipRect.position = position + Vector3.up * 180;
        }
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