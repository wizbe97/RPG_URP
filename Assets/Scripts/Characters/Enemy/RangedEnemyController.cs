using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    // Public variables
    public float movementSpeed;
    private bool isShooting;
    public float lineOfSight;
    public float retreatDistance;
    public float fireRate;
    public float fireForce;
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Private variables
    private float timeSinceLastShot = 0f;
    private Animator animator;
    private Transform player;
    private bool isMoving = false; // Flag to track if the enemy is moving

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Action();
        SetAnimationDirection();
        timeSinceLastShot += Time.deltaTime;
    }

    void Action()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > lineOfSight)
        {
            MoveTowardsPlayer();
            isMoving = true; // Enemy is moving
        }
        else if (distanceToPlayer < lineOfSight && distanceToPlayer > retreatDistance)
        {
            if (!isMoving) // Only allow shooting when not moving
            {
                if (timeSinceLastShot >= 1 / fireRate)
                {
                    Shoot();
                    timeSinceLastShot = 0f;
                }
            }
            else
            {
                timeSinceLastShot = 0f; // Reset timeSinceLastShot when starting to move
            }
            isMoving = false; // Enemy is not moving
        }
        else if (distanceToPlayer < retreatDistance)
        {
            RetreatFromPlayer();
            isMoving = true; // Enemy is moving
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        animator.Play("Walk");
    }

    void RetreatFromPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, -movementSpeed * Time.deltaTime);
        animator.Play("Walk");
    }

    void Shoot()
    {
        animator.Play("Shoot");
        Vector2 direction = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireForce;
    }
    
    void OnShootEnd() { // Called at the end of the shoot animation to reset enemy state
        animator.Play("Idle");
    }

    void SetAnimationDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        animator.SetFloat("xMove", direction.x);
        animator.SetFloat("yMove", direction.y);
    }
}
