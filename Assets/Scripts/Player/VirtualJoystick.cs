using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Joystick Components")]
    public RectTransform handle;
    public RectTransform background;

    [Header("Settings")]
    public float handleRange = 50f;
    public bool snapX = false;
    public bool snapY = false;

    [Header("Output")]
    public Vector2 inputVector = Vector2.zero;

    private Vector2 backgroundCenter;
    private Canvas canvas;
    private Camera cam;

    void Start()
    {
        // Get components
        canvas = GetComponentInParent<Canvas>();

        // Set background reference if not assigned
        if (background == null)
            background = GetComponent<RectTransform>();

        // Set handle reference if not assigned
        if (handle == null)
            handle = transform.GetChild(0).GetComponent<RectTransform>();

        // Set handle range based on background size if not set
        if (handleRange == 50f)
            handleRange = background.sizeDelta.x / 2f - handle.sizeDelta.x / 2f;

        // Get camera for screen space conversion
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;

        inputVector = (eventData.position - position) / (radius * canvas.scaleFactor);

        // Clamp input to unit circle
        if (inputVector.magnitude > 1f)
        {
            inputVector = inputVector.normalized;
        }

        // Apply snapping if enabled
        if (snapX)
        {
            if (inputVector.x > 0)
                inputVector.x = 1;
            else if (inputVector.x < 0)
                inputVector.x = -1;
            else
                inputVector.x = 0;
        }

        if (snapY)
        {
            if (inputVector.y > 0)
                inputVector.y = 1;
            else if (inputVector.y < 0)
                inputVector.y = -1;
            else
                inputVector.y = 0;
        }

        // Move handle
        handle.anchoredPosition = new Vector2(
            inputVector.x * handleRange,
            inputVector.y * handleRange
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
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
}