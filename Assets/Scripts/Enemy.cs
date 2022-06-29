using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    private PlayerHealthScript healthscript;
    private Rigidbody2D rb;

    public float timer = 0.0f;
    private bool canAttack = false;
    public float enemyhealth;
    public bool hasdied = false;
    public float AttackDelay;
    public float jumpheight;
    public int Damage;
    public float animcooldown;

    private Animator anim;
    public Collider2D attack_collider;
    public Collider2D collision_collider;
    private PlayerCheck player_check;
    public SpriteRenderer sprite;
    private SoundManagerScript soundmanager;
    private AudioSource enemysource;

    public bool isMoving = false;
    private bool overideanimation = false;
    private bool Morphed = false;

    public AnimationClip[] Animations;
    public enum enemytype
    {
        Happie,
        Spinnygurl,
        Beefsteak,
        Chompstool,
        Daisyclaw,
        Cactricry,
        Hollorch
    }
    public enemytype current_enemytype;
    public float Speed;
    private Transform player;

    void Start()
    {
        soundmanager = FindObjectOfType<SoundManagerScript>();
        enemysource = GetComponent<AudioSource>();
        player_check = GetComponentInChildren<PlayerCheck>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        healthscript = FindObjectOfType<PlayerHealthScript>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (enemyhealth <= 0 && !this.hasdied)
        {
            Die();
            this.hasdied = true;
        }
        if (canAttack && !hasdied)
        {
            attack();
        }
        if (!this.hasdied)
        {
            if (isMoving)
            {
                if (current_enemytype != enemytype.Happie && current_enemytype != enemytype.Hollorch && current_enemytype != enemytype.Daisyclaw)
                {
                    PlayAnimation(1);
                }
                else
                {
                    PlayAnimation(0);
                }
            }
            else
            {
                PlayAnimation(0);
            }
        }
    }
    private void FixedUpdate()
    {
        if (transform.position.x > player.transform.position.x)
        {
            if (!this.hasdied)
            {
                sprite.flipX = false;
            }
            
        }  
        else if (transform.position.x < player.transform.position.x)
        {
            if (!this.hasdied)
            {
                sprite.flipX = true;
            }
        }
        if (current_enemytype != enemytype.Happie && current_enemytype != enemytype.Hollorch && current_enemytype != enemytype.Daisyclaw)
        {
            if (isMoving && !hasdied)
            {
                float y = transform.position.y;
                Vector3 pos = Vector3.MoveTowards(transform.position, player.position, Speed * Time.deltaTime);
                pos.y = y + jumpheight;
                transform.position = pos;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (this.hasdied)
        {
            return;
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                canAttack = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.hasdied)
        {
            return;
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                canAttack = false;
            }
        }
    }
    

    void attack()
    {
        timer += Time.deltaTime;
        if (timer > AttackDelay)
        {
            PlayAnimation(2);
            isMoving = false;
            StartCoroutine(MovingCooldown(.5f));
            StartCoroutine(Anim_Cooldown());
            if (!player.GetComponent<PlayerMovement>().isBlocking)
            {
                StartCoroutine(DoDamage(.2f));
            }
            switch (current_enemytype)
            {
                case enemytype.Beefsteak:
                    soundmanager.BeefsteakSFX(enemysource, 1);
                    break;
                case enemytype.Chompstool:
                    soundmanager.ChompstoolSFX(enemysource, 1);
                    break;
                default:
                    soundmanager.PlayEnemySFX(enemysource, 1);
                    break;
            }
            
            timer = 0.0f;
        }
    }
    public void TakeDamage(float amount)
    {
        if (!hasdied)
        {
            enemyhealth -= amount;
            isMoving = false;
            PlayAnimation(3);
            StartCoroutine(MovingCooldown(.5f));
            StartCoroutine(Anim_Cooldown());
            StartCoroutine(TakeDamageSFX());
        }
    }
    private void Die()
    {
        switch (current_enemytype)
        {
            case enemytype.Beefsteak:
                soundmanager.BeefsteakSFX(enemysource, 2);
                break;
            default:
                soundmanager.PlayEnemySFX(enemysource, 2);
                break;
        }
        PlayAnimation(4);
        StartCoroutine(Anim_Cooldown());
        attack_collider.enabled = false;
        collision_collider.enabled = false;
        isMoving = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        if (player_check != null)
        {
            Destroy(player_check.gameObject);
        }
        
    }
    private void PlayAnimation(int animation_type) // 0 = idle |  1 = walk |  2 = attack |  3 = hurt | 4 = death | 5 = bloem animation
    {
        if (animation_type == 2 || animation_type == 3 || animation_type == 4 || animation_type == 5)
        {
            anim.Play(Animations[animation_type].name);
            overideanimation = true;
        }
        else
        {
            if (overideanimation)
            {
                return;
            }
            else
            {
                if (Animations[animation_type] != null)
                {
                    anim.Play(Animations[animation_type].name);
                }
            }
        }   
    }
    IEnumerator Anim_Cooldown()
    {
        yield return new WaitForSeconds(animcooldown);
        overideanimation = false;
    }
    IEnumerator MovingCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        isMoving = true;
    }
    public IEnumerator Morph_Anim()
    {
        if (Morphed)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            PlayAnimation(5);
            soundmanager.PlayEnemySFX(enemysource, 5);
            PlayerHealthScript HealthScript = FindObjectOfType<PlayerHealthScript>();
            float a = HealthScript.health + 5;
            if (a < 105)
            {
                HealthScript.health += 5;
            }
            Morphed = true;
        }
    }
    IEnumerator DoDamage(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        healthscript.health = healthscript.health - Damage;
    }
    IEnumerator TakeDamageSFX()
    {
        yield return new WaitForSeconds(0.4f);
        soundmanager.PlayEnemySFX(enemysource, 0);
    }
}
