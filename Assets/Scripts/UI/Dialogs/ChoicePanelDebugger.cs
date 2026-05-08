using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Attach this to the ChoicePanel to debug why buttons aren't clickable
/// </summary>
public class ChoicePanelDebugger : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("[ChoiceDebug] ChoicePanel enabled");
        CheckUISetup();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckRaycast();
        }
    }

    void CheckUISetup()
    {
        // Check EventSystem
        if (EventSystem.current == null)
        {
            Debug.LogError("[ChoiceDebug] NO EVENT SYSTEM FOUND! UI cannot work without one.");
        }
        else
        {
            Debug.Log($"[ChoiceDebug] EventSystem found: {EventSystem.current.name}");
        }

        // Check Canvas
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[ChoiceDebug] No Canvas found in parent!");
        }
        else
        {
            Debug.Log($"[ChoiceDebug] Canvas found: {canvas.name}, RenderMode: {canvas.renderMode}");
            
            // Check GraphicRaycaster
            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null)
            {
                Debug.LogError("[ChoiceDebug] No GraphicRaycaster on Canvas!");
            }
            else
            {
                Debug.Log($"[ChoiceDebug] GraphicRaycaster found, enabled: {raycaster.enabled}");
            }
        }

        // Check CanvasGroup
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            Debug.Log($"[ChoiceDebug] CanvasGroup - alpha: {cg.alpha}, interactable: {cg.interactable}, blocksRaycasts: {cg.blocksRaycasts}");
        }

        // Check buttons
        Button[] buttons = GetComponentsInChildren<Button>(true);
        Debug.Log($"[ChoiceDebug] Found {buttons.Length} buttons");
        foreach (Button btn in buttons)
        {
            Debug.Log($"[ChoiceDebug] Button: {btn.name}, interactable: {btn.interactable}, active: {btn.gameObject.activeSelf}");
        }
    }

    void CheckRaycast()
    {
        if (EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count == 0)
        {
            Debug.LogWarning("[ChoiceDebug] No UI elements hit by raycast!");
        }
        else
        {
            Debug.Log($"[ChoiceDebug] Raycast hit {results.Count} elements:");
            foreach (RaycastResult result in results)
            {
                Debug.Log($"  - {result.gameObject.name} (sortOrder: {result.sortingOrder})");
            }
        }
    }
}
