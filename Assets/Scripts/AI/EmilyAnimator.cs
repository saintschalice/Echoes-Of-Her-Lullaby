using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class EmilyAnimator : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb;

    // reference to the root EmilyGhost object
    Transform _root;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _root = transform.parent;

        if (_root != null)
            _rb = _root.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_rb == null) return;

        Vector2 vel = _rb.linearVelocity;
        bool moving = vel.sqrMagnitude > 0.01f;

        _anim.SetBool("isWalking", moving);

        if (moving)
        {
            // Feed movement direction for 4-way blend tree
            _anim.SetFloat("MoveX", vel.x);
            _anim.SetFloat("MoveY", vel.y);
        }
    }

    // ----------------------------------------------------------
    // CALL THIS FROM EmilyGhost ON CATCH
    // ----------------------------------------------------------
    public void PlayHit()
    {
        _anim.SetTrigger("Hit");
    }
}
