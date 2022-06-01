using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isCoolDown = false;
    public Animator anim;
    public float range;
    public LayerMask enemylayer;
    public GameObject attackpoint;
    public float attack_damage;
    private PlayerMovement movementscript;
    private void Start()
    {
        movementscript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!isCoolDown && Input.GetMouseButtonDown(0))
        {
            Attack();
            StartCoroutine(CoolDown(0.5f));
        }
    }
    //voor nu aleen de attack funtie toegevoegd, om de code te implementen moet enemy script af zijn
    private void Attack()
    {
        if (movementscript.IsGrounded())
        {
            anim.Play("Player_Attack");
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.transform.position, range, enemylayer);
            foreach (Collider2D enemy in hitenemies)
            {
                enemy.GetComponentInParent<Enemy>().TakeDamage(attack_damage);
            }
        }
    }
    //cooldown voor de attack dat je het niet kan spammen, (kan aangepast worden)
    IEnumerator CoolDown(float amount)
    {
        isCoolDown = true;
        yield return new WaitForSecondsRealtime(amount);
        isCoolDown = false;
    }

}
