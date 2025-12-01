using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Tracks nearby interactables and forwards the Interact input to the focused target.
/// </summary>
public class PlayerInteractionTracker : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Vector2 detectionSize = new Vector2(1f, 2f);
    [SerializeField] private CapsuleDirection2D detectionDirection = CapsuleDirection2D.Vertical;
    [SerializeField] private float detectionAngle = 0f;
    [SerializeField] private Vector2 detectionOffset = Vector2.zero;

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
        Vector3 basePos = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Vector3 origin = basePos + (Vector3)detectionOffset;

        // CHANGED: Use OverlapCapsuleAll instead of OverlapCircleAll
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(origin, detectionSize, detectionDirection, detectionAngle, interactableLayers);

        nearbyInteractables.Clear();
        focusedInteractable = null;

        float closestDistance = float.MaxValue;
        foreach (Collider2D hit in hits)
        {
            MonoBehaviour interactable = FindInteractableOnCollider(hit);
            if (interactable == null)
                continue;

            nearbyInteractables.Add(interactable);

            // Calculate distance from the detection center (origin) to the interactable
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
        Vector3 basePos = detectionOrigin != null ? detectionOrigin.position : transform.position;
        Vector3 origin = basePos + (Vector3)detectionOffset;

        // Save original matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Apply rotation for the capsule angle
        Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.Euler(0, 0, detectionAngle), Vector3.one);

        // Draw a wire cube representing the bounds of the capsule (visual approximation)
        // Since Unity Gizmos doesn't have a 2D Capsule drawer, this shows the area covered.
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(detectionSize.x, detectionSize.y, 0.1f));

        // Restore matrix
        Gizmos.matrix = originalMatrix;
    }
}