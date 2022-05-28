using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerHealthScript healthscript;

    private bool SeenPlayer;
    private Vector2 TargetLocation;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float AttackDelay;


    public bool EnemyPlant;
    

    public bool Enemytype1;


    public bool Enemytype2;


    public bool Enemytype3;


    public bool Enemytype4;


    public bool Enemytype5;


    public float Speed;
    public Transform player;

    void Start()
    {
        healthscript = FindObjectOfType<PlayerHealthScript>();
        healthscript.health = healthscript.health - 1;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (EnemyPlant)
        {
            
        }
        if (Enemytype1)
        {
            
        }
    }
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if(collision.tag == "Player")
        {
            if (EnemyPlant)
            {
                AttackDelay = AttackDelay - Time.deltaTime;
                if (AttackDelay <= 0)
                {
                    healthscript.health = healthscript.health - 20;
                    print(healthscript.health);
                }
            }
            if (Enemytype1)
            {
                Vector3 direction = player.position - transform.position;
                direction.Normalize();
                movement = direction;
                moveEnemy(movement);
            }
        }
        
        
    }

    void moveEnemy(Vector2 direction)
    {

        rb.MovePosition((Vector2)transform.position + (direction * Speed * Time.deltaTime));
    }
}
