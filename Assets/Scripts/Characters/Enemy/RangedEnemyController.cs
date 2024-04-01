using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemyController : MonoBehaviour
{
    public float movementSpeed;
    public float lineOfSight;
    public float retreatDistance;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public float fireForce;
    private bool isMoving;
    private bool isAttacking;
    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;

    public Transform firePoint;
    public GameObject bulletPrefab; // This should be assigned in the Inspector

    public enum EnemyStates
    {
        IDLE,
        WALK,
        SHOOT,
        DIE
    }

    public EnemyStates currentState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        timeBetweenShots = startTimeBetweenShots;
        currentState = EnemyStates.IDLE;
    }

    void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }

        if (timeBetweenShots <= 0 && !isMoving)
        {
            Shoot();
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) > lineOfSight)
        {
            isMoving = true;
            currentState = EnemyStates.WALK;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * movementSpeed;
            SetAnimationState(direction, movementSpeed);
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            isMoving = true;
            currentState = EnemyStates.WALK;
            Vector2 direction = (transform.position - player.position).normalized;
            rb.velocity = direction * movementSpeed;
            SetAnimationState(direction, movementSpeed);
        }
        else
        {
            isMoving = false;
            isAttacking = false;
            rb.velocity = Vector2.zero;
            currentState = EnemyStates.IDLE;
            SetAnimationState(Vector2.zero, 0);
        }
    }

    void Shoot()
    {
        // Calculate direction to the player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Calculate rotation angle from direction, adding 90 degrees for left rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));

        // Apply force in the direction of the player
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireForce;
    }

    IEnumerator FireRoutine()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this delay as needed
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = direction * fireForce;
        isAttacking = false;
        currentState = EnemyStates.IDLE;
    }

    void SetAnimationState(Vector2 direction, float speed)
    {
        if (isAttacking == false)
        {
            if (isMoving == true)
            {
                animator.Play("Walk");
                SetAnimationDirection();
            }
            else
            {
                animator.Play("Idle");
                SetAnimationDirection();
            }
        }
        else
        {
            animator.Play("Shoot");
            SetAnimationDirection();
        }
    }

    void SetAnimationDirection()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float xMove = 0f;
        float yMove = -1f; // Default: facing down

        if (Vector2.Distance(transform.position, player.position) <= lineOfSight)
        {
            // If player is within line of sight, update animation direction
            xMove = direction.x;
            yMove = direction.y;
        }

        animator.SetFloat("xMove", xMove);
        animator.SetFloat("yMove", yMove);
    }
}
