using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerHealthScript healthscript;
    private Rigidbody2D rb;
    private float timer = 0.0f;
    private bool canAttack = false;
    private bool SeenPlayer;
    private Vector2 TargetLocation;
    private Vector2 movement;
    public float enemyhealth;
    private bool haddied = false;
    public float AttackDelay;
    public int Damage;
    


    public bool EnemyPlant;
    

    public bool Enemytype1;


    public bool BeefSteak;


    public bool Hollorch;


    public bool Cactricry;


    public float Speed;
    private Transform player;

    void Start()
    {
        player = GameObject.Find("PlayerHolder").transform;

        healthscript = FindObjectOfType<PlayerHealthScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (enemyhealth <= 0 && !haddied)
        {
            Die();
        }

        if (canAttack)
        {
            attack();
        }
        if (BeefSteak)
        {
            
        }
    }
    private void FixedUpdate()
    {
        if (BeefSteak)
        {
            float y = transform.position.y;
            Vector3 pos = Vector3.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
            pos.y = y + 0.040f;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.CompareTag("Player"))
        {
            canAttack = true;

            if (EnemyPlant)
            {
                
            }

            if (Enemytype1)
            {
                
                moveEnemy(player.transform.position);
            }

            if (BeefSteak)
            {
                
            }

            if (Hollorch)
            {
                
            }

            if (Cactricry)
            {
                
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canAttack = false;
        }
    }

    void moveEnemy(Vector2 direction)
    {

        rb.MovePosition((Vector2)transform.position + (direction * Speed * Time.deltaTime));
    }

    void attack()
    {
        timer += Time.deltaTime;
        if (timer > AttackDelay)
        {
            healthscript.health = healthscript.health - Damage;
            print(healthscript.health);

            timer = 0.0f;
        }
    }
    public void TakeDamage(float amount)
    {
        enemyhealth -= amount;
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
