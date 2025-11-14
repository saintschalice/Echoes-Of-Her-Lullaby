using UnityEngine;
using System.Collections;

/// <summary>
/// Handles Emily's perception: vision cone, hearing, proximity detection
/// Optimized for mobile with coroutine-based checks
/// </summary>
public class EmilyPerception : MonoBehaviour
{
    [Header("Detection Radii by State")]
    public float patrolDetectionRadius = 3f;
    public float investigateDetectionRadius = 5f;
    public float huntDetectionRadius = 8f;
    public float searchDetectionRadius = 6f;
    public float cooldownDetectionRadius = 2.5f;

    [Header("Vision Cone")]
    public float visionAngle = 60f;
    public float visionRange = 6f;
    public int visionRayCount = 3;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Hearing")]
    public float hearingRadius = 10f;
    public float noiseDecayTime = 5f;

    [Header("Check Intervals")]
    public float visionCheckInterval = 0.1f;

    private EmilyAIController controller;
    public float currentDetectionRadius;
    public Vector3 lastKnownPlayerPosition;
    private bool isPlayerVisible;
    private float lastNoiseTime;
    private Coroutine visionCheckCoroutine;

    public void Initialize(EmilyAIController ctrl)
    {
        controller = ctrl;
        UpdateDetectionRadius();
        lastKnownPlayerPosition = Vector3.zero;
    }

    private void OnEnable()
    {
        if (controller == null)
            controller = GetComponent<EmilyAIController>();

        if(visionCheckCoroutine != null)
        StopCoroutine(visionCheckCoroutine);

        visionCheckCoroutine = StartCoroutine(VisionCheckRoutine());
    }


    private void OnDisable()
    {
        if (visionCheckCoroutine != null)
        {
            StopCoroutine(visionCheckCoroutine);
        }
    }

    public void UpdateDetectionRadius()
    {
        switch (controller.stateMachine.currentState)
        {
            case EmilyState.PATROL:
                currentDetectionRadius = patrolDetectionRadius;
                break;
            case EmilyState.INVESTIGATE:
                currentDetectionRadius = investigateDetectionRadius;
                break;
            case EmilyState.HUNT:
                currentDetectionRadius = huntDetectionRadius;
                break;
            case EmilyState.SEARCH:
                currentDetectionRadius = searchDetectionRadius;
                break;
            case EmilyState.COOLDOWN:
                currentDetectionRadius = cooldownDetectionRadius;
                break;
        }
    }

    IEnumerator VisionCheckRoutine()
    {
        while (true)
        {
            CheckVision();
            yield return new WaitForSeconds(visionCheckInterval);
        }
    }

    void CheckVision()
    {
        if (controller.player == null)
        {
            isPlayerVisible = false;
            return;
        }

        Vector2 toPlayer = controller.player.position - transform.position;
        float distance = toPlayer.magnitude;

        // Proximity check first
        if (distance > currentDetectionRadius)
        {
            isPlayerVisible = false;
            return;
        }

        // Vision cone angle check
        Vector2 forward = controller.movement.GetForwardDirection();
        float angle = Vector2.Angle(forward, toPlayer);

        if (angle > visionAngle / 2f)
        {
            isPlayerVisible = false;
            return;
        }

        // Raycast for line of sight
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            toPlayer.normalized,
            distance,
            obstacleMask | playerMask
        );

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isPlayerVisible = true;
            lastKnownPlayerPosition = controller.player.position;
        }
        else
        {
            isPlayerVisible = false;
        }
    }

    public bool IsPlayerVisible()
    {
        return isPlayerVisible;
    }

    public bool HasRecentNoise()
    {
        return (Time.time - lastNoiseTime) < noiseDecayTime;
    }

    public void OnNoiseHeard(Vector3 position, float strength)
    {
        float distance = Vector3.Distance(transform.position, position);

        if (distance <= hearingRadius * strength)
        {
            lastKnownPlayerPosition = position;
            lastNoiseTime = Time.time;
            controller.stateMachine.OnNoiseHeard(position);
            Debug.Log($"[EmilyAI] Noise heard at {position}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vision cone visualization
        Gizmos.color = Color.yellow;
        Vector2 forward = transform.up;
        float halfAngle = visionAngle / 2f;

        Vector2 leftBound = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Vector2 rightBound = Quaternion.Euler(0, 0, halfAngle) * forward;

        Gizmos.DrawRay(transform.position, leftBound * visionRange);
        Gizmos.DrawRay(transform.position, rightBound * visionRange);
        Gizmos.DrawWireSphere(transform.position, currentDetectionRadius);
    }
}