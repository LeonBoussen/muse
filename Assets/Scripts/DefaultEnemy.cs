using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatUsGround, whatIsPlayer;

    public Vector3 walkpoint;

    bool walkPointSet;
    bool alreadyAttacked;
    public bool playerInRange, playerInAttack;

    public float walkPointRange;
    public float timeBetweenAttacks;
    public float range, attackRange;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, range, whatIsPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange && !playerInAttack) patroling();
        if (playerInRange && !playerInAttack) chasePlayer();
        if (playerInRange && playerInAttack) attackPlayer();
    }

    void patroling()
    {
        if (walkPointSet) searchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkpoint);

        Vector3 distanceToWalkPoint = transform.position - walkpoint;
        if (distanceToWalkPoint.magnitude < 1)
            walkPointSet = false;
    }
    void searchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //float randomY = Random.Range(-walkPointRange, walkPointRange);
        //float randomZ = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatUsGround))
            walkPointSet = true;
    }

    void chasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void attackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        // attacking code here

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
