using UnityEngine;

/// <summary>
/// Handles Emily's animations based on movement and state
/// Supports 4-directional idle, walk, and hit animations
/// FIXED: Uses InputX/InputY to match your Animator Controller
/// </summary>
public class EmilyAnimator : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Animation Parameters")]
    // FIXED: Changed to match your Animator Controller
    private readonly int inputXHash = Animator.StringToHash("InputX");
    private readonly int inputYHash = Animator.StringToHash("InputY");
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");

    private EmilyAIController controller;
    private EmilyMovement movement;
    private Vector2 lastMoveDirection = Vector2.down; // Default facing direction
    private bool animatorIsValid = false;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if animator has a valid controller
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animatorIsValid = true;
            Debug.Log("[EmilyAnimator] Animator controller found and validated");

            // Verify parameters exist
            bool hasInputX = HasParameter(inputXHash);
            bool hasInputY = HasParameter(inputYHash);
            bool hasIsWalking = HasParameter(isWalkingHash);

            Debug.Log($"[EmilyAnimator] Parameters - InputX: {hasInputX}, InputY: {hasInputY}, isWalking: {hasIsWalking}");
        }
        else
        {
            animatorIsValid = false;
            Debug.LogWarning("[EmilyAnimator] No Animator Controller assigned! Animations will not play.");
        }
    }

    private void Start()
    {
        controller = GetComponent<EmilyAIController>();
        movement = GetComponent<EmilyMovement>();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (controller == null || movement == null) return;
        if (!animatorIsValid) return;

        Vector2 direction = movement.GetForwardDirection();
        float speed = movement.GetCurrentSpeed();
        bool isMoving = speed > 0.1f;

        // Update animation parameters
        if (isMoving)
        {
            lastMoveDirection = direction;

            // Set input direction
            animator.SetFloat(inputXHash, direction.x);
            animator.SetFloat(inputYHash, direction.y);

            // Set walking state
            animator.SetBool(isWalkingHash, true);
        }
        else
        {
            // Idle - keep last direction
            animator.SetFloat(inputXHash, lastMoveDirection.x);
            animator.SetFloat(inputYHash, lastMoveDirection.y);

            // Not walking
            animator.SetBool(isWalkingHash, false);
        }
    }

    bool HasParameter(int hash)
    {
        if (animator == null || !animatorIsValid) return false;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.nameHash == hash)
                return true;
        }
        return false;
    }

    public void PlayHitAnimation()
    {
        if (animatorIsValid)
        {
            // Trigger hit animation if you have one
            // animator.SetTrigger("Hit");
            Debug.Log("[EmilyAnimator] Hit animation triggered (if configured)");
        }
    }

    public void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;
    }

    /// <summary>
    /// Manually set animation direction (for when not moving)
    /// </summary>
    public void SetAnimationDirection(Vector2 direction)
    {
        if (!animatorIsValid) return;

        lastMoveDirection = direction;
        animator.SetFloat(inputXHash, direction.x);
        animator.SetFloat(inputYHash, direction.y);
    }

    /// <summary>
    /// Get the current animation facing direction
    /// </summary>
    public Vector2 GetAnimationDirection()
    {
        return lastMoveDirection;
    }

    /// <summary>
    /// Force idle animation
    /// </summary>
    public void ForceIdle()
    {
        if (!animatorIsValid) return;

        animator.SetBool(isWalkingHash, false);
    }

    /// <summary>
    /// Force walk animation
    /// </summary>
    public void ForceWalk()
    {
        if (!animatorIsValid) return;

        animator.SetBool(isWalkingHash, true);
    }
}