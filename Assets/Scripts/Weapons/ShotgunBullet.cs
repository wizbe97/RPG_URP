using System.Collections;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    private Coroutine damageCoroutine;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(spawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }
        }
        base.OnCollisionEnter2D(collision);
    }

    protected override int CalculateDamage(float travelDistance)
    {
        return base.CalculateDamage(travelDistance);
    }

    protected override IEnumerator DestroyAfterDelay(float delay)
    {
        return base.DestroyAfterDelay(delay);
    }
}
