using System;
using UnityEngine;

public class JoystickPlayerController : MonoBehaviour
{
    public static JoystickPlayerController Instance { get; private set; }

    private static event Action<JoystickPlayerController> instanceChanged;
    public static event Action<JoystickPlayerController> OnInstanceChanged
    {
        add
        {
            instanceChanged -= value;
            instanceChanged += value;
            value?.Invoke(Instance);
        }
        remove
        {
            instanceChanged -= value;
        }
    }

    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool usePhysics = true;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Joystick Reference")]
    public VirtualJoystick joystick;

    // Variables to store input and state
    private Vector2 moveDirection = Vector2.zero; // Stores input from Update()
    private Vector2 lastDirection = Vector2.down; // Used for idle animation direction

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        instanceChanged?.Invoke(this);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            instanceChanged?.Invoke(null);
        }
    }

    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find joystick if not assigned
        if (joystick == null)
            joystick = FindFirstObjectByType<VirtualJoystick>();

        // Run your custom spawn logic
        SpawnAtSavedPoint();

        // **IMPORTANT INSPECTOR CHECK:** // 1. Ensure Rigidbody2D Collision Detection is set to 'Continuous'.
        // 2. Ensure Rigidbody2D Interpolate is set to 'Interpolate'.
    }

    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }

    void SpawnAtSavedPoint()
    {
        string spawnName = "DefaultSpawn";
        if (GameManager.Instance != null)
        {
            spawnName = GameManager.Instance.currentSpawnPointName;
        }

        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.spawnPointName == spawnName)
            {
                transform.position = sp.transform.position;
                Debug.Log($"[Player] Spawned at: {spawnName}");
                return;
            }
        }

        // Fallback
        if (spawnPoints.Length > 0)
        {
            transform.position = spawnPoints[0].transform.position;
            Debug.Log("[Player] Spawned at fallback spawn point");
        }
    }

    // Runs once per frame (Variable framerate). Used for input and visual logic.
    void Update()
    {
        // **1. Handle Input (Store the direction for FixedUpdate)**
        moveDirection = joystick.Direction();

        // **2. Handle Visuals**
        HandleAnimation();
    }

    // Runs at a fixed interval. Used for all physics logic.
    void FixedUpdate()
    {
        // **3. Handle Physics Movement**
        HandleMovement();
    }

    void HandleMovement()
    {
        // This is now in FixedUpdate, using the 'moveDirection' stored in Update().
        if (usePhysics && rb != null)
        {
            // Set the linear velocity. This is the correct, non-obsolete way for a physics body.
            rb.linearVelocity = moveDirection * moveSpeed;
           // Debug.Log($"[PlayerController] Setting linearVelocity: {rb.linearVelocity}");
        }
        else
        {
            // Non-physics movement, using Time.fixedDeltaTime for consistency in FixedUpdate
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.fixedDeltaTime;
            transform.Translate(movement);
           // Debug.Log($"[PlayerController] Translating: {movement}");
        }
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveDirection.magnitude > 0.1f;

        if (isMoving)
        {
            lastDirection = moveDirection;
        }

        Vector2 animDirection = isMoving ? moveDirection : lastDirection;

        animator.SetBool("isWalking", isMoving);
        animator.SetFloat("InputX", animDirection.x);
        animator.SetFloat("InputY", animDirection.y);
    }

    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }

    public Vector2 GetMovementDirection()
    {
        return moveDirection;
    }

    public Vector2 GetFacingDirection()
    {
        if (moveDirection.magnitude > 0.1f)
            return moveDirection.normalized;
        return lastDirection.normalized;
    }

    public Vector2 GetFacingDirectionFromAnimator()
    {
        if (animator == null) return lastDirection;
        float x = animator.GetFloat("InputX");
        float y = animator.GetFloat("InputY");
        return new Vector2(x, y).normalized;
    }
}