using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// CHANGED: Removed IPointerEnterHandler, IPointerExitHandler to prevent mouse-hover interference on mobile
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

    // --- MANUAL DOUBLE TAP VARIABLES ---
    private float lastTapTime = -1f; // Initialize to -1 so the first click at time 0 doesn't trigger it
    private const float DOUBLE_TAP_THRESHOLD = 0.3f; // 0.3 seconds is standard for mobile double taps

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
            // Restore original color
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

    // --- UPDATED OnPointerClick FOR ANDROID/MOBILE ---
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryUI == null) return;

        if (isEmpty)
        {
            // Clicked an empty slot, hide any active tooltip
            inventoryUI.HideItemTooltip();
            return;
        }

        // We use Time.unscaledTime to handle pausing correctly (if UI works while paused)
        float currentTime = Time.unscaledTime;

        // Check if the time since the last tap is within our threshold (0.3s)
        if (currentTime - lastTapTime < DOUBLE_TAP_THRESHOLD)
        {
            // --- DOUBLE TAP DETECTED ---
            // Reset lastTapTime so a 3rd rapid click doesn't trigger this again immediately
            lastTapTime = -1f;

            inventoryUI.HideItemTooltip();
            inventoryUI.OnSlotClicked(this); // Use the item
            PlayClickSound();
        }
        else
        {
            // --- SINGLE TAP DETECTED ---
            // (Or the first tap of a double tap sequence)
            lastTapTime = currentTime;

            inventoryUI.ShowItemTooltip(currentItem, transform.position);
            PlayHoverSound(); // Re-using hover sound for single tap feedback
        }
    }

    void PlayClickSound()
    {
        Debug.Log($"[InventorySlot] Double-tap detected: {currentItem?.itemName}");
        // AudioManager.Instance?.PlaySFX("inventory_click");
    }

    void PlayHoverSound()
    {
        Debug.Log($"[InventorySlot] Single-tap detected: {currentItem?.itemName}");
        // AudioManager.Instance?.PlaySFX("inventory_hover");
    }

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