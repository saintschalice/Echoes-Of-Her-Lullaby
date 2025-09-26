using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonKeyPressHandler : MonoBehaviour
{
    private Button button;

    void Start()
    {
        // Get the Button component attached to this GameObject.
        button = GetComponent<Button>();
    }

    void Update()
    {
        // Check if the 'G' key is pressed down.
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Programmatically trigger the button's OnClick() event.
            button.onClick.Invoke();
        }
    }
}