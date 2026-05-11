using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple test script to debug button clicks
/// Attach this to DrainCover_Button to test if it's receiving clicks
/// </summary>
public class TestButtonClick : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        
        if (button != null)
        {
            Debug.Log("[TestButton] Button component found!");
            Debug.Log($"[TestButton] Interactable: {button.interactable}");
            
            // Add listener
            button.onClick.AddListener(OnButtonClicked);
            Debug.Log("[TestButton] Listener added!");
        }
        else
        {
            Debug.LogError("[TestButton] No Button component found!");
        }
        
        // Check Image component
        Image img = GetComponent<Image>();
        if (img != null)
        {
            Debug.Log($"[TestButton] Image found. Raycast Target: {img.raycastTarget}");
        }
        else
        {
            Debug.LogError("[TestButton] No Image component found!");
        }
    }
    
    void OnButtonClicked()
    {
        Debug.Log("========================================");
        Debug.Log("[TestButton] ✅ BUTTON CLICKED!");
        Debug.Log("========================================");
    }
    
    void OnMouseDown()
    {
        Debug.Log("[TestButton] OnMouseDown detected!");
    }
    
    void OnMouseEnter()
    {
        Debug.Log("[TestButton] Mouse entered button area!");
    }
}
