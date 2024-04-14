using UnityEngine;

public class EnemyBullet : Bullet
{
    private Coroutine damageCoroutine;
    private Vector3 gunSpawnPosition;
    private Animator animator;
    private Transform playerTransform; // Reference to the player's transform
    private bool hasCollided = false;

    protected override void Start()
    {
        base.Start();
        // Initialize gunSpawnPosition in the child class
        gunSpawnPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Only process collision if the bullet hasn't collided before
        if (hasCollided) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(gunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Player if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(calculatedDamage, 0f));
            }

            // Set the bullet as a child of the player
            transform.SetParent(player.transform);

            // Store a reference to the player's transform
            playerTransform = player.transform;

            // Trigger animation for sticking to the object
            animator.SetBool("hasCollided", true);

            // Set collision flag to true to prevent further collisions
            hasCollided = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(gunSpawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }

            // Set the bullet as a child of the enemy
            transform.SetParent(enemy.transform);

            // Set collision flag to true to prevent further collisions
            animator.SetBool("hasCollided", true);
            hasCollided = true;
        }
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