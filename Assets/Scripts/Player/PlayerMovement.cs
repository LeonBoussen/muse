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
    private PlayerHealthScript player_health;

    public LayerMask layer;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private bool override_anim = true;
    private bool move_player = false;
    private bool camera_on = false;

    private enum MovementState { Player_Idle, Player_Run, Player_Jump, Player_Fall, Player_Death, Player_Camera}

    // Start is called before the first frame update
    private void Start()
    {
        player_health = GetComponent<PlayerHealthScript>();
        attack_script = GetComponent<PlayerAttack>();
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
            if (IsGrounded() && !player_health.hasdied && !camera_on)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown(KeyCode.E))
        {
            if (!camera_on && IsGrounded())
            {
                StartCoroutine(Player_Camera_Cooldown(2.1f));
            }
        }
        if (!IsGrounded() && !camera_on)
        {
            override_anim = false;
        }
        else if (IsGrounded() && !camera_on)
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
        if (move_player && !player_health.hasdied && !camera_on)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        else if (!move_player && !player_health.hasdied && !camera_on)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        } 
    }
    private void UpdateAnimationState()
    {
        MovementState cur_state = new MovementState();
        if (!player_health.hasdied)
        {
            if (dirX > 0f || dirX < 0f)
            {
                cur_state = MovementState.Player_Run; // run
            }
            if (dirX > 0.3f && !camera_on)
            {
                sprite.flipX = false;
                move_player = true;
            }
            else if (dirX < -0.3f && !camera_on)
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
        }
        else
        {
            cur_state = MovementState.Player_Death;
        }
        PlayAnim(cur_state.ToString());
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(groundcheck.transform.position, Vector2.down, .15f, jumpableGround);
    }
    void PlayAnim(string animation)
    {
        if (animation == "Player_Death")
        {
            anim.Play(animation);
            return;
        }
        if (!override_anim)
        {
            if (!animation.Contains("Player_Fall") || animation.Contains("Player_Jump") || animation.Contains("Player_Camera"))
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
    IEnumerator Player_Camera_Cooldown(float cooldown)
    {
        anim.Play("Player_Camera");
        move_player = false;
        camera_on = true;
        override_anim = false;
        Vector2 dir = new Vector2();
        if (sprite.flipX == true)
        {
            dir = Vector2.left;
        }
        else if (sprite.flipX == false)
        {
            dir = Vector2.right;
        }
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, 10f, layer);
        foreach(RaycastHit2D cast in hit)
        {
            if (cast.collider != null)
            {
                Enemy enem = cast.collider.GetComponent<Enemy>();
                if (enem != null)
                {
                    if (enem.hasdied)
                    {
                        StartCoroutine(enem.Morph_Anim());
                    }
                }
            }
        }
        yield return new WaitForSecondsRealtime(cooldown);
        override_anim = true;
        camera_on = false;

    }
    
    
}
