using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("D-Pad Button Areas - Assign These!")]
    [Tooltip("Drag the 4 button GameObjects here - they define the detection zones")]
    public RectTransform upButton;
    public RectTransform downButton;
    public RectTransform leftButton;
    public RectTransform rightButton;

    [Header("Output")]
    public Vector2 inputVector = Vector2.zero;

    private Canvas canvas;
    private Camera cam;
    private bool isPressed = false;
    private PlayerInputRouter inputRouter;

    public event Action InteractPressed;

    private void OnEnable()
    {
        PlayerInputRouter.OnInstanceChanged += HandleInputRouterChanged;
        HandleInputRouterChanged(PlayerInputRouter.Instance);
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("VirtualJoystick: No Canvas found in parent!");
            return;
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        // Make sure this GameObject can receive touch events
        Image img = GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("VirtualJoystick: Add an Image component to this GameObject!");
        }
        else
        {
            img.raycastTarget = true;
        }

        // Disable raycasting on button areas (we'll detect them manually)
        DisableButtonRaycast(upButton);
        DisableButtonRaycast(downButton);
        DisableButtonRaycast(leftButton);
        DisableButtonRaycast(rightButton);

        Debug.Log("VirtualJoystick: Single-touch D-pad initialized!");
    }

    private void OnDisable()
    {
        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= OnInteractAction;
        }

        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;
    }

    private void OnDestroy()
    {
        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;
    }

    void DisableButtonRaycast(RectTransform button)
    {
        if (button != null)
        {
            Image img = button.GetComponent<Image>();
            if (img != null)
                img.raycastTarget = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        UpdateDirection(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPressed)
            UpdateDirection(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        inputVector = Vector2.zero;
        Debug.Log("Released - stopped moving");
    }

    public void OnInteractButtonPressed()
    {
        OnInteractAction();
    }

    void UpdateDirection(PointerEventData eventData)
    {
        // Check which button area the touch is over
        Vector2 direction = Vector2.zero;
        string buttonName = "none";

        if (upButton != null && RectTransformUtility.RectangleContainsScreenPoint(upButton, eventData.position, cam))
        {
            direction = Vector2.up;
            buttonName = "UP";
        }
        else if (downButton != null && RectTransformUtility.RectangleContainsScreenPoint(downButton, eventData.position, cam))
        {
            direction = Vector2.down;
            buttonName = "DOWN";
        }
        else if (leftButton != null && RectTransformUtility.RectangleContainsScreenPoint(leftButton, eventData.position, cam))
        {
            direction = Vector2.left;
            buttonName = "LEFT";
        }
        else if (rightButton != null && RectTransformUtility.RectangleContainsScreenPoint(rightButton, eventData.position, cam))
        {
            direction = Vector2.right;
            buttonName = "RIGHT";
        }

        // Only update if direction changed
        if (inputVector != direction)
        {
            inputVector = direction;
            if (direction != Vector2.zero)
            {
                Debug.Log($"Moving {buttonName}: {direction}");
            }
            else
            {
                Debug.Log("Moved to center/gap - stopped");
            }
        }
    }

    // Public methods to get input
    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }

    public Vector2 Direction()
    {
        return inputVector;
    }

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= OnInteractAction;
        }

        inputRouter = router;

        if (inputRouter != null)
        {
            inputRouter.InteractPerformed += OnInteractAction;
        }
    }

    private void OnInteractAction()
    {
        InteractPressed?.Invoke();
    }
}