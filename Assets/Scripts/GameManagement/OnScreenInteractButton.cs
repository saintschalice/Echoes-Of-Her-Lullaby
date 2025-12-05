using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Binds a UI button to the player's interact action.
/// Simplified to ensure reliability without getting stuck in Disabled states.
/// </summary>
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
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
    private CanvasGroup canvasGroup;
    private Coroutine showCoroutine;

    // Allows external scripts (like IslandHide) to lock the button "On"
    private bool externalInteractionLock = false;

    private void Reset()
    {
        interactButton = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        SetupButtonNavigation();
    }

    private void Awake()
    {
        if (interactButton == null) interactButton = GetComponent<Button>();

        SetupButtonNavigation();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void SetupButtonNavigation()
    {
        // Disable navigation to prevent the button from holding "Selected" focus visually
        if (interactButton != null)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            interactButton.navigation = nav;
        }
    }

    private void OnEnable()
    {
        if (interactButton == null) interactButton = GetComponent<Button>();

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
        bool isDialogueOpen = DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive();

        if (isDialogueOpen)
        {
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            return;
        }
        else
        {
            if (canvasGroup.alpha == 0f && showCoroutine == null)
            {
                showCoroutine = StartCoroutine(ShowButtonDelayed());
            }
        }

        // Only update interactivity if fully visible
        if (canvasGroup.alpha > 0.9f)
        {
            // Safety: Ensure Raycasts are always allowed when visible
            if (!canvasGroup.blocksRaycasts) canvasGroup.blocksRaycasts = true;

            if (interactionTracker != null && interactButton != null)
            {
                bool hasFocus = interactionTracker.FocusedInteractable != null;

                // LOGIC: Active if Focus exists OR Debug is checked OR External Lock is active
                bool shouldBeInteractable = hasFocus || debugForceEnable || externalInteractionLock;

                // FIX: Check BOTH interactButton AND canvasGroup.
                // Previously, we only checked interactButton. If interactButton was ALREADY true (from before dialogue),
                // but canvasGroup was false (from being hidden), this check would fail, leaving the CanvasGroup disabled.
                if (interactButton.interactable != shouldBeInteractable || canvasGroup.interactable != shouldBeInteractable)
                {
                    interactButton.interactable = shouldBeInteractable;
                    canvasGroup.interactable = shouldBeInteractable;
                }
            }
            else if (interactionTracker == null)
            {
                // If no tracker assigned, always enabled
                if (interactButton.interactable == false)
                {
                    interactButton.interactable = true;
                    canvasGroup.interactable = true;
                }
            }
        }
    }

    private IEnumerator ShowButtonDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        showCoroutine = null;
    }

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        inputRouter = router;
    }

    public void TriggerInteract()
    {
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) return;

        // Guard: If logic says we shouldn't interact, abort.
        if (interactionTracker != null && interactionTracker.FocusedInteractable == null && !debugForceEnable && !externalInteractionLock)
        {
            return;
        }

        inputRouter?.TriggerInteract();

        // FIX: Simply clear the EventSystem selection.
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Called by Hiding/Interaction scripts to force the button to remain Interactable.
    /// </summary>
    public void SetInteractionLock(bool isLocked)
    {
        externalInteractionLock = isLocked;
    }
}