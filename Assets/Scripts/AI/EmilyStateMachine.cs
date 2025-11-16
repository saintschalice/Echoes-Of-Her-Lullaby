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
/// OPTIMIZED State machine for Emily AI
/// Key fixes:
/// - Reduced Update() frequency from 60fps to configurable rate
/// - Hunt state pathfinding reduced from 20x/sec to 2x/sec
/// - Cached player reference
/// - Android-specific optimizations
/// </summary>
public class EmilyStateMachine : MonoBehaviour
{
    [Header("State Settings")]
    public EmilyState currentState = EmilyState.PATROL;

    [Header("Timing")]
    public float searchDuration = 15f;
    public float cooldownDuration = 20f;
    public float lostLOSGracePeriod = 2f;

    [Header("Performance (CRITICAL FOR ANDROID)")]
    [Tooltip("How often to update AI logic (lower = better performance)")]
    public float aiUpdateRate = 0.15f; // Update AI only ~7 times per second instead of 60
    private float lastAIUpdate = 0f;

    private EmilyAIController controller;
    private float stateTimer;
    private float lostLOSTimer;

    // State classes
    private PatrolState patrolState;
    private InvestigateState investigateState;
    private HuntState huntState;
    private SearchState searchState;
    private CooldownState cooldownState;

    // Cached references
    private EmilyPerception cachedPerception;
    private Transform cachedPlayerTransform;

    public void Initialize(EmilyAIController ctrl)
    {
        controller = ctrl;

        // Cache references
        cachedPerception = controller.perception;
        cachedPlayerTransform = controller.player;

        // Initialize state behaviors
        patrolState = new PatrolState(this, controller);
        investigateState = new InvestigateState(this, controller);
        huntState = new HuntState(this, controller);
        searchState = new SearchState(this, controller);
        cooldownState = new CooldownState(this, controller);

        Debug.Log("[EmilyStateMachine] Initialized with optimized update rate");
    }

    private void Update()
    {
        if (!controller.isActive) return;

        // CRITICAL OPTIMIZATION: Only update AI logic at specified rate
        float currentTime = Time.time;
        if (currentTime - lastAIUpdate < aiUpdateRate)
            return;

        float deltaTime = currentTime - lastAIUpdate;
        lastAIUpdate = currentTime;

        stateTimer += deltaTime;

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
                CheckHuntLOSLoss(deltaTime);
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
        // Use cached perception reference
        bool playerVisible = cachedPerception.IsPlayerVisible();
        bool hasNoise = cachedPerception.HasRecentNoise();

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

    void CheckHuntLOSLoss(float deltaTime)
    {
        if (!cachedPerception.IsPlayerVisible())
        {
            lostLOSTimer += deltaTime;
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

        ExitState(currentState);
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

// ========== STATE BEHAVIOR CLASSES (OPTIMIZED) ==========

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

    // CRITICAL FIX: Reduced from 0.05s to 0.5s
    private float updateInterval = 0.5f; // Update path only 2 times per second
    private float updateTimer;

    // Cache player transform for faster access
    //private Transform playerTransform;

    public HuntState(EmilyStateMachine sm, EmilyAIController ctrl)
    {
        machine = sm;
        controller = ctrl;
       //playerTransform = ctrl.player;
    }

    public void Enter()
    {
        updateTimer = 0f;
        // Set destination immediately on entering hunt

        // CHANGE THIS: Use controller.player instead of playerTransform
        if (controller.player != null)
        {
            controller.movement.PursueDirect(controller.player.position);
        }
    }

    public void Execute()
    {
        updateTimer += Time.deltaTime;

        // Use controller.player directly, which is correct
        if (updateTimer >= updateInterval && controller.player != null)
        {
            controller.movement.PursueDirect(controller.player.position);
            updateTimer = 0f;
        }

        // Check catch distance (use sqrMagnitude for better performance)
        if (controller.player == null) return; // Failsafe
        float sqrDistance = (controller.transform.position - controller.player.position).sqrMagnitude;
        float catchRadiusSqr = controller.movement.catchRadius * controller.movement.catchRadius;

        if (sqrDistance <= catchRadiusSqr)
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