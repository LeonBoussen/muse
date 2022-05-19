using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isCoolDown = false;

    //public Animator anim;
    public float range;
    public LayerMask enemylayer;
    public GameObject attackpoint;
    private void Update()
    {
        if (!isCoolDown && Input.GetMouseButtonDown(0))
        {
            Attack(0, false);
            StartCoroutine(CoolDown(0.5f));
        }
    }
    //voor nu aleen de attack funtie toegevoegd, om de code te implementen moet enemy script af zijn
    private void Attack(float damage, bool setup)
    {
        //animator.play("attackanimation");
        if (setup)
        {
            Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.transform.position, range, enemylayer);
            foreach (Collider2D enemy in hitenemies)
            {
                //enemy.getcomponent<EnemyScript>().takedamage(damage);
            }
        }
        else
        {
            Debug.Log("test");
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
