using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Test what UI elements are being hit when clicking on inventory.
/// Attach to any GameObject and click on inventory items.
/// </summary>
public class InventoryRaycastTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestRaycast();
        }
    }

    void TestRaycast()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        Debug.Log($"=== RAYCAST TEST at {Input.mousePosition} ===");
        Debug.Log($"Total hits: {results.Count}");

        if (results.Count == 0)
        {
            Debug.LogError("NO UI ELEMENTS HIT! Possible causes:");
            Debug.LogError("1. EventSystem disabled");
            Debug.LogError("2. GraphicRaycaster missing");
            Debug.LogError("3. CanvasGroup blocksRaycasts = false");
        }
        else
        {
            for (int i = 0; i < Mathf.Min(results.Count, 10); i++)
            {
                RaycastResult result = results[i];
                Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
                string canvasName = canvas != null ? canvas.name : "NO CANVAS";
                int sortOrder = canvas != null ? canvas.sortingOrder : -1;

                Debug.Log($"[{i}] {result.gameObject.name}");
                Debug.Log($"    Canvas: {canvasName}, SortOrder: {sortOrder}");
                Debug.Log($"    Layer: {LayerMask.LayerToName(result.gameObject.layer)}");

                // Check if this is blocking inventory
                if (result.gameObject.name.Contains("Recipe") || 
                    result.gameObject.name.Contains("Book") ||
                    canvasName.Contains("Recipe"))
                {
                    Debug.LogWarning($"    ⚠️ BLOCKING ELEMENT: {result.gameObject.name}");
                    Debug.LogWarning($"    This is blocking inventory clicks!");
                    
                    // Check CanvasGroup
                    CanvasGroup cg = result.gameObject.GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        Debug.LogWarning($"    CanvasGroup: alpha={cg.alpha}, blocksRaycasts={cg.blocksRaycasts}");
                    }
                }
            }
        }

        Debug.Log("=== END RAYCAST TEST ===");
    }

    [ContextMenu("Test Current Mouse Position")]
    void TestCurrentPosition()
    {
        TestRaycast();
    }
}
