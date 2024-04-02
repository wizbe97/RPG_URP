﻿using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RangedEnemyCharacter : Character
{
    public int damageStrength;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    [SerializeField] private FloatingHealthBar healthBar;
    public GameObject floatingDamage;
    Coroutine damageCoroutine;

    float hitPoints;

    private void OnEnable()
    {
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(hitPoints, maxHitPoints);

    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints -= damage;
            healthBar.UpdateHealthBar(hitPoints, maxHitPoints);
            GameObject damageNumber = Instantiate(floatingDamage, transform.position, Quaternion.identity) as GameObject;
            damageNumber.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
            if (hitPoints <= 0)
            {
                KillCharacter();
                break;
            }

            if (interval > 0)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }


    // Damage Player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            // Only call DamageCharacter on the player if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    // Kill Character (Enemy)
    public override void KillCharacter()
    {
        animator.Play("Death");
        capsuleCollider2D.enabled = false;

        RangedEnemyController rangedEnemyController = GetComponent<RangedEnemyController>();
        if (rangedEnemyController != null)
        {
            rangedEnemyController.DisableMovement(); // Call a method to disable movement
        }

    }

    public void OnDeathEnd()
    {
        base.KillCharacter();
    }

}