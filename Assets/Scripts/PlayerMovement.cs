using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { Player_Idle, Player_Run, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }
    private void FixedUpdate()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {

        MovementState cur_state = new MovementState();
        if (dirX > 0f || dirX < 0f)
        {
            cur_state = MovementState.Player_Run;
        }
        if (dirX > 0f)
        {
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            sprite.flipX = true;
        }
        else
        {
            cur_state = MovementState.Player_Idle;
        }

        if (rb.velocity.y > .1f)
        {
            //state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            //cur_state = MovementState.falling;
        }
        anim.Play(cur_state.ToString());
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
