using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Essential References")]
    public GameObject inventoryPanel;
    public Transform slotParent; // Where slots will be created
    public GameObject slotPrefab; // Your InventorySlot prefab
    public Button toggleButton; // Button to open/close inventory

    [Header("Settings")]
    public int maxSlots = 20; // Changed to 20 as requested
    public bool startOpen = false;
    public KeyCode toggleKey = KeyCode.I;

    [Header("Tooltip (Optional)")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipItemName;
    public TextMeshProUGUI tooltipDescription;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventoryManager inventoryManager;
    private bool isOpen = false;

    void Start()
    {
        // Find inventory manager
        inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            inventoryManager = FindFirstObjectByType<InventoryManager>();
        }

        // Setup the inventory
        SetupInventory();
        CreateSlots();

        // Set initial state
        SetVisible(startOpen);
        isOpen = startOpen;

        // Setup toggle button
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(() => {
                Debug.Log("Toggle button clicked!");
                ToggleInventory();
            });
        }

        // Hide tooltip
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);

        Debug.Log("[InventoryUI] Simple inventory system initialized");
    }

    void SetupInventory()
    {
        // Ensure inventory panel exists and is positioned correctly
        if (inventoryPanel == null)
        {
            Debug.LogError("[InventoryUI] No inventory panel assigned!");
            return;
        }

        // Make sure the panel is active (we'll control visibility through CanvasGroup)
        inventoryPanel.SetActive(true);

        // Add or get CanvasGroup for smooth show/hide
        CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = inventoryPanel.AddComponent<CanvasGroup>();
        }

        // Setup ScrollRect properly
        SetupScrollRect();

        // Ensure proper RectTransform settings
        RectTransform panelRect = inventoryPanel.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            // Position at bottom-center of screen
            panelRect.anchorMin = new Vector2(0.5f, 0f);  // Bottom-center
            panelRect.anchorMax = new Vector2(0.5f, 0f);  // Bottom-center
            panelRect.pivot = new Vector2(0.5f, 0f);      // Pivot at bottom
            panelRect.anchoredPosition = new Vector2(0, 20); // 20 units above bottom

            // Ensure reasonable size
            if (panelRect.sizeDelta.x < 100 || panelRect.sizeDelta.y < 50)
            {
                panelRect.sizeDelta = new Vector2(400, 80); // Default size
            }
        }
    }

    void SetupScrollRect()
    {
        // Find ScrollRect in the inventory panel
        ScrollRect scrollRect = inventoryPanel.GetComponentInChildren<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogWarning("[InventoryUI] No ScrollRect found in inventory panel");
            return;
        }

        Debug.Log($"[InventoryUI] Found ScrollRect: {scrollRect.name}");
        Debug.Log($"[InventoryUI] ScrollRect content is: {(scrollRect.content != null ? scrollRect.content.name : "NULL")}");
        Debug.Log($"[InventoryUI] slotParent is: {(slotParent != null ? slotParent.name : "NULL")}");

        // The ScrollRect content should be the slotParent (where slots are created)
        if (scrollRect.content == null || scrollRect.content != slotParent)
        {
            RectTransform slotParentRect = slotParent.GetComponent<RectTransform>();
            if (slotParentRect != null)
            {
                scrollRect.content = slotParentRect;
                Debug.Log($"[InventoryUI] Set ScrollRect content to: {slotParent.name}");
            }
        }

        // Configure ScrollRect for horizontal scrolling
        scrollRect.horizontal = true;
        scrollRect.vertical = false;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        // Setup content size and positioning for proper masking
        if (scrollRect.content != null)
        {
            // Calculate content width based on slot count and spacing
            HorizontalLayoutGroup layout = slotParent.GetComponent<HorizontalLayoutGroup>();
            float spacing = layout != null ? layout.spacing : 5f;
            float slotWidth = 60f; // Slot size
            float contentWidth = (maxSlots * slotWidth) + ((maxSlots - 1) * spacing);
            
            // CRITICAL: Set proper anchors for content to enable masking
            scrollRect.content.anchorMin = new Vector2(0, 0);
            scrollRect.content.anchorMax = new Vector2(0, 1);
            scrollRect.content.pivot = new Vector2(0, 0.5f);
            scrollRect.content.anchoredPosition = new Vector2(0, 0);
            scrollRect.content.sizeDelta = new Vector2(contentWidth, 0);
            
            Debug.Log($"[InventoryUI] Content setup - Width: {contentWidth}, Anchors: (0,0)-(0,1), Position: (0,0)");
        }

        // Ensure Viewport has proper masking
        Transform viewport = scrollRect.transform.Find("Viewport");
        if (viewport != null)
        {
            // Check for masking component
            Mask mask = viewport.GetComponent<Mask>();
            RectMask2D rectMask = viewport.GetComponent<RectMask2D>();
            
            if (mask == null && rectMask == null)
            {
                // Add RectMask2D if no masking component exists
                viewport.gameObject.AddComponent<RectMask2D>();
                Debug.Log("[InventoryUI] Added RectMask2D to Viewport");
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

                // Fix slot size to fit in scroll area
                RectTransform slotRect = slotObj.GetComponent<RectTransform>();
                if (slotRect != null)
                {
                    // Set slot size to reasonable dimensions
                    slotRect.sizeDelta = new Vector2(60, 60); // 60x60 pixel slots
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
        Debug.Log($"[InventoryUI] Toggled inventory - now open: {isOpen}");
    }

    public void OpenInventory()
    {
        isOpen = true;
        SetVisible(true);
        RefreshInventory();
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

        // Debug: List all items
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log($"[InventoryUI] Item {i}: {items[i].itemName} ({items[i].itemId})");
        }

        // Update all slots
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
                slots[i].SetItem(null); // Empty slot
                slots[i].gameObject.SetActive(true); // Keep visible but empty
            }
        }

        Debug.Log($"[InventoryUI] Refreshed {items.Count} items in {slots.Count} slots");
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty) return;

        InventoryItem item = slot.CurrentItem;
        Debug.Log($"[InventoryUI] Clicked item: {item.itemName}");

        // Use the item
        if (inventoryManager != null)
        {
            inventoryManager.UseItem(item.itemId);
        }

        // Refresh after use
        RefreshInventory();
    }

    public void ShowItemTooltip(InventoryItem item, Vector3 position)
    {
        if (tooltipPanel == null || item == null) return;

        // Update tooltip content
        if (tooltipItemName != null)
            tooltipItemName.text = item.itemName;

        if (tooltipDescription != null)
            tooltipDescription.text = item.description;

        // Show tooltip
        tooltipPanel.SetActive(true);

        // Position tooltip (basic positioning)
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
        // Handle toggle input
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }

        // Debug keys
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Test add item
            if (inventoryManager != null)
            {
                inventoryManager.AddItem("house_key");
                RefreshInventory();
            }
        }
    }

    // Public accessors
    public bool IsOpen => isOpen;
    public bool IsAnimating => false; // No animations in simple version

    // Called by InventoryManager when inventory changes
    public void OnInventoryChanged()
    {
        RefreshInventory();
    }

    // Context menu helpers
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