using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    // Define public variables
    public float lineOfSight = 5f;
    public float shootingRange = 3f;
    public float moveSpeed = 2f;
    public float fireRate = 1f;
    public float fireForce = 10f;
    public GameObject bulletPrefab;
    public Transform[] firePoints; // Array of fire points for different directions

    // Define private variables
    private float nextFireTime;
    private Animator animator;
    private Transform player;
    private bool playerInLineOfSight = false;
    private bool playerInShootingRange = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        animator = GetComponent<Animator>();
        nextFireTime = Time.time;
    }


    void Update()
    {
        if (player == null) return; // Check if the player object is null and return if it is

        SetAnimationDirection();

        if (Vector2.Distance(transform.position, player.position) < lineOfSight)
        {
            playerInLineOfSight = true;

            if (Vector2.Distance(transform.position, player.position) <= shootingRange)
            {
                playerInShootingRange = true;
            }
            else
            {
                playerInShootingRange = false;
            }
        }
        else
        {
            playerInLineOfSight = false;
            playerInShootingRange = false;
            animator.Play("Idle");
        }

        if (playerInLineOfSight && !playerInShootingRange)
        {
            MoveTowardsPlayer();
        }

        if (playerInShootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void MoveTowardsPlayer()
    {
        if (!playerInShootingRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            animator.Play("Walk");
        }
    }

    void Shoot()
    {
        animator.Play("Shoot");
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        // Determine the appropriate fire point based on direction
        int firePointIndex = GetFirePointIndex(direction);
        Transform selectedFirePoint = firePoints[firePointIndex];

        GameObject bullet = Instantiate(bulletPrefab, selectedFirePoint.position, Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireForce;
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

    void OnShootEnd()
    {
        animator.Play("Idle");
    }

    void SetAnimationDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        animator.SetFloat("xMove", direction.x);
        animator.SetFloat("yMove", direction.y);
    }

    // Draw gizmos to visualize line of sight and shooting range
    private void OnDrawGizmosSelected()
    {
        // Draw line of sight circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);

        // Draw shooting range circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
