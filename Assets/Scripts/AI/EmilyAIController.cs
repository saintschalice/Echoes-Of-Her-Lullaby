using UnityEngine;
using System.Collections;

/// <summary>
/// Main controller for Emily ghost AI
/// Manages state machine, perception, movement, animation, and audio
/// </summary>
public class EmilyAIController : MonoBehaviour
{
    public static EmilyAIController Instance { get; private set; }

    [Header("Components")]
    public EmilyStateMachine stateMachine;
    public EmilyPerception perception;
    public EmilyMovement movement;
    public EmilyAnimator emilyAnimator;
    public EmilyAudio audioController;

    [Header("Settings")]
    public bool isActive = false;
    public bool debugMode = false;

    [Header("Catch Settings")]
    public float defaultCatchDelay = 0.5f;   // how long after activation before she can kill
    [HideInInspector] public float catchEnabledTime = 0f;

    [Header("Player Reference")]
    public Transform player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[EmilyAI] Duplicate instance detected, destroying new instance");
            Destroy(gameObject);
            return;
        }

        Instance = this;


        // Allow multiple instances across scenes (managed by PersistentEmilyManager)
        InitializeComponents();

        Debug.Log("[EmilyAI] Instance created");
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("[EmilyAI] Player not found!");
            }
        }

        if (isActive)
        {
            ActivateEmily();
        }
        else
        {
            DeactivateEmily();
        }
    }

    void InitializeComponents()
    {
        if (stateMachine == null) stateMachine = GetComponent<EmilyStateMachine>();
        if (perception == null) perception = GetComponent<EmilyPerception>();
        if (movement == null) movement = GetComponent<EmilyMovement>();
        if (emilyAnimator == null) emilyAnimator = GetComponent<EmilyAnimator>();
        if (audioController == null) audioController = GetComponent<EmilyAudio>();

        // Cross-reference components
        if (stateMachine) stateMachine.Initialize(this);
        if (perception) perception.Initialize(this);
        if (movement) movement.Initialize(this);
    }

    public void ActivateEmily()
    {
        isActive = true;
        gameObject.SetActive(true);
        perception?.StopAllCoroutines();

        // Start perception immediately. It was being delayed, which also
        // contributed to not detecting the player.
        perception?.StartCoroutine("VisionCheckRoutine");

        // DO NOT set a default state here.
        // Let the PersistentEmilyManager or the Trigger set the state.

        audioController?.PlayPresenceSound();
        Debug.Log("[EmilyAI] Activated");
    }

    public void DeactivateEmily()
    {
        isActive = false;
        gameObject.SetActive(false);
        audioController?.StopAllSounds();
        Debug.Log("[EmilyAI] Deactivated");
    }

    public void TeleportTo(Vector3 position)
    {
        transform.position = position;
        movement?.ResetNavigation();
        Debug.Log($"[EmilyAI] Teleported to {position}");
    }

    public void ForceState(EmilyState newState)
    {
        stateMachine?.ForceTransition(newState);
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        // Draw perception radius
        if (perception != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, perception.currentDetectionRadius);
        }
    }


    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(0.2f);
        perception?.StartCoroutine("VisionCheckRoutine");
        stateMachine?.ActivateState(EmilyState.PATROL);
    }

}