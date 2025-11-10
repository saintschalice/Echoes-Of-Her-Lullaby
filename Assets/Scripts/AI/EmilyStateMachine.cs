using UnityEngine;
using System.Collections;

public enum EmilyState
{
    PATROL,
    INVESTIGATE,
    HUNT,
    SEARCH,
    COOLDOWN
}

/// <summary>
/// State machine for Emily AI behavior
/// Manages state transitions and timing
/// </summary>
public class EmilyStateMachine : MonoBehaviour
{
    [Header("State Settings")]
    public EmilyState currentState = EmilyState.PATROL;

    [Header("Timing")]
    public float searchDuration = 15f;
    public float cooldownDuration = 20f;
    public float lostLOSGracePeriod = 2f; // Time before transitioning from HUNT to SEARCH

    private EmilyAIController controller;
    private float stateTimer;
    private float lostLOSTimer;
    private bool hasLineOfSight;

    // State classes
    private PatrolState patrolState;
    private InvestigateState investigateState;
    private HuntState huntState;
    private SearchState searchState;
    private CooldownState cooldownState;

    public void Initialize(EmilyAIController ctrl)
    {
        controller = ctrl;

        // Initialize state behaviors
        patrolState = new PatrolState(this, controller);
        investigateState = new InvestigateState(this, controller);
        huntState = new HuntState(this, controller);
        searchState = new SearchState(this, controller);
        cooldownState = new CooldownState(this, controller);
    }

    private void Update()
    {
        if (!controller.isActive) return;

        stateTimer += Time.deltaTime;

        // Execute current state logic
        switch (currentState)
        {
            case EmilyState.PATROL:
                patrolState?.Execute();
                break;
            case EmilyState.INVESTIGATE:
                investigateState?.Execute();
                break;
            case EmilyState.HUNT:
                huntState?.Execute();
                CheckHuntLOSLoss();
                break;
            case EmilyState.SEARCH:
                searchState?.Execute();
                CheckSearchTimeout();
                break;
            case EmilyState.COOLDOWN:
                cooldownState?.Execute();
                CheckCooldownTimeout();
                break;
        }

        // Check for state transitions
        CheckTransitions();
    }

    void CheckTransitions()
    {
        // Get perception data
        bool playerVisible = controller.perception.IsPlayerVisible();
        bool hasNoise = controller.perception.HasRecentNoise();

        switch (currentState)
        {
            case EmilyState.PATROL:
                if (playerVisible)
                {
                    TransitionTo(EmilyState.HUNT);
                }
                else if (hasNoise)
                {
                    TransitionTo(EmilyState.INVESTIGATE);
                }
                break;

            case EmilyState.INVESTIGATE:
                if (playerVisible)
                {
                    TransitionTo(EmilyState.HUNT);
                }
                break;

            case EmilyState.HUNT:
                // Handled in CheckHuntLOSLoss()
                break;

            case EmilyState.SEARCH:
                if (playerVisible)
                {
                    TransitionTo(EmilyState.HUNT);
                }
                break;

            case EmilyState.COOLDOWN:
                // Handled in CheckCooldownTimeout()
                break;
        }
    }

    void CheckHuntLOSLoss()
    {
        if (!controller.perception.IsPlayerVisible())
        {
            lostLOSTimer += Time.deltaTime;
            if (lostLOSTimer >= lostLOSGracePeriod)
            {
                TransitionTo(EmilyState.SEARCH);
            }
        }
        else
        {
            lostLOSTimer = 0f;
        }
    }

    void CheckSearchTimeout()
    {
        if (stateTimer >= searchDuration)
        {
            TransitionTo(EmilyState.COOLDOWN);
        }
    }

    void CheckCooldownTimeout()
    {
        if (stateTimer >= cooldownDuration)
        {
            TransitionTo(EmilyState.PATROL);
        }
    }

    public void TransitionTo(EmilyState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"[EmilyAI] State: {currentState} → {newState}");

        // Exit current state
        ExitState(currentState);

        // Enter new state
        currentState = newState;
        stateTimer = 0f;
        lostLOSTimer = 0f;
        EnterState(newState);
    }

    void EnterState(EmilyState state)
    {
        switch (state)
        {
            case EmilyState.PATROL:
                patrolState?.Enter();
                controller.movement.SetSpeed(controller.movement.patrolSpeed);
                controller.audioController?.SetAudioState(EmilyAudioState.Patrol);
                break;

            case EmilyState.INVESTIGATE:
                investigateState?.Enter();
                controller.movement.SetSpeed(controller.movement.investigateSpeed);
                controller.audioController?.SetAudioState(EmilyAudioState.Investigate);
                break;

            case EmilyState.HUNT:
                huntState?.Enter();
                controller.movement.SetSpeed(controller.movement.huntSpeed);
                controller.audioController?.SetAudioState(EmilyAudioState.Hunt);
                break;

            case EmilyState.SEARCH:
                searchState?.Enter();
                controller.movement.SetSpeed(controller.movement.searchSpeed);
                controller.audioController?.SetAudioState(EmilyAudioState.Search);
                break;

            case EmilyState.COOLDOWN:
                cooldownState?.Enter();
                controller.movement.SetSpeed(controller.movement.cooldownSpeed);
                controller.audioController?.SetAudioState(EmilyAudioState.Cooldown);
                break;
        }
    }

    void ExitState(EmilyState state)
    {
        // Clean up state-specific logic
    }

    public void ActivateState(EmilyState state)
    {
        TransitionTo(state);
    }

    public void ForceTransition(EmilyState newState)
    {
        TransitionTo(newState);
    }

    public void OnNoiseHeard(Vector3 noisePosition)
    {
        if (currentState == EmilyState.PATROL || currentState == EmilyState.COOLDOWN)
        {
            controller.perception.lastKnownPlayerPosition = noisePosition;
            TransitionTo(EmilyState.INVESTIGATE);
        }
    }
}

// ========== STATE BEHAVIOR CLASSES ==========

public class PatrolState
{
    private EmilyStateMachine machine;
    private EmilyAIController controller;
    private float wanderTimer;
    private float wanderInterval = 5f;

    public PatrolState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
    }

    public void Enter()
    {
        wanderTimer = 0f;
    }

    public void Execute()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            controller.movement.SetRandomDestination();
            wanderTimer = 0f;
        }
    }
}

public class InvestigateState
{
    private EmilyStateMachine machine;
    private EmilyAIController controller;

    public InvestigateState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
    }

    public void Enter()
    {
        Vector3 target = controller.perception.lastKnownPlayerPosition;
        controller.movement.SetDestination(target);
    }

    public void Execute()
    {
        if (controller.movement.HasReachedDestination())
        {
            machine.TransitionTo(EmilyState.SEARCH);
        }
    }
}

public class HuntState
{
    private EmilyStateMachine machine;
    private EmilyAIController controller;
    private float updateInterval = 0.3f;
    private float updateTimer;

    public HuntState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
    }

    public void Enter()
    {
        updateTimer = 0f;
    }

    public void Execute()
    {
        updateTimer += Time.deltaTime;

        // Direct pursuit in HUNT mode
        if (updateTimer >= updateInterval)
        {
            controller.movement.PursueDirect(controller.player.position);
            updateTimer = 0f;
        }

        // Check for catch
        float distance = Vector2.Distance(controller.transform.position, controller.player.position);
        if (distance <= controller.movement.catchRadius)
        {
            controller.audioController?.PlayCatchSound();
            GameOverManager.Instance?.TriggerGameOver("Emily caught you...");
        }
    }
}

public class SearchState
{
    private EmilyStateMachine machine;
    private EmilyAIController controller;
    private int searchPointIndex = 0;

    public SearchState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
    }

    public void Enter()
    {
        searchPointIndex = 0;
        MoveToNextSearchPoint();
    }

    public void Execute()
    {
        if (controller.movement.HasReachedDestination())
        {
            searchPointIndex++;
            if (searchPointIndex < 4)
            {
                MoveToNextSearchPoint();
            }
        }
    }

    void MoveToNextSearchPoint()
    {
        Vector3 searchPoint = GetSearchPoint(searchPointIndex);
        controller.movement.SetDestination(searchPoint);
    }

    Vector3 GetSearchPoint(int index)
    {
        Vector3 lastKnown = controller.perception.lastKnownPlayerPosition;
        float radius = 3f;
        float angle = index * 90f * Mathf.Deg2Rad;
        return lastKnown + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
}

public class CooldownState
{
    private EmilyStateMachine machine;
    private EmilyAIController controller;

    public CooldownState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
    }

    public void Enter()
    {
        controller.movement.SetRandomDestination();
    }

    public void Execute()
    {
        // Slow patrol
        if (controller.movement.HasReachedDestination())
        {
            controller.movement.SetRandomDestination();
        }
    }
}