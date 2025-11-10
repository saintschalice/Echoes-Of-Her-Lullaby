using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement controller for Emily
/// Uses NavMesh path calculation with 2D Rigidbody2D movement
/// UPDATED: Works with Unity 6.2 - Uses linearVelocity instead of velocity
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EmilyMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    public float patrolSpeed = 1.5f;
    public float investigateSpeed = 2.5f;
    public float huntSpeed = 4f;
    public float searchSpeed = 2f;
    public float cooldownSpeed = 1f;

    [Header("Navigation")]
    public float pathUpdateInterval = 0.3f;
    public float reachedThreshold = 0.5f;
    public float catchRadius = 0.8f;
    public float cornerReachThreshold = 0.3f;

    [Header("Patrol Bounds")]
    public Vector2 patrolAreaMin = new Vector2(-10, -10);
    public Vector2 patrolAreaMax = new Vector2(10, 10);

    [Header("2D Movement")]
    public bool use2DMovement = true; // Toggle for 2D vs 3D NavMesh

    private EmilyAIController controller;
    private Rigidbody2D rb2D;
    private float pathUpdateTimer;
    private Vector3 currentDestination;
    private bool useDirectPursuit;

    // 2D NavMesh path following
    private NavMeshPath navPath;
    private int currentPathCorner = 0;
    private float currentSpeed;

    public void Initialize(EmilyAIController ctrl)
    {
        controller = ctrl;
        rb2D = GetComponent<Rigidbody2D>();
        navPath = new NavMeshPath();
        ConfigureRigidbody2D();
    }

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        navPath = new NavMeshPath();
    }

    void ConfigureRigidbody2D()
    {
        if (rb2D == null) return;

        rb2D.bodyType = RigidbodyType2D.Kinematic; // << prevents physical pushes
        rb2D.gravityScale = 0f;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
    }


    private void Update()
    {
        pathUpdateTimer += Time.deltaTime;

        // Update path periodically
        if (pathUpdateTimer >= pathUpdateInterval && !useDirectPursuit)
        {
            pathUpdateTimer = 0f;
            if (currentDestination != Vector3.zero)
            {
                CalculatePath(currentDestination);
            }
        }
    }

    private void FixedUpdate()
    {
        if (use2DMovement)
        {
            FollowPath2D();
        }
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
        useDirectPursuit = false;
        CalculatePath(destination);
    }

    void CalculatePath(Vector3 destination)
    {
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navPath))
        {
            if (navPath.status == NavMeshPathStatus.PathComplete)
            {
                currentPathCorner = 0;

                if (controller != null && controller.debugMode)
                {
                    Debug.Log($"[EmilyMovement] Path calculated with {navPath.corners.Length} corners");
                }
            }
            else
            {
                if (controller != null && controller.debugMode)
                {
                    Debug.LogWarning($"[EmilyMovement] Path status: {navPath.status}");
                }
            }
        }
    }

    void FollowPath2D()
    {
        if (useDirectPursuit)
        {
            // Direct pursuit for HUNT state
            PursueDirect(currentDestination);
            return;
        }

        if (navPath == null || navPath.corners.Length == 0)
        {
            rb2D.linearVelocity = Vector2.zero;
            return;
        }

        // Check if we've reached the end of the path
        if (currentPathCorner >= navPath.corners.Length)
        {
            rb2D.linearVelocity = Vector2.zero;
            return;
        }

        // Get the current target corner
        Vector3 targetCorner = navPath.corners[currentPathCorner];
        Vector2 direction = ((Vector2)targetCorner - (Vector2)transform.position).normalized;
        float distanceToCorner = Vector2.Distance(transform.position, targetCorner);

        // Move towards the corner
        if (distanceToCorner > cornerReachThreshold)
        {
            rb2D.linearVelocity = direction * currentSpeed;
        }
        else
        {
            // Reached this corner, move to the next one
            currentPathCorner++;

            if (currentPathCorner >= navPath.corners.Length)
            {
                rb2D.linearVelocity = Vector2.zero;
            }
        }
    }

    public void SetRandomDestination()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(patrolAreaMin.x, patrolAreaMax.x),
            Random.Range(patrolAreaMin.y, patrolAreaMax.y),
            0
        );
        SetDestination(randomPoint);
    }

    public void PursueDirect(Vector3 targetPosition)
    {
        if (rb2D == null) return;

        // Direct steering for HUNT state
        useDirectPursuit = true;
        currentDestination = targetPosition;

        Vector2 direction = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
        rb2D.linearVelocity = direction * currentSpeed;
    }

    public bool HasReachedDestination()
    {
        if (useDirectPursuit) return false;

        if (navPath == null || navPath.corners.Length == 0)
            return true;

        // Check if we've reached the final corner
        if (currentPathCorner >= navPath.corners.Length)
            return true;

        // Check distance to final destination
        float distanceToDestination = Vector2.Distance(transform.position, currentDestination);
        return distanceToDestination <= reachedThreshold;
    }

    public Vector2 GetForwardDirection()
    {
        if (rb2D == null) return Vector2.up;

        if (rb2D.linearVelocity.magnitude > 0.1f)
        {
            return rb2D.linearVelocity.normalized;
        }

        return Vector2.down; // Default facing direction
    }

    public void ResetNavigation()
    {
        if (navPath != null)
        {
            navPath.ClearCorners();
        }
        currentPathCorner = 0;
        useDirectPursuit = false;
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Get current movement speed
    /// </summary>
    public float GetCurrentSpeed()
    {
        if (rb2D == null) return 0f;
        return rb2D.linearVelocity.magnitude;
    }

    /// <summary>
    /// Check if agent is currently moving
    /// </summary>
    public bool IsMoving()
    {
        if (rb2D == null) return false;
        return rb2D.linearVelocity.magnitude > 0.1f;
    }

    /// <summary>
    /// Get distance to current destination
    /// </summary>
    public float GetDistanceToDestination()
    {
        if (currentDestination == Vector3.zero)
            return float.MaxValue;

        return Vector2.Distance(transform.position, currentDestination);
    }

    private void OnDrawGizmos()
    {
        if (controller != null && controller.debugMode && navPath != null && navPath.corners.Length > 0)
        {
            // Draw the path
            Gizmos.color = Color.yellow;
            for (int i = 0; i < navPath.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(navPath.corners[i], navPath.corners[i + 1]);
            }

            // Draw current target corner
            if (currentPathCorner < navPath.corners.Length)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(navPath.corners[currentPathCorner], 0.3f);
            }

            // Draw destination
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentDestination, 0.5f);
        }

        // Draw patrol bounds
        if (controller != null && controller.debugMode)
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new Vector3(
                (patrolAreaMin.x + patrolAreaMax.x) / 2,
                (patrolAreaMin.y + patrolAreaMax.y) / 2,
                0
            );
            Vector3 size = new Vector3(
                patrolAreaMax.x - patrolAreaMin.x,
                patrolAreaMax.y - patrolAreaMin.y,
                1
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}