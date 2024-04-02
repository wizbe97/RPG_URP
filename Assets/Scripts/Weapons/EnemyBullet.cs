using System.Collections;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private Coroutine damageCoroutine;
    private Vector3 gunSpawnPosition;
    private Animator animator;
    private Rigidbody2D rb2D;

    protected override void Start()
    {
        base.Start();
        // Initialize gunSpawnPosition in the child class
        gunSpawnPosition = transform.position;
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(gunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Player if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(calculatedDamage, 0f));
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            RangedEnemyCharacter enemy = collision.gameObject.GetComponent<RangedEnemyCharacter>();
            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(gunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }
        }

        rb2D.velocity = Vector2.zero;
        rb2D.simulated = false;

        // Set the collided object as the parent of the bullet
        transform.parent = collision.transform;

        // Trigger animation for sticking to the object
        animator.SetBool("hasCollided", true);
    }


    protected override int CalculateDamage(float travelDistance)
    {
        // Calculate damage based on travel distance
        float t = Mathf.Clamp01(travelDistance / maxTravelDistance);
        int damage = Mathf.RoundToInt(Mathf.Lerp(maximumBulletDamage, minimumBulletDamage, t));
        return damage;
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
