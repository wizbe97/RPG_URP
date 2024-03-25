using System.Collections;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    private Coroutine damageCoroutine;
    private Vector3 shotgunSpawnPosition;

    protected override void Start()
    {
        base.Start();
        // Initialize shotgunSpawnPosition in the child class
        shotgunSpawnPosition = transform.position;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(shotgunSpawnPosition, transform.position);
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
        // Calculate damage based on travel distance
        float t = Mathf.Clamp01(travelDistance / maxTravelDistance);
        int damage = Mathf.RoundToInt(Mathf.Lerp(maximumBulletDamage, minimumBulletDamage, t));
        return damage;
    }
}
