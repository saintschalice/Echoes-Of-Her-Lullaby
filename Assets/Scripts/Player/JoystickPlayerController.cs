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

    public event Action InteractPerformed;

    private PlayerInputRouter inputRouter;
    private Vector2 moveInputFromRouter = Vector2.zero;

    // Variables to store input and state
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lastDirection = Vector2.down;

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

    private void OnEnable()
    {
        SubscribeToJoystick(joystick);

        PlayerInputRouter.OnInstanceChanged += HandleInputRouterChanged;
        HandleInputRouterChanged(PlayerInputRouter.Instance);
    }

    private void OnDisable()
    {
        UnsubscribeFromJoystick();
        PlayerInputRouter.OnInstanceChanged -= HandleInputRouterChanged;
        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= OnInteractTriggered;
            inputRouter.MoveVectorChanged -= OnMoveVectorChanged;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            instanceChanged?.Invoke(null);
        }

        UnsubscribeFromJoystick();
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

        SubscribeToJoystick(joystick);

        SpawnAtSavedPoint();
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
        // Ensure GameManager exists before checking it to prevent potential null refs here too
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
                return;
            }
        }

        if (spawnPoints.Length > 0)
        {
            transform.position = spawnPoints[0].transform.position;
        }
    }

    void Update()
    {
        // 1. Safety Check: Try to find joystick if missing
        if (joystick == null)
        {
            SubscribeToJoystick(FindFirstObjectByType<VirtualJoystick>());
        }

        // 2. Handle Input
        // We use the null conditional operator (?) to safely access Direction
        Vector2 joystickDirection = (joystick != null) ? joystick.Direction() : Vector2.zero;

        moveDirection = joystickDirection != Vector2.zero ? joystickDirection : moveInputFromRouter;

        // 3. Handle Visuals
        HandleAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void SubscribeToJoystick(VirtualJoystick target)
    {
        if (target == joystick && target != null)
            return;

        UnsubscribeFromJoystick();
        joystick = target;

        if (joystick != null)
        {
            joystick.InteractPressed += OnInteractTriggered;
        }

        RefreshRouterSubscription();
    }

    private void UnsubscribeFromJoystick()
    {
        if (joystick != null)
        {
            joystick.InteractPressed -= OnInteractTriggered;
        }

        joystick = null;
        RefreshRouterSubscription();
    }

    private void HandleInputRouterChanged(PlayerInputRouter router)
    {
        if (inputRouter != null)
        {
            inputRouter.InteractPerformed -= OnInteractTriggered;
            inputRouter.MoveVectorChanged -= OnMoveVectorChanged;
        }

        inputRouter = router;

        RefreshRouterSubscription();

        if (inputRouter != null)
        {
            moveInputFromRouter = inputRouter.LastMoveVector;
        }
    }

    private void OnInteractTriggered()
    {
        InteractPerformed?.Invoke();
    }

    private void RefreshRouterSubscription()
    {
        if (inputRouter == null)
            return;

        inputRouter.InteractPerformed -= OnInteractTriggered;
        inputRouter.MoveVectorChanged -= OnMoveVectorChanged;

        inputRouter.InteractPerformed += OnInteractTriggered;
        inputRouter.MoveVectorChanged += OnMoveVectorChanged;
    }

    private void OnMoveVectorChanged(Vector2 moveInput)
    {
        moveInputFromRouter = moveInput;
    }

    void HandleMovement()
    {
        if (usePhysics && rb != null)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.fixedDeltaTime;
            transform.Translate(movement);
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