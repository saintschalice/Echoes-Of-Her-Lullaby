using UnityEngine;

public class JoystickPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool usePhysics = true;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Joystick Reference")]
    public VirtualJoystick joystick;

    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        // Spawn at the correct point
        SpawnAtSavedPoint();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (joystick == null)
            joystick = FindFirstObjectByType<VirtualJoystick>();
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

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        // D-pad already gives us pure cardinal directions (1,0), (0,1), (-1,0), (0,-1)
        Vector2 moveDirection = joystick.Direction();
        
        Debug.Log($"[PlayerController] moveDirection: {moveDirection}, magnitude: {moveDirection.magnitude}");

        if (usePhysics && rb != null)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
            Debug.Log($"[PlayerController] Setting velocity: {rb.linearVelocity}");
        }
        else
        {
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            Debug.Log($"[PlayerController] Translating: {movement}");
        }
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        Vector2 moveDirection = joystick.Direction();
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
        return joystick.Direction().magnitude > 0.1f;
    }

    public Vector2 GetMovementDirection()
    {
        return joystick.Direction();
    }
}