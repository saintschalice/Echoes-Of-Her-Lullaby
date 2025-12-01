using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Binds a UI button to the player's interact action so mobile/on-screen buttons
/// can trigger world interactions without enabling menu inputs.
/// </summary>
[RequireComponent(typeof(Button))]
public class OnScreenInteractButton : MonoBehaviour
{
    [Tooltip("Button used to trigger the interact action.")]
    [SerializeField] private Button interactButton;

    [Tooltip("Optional tracker to disable the button when no interactable is in focus.")]
    [SerializeField] private PlayerInteractionTracker interactionTracker;

    [Header("Debug")]
    [Tooltip("Check this to force the button to be clickable, even if no object is found.")]
    [SerializeField] private bool debugForceEnable = false;

    private PlayerInputRouter inputRouter;
    private bool lastInteractableState = true; // For debug logging only

    private void Reset()
    {
        interactButton = GetComponent<Button>();
    }

    private void Awake()
    {
        if (interactButton == null)
        {
            interactButton = GetComponent<Button>();
        }
    }

    private void OnEnable()
    {
        if (interactButton == null)
        {
            interactButton = GetComponent<Button>();
        }

        // Clean up: Remove the listener first so we don't stack them up if enabled/disabled multiple times
        interactButton.onClick.RemoveListener(TriggerInteract);
        interactButton.onClick.AddListener(TriggerInteract);

        PlayerInputRouter.OnInstanceChanged += HandleInputRouterChanged;
        HandleInputRouterChanged(PlayerInputRouter.Instance);
    }

    private void OnDisable()
    {
        if (interactButton != null)
            interactButton.onClick.RemoveListener(TriggerInteract);

        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;
    }

    private void Update()
    {
        if (interactionTracker != null && interactButton != null)
        {
            // LOGIC: Enable button if we found a target OR if debug mode is on
            bool hasFocus = interactionTracker.FocusedInteractable != null;
            bool shouldBeInteractable = hasFocus || debugForceEnable;

            if (interactButton.interactable != shouldBeInteractable)
            {
                interactButton.interactable = shouldBeInteractable;

                string status = shouldBeInteractable ? "ENABLED" : "DISABLED (No Target)";
                Debug.Log($"[OnScreenInteractButton] State updated to: {status}");
            }
        }
        else if (interactionTracker == null)
        {
            // If no tracker is assigned, default to enabled so the button isn't broken
            if (interactButton.interactable == false)
            {
                interactButton.interactable = true;
                Debug.LogWarning("[OnScreenInteractButton] No InteractionTracker assigned. Button forcing to ENABLED.");
            }
        }
    }

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        inputRouter = router;
    }

    public void TriggerInteract()
    {
        Debug.Log("[OnScreenInteractButton] Button Clicked!");

        if (interactionTracker != null)
        {
            if (interactionTracker.FocusedInteractable == null)
            {
                // If we forced the button on, we will hit this warning.
                // This confirms the button works, but the Tracker is blind.
                Debug.LogWarning("[OnScreenInteractButton] Clicked, but Tracker says nothing is focused. Please check Colliders on the object.");
                return;
            }
            else
            {
                Debug.Log($"[OnScreenInteractButton] Target confirm: {interactionTracker.FocusedInteractable.name}");
            }
        }

        if (inputRouter == null)
        {
            Debug.LogError("[OnScreenInteractButton] CRITICAL: PlayerInputRouter is missing!");
            return;
        }

        inputRouter?.TriggerInteract();
    }
}