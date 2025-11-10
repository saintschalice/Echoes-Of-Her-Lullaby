using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Attach this to Emily temporarily to diagnose movement issues
/// Shows detailed debug info in Console
/// </summary>
public class EmilyMovementDebugger : MonoBehaviour
{
    private EmilyAIController controller;
    private EmilyMovement movement;
    private EmilyStateMachine stateMachine;
    private Rigidbody2D rb2D;

    private float logInterval = 1f;
    private float logTimer = 0f;

    void Start()
    {
        controller = GetComponent<EmilyAIController>();
        movement = GetComponent<EmilyMovement>();
        stateMachine = GetComponent<EmilyStateMachine>();
        rb2D = GetComponent<Rigidbody2D>();

        Debug.Log("========== EMILY MOVEMENT DEBUGGER STARTED ==========");
        LogInitialSetup();
    }

    void LogInitialSetup()
    {
        Debug.Log("=== INITIAL SETUP ===");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"EmilyAIController: {(controller != null ? "✓" : "✗")}");
        Debug.Log($"EmilyMovement: {(movement != null ? "✓" : "✗")}");
        Debug.Log($"EmilyStateMachine: {(stateMachine != null ? "✓" : "✗")}");
        Debug.Log($"Rigidbody2D: {(rb2D != null ? "✓" : "✗")}");

        if (controller != null)
        {
            Debug.Log($"Is Active: {controller.isActive}");
            Debug.Log($"Player Reference: {(controller.player != null ? controller.player.name : "NULL")}");
        }

        if (movement != null)
        {
            Debug.Log($"Patrol Area: Min({movement.patrolAreaMin}) Max({movement.patrolAreaMax})");
            Debug.Log($"Use 2D Movement: {movement.use2DMovement}");
        }

        if (rb2D != null)
        {
            Debug.Log($"Body Type: {rb2D.bodyType}");
            Debug.Log($"Gravity Scale: {rb2D.gravityScale}");
            Debug.Log($"Constraints: {rb2D.constraints}");
        }

        // Check NavMesh
        CheckNavMeshStatus();
    }

    void CheckNavMeshStatus()
    {
        NavMeshHit hit;
        bool onNavMesh = NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas);

        Debug.Log($"=== NAVMESH STATUS ===");
        Debug.Log($"On NavMesh: {(onNavMesh ? "✓ YES" : "✗ NO")}");

        if (onNavMesh)
        {
            Debug.Log($"NavMesh Position: {hit.position}");
            Debug.Log($"Distance to NavMesh: {hit.distance}");
        }
        else
        {
            Debug.LogError("Emily is NOT on NavMesh! She cannot move!");

            // Try to find nearest NavMesh
            if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                Debug.LogWarning($"Nearest NavMesh found at: {hit.position} (distance: {hit.distance})");
            }
        }

        // Check if NavMesh exists at all
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Debug.Log($"Total NavMesh vertices in scene: {triangulation.vertices.Length}");

        if (triangulation.vertices.Length == 0)
        {
            Debug.LogError("NO NAVMESH FOUND IN SCENE! Did you bake the NavMesh?");
        }
    }

    void Update()
    {
        logTimer += Time.deltaTime;

        if (logTimer >= logInterval)
        {
            logTimer = 0f;
            LogCurrentStatus();
        }
    }

    void LogCurrentStatus()
    {
        Debug.Log("========== EMILY STATUS ==========");
        Debug.Log($"Time: {Time.time:F1}s");
        Debug.Log($"Position: {transform.position}");

        // AI State
        if (controller != null && stateMachine != null)
        {
            Debug.Log($"AI Active: {controller.isActive}");
            Debug.Log($"Current State: {stateMachine.currentState}");
        }

        // Movement
        if (movement != null)
        {
            Debug.Log($"Is Moving: {movement.IsMoving()}");
            Debug.Log($"Current Speed: {movement.GetCurrentSpeed():F2}");
            Debug.Log($"Distance to Destination: {movement.GetDistanceToDestination():F2}");
            Debug.Log($"Has Reached Destination: {movement.HasReachedDestination()}");
        }

        // Physics
        if (rb2D != null)
        {
            Debug.Log($"Linear Velocity: {rb2D.linearVelocity}");
            Debug.Log($"Is Kinematic: {rb2D.isKinematic}");
            Debug.Log($"Is Sleeping: {rb2D.IsSleeping()}");
        }

        // Player tracking
        if (controller != null && controller.player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, controller.player.position);
            Debug.Log($"Distance to Player: {distanceToPlayer:F2}");
        }

        Debug.Log("==================================");
    }

    // Manual test buttons
    [ContextMenu("Force Random Destination")]
    void ForceRandomDestination()
    {
        if (movement != null)
        {
            movement.SetRandomDestination();
            Debug.Log("✓ Forced random destination");
        }
    }

    [ContextMenu("Check NavMesh Now")]
    void CheckNavMeshNow()
    {
        CheckNavMeshStatus();
    }

    [ContextMenu("Force Movement Test")]
    void ForceMovementTest()
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.right * 2f;
            Debug.Log("✓ Forced movement right at 2 units/sec");
        }
    }

    void OnDrawGizmos()
    {
        // Draw patrol bounds
        if (movement != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new Vector3(
                (movement.patrolAreaMin.x + movement.patrolAreaMax.x) / 2,
                (movement.patrolAreaMin.y + movement.patrolAreaMax.y) / 2,
                0
            );
            Vector3 size = new Vector3(
                movement.patrolAreaMax.x - movement.patrolAreaMin.x,
                movement.patrolAreaMax.y - movement.patrolAreaMin.y,
                1
            );
            Gizmos.DrawWireCube(center, size);
        }

        // Draw position marker
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2);
    }
}