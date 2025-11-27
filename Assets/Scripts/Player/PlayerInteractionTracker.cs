using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Tracks nearby interactables and forwards the Interact input to the focused target.
/// </summary>
public class PlayerInteractionTracker : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2.5f;
    [SerializeField] private LayerMask interactableLayers = ~0;
    [SerializeField] private Transform detectionOrigin;

    private readonly List<MonoBehaviour> nearbyInteractables = new();
    private MonoBehaviour focusedInteractable;
    private JoystickPlayerController playerController;

    public MonoBehaviour FocusedInteractable => focusedInteractable;

    private void Awake()
    {
        playerController = GetComponent<JoystickPlayerController>();
    }

    private void OnEnable()
    {
        if (playerController == null)
            playerController = GetComponent<JoystickPlayerController>();

        if (playerController != null)
        {
            playerController.InteractPerformed += HandleInteractInput;
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.InteractPerformed -= HandleInteractInput;
        }
    }

    private void Update()
    {
        RefreshNearbyInteractables();
    }

    private void RefreshNearbyInteractables()
    {
        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, detectionRadius, interactableLayers);

        nearbyInteractables.Clear();
        focusedInteractable = null;

        float closestDistance = float.MaxValue;
        foreach (Collider2D hit in hits)
        {
            MonoBehaviour interactable = FindInteractableOnCollider(hit);
            if (interactable == null)
                continue;

            nearbyInteractables.Add(interactable);

            float distance = Vector2.Distance(origin, interactable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                focusedInteractable = interactable;
            }
        }
    }

    private MonoBehaviour FindInteractableOnCollider(Collider2D hit)
    {
        MonoBehaviour[] behaviours = hit.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour == null)
                continue;

            if (HasInteractMethod(behaviour))
            {
                return behaviour;
            }
        }

        return null;
    }

    private static bool HasInteractMethod(MonoBehaviour behaviour)
    {
        MethodInfo methodInfo = behaviour.GetType().GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return methodInfo != null && methodInfo.GetParameters().Length == 0;
    }

    private void HandleInteractInput()
    {
        if (focusedInteractable == null)
            return;

        MethodInfo methodInfo = focusedInteractable.GetType().GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        methodInfo?.Invoke(focusedInteractable, null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Gizmos.DrawWireSphere(origin, detectionRadius);
    }
}
