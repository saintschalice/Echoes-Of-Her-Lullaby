using UnityEngine;

/// <summary>
/// Controls child-shaped shadow sprites that only move when player isn't looking
/// Compatible with JoystickPlayerController
/// </summary>
public class ShadowController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 1f;
    public float detectionAngle = 45f;
    public LayerMask obstacleMask;

    [Header("Movement")]
    public Transform[] waypoints;
    private int currentWaypoint = 0;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private bool isVisible;

    private JoystickPlayerController playerController;
    private Animator playerAnimator;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            playerController = player.GetComponent<JoystickPlayerController>();
            playerAnimator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("[ShadowController] Player not found!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("[ShadowController] SpriteRenderer not found!");
        }
    }

    private void Update()
    {
        CheckIfPlayerLooking();

        if (!isVisible && player != null)
        {
            MoveTowardsWaypoint();
        }
    }

    void CheckIfPlayerLooking()
    {
        if (player == null)
        {
            isVisible = false;
            return;
        }

        Vector2 toShadow = transform.position - player.position;
        float distance = toShadow.magnitude;

        // Get player facing direction
        JoystickPlayerController playerController = player.GetComponent<JoystickPlayerController>();
        Vector2 playerFacing = Vector2.up; // Default

        if (playerController != null)
        {
            playerFacing = playerController.GetFacingDirection();
        }
        else
        {
            // Fallback: USE THE CACHED VARIABLE (playerAnimator)
            if (playerAnimator != null)
            {
                float x = playerAnimator.GetFloat("InputX");
                float y = playerAnimator.GetFloat("InputY");
                if (x != 0 || y != 0)
                {
                    playerFacing = new Vector2(x, y).normalized;
                }
            }
        }

        // Ensure playerFacing is not zero
        if (playerFacing.magnitude < 0.1f)
        {
            playerFacing = Vector2.up;
        }

        float angle = Vector2.Angle(playerFacing, toShadow);

        // Check if player is looking at shadow
        if (angle < detectionAngle)
        {
            // Raycast for line of sight
            RaycastHit2D hit = Physics2D.Raycast(
                player.position,
                toShadow.normalized,
                distance,
                obstacleMask
            );

            if (hit.collider == null || hit.collider.gameObject == gameObject)
            {
                isVisible = true;
                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;
                return;
            }
        }

        isVisible = false;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        if (target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw waypoint path
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawWireSphere(waypoints[i].position, 0.2f);

                    // Draw line to next waypoint
                    int nextIndex = (i + 1) % waypoints.Length;
                    if (waypoints[nextIndex] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                    }
                }
            }
        }

        // Draw detection angle cone
        if (player != null)
        {
            Gizmos.color = isVisible ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}