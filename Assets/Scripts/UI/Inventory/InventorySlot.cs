using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public Image itemIcon;
    public Image slotBackground;
    public Image keyItemIndicator; // Special border for key items
    public GameObject quantityPanel;
    public TextMeshProUGUI quantityText;

    [Header("Visual States")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 1f, 1f, 0.8f);
    public Color keyItemColor = new Color(1f, 0.8f, 0.2f); // Golden tint
    public Color emptySlotColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);

    private InventoryItem currentItem;
    private InventoryUI inventoryUI;
    private bool isEmpty = true;
    private bool isHovered = false;

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
            slotBackground.color = currentItem.isKeyItem ? keyItemColor : normalColor;
        }

        if (keyItemIndicator != null)
        {
            keyItemIndicator.gameObject.SetActive(currentItem.isKeyItem);
        }

        // For now, assume quantity is always 1
        // You can expand this later if you add stackable items
        if (quantityPanel != null)
        {
            quantityPanel.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEmpty) return;

        if (inventoryUI != null)
        {
            inventoryUI.OnSlotClicked(this);
        }

        // Add click feedback
        PlayClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEmpty) return;

        isHovered = true;

        if (slotBackground != null)
        {
            Color hoverTint = currentItem.isKeyItem ? keyItemColor : normalColor;
            slotBackground.color = Color.Lerp(hoverTint, hoverColor, 0.3f);
        }

        // Show item tooltip positioned above this slot
        if (inventoryUI != null)
        {
            inventoryUI.ShowItemTooltip(currentItem, transform.position);
        }

        PlayHoverSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEmpty) return;

        isHovered = false;

        if (slotBackground != null)
        {
            slotBackground.color = currentItem.isKeyItem ? keyItemColor : normalColor;
        }

        // Hide tooltip
        if (inventoryUI != null)
        {
            inventoryUI.HideItemTooltip();
        }
    }

    void PlayClickSound()
    {
        // Hook into your audio system here
        // AudioManager.Instance?.PlaySFX("inventory_click");
        Debug.Log($"Inventory slot clicked: {currentItem?.itemName}");
    }

    void PlayHoverSound()
    {
        // Hook into your audio system here
        // AudioManager.Instance?.PlaySFX("inventory_hover");
    }

    // Animation helpers
    public void AnimatePickup()
    {
        if (isEmpty) return;

        // Simple scale animation using Unity's built-in system
        StartCoroutine(ScaleAnimation(gameObject.transform, Vector3.one * 1.2f, 0.1f, () => {
            StartCoroutine(ScaleAnimation(gameObject.transform, Vector3.one, 0.1f));
        }));
    }

    public void AnimateUse()
    {
        if (isEmpty) return;

        // Flash animation to indicate item use
        StartCoroutine(FlashAnimation());
    }

    // Unity built-in scale animation
    System.Collections.IEnumerator ScaleAnimation(Transform target, Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;

            // Ease out quad
            progress = 1f - (1f - progress) * (1f - progress);

            target.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        target.localScale = targetScale;
        onComplete?.Invoke();
    }

    // Flash animation for item use
    System.Collections.IEnumerator FlashAnimation()
    {
        Color originalColor = itemIcon.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);

        // Fade to flash color
        yield return StartCoroutine(ColorAnimation(itemIcon, flashColor, 0.15f));

        // Fade back to original
        yield return StartCoroutine(ColorAnimation(itemIcon, originalColor, 0.15f));
    }

    // Color animation helper
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

    // Utility method for external systems
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