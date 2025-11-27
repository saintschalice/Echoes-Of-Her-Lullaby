using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour
{
    private PlayerInputRouter inputRouter;

    void OnEnable()
    {
        PlayerInputRouter.OnInstanceChanged += HandleInputRouterChanged;
        HandleInputRouterChanged(PlayerInputRouter.Instance);
    }

    void OnDisable()
    {
        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Only check on the first frame of touch
            if (touch.phase == TouchPhase.Began)
            {
                // Check if touching UI
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("Touched UI, ignoring game objects");
                    return;
                }

                // If we get here, we're NOT touching UI
                // Do your raycast for game objects
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    Debug.Log("Touched: " + hit.collider.gameObject.name);
                    // Your game object touch logic here
                }

                inputRouter?.TriggerInteract();
            }
        }
    }

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        inputRouter = router;
    }
}