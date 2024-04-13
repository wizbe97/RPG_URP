using UnityEngine;

public class WeaponBullet : Bullet
{
    private Coroutine damageCoroutine;
    private Vector3 gunSpawnPosition;

    protected override void Start()
    {
        base.Start();
        // Initialize gunSpawnPosition in the child class
        gunSpawnPosition = transform.position;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyCharacter enemy = other.gameObject.GetComponent<EnemyCharacter>();

            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(gunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }
        }
        base.OnTriggerEnter2D(other);
    }
}
