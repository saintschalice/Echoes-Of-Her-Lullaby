using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple test button to start Mirror 3 puzzle
/// Attach to a UI Button for testing
/// </summary>
public class Mirror3_TestButton : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("[Mirror3_TestButton] Test button ready!");
        }
    }
    
    void OnButtonClick()
    {
        Debug.Log("[Mirror3_TestButton] ===== TEST BUTTON CLICKED =====");
        
        Mirror3_VanityTerror mirror3 = FindObjectOfType<Mirror3_VanityTerror>();
        
        if (mirror3 != null)
        {
            Debug.Log("[Mirror3_TestButton] Found Mirror3_VanityTerror, calling StartPuzzle()");
            mirror3.StartPuzzle();
        }
        else
        {
            Debug.LogError("[Mirror3_TestButton] ❌ Mirror3_VanityTerror NOT FOUND!");
            Debug.LogError("[Mirror3_TestButton] Make sure there's a GameObject with Mirror3_VanityTerror component in the scene!");
        }
    }
}
