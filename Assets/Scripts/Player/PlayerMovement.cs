using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Collider2D coll;
    public SpriteRenderer sprite;
    private Animator anim;
    private PlayerAttack attack_script;
    public GameObject groundcheck;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    private bool override_anim = true;
    private bool move_player = false;
    private enum MovementState { Player_Idle, Player_Run, Player_Jump, Player_Fall }

    // Start is called before the first frame update
    private void Start()
    {
        attack_script = gameObject.GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("joystick button 1") || Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        if (!IsGrounded())
        {
            override_anim = false;
        }
        else
        {
            override_anim = true;
        }
    }
    private void FixedUpdate()
    {
        if (!attack_script.isCoolDown)
        {
            UpdateAnimationState();
        }
        if (move_player)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        
    }

    private void UpdateAnimationState()
    {
        MovementState cur_state = new MovementState();
        if (dirX > 0f || dirX < 0f)
        {
            cur_state = MovementState.Player_Run; // run
        }
        if (dirX > 0.3f)
        {
            sprite.flipX = false;
            move_player = true;
        }
        else if (dirX < -0.3f)
        {
            sprite.flipX = true;
            move_player = true;
        }
        else
        {
            cur_state = MovementState.Player_Idle;
            move_player = false;
            
        }
        if (rb.velocity.y > 4f)
        {
            cur_state = MovementState.Player_Jump;
            move_player = true;
        }
        else if (rb.velocity.y < -.2f && !IsGrounded())
        {
            cur_state = MovementState.Player_Fall;
            move_player = true;
        }
        PlayAnim(cur_state.ToString());
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(groundcheck.transform.position, Vector2.down, .15f, jumpableGround);
    }
    void PlayAnim(string animation)
    {
        if (!override_anim)
        {
            if (!animation.Contains("Player_Fall") || animation.Contains("Player_Jump"))
            {
                return;
            }
            else
            {
                anim.Play(animation);
            }
        }
        else
        {
            anim.Play(animation);
        }
        


    }
    
    
}
