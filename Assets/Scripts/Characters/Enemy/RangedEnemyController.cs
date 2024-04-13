using UnityEngine;

public class RangedEnemyController : EnemyController
{
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float fireForce = 10f;
    public float shootingRange = 10f;
    public float fireRate = 1f; // Rate of fire in shots per second
    private float nextFireTime = 0f; // Time when the enemy can fire next

    bool IsMoving
    {
        set
        {
            isMoving = value;

            if (isMoving)
            {
                rb.drag = moveDrag;
            }
            else
            {
                rb.drag = stopDrag;
            }
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        // Check if the player object is null
        if (player == null)
        {
            Wander(); // Continue wandering if player is not present
        }

        else if (IsPlayerInLineOfSight())
        {
            if (IsPlayerInShootingRange())
            {
                // Stop moving when in shooting range
                rb.velocity = Vector2.zero;
            }
            else
            {
                // Move towards player if not in shooting range
                MoveTowardsPlayer();
            }
        }
        else
        {
            // Wander if player is not in line of sight
            Wander();
        }

        // Shoot only if player is in shooting range and enough time has passed since the last shot
        if (IsPlayerInShootingRange() && Time.time >= nextFireTime)
        {
            Shoot();
            // Calculate the next allowed time for firing
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public bool IsPlayerInShootingRange()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= shootingRange;
    }

    public void Shoot()
    {
        if (!canMove)
            return;
        animator.Play("Shoot");
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        // Determine the appropriate fire point based on direction
        int firePointIndex = GetFirePointIndex(direction);
        Transform selectedFirePoint = firePoints[firePointIndex];

        GameObject bullet = Instantiate(bulletPrefab, selectedFirePoint.position, Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireForce;
        IsMoving = false;
    }

    int GetFirePointIndex(Vector2 direction)
    {
        // Determine the index of the fire point based on direction
        int index = 0;

        if (direction.x > 0.5f)
        {
            if (direction.y > 0.5f)
            {
                index = 1; // Right Up
            }
            else if (direction.y < -0.5f)
            {
                index = 7; // Right Down
            }
            else
            {
                index = 0; // Right
            }
        }
        else if (direction.x < -0.5f)
        {
            if (direction.y > 0.5f)
            {
                index = 3; // Left Up
            }
            else if (direction.y < -0.5f)
            {
                index = 5; // Left Down
            }
            else
            {
                index = 4; // Left
            }
        }
        else
        {
            if (direction.y > 0.5f)
            {
                index = 2; // Up
            }
            else if (direction.y < -0.5f)
            {
                index = 6; // Down
            }
        }

        return index;
    }

    public override void Wander()
    {
        base.Wander();
    }

    void OnShootEnd()
    {
        animator.Play("Idle");
    }
}
