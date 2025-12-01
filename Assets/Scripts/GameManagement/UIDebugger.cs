using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDebugger : MonoBehaviour
{
    void Update()
    {
        // Detects mouse click or touch tap
        if (Input.GetMouseButtonDown(0))
        {
            CheckWhatIsBlocking();
        }
    }

    void CheckWhatIsBlocking()
    {
        // 1. Check if EventSystem exists
        if (EventSystem.current == null)
        {
            Debug.LogError("UIDebugger: NO EVENT SYSTEM FOUND! UI cannot work without one.");
            return;
        }

        // 2. Raycast into the UI manually to see what we hit
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // 3. Report results
        if (results.Count > 0)
        {
            Debug.Log($"--- UI CLICK DETECTED at {Input.mousePosition} ---");
            foreach (RaycastResult result in results)
            {
                // Print the name of everything under the mouse
                Debug.Log($"HIT: '{result.gameObject.name}' (Depth: {result.depth}, SortOrder: {result.sortingOrder})");
            }

            // The first item in the list is the one "stealing" the click
            Debug.Log($" >>> WINNER (The object receiving the click): {results[0].gameObject.name}");
        }
        else
        {
            Debug.Log("UIDebugger: Clicked, but hit NO UI elements.");
        }
    }
}