using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(JoystickPlayerController))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 2.5f;
    [SerializeField] private LayerMask interactableLayers = ~0;
    [SerializeField] private Transform interactionOrigin;

    private InputSystem_Actions inputActions;
    private SimpleInteractable2D currentInteractable;
    private readonly Collider2D[] overlapResults = new Collider2D[8];
    private JoystickPlayerController playerController;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerController = GetComponent<JoystickPlayerController>();

        if (interactionOrigin == null)
        {
            interactionOrigin = transform;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void Update()
    {
        RefreshInteractable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EvaluateInteractable(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EvaluateInteractable(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SimpleInteractable2D interactable = other.GetComponentInParent<SimpleInteractable2D>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    private void EvaluateInteractable(Collider2D collider)
    {
        SimpleInteractable2D interactable = collider.GetComponentInParent<SimpleInteractable2D>();
        if (interactable == null || !interactable.interactable)
        {
            return;
        }

        float distance = Vector2.Distance(interactionOrigin.position, interactable.transform.position);
        float allowedRange = Mathf.Max(interactable.interactionRadius, detectionRadius);

        if (distance > allowedRange)
        {
            return;
        }

        if (currentInteractable == null)
        {
            currentInteractable = interactable;
            return;
        }

        float currentDistance = Vector2.Distance(interactionOrigin.position, currentInteractable.transform.position);
        if (distance < currentDistance)
        {
            currentInteractable = interactable;
        }
    }

    private void RefreshInteractable()
    {
        if (currentInteractable != null)
        {
            float allowedRange = Mathf.Max(currentInteractable.interactionRadius, detectionRadius);
            float currentDistance = Vector2.Distance(interactionOrigin.position, currentInteractable.transform.position);

            if (currentInteractable.interactable && currentDistance <= allowedRange)
            {
                return;
            }

            currentInteractable = null;
        }

        int count = Physics2D.OverlapCircleNonAlloc(interactionOrigin.position, detectionRadius, overlapResults, interactableLayers);
        SimpleInteractable2D closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            SimpleInteractable2D candidate = overlapResults[i].GetComponentInParent<SimpleInteractable2D>();
            if (candidate == null || !candidate.interactable)
            {
                continue;
            }

            float distance = Vector2.Distance(interactionOrigin.position, candidate.transform.position);
            float allowedRange = Mathf.Max(candidate.interactionRadius, detectionRadius);

            if (distance <= allowedRange && distance < closestDistance)
            {
                closest = candidate;
                closestDistance = distance;
            }
        }

        currentInteractable = closest;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (currentInteractable != null && currentInteractable.interactable)
        {
            currentInteractable.Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionOrigin == null)
        {
            interactionOrigin = transform;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactionOrigin.position, detectionRadius);
    }
}
