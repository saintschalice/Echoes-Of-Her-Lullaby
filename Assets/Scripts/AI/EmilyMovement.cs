using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public sealed class EmilyMovement : MonoBehaviour
{
    Rigidbody2D _rb; NavMeshAgent _agent; Vector2 _wanderMin, _wanderMax;
    readonly System.Random _rng = new System.Random();

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false; _agent.updateUpAxis = false;
        _wanderMin = new(-11, -3); _wanderMax = new(11, 3); // tweak per room
    }

    public bool Reached => !_agent.hasPath || _agent.remainingDistance < 0.4f;

    public void Wander() => GoTo(RandomPoint(), _agent.speed);

    public void GoTo(Vector3 pos, float spd)
    {
        _agent.speed = spd; _agent.isStopped = false;
        _agent.SetDestination(pos);
    }

    public void Pursue(Vector3 pos, float spd)
    {
        _agent.isStopped = true;   // stop NavMesh
        Vector2 dir = ((Vector2)pos - (Vector2)transform.position).normalized;
        _rb.linearVelocity = dir * spd;
    }

    public void SearchAround(Vector3 center, float spd)
    {
        GoTo(center + (Vector3)Random.insideUnitCircle * 2.5f, spd);
    }

    Vector3 RandomPoint()
    {
        return new Vector3(
            Mathf.Lerp(_wanderMin.x, _wanderMax.x, (float)_rng.NextDouble()),
            Mathf.Lerp(_wanderMin.y, _wanderMax.y, (float)_rng.NextDouble()),
            0f);
    }

    void FixedUpdate()
    {
        if (!_agent.isStopped) // NavMesh drive
            _rb.linearVelocity = _agent.desiredVelocity;
    }
}
