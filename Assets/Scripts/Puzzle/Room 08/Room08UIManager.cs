using UnityEngine;

/// <summary>
/// UI Manager for Room 08 - Lisa's Bathroom
/// Manages mirror panel display
/// </summary>
public class Room08UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mirrorPanel; // Panel with mirror tap puzzle

    private void Start()
    {
        // Hide all panels at start
        HideAllPanels();
    }

    public void HideAllPanels()
    {
        if (mirrorPanel != null) mirrorPanel.SetActive(false);
    }

    public void ShowMirrorPanel()
    {
        HideAllPanels();
        
        if (mirrorPanel != null)
        {
            mirrorPanel.SetActive(true);
            Debug.Log("[Room08] Showing Mirror Panel");
            
            // Start the QTE
            Room08_MirrorQTE qte = mirrorPanel.GetComponentInChildren<Room08_MirrorQTE>();
            if (qte != null)
            {
                qte.StartQTE();
            }
            else
            {
                Debug.LogError("[Room08] Room08_MirrorQTE not found in mirror panel!");
            }
        }
        else
        {
            Debug.LogError("[Room08] Mirror Panel is NULL! Assign it in Room08UIManager.");
        }
    }

    // Called by Room08_MirrorQTE when puzzle is complete
    public void OnMirrorPuzzleComplete()
    {
        HideAllPanels();
        
        // Re-enable mirror interactable so player can climb through
        Room08_Interactable[] interactables = FindObjectsByType<Room08_Interactable>(FindObjectsSortMode.None);
        foreach (Room08_Interactable interactable in interactables)
        {
            if (interactable.myType == Room08_Interactable.ObjectType.Mirror)
            {
                interactable.enabled = true;
                Debug.Log("[Room08] Re-enabled mirror interactable for passage");
                break;
            }
        }
        
        // Notify flow controller
        Room08_FlowController.Instance?.OnMirrorBroken();
    }
}
