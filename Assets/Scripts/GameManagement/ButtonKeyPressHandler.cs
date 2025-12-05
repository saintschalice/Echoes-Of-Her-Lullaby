using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonKeyPressHandler : MonoBehaviour
{
    private Button button;
    private PlayerInputRouter inputRouter;

    void OnEnable()
    {
        // Get the Button component attached to this GameObject.
        button = GetComponent<Button>();

        PlayerInputRouter.OnInstanceChanged += HandleInputRouterChanged;
        HandleInputRouterChanged(PlayerInputRouter.Instance);
    }

    void OnDisable()
    {
        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;

        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= HandleInteractPerformed;
        }
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

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= HandleInteractPerformed;
        }

        inputRouter = router;

        if (inputRouter != null)
        {
            inputRouter.InteractPerformed += HandleInteractPerformed;
        }
    }

    private void HandleInteractPerformed()
    {
        button?.onClick?.Invoke();
    }
}