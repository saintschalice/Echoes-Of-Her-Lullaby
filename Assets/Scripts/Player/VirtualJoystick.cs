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
    public bool force4Directional = true; // NEW: Force 4-directional movement

    [Header("Output")]
    public Vector2 inputVector = Vector2.zero;

    private Vector2 backgroundCenter;
    private Canvas canvas;
    private Camera cam;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        if (background == null)
            background = GetComponent<RectTransform>();

        if (handle == null)
            handle = transform.GetChild(0).GetComponent<RectTransform>();

        if (handleRange == 50f)
            handleRange = background.sizeDelta.x / 2f - handle.sizeDelta.x / 2f;

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

        // NEW: Force 4-directional movement (no diagonals)
        if (force4Directional)
        {
            // Determine which direction is dominant
            if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
            {
                // Horizontal movement is dominant
                inputVector.y = 0;
                inputVector.x = inputVector.x > 0 ? 1 : -1;
            }
            else
            {
                // Vertical movement is dominant
                inputVector.x = 0;
                inputVector.y = inputVector.y > 0 ? 1 : -1;
            }
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