using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lineOfSight = 15f;
    public float moveSpeed = 12500;
    public float wanderSpeed = 10000f; // Speed for wandering
    public float wanderTime = 5f; // Time interval for changing wander direction

    public float moveDrag = 15f;
    public float stopDrag = 25f;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform player;
    [HideInInspector] public bool isMoving = false;

    [HideInInspector] public Vector2 wanderDirection;
    [HideInInspector] public float nextWanderTime;
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

    public virtual void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        SetAnimationDirection();

    }
    public virtual bool IsPlayerInLineOfSight()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= lineOfSight;
    }

    public virtual void MoveTowardsPlayer()
    {
        if (!canMove) return;
        animator.Play("Walk");

        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(moveSpeed * Time.deltaTime * direction, ForceMode2D.Force);
        rb.drag = moveDrag; // Apply drag when moving
        IsMoving = true;
    }

    public virtual void Wander()
    {
        if (Time.time > nextWanderTime)
        {
            // Change wander direction after wanderTime interval
            wanderDirection = Random.insideUnitCircle.normalized;
            nextWanderTime = Time.time + wanderTime;
        }

        rb.AddForce(wanderSpeed * Time.deltaTime * wanderDirection, ForceMode2D.Force);
        animator.Play("Walk");
        IsMoving = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public virtual void SetAnimationDirection()
    {
        if (IsPlayerInLineOfSight())
        {
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("xMove", direction.x);
            animator.SetFloat("yMove", direction.y);
        }
        else
        {
            Vector2 direction = rb.velocity.normalized; // Assuming rb is the Rigidbody2D component
            animator.SetFloat("xMove", direction.x);
            animator.SetFloat("yMove", direction.y);
        }
    }

    void OnShootEnd()
    {
        animator.Play("Idle");
    }
}
