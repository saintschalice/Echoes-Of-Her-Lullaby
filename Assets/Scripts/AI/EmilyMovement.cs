using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public sealed class EmilyMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    NavMeshAgent _agent;

    bool _directPursuit = false;
    float _directSpeed = 0f;
    Vector3 _directTarget;

    Vector2 _wanderMin = new(-11, -3);
    Vector2 _wanderMax = new(11, 3);
    readonly System.Random _rng = new();

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();

        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    public bool Reached => !_agent.hasPath || _agent.remainingDistance < 0.4f;

    public void Wander()
    {
        _directPursuit = false;
        _agent.isStopped = false;
        GoTo(RandomPoint(), _agent.speed);
    }

    public void GoTo(Vector3 pos, float spd)
    {
        _directPursuit = false;
        _agent.isStopped = false;

        _agent.speed = spd;
        _agent.SetDestination(pos);
    }

    public void Pursue(Vector3 pos, float spd)
    {
        _directPursuit = true;
        _directSpeed = spd;
        _directTarget = pos;

        _agent.isStopped = true;
    }

    public void SearchAround(Vector3 center, float spd)
    {
        _directPursuit = false;
        _agent.isStopped = false;

        Vector3 target = center + (Vector3)Random.insideUnitCircle * 2.5f;
        GoTo(target, spd);
    }

    public void StopMovement()
    {
        _directPursuit = false;
        _directSpeed = 0f;

        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        if (_rb != null)
            _rb.linearVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (_directPursuit)
        {
            Vector2 dir = ((Vector2)_directTarget - (Vector2)transform.position).normalized;
            _rb.linearVelocity = dir * _directSpeed;
            return;
        }

        if (!_agent.isStopped)
            _rb.linearVelocity = _agent.desiredVelocity;
        else
            _rb.linearVelocity = Vector2.zero;
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
