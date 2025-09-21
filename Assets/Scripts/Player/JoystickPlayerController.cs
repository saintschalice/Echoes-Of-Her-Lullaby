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



    // Store last direction for idle animations
    private Vector2 lastDirection = Vector2.down; // Start facing down

    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find joystick if not assigned
        if (joystick == null)
            joystick = FindFirstObjectByType<VirtualJoystick>();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void HandleMovement()
    {
        // Get input from joystick
        Vector2 moveDirection = joystick.Direction();

        // Convert to 4-directional movement for actual movement too
        Vector2 cardinalDirection = Vector2.zero;
        bool isMoving = moveDirection.magnitude > 0.1f;

        if (isMoving)
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                // More horizontal movement
                cardinalDirection = moveDirection.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                // More vertical movement
                cardinalDirection = moveDirection.y > 0 ? Vector2.up : Vector2.down;
            }
        }

        if (usePhysics && rb != null)
        {
            // Physics-based movement
            rb.linearVelocity = cardinalDirection * moveSpeed;
        }
        else
        {
            // Transform-based movement
            Vector3 movement = new Vector3(cardinalDirection.x, cardinalDirection.y, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        Vector2 moveDirection = joystick.Direction();
        bool isMoving = moveDirection.magnitude > 0.1f;

        Vector2 cardinalDirection = Vector2.zero;

        if (isMoving)
        {
            // Convert to 4-directional movement (no diagonals)
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                // More horizontal movement
                cardinalDirection = moveDirection.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                // More vertical movement  
                cardinalDirection = moveDirection.y > 0 ? Vector2.up : Vector2.down;
            }

            lastDirection = cardinalDirection;
        }

        // Use current cardinal direction or last direction when idle
        Vector2 animDirection = isMoving ? cardinalDirection : lastDirection;

        animator.SetBool("isWalking", isMoving);
        animator.SetFloat("InputX", animDirection.x);
        animator.SetFloat("InputY", animDirection.y);

        /* Handle sprite flipping
        if (animDirection.x > 0.1f)
        {
            spriteRenderer.flipX = false; // Moving right
        }
        else if (animDirection.x < -0.1f)
        {
            spriteRenderer.flipX = true; // Moving left
        }

        */
    }

    // Optional: Get movement state for other scripts
    public bool IsMoving()
    {
        return joystick.Direction().magnitude > 0.1f;
    }

    public Vector2 GetMovementDirection()
    {
        return joystick.Direction();
    }
}