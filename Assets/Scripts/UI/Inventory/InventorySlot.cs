using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// CHANGED: Removed IPointerEnterHandler, IPointerExitHandler
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image itemIcon;
    public Image slotBackground;
    public Image keyItemIndicator;
    public GameObject quantityPanel;
    public TextMeshProUGUI quantityText;

    [Header("Visual States")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 1f, 1f, 0.8f);
    public Color keyItemColor = new Color(1f, 0.8f, 0.2f);
    public Color emptySlotColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);

    private InventoryItem currentItem;
    private InventoryUI inventoryUI;
    private bool isEmpty = true;

    public InventoryItem CurrentItem => currentItem;
    public bool IsEmpty => isEmpty;

    void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
        SetEmptyState();
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        isEmpty = item == null;

        if (isEmpty)
        {
            SetEmptyState();
        }
        else
        {
            SetFilledState();
        }
    }

    void SetEmptyState()
    {
        isEmpty = true;
        currentItem = null;
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
        }
        if (slotBackground != null)
        {
            slotBackground.color = emptySlotColor;
        }
        if (keyItemIndicator != null)
        {
            keyItemIndicator.gameObject.SetActive(false);
        }
        if (quantityPanel != null)
        {
            quantityPanel.SetActive(false);
        }
    }

    void SetFilledState()
    {
        isEmpty = false;
        if (itemIcon != null && currentItem.itemIcon != null)
        {
            itemIcon.sprite = currentItem.itemIcon;
            itemIcon.color = Color.white;
        }
        if (slotBackground != null)
        {
            // Restore original color when not hovered
            slotBackground.color = currentItem.isKeyItem ? keyItemColor : normalColor;
        }
        if (keyItemIndicator != null)
        {
            keyItemIndicator.gameObject.SetActive(currentItem.isKeyItem);
        }
        if (quantityPanel != null)
        {
            quantityPanel.SetActive(false);
        }
    }

    // --- NEW OnPointerClick LOGIC for Taps ---
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryUI == null) return;

        if (isEmpty)
        {
            // Clicked an empty slot, hide any active tooltip
            inventoryUI.HideItemTooltip();
            return;
        }

        if (eventData.clickCount >= 2)
        {
            // --- DOUBLE TAP ---
            // Use the item (which triggers dialogue/readers)
            inventoryUI.HideItemTooltip();
            inventoryUI.OnSlotClicked(this); // This calls InventoryManager.UseItem
            PlayClickSound();
        }
        else if (eventData.clickCount == 1)
        {
            // --- SINGLE TAP ---
            // Show the tooltip
            inventoryUI.ShowItemTooltip(currentItem, transform.position);
            PlayHoverSound(); // Re-using hover sound for tap
        }
    }

    // --- DELETED OnPointerEnter and OnPointerExit ---

    void PlayClickSound()
    {
        Debug.Log($"Inventory slot double-clicked: {currentItem?.itemName}");
        // AudioManager.Instance?.PlaySFX("inventory_click");
    }

    void PlayHoverSound()
    {
        Debug.Log($"Inventory slot single-tapped: {currentItem?.itemName}");
        // AudioManager.Instance?.PlaySFX("inventory_hover");
    }

    // (All animation helper methods remain the same)

    // ... (rest of the file is unchanged) ...

    public void AnimatePickup()
    {
        if (isEmpty) return;
        StartCoroutine(ScaleAnimation(gameObject.transform, Vector3.one * 1.2f, 0.1f, () => {
            StartCoroutine(ScaleAnimation(gameObject.transform, Vector3.one, 0.1f));
        }));
    }

    public void AnimateUse()
    {
        if (isEmpty) return;
        StartCoroutine(FlashAnimation());
    }

    System.Collections.IEnumerator ScaleAnimation(Transform target, Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            progress = 1f - (1f - progress) * (1f - progress);
            target.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }
        target.localScale = targetScale;
        onComplete?.Invoke();
    }

    System.Collections.IEnumerator FlashAnimation()
    {
        Color originalColor = itemIcon.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        yield return StartCoroutine(ColorAnimation(itemIcon, flashColor, 0.15f));
        yield return StartCoroutine(ColorAnimation(itemIcon, originalColor, 0.15f));
    }

    System.Collections.IEnumerator ColorAnimation(Image target, Color targetColor, float duration)
    {
        Color startColor = target.color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            target.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }
        target.color = targetColor;
    }

    public void RefreshVisuals()
    {
        if (currentItem != null)
        {
            SetFilledState();
        }
        else
        {
            SetEmptyState();
        }
    }
}