using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// SIMPLE drag and drop system for puzzle items
/// NO PREFABS NEEDED - just attach to UI Image
/// Works with touch and mouse
/// </summary>
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Settings")]
    [Tooltip("Unique ID like 'bottle_1973', 'piece1', 'page1', 'rope'")]
    public string itemId;
    
    [Tooltip("Which puzzle: 1=Medicine, 2=Bathtub, 3=Vanity, 4=Evidence")]
    public int puzzleNumber;
    
    [Header("Drag Settings")]
    [Tooltip("Return to start position if not placed in slot")]
    public bool returnToOriginalPosition = true;
    
    [Tooltip("Detection radius - higher = easier to snap to slots")]
    public float detectionRadius = 150f; // Adjust in Inspector!
    
    [Tooltip("For Mirror 3: Keep item in puzzle panel (don't move to Canvas root)")]
    public bool stayInPanel = false; // Set to TRUE for Mirror 3 items!
    
    [Header("Visual Feedback")]
    [Tooltip("Make semi-transparent while dragging")]
    public bool fadeWhileDragging = true;
    public float dragAlpha = 0.6f;
    
    // Private variables
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private int originalSiblingIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // Add CanvasGroup if not present (for alpha control)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Find canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"[DraggableItem] {gameObject.name} is not under a Canvas!");
        }
    }

    // Called when drag starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store original position and parent
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        
        // Make semi-transparent while dragging
        if (fadeWhileDragging)
        {
            canvasGroup.alpha = dragAlpha;
        }
        
        // Disable raycast blocking so we can detect slots underneath
        canvasGroup.blocksRaycasts = false;
        
        // For Mirror 3 (stayInPanel = true), don't move to Canvas root
        // This prevents items from going outside the puzzle panel
        if (stayInPanel)
        {
            // Just move to last sibling for rendering on top, but stay in current parent
            transform.SetAsLastSibling();
            Debug.Log($"[DraggableItem] {itemId} staying in panel (stayInPanel=true)");
        }
        else
        {
            // Move to canvas root for free movement
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            Debug.Log($"[DraggableItem] {itemId} moved to Canvas root");
        }
        
        Debug.Log($"[DraggableItem] Started dragging: {itemId}");
    }

    // Called while dragging
    public void OnDrag(PointerEventData eventData)
    {
        // Move with pointer/finger
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    // Called when drag ends
    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore alpha
        canvasGroup.alpha = 1f;
        
        // Re-enable raycast blocking
        canvasGroup.blocksRaycasts = true;
        
        // Check if dropped on a valid slot
        GameObject droppedSlot = GetSlotUnderPointer(eventData);
        
        if (droppedSlot != null)
        {
            Debug.Log($"[DraggableItem] {itemId} dropped on {droppedSlot.name}");
            
            // Notify puzzle script - it will handle the placement/swap
            bool placementAccepted = NotifyPuzzleScript(droppedSlot);
            
            if (!placementAccepted)
            {
                // Invalid placement - return to original position
                Debug.Log($"[DraggableItem] {itemId} placement rejected - returning to original position");
                ReturnToOriginalPosition();
            }
            // If accepted, puzzle script already moved the item, so we don't need to do anything
        }
        else
        {
            Debug.Log($"[DraggableItem] {itemId} dropped on nothing");
            
            // No valid slot - return to original position
            if (returnToOriginalPosition)
            {
                ReturnToOriginalPosition();
            }
        }
    }

    private GameObject GetSlotUnderPointer(PointerEventData eventData)
    {
        // Raycast to find what's under the pointer
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        GameObject bestSlot = null;
        float closestDistance = float.MaxValue;
        
        foreach (var result in results)
        {
            // Skip self
            if (result.gameObject == gameObject) continue;
            
            // Skip if it's a container (parent of slots)
            if (result.gameObject.name.Contains("Container")) continue;
            
            // Check if it's a slot by name
            // Slots should have "Slot" in name or "Frame" in name
            if (result.gameObject.name.Contains("Slot") || 
                result.gameObject.name.Contains("Frame"))
            {
                // Make sure it's not a container
                if (!result.gameObject.name.Contains("Container"))
                {
                    // Get RectTransform
                    RectTransform slotRect = result.gameObject.GetComponent<RectTransform>();
                    if (slotRect != null)
                    {
                        // Calculate distance between bottle and slot
                        float distance = Vector2.Distance(
                            rectTransform.position, 
                            slotRect.position
                        );
                        
                        // If within detection radius and closer than previous best
                        if (distance < detectionRadius && distance < closestDistance)
                        {
                            bestSlot = result.gameObject;
                            closestDistance = distance;
                        }
                    }
                }
            }
        }
        
        if (bestSlot != null)
        {
            Debug.Log($"[DraggableItem] Found valid slot: {bestSlot.name} (distance: {closestDistance:F1})");
        }
        else
        {
            Debug.Log($"[DraggableItem] No valid slot found within {detectionRadius} units");
        }
        
        return bestSlot;
    }

    private void PlaceInSlot(GameObject slot)
    {
        // Move to slot
        transform.SetParent(slot.transform);
        rectTransform.anchoredPosition = Vector2.zero;
        
        Debug.Log($"[DraggableItem] {itemId} placed in {slot.name}");
    }

    private void ReturnToOriginalPosition()
    {
        // Return to original parent and position
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
        rectTransform.anchoredPosition = originalPosition;
        
        Debug.Log($"[DraggableItem] {itemId} returned to original position");
    }

    private bool NotifyPuzzleScript(GameObject slot)
    {
        // Notify the appropriate puzzle script based on puzzle number
        // Returns true if placement was accepted, false if rejected
        
        switch (puzzleNumber)
        {
            case 1: // Medicine Cabinet
                Mirror1_MedicineCabinet mirror1 = FindObjectOfType<Mirror1_MedicineCabinet>();
                if (mirror1 != null)
                {
                    return mirror1.ValidateAndPlaceBottle(slot, itemId);
                }
                else
                {
                    Debug.LogWarning("[DraggableItem] Mirror1_MedicineCabinet not found!");
                    return false;
                }
                
            case 2: // Bathtub Drain
                Mirror2_BathtubDrain mirror2 = FindObjectOfType<Mirror2_BathtubDrain>();
                if (mirror2 != null)
                {
                    mirror2.OnPiecePlacedInSlot(slot, itemId);
                    return true; // Mirror 2 doesn't validate yet
                }
                else
                {
                    Debug.LogWarning("[DraggableItem] Mirror2_BathtubDrain not found!");
                    return false;
                }
                
            case 3: // Vanity Terror
                Mirror3_VanityTerror mirror3 = FindObjectOfType<Mirror3_VanityTerror>();
                if (mirror3 != null)
                {
                    mirror3.OnPagePlacedInSlot(slot, itemId);
                    return true; // Mirror 3 doesn't validate yet
                }
                else
                {
                    Debug.LogWarning("[DraggableItem] Mirror3_VanityTerror not found!");
                    return false;
                }
                
            case 4: // Evidence Sequence
                Mirror4_EvidenceSequence mirror4 = FindObjectOfType<Mirror4_EvidenceSequence>();
                if (mirror4 != null)
                {
                    mirror4.OnItemPlacedInFrame(slot, itemId);
                    return true; // Mirror 4 doesn't validate yet
                }
                else
                {
                    Debug.LogWarning("[DraggableItem] Mirror4_EvidenceSequence not found!");
                    return false;
                }
                
            default:
                Debug.LogError($"[DraggableItem] ❌ INVALID PUZZLE NUMBER on GameObject '{gameObject.name}'!");
                Debug.LogError($"[DraggableItem] Item ID: '{itemId}', Puzzle Number: {puzzleNumber}");
                Debug.LogError($"[DraggableItem] Valid puzzle numbers are: 1 (Medicine), 2 (Bathtub), 3 (Vanity), 4 (Evidence)");
                Debug.LogError($"[DraggableItem] Please check the Inspector for '{gameObject.name}' and set Puzzle Number to 1, 2, 3, or 4");
                return false;
        }
    }

    // Public method to reset item (can be called by puzzle scripts)
    public void ResetItem()
    {
        ReturnToOriginalPosition();
    }
    
    // Public method to set item data (useful for dynamic setup)
    public void SetItemData(string id, int puzzle)
    {
        itemId = id;
        puzzleNumber = puzzle;
    }
}
