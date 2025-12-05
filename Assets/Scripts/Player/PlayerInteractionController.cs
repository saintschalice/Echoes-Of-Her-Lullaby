using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks interactables near the player and routes the interact input to the
/// closest target.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionRadius = 2.25f;
    public LayerMask interactableLayers = ~0;
    public KeyCode interactKey = KeyCode.E;

    [Header("Debug")]
    public bool drawRadius = false;

    private readonly HashSet<IInteractable> nearby = new HashSet<IInteractable>();
    private readonly List<IInteractable> buffer = new List<IInteractable>();
    private Collider2D[] overlapBuffer = new Collider2D[16];
    private PlayerContext context;

    void Awake()
    {
        context = new PlayerContext(gameObject);
    }

    void Update()
    {
        RefreshNearbyInteractables();

        if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("Submit"))
        {
            InteractWithClosest();
        }
    }

    void RefreshNearbyInteractables()
    {
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, overlapBuffer, interactableLayers);

        buffer.Clear();
        for (int i = 0; i < hits; i++)
        {
            Collider2D col = overlapBuffer[i];
            if (col == null) continue;

            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable != null && !buffer.Contains(interactable))
            {
                buffer.Add(interactable);
                nearby.Add(interactable);
                interactable.OnFocus(context);
            }
        }

        // Remove interactables that are no longer nearby
        var removals = new List<IInteractable>();
        foreach (var interactable in nearby)
        {
            if (!buffer.Contains(interactable))
            {
                removals.Add(interactable);
            }
        }

        foreach (var interactable in removals)
        {
            nearby.Remove(interactable);
            interactable.OnBlur(context);
        }
    }

    void InteractWithClosest()
    {
        if (nearby.Count == 0) return;

        IInteractable closest = null;
        float closestDist = float.MaxValue;

        foreach (var interactable in nearby)
        {
            var interactableBehaviour = interactable as MonoBehaviour;
            if (interactableBehaviour == null) continue;

            float dist = Vector2.Distance(transform.position, interactableBehaviour.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = interactable;
            }
        }

        closest?.OnInteract(context);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawRadius) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
