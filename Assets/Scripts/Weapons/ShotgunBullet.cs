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

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            RangedEnemyCharacter enemy = other.gameObject.GetComponent<RangedEnemyCharacter>();

            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(shotgunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }
        }
        base.OnTriggerEnter2D(other);
    }

    protected override int CalculateDamage(float travelDistance)
    {
        // Calculate damage based on travel distance
        float t = Mathf.Clamp01(travelDistance / maxTravelDistance);
        int damage = Mathf.RoundToInt(Mathf.Lerp(maximumBulletDamage, minimumBulletDamage, t));
        return damage;
    }
}
