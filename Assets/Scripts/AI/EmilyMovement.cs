using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public sealed class EmilyMovement : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true, movement is locked to Up, Down, Left, Right only (GameBoy style).")]
    public bool useFourDirections = true;

    [Tooltip("How long (in seconds) to lock a direction before allowing a switch. Prevents rapid zig-zagging.")]
    public float directionLockTime = 0.2f;

    [Tooltip("Distance at which Emily stops moving completely to prevent arrival jitter.")]
    public float stopDistance = 0.2f;

    Rigidbody2D _rb;
    NavMeshAgent _agent;

    bool _directPursuit = false;
    float _directSpeed = 0f;
    Vector3 _directTarget;

    Vector2 _wanderMin = new(-11, -3);
    Vector2 _wanderMax = new(11, 3);
    readonly System.Random _rng = new();

    // HYSTERESIS STATE
    bool _lastAxisWasHorizontal = true;
    float _timeSinceDirectionChange = 0f;
    Vector2 _lastNonZeroDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        // CRITICAL: Prevent Agent from overriding Rigidbody position
        _agent.updatePosition = false;
    }

    // Public getter for State Machine checks
    public bool Reached
    {
        get
        {
            // CRITICAL FIX: If not on NavMesh, we can't check path status.
            // Return false or handle gracefully to prevent crashes.
            if (_agent == null || !_agent.isOnNavMesh) return false;

            if (_agent.pathPending) return false;
            return !_agent.hasPath || _agent.remainingDistance <= stopDistance;
        }
    }

    public void Wander()
    {
        _directPursuit = false;
        if (_agent.isOnNavMesh) _agent.isStopped = false;
        GoTo(RandomPoint(), _agent.speed);
    }

    public void GoTo(Vector3 pos, float spd)
    {
        _directPursuit = false;
        if (_agent.isOnNavMesh) _agent.isStopped = false;

        _agent.speed = spd;
        _agent.SetDestination(pos);
    }

    public void Pursue(Vector3 pos, float spd)
    {
        _directPursuit = true;
        _directSpeed = spd;
        _directTarget = pos;

        if (_agent.isOnNavMesh) _agent.isStopped = true;
    }

    public void SearchAround(Vector3 center, float spd)
    {
        _directPursuit = false;
        if (_agent.isOnNavMesh) _agent.isStopped = false;

        // FIX: Ensure the random point is actually ON the NavMesh
        Vector3 randomOffset = (Vector3)Random.insideUnitCircle * 2.5f;
        Vector3 targetPos = center + randomOffset;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 3.0f, NavMesh.AllAreas))
        {
            GoTo(hit.position, spd);
        }
        else
        {
            GoTo(center, spd);
        }
    }

    public void StopMovement()
    {
        _directPursuit = false;
        _directSpeed = 0f;

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        if (_rb != null)
            _rb.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        // CRITICAL FIX: The error "IsStopped can only be called on an active agent..." 
        // happens here if we try to read _agent.isStopped before the agent is placed on the NavMesh.
        if (_agent == null || !_agent.isOnNavMesh)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _timeSinceDirectionChange += Time.fixedDeltaTime;

        Vector2 finalVelocity = Vector2.zero;
        bool shouldMove = true;

        // 1. Determine Raw Desired Velocity
        if (_directPursuit)
        {
            // Simple pursuit logic
            Vector2 toTarget = ((Vector2)_directTarget - (Vector2)transform.position);
            if (toTarget.sqrMagnitude < 0.1f) shouldMove = false; // Too close
            else finalVelocity = toTarget.normalized * _directSpeed;
        }
        else
        {
            // NavMesh logic
            // We use the property Reached which now has a safety check
            if (_agent.isStopped || Reached)
            {
                shouldMove = false;
            }
            else
            {
                finalVelocity = _agent.desiredVelocity;
            }
        }

        if (!shouldMove)
        {
            _rb.linearVelocity = Vector2.zero;
            // Keep agent synced even when stopped
            if (_agent != null) _agent.nextPosition = transform.position;
            return;
        }

        // 2. Apply 4-Directional Snapping
        if (useFourDirections && finalVelocity.sqrMagnitude > 0.01f)
        {
            finalVelocity = SnapToCardinal(finalVelocity);
        }

        // 3. Apply to Rigidbody
        _rb.linearVelocity = finalVelocity;

        // 4. Update internal tracking for next frame's turn logic
        if (finalVelocity.sqrMagnitude > 0.01f)
        {
            _lastNonZeroDirection = finalVelocity.normalized;
        }

        // 5. SYNC: Keep the NavMeshAgent thinking it is at the Rigidbody's position
        if (_agent != null)
        {
            _agent.nextPosition = transform.position;
        }
    }

    Vector2 SnapToCardinal(Vector2 input)
    {
        float speed = input.magnitude;
        if (speed < 0.001f) return Vector2.zero;

        float absX = Mathf.Abs(input.x);
        float absY = Mathf.Abs(input.y);

        // LOGIC A: EMERGENCY TURN ALLOWANCE
        if (_lastNonZeroDirection.sqrMagnitude > 0.1f)
        {
            float dot = Vector2.Dot(_lastNonZeroDirection, input.normalized);
            if (dot < 0)
            {
                _timeSinceDirectionChange = directionLockTime + 1f;
            }
        }

        // LOGIC B: Timer Lock
        if (_timeSinceDirectionChange < directionLockTime)
        {
            if (_lastAxisWasHorizontal)
            {
                if (absX > 0.1f) return new Vector2(Mathf.Sign(input.x), 0f) * speed;
            }
            else
            {
                if (absY > 0.1f) return new Vector2(0f, Mathf.Sign(input.y)) * speed;
            }
        }

        // LOGIC C: Hysteresis Bias
        float bias = 1.2f;

        if (_lastAxisWasHorizontal) absX *= bias;
        else absY *= bias;

        // Determine new direction
        bool newHorizontal = (absX >= absY);

        if (newHorizontal != _lastAxisWasHorizontal)
        {
            _lastAxisWasHorizontal = newHorizontal;
            _timeSinceDirectionChange = 0f;
        }

        if (newHorizontal)
            return new Vector2(Mathf.Sign(input.x), 0f) * speed;
        else
            return new Vector2(0f, Mathf.Sign(input.y)) * speed;
    }

    Vector3 RandomPoint()
    {
        return new Vector3(
            Mathf.Lerp(_wanderMin.x, _wanderMax.x, (float)_rng.NextDouble()),
            Mathf.Lerp(_wanderMin.y, _wanderMax.y, (float)_rng.NextDouble()),
            0f
        );
    }
}