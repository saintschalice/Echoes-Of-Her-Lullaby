using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Debug script to diagnose inventory click issues.
/// Attach to InventoryPanel to see what's blocking clicks.
/// </summary>
public class InventoryClickDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool enableDebug = true;
    public KeyCode debugKey = KeyCode.D;

    void Update()
    {
        if (enableDebug && Input.GetKeyDown(debugKey))
        {
            DebugInventoryUI();
        }

        // Debug click position
        if (enableDebug && Input.GetMouseButtonDown(0))
        {
            DebugClickPosition();
        }
    }

    void DebugInventoryUI()
    {
        Debug.Log("=== INVENTORY UI DEBUG ===");

        // Check InventoryUI
        if (InventoryUI.Instance != null)
        {
            Debug.Log($"[Debug] InventoryUI.Instance exists: {InventoryUI.Instance.name}");
            Debug.Log($"[Debug] Inventory is open: {InventoryUI.Instance.IsOpen}");
        }
        else
        {
            Debug.LogError("[Debug] InventoryUI.Instance is NULL!");
        }

        // Check InventoryPanel
        GameObject inventoryPanel = GameObject.Find("InventoryPanel");
        if (inventoryPanel != null)
        {
            Debug.Log($"[Debug] InventoryPanel found: {inventoryPanel.name}");
            Debug.Log($"[Debug] InventoryPanel active: {inventoryPanel.activeSelf}");

            // Check CanvasGroup
            CanvasGroup cg = inventoryPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log($"[Debug] CanvasGroup - Alpha: {cg.alpha}, Interactable: {cg.interactable}, BlocksRaycasts: {cg.blocksRaycasts}");
            }
            else
            {
                Debug.LogWarning("[Debug] No CanvasGroup on InventoryPanel!");
            }

            // Check Canvas
            Canvas canvas = inventoryPanel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"[Debug] Canvas found: {canvas.name}, RenderMode: {canvas.renderMode}, SortingOrder: {canvas.sortingOrder}");

                // Check GraphicRaycaster
                GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
                if (raycaster != null)
                {
                    Debug.Log($"[Debug] GraphicRaycaster enabled: {raycaster.enabled}");
                }
                else
                {
                    Debug.LogError("[Debug] No GraphicRaycaster on Canvas!");
                }
            }
            else
            {
                Debug.LogError("[Debug] No Canvas found!");
            }
        }
        else
        {
            Debug.LogError("[Debug] InventoryPanel not found!");
        }

        // Check for blocking UI
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        Debug.Log($"[Debug] Total canvases in scene: {allCanvases.Length}");
        foreach (Canvas c in allCanvases)
        {
            if (c.sortingOrder > 100) // Inventory is usually around 100
            {
                Debug.LogWarning($"[Debug] Canvas with higher sorting order: {c.name}, Order: {c.sortingOrder}");
            }
        }

        Debug.Log("=== END DEBUG ===");
    }

    void DebugClickPosition()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log($"[Click Debug] Clicked at {Input.mousePosition}, hit {results.Count} UI elements:");
            for (int i = 0; i < Mathf.Min(results.Count, 5); i++)
            {
                Debug.Log($"  [{i}] {results[i].gameObject.name} (Canvas: {results[i].gameObject.GetComponentInParent<Canvas>()?.name})");
            }
        }
        else
        {
            Debug.LogWarning($"[Click Debug] Clicked at {Input.mousePosition}, but hit NOTHING!");
        }
    }
}
