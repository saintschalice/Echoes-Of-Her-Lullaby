using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutleryDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Puzzle Settings")]
    public RectTransform targetSlot;   // I-drag dito ang Spoon_Slot (shadow)
    public float snapThreshold = 100f; // Lakasan natin para mas madaling mag-snap

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private bool isLocked = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        // Move spoon based on drag
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // CHECK DISTANCE
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetSlot.anchoredPosition);

        if (distance <= snapThreshold)
        {
            // SUCCESS: Auto-snap at Auto-submit
            rectTransform.anchoredPosition = targetSlot.anchoredPosition;
            isLocked = true;

            // Tawagin na agad ang controller
            Room05_DiningRoomController.Instance.OnPlaceSpoonConfirmed();

            this.enabled = false; // Stop dragging
        }
        else
        {
            // FAIL: Balik sa start position
            rectTransform.anchoredPosition = startPosition;
        }
    }
}