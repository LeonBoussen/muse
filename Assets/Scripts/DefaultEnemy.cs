using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : MonoBehaviour
{
    [HideInInspector]
    public bool patroling;
    private bool mustTurn;

    public float walksSpeed;

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public LayerMask groundLayer;

    void Start()
    {
        patroling = true;
    }

    void Update()
    {
        if (patroling)
            Patrol();
    }
    private void FixedUpdate()
    {
        if (patroling)
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
    }

    void Patrol()
    {
        rb.velocity = new Vector2(walksSpeed + Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        patroling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walksSpeed *= -1;
        patroling = true;
    }
}
