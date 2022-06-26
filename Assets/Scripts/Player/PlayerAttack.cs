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
    private AudioSource playersource;
    private SoundManagerScript soundmanager;
    private void Start()
    {
        playersource = GetComponent<AudioSource>();
        soundmanager = FindObjectOfType<SoundManagerScript>();
        movementscript = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!isCoolDown && Input.GetKeyDown("joystick button 2") || Input.GetMouseButtonDown(0))
        {
            Attack();
            StartCoroutine(CoolDown(0.5f));
        }
    }
    
    private void Attack()
    {
        if (movementscript.IsGrounded())
        {
            //var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            /*
            if (Input.mousePosition.x < playerScreenPoint.x)
            {
                movementscript.sprite.flipX = true;
            }
            else
            {
                movementscript.sprite.flipX = false;
            }*/
            soundmanager.PlayPlayerSFX(playersource, 1);
            movementscript.PlayAnim("Player_Attack");
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
