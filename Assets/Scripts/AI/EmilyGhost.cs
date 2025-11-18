using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public sealed class EmilyGhost : MonoBehaviour
{
    // ───────── CONFIG ─────────
    [Header("Speed (u/s)")]
    public float patrolSpeed = 1.4f;
    public float investigateSpeed = 1.8f;
    public float huntSpeed = 2f;

    [Header("State Timers (s)")]
    public float searchTime = 12f;
    public float cooldownTime = 18f;
    public float lostLOSTime = 1.8f;

    // ───────── RUNTIME ─────────
    public enum State { Patrol, Investigate, Hunt, Search, Cooldown }
    State _cur = State.Patrol;
    float _stateT;

    EmilyPerception _perception;
    EmilyMovement _move;
    EmilyAudio _audio;
    Transform _player;
    NavMeshAgent _agent;
    Animator _anim;
    Rigidbody2D _rb;
    EmilyAnimator _animator;


    void Awake()
    {
        _perception = gameObject.AddComponent<EmilyPerception>();
        _move = gameObject.AddComponent<EmilyMovement>();
        _audio = gameObject.AddComponent<EmilyAudio>();

        _animator = GetComponentInChildren<EmilyAnimator>();

        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();


        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = patrolSpeed;

        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Debug.Log("[EMILY] Awake on " + gameObject.name);

        SetState(State.Patrol);
    }


    void OnEnable()
    {
        Debug.Log("[EMILY] Enabled");
        SetState(State.Patrol); // start wandering immediately
    }

    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _stateT += dt;

        switch (_cur)
        {
            case State.Patrol:
                if (_perception.PlayerVisible) SetState(State.Hunt);
                else if (_perception.HeardNoise) SetState(State.Investigate);
                break;

            case State.Investigate:
                if (_perception.PlayerVisible) SetState(State.Hunt);
                else if (_move.Reached) SetState(State.Search);
                break;

            case State.Hunt:
                if (_perception.PlayerVisible)
                {
                    _stateT = 0f;
                    _move.Pursue(_player.position, huntSpeed);
                }
                else if (_stateT >= lostLOSTime) SetState(State.Search);
                break;

            case State.Search:
                if (_perception.PlayerVisible) SetState(State.Hunt);
                else if (_stateT >= searchTime) SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if (_perception.PlayerVisible) SetState(State.Hunt);
                else if (_stateT >= cooldownTime) SetState(State.Patrol);
                break;
        }

        // catch check
        if (_cur == State.Hunt &&
    (transform.position - _player.position).sqrMagnitude < 1.0f)
        {
            Debug.Log("[EMILY] CATCH TRIGGERED");

            // Stop movement completely
            _move.StopMovement();

            // Play hit animation
            if (_animator != null)
                _animator.PlayHit();

            // Catch SFX
            _audio.PlayCatch();

            // Game Over UI
            FindAnyObjectByType<GameOverManager>()?.TriggerGameOver("Emily caught you…");

            return;
        }

        // animator
        Vector2 vel = _rb.linearVelocity;
        _anim.SetBool("isWalking", vel.sqrMagnitude > 0.01f);
        if (vel.sqrMagnitude > 0.01f)
        {
            _anim.SetFloat("InputX", vel.x);
            _anim.SetFloat("InputY", vel.y);

        }



    }

    void SetState(State next)
    {
        if (_cur == next) return;
        _cur = next;
        _stateT = 0f;

        Debug.Log($"[EMILY] State -> {next}");

        switch (next)
        {
            case State.Patrol:
                _move.Wander();
                _audio.ToPatrol();
                _agent.speed = patrolSpeed;
                break;

            case State.Investigate:
                _move.GoTo(_perception.LastNoisePos, investigateSpeed);
                _audio.ToInvestigate();
                _agent.speed = investigateSpeed;
                break;

            case State.Hunt:
                _audio.ToHunt();
                _agent.speed = huntSpeed;
                break;

            case State.Search:
                _move.SearchAround(_perception.LastSeenPos, investigateSpeed);
                _audio.ToSearch();
                _agent.speed = investigateSpeed;
                break;

            case State.Cooldown:
                _move.Wander();
                _audio.ToCooldown();
                _agent.speed = patrolSpeed;
                break;
        }
    }
}
