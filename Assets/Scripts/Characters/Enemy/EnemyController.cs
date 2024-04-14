using UnityEngine;

public abstract class EnemyController : MonoBehaviour
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

    public EnemyStates currentStateValue;
    public enum EnemyStates
    {
        IDLE,
        WALK,
        RUN,
        ATTACK,
        SHOOT,
        DIE
    }

    public EnemyStates CurrentState
    {
        set
        {
            currentStateValue = value;
            switch (currentStateValue)
            {
                case EnemyStates.IDLE:
                    animator.Play("Idle");
                    canMove = true;
                    break;
                case EnemyStates.WALK:
                    animator.Play("Walk");
                    canMove = true;
                    break;
                case EnemyStates.RUN:
                    animator.Play("Run");
                    canMove = true;
                    break;
                case EnemyStates.ATTACK:
                    animator.Play("Attack");
                    rb.velocity = Vector2.zero;
                    canMove = false;
                    break;
                case EnemyStates.SHOOT:
                    animator.Play("Shoot");
                    rb.velocity = Vector2.zero;
                    canMove = false;
                    break;
                case EnemyStates.DIE:
                    animator.Play("Die");
                    canMove = false;
                    break;
            }
        }
    }
    public bool IsMoving
    {
        set
        {
            isMoving = value;
            UpdateAnimationState();

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
        if (player == null)
        {
            Wander(); // Continue wandering if player is not present
        }
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

        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(moveSpeed * Time.deltaTime * direction, ForceMode2D.Force);
        rb.drag = moveDrag; // Apply drag when moving
        IsMoving = true;
    }

    public virtual void Wander()
    {
        if (!canMove) return;
        if (Time.time > nextWanderTime)
        {
            // Change wander direction after wanderTime interval
            wanderDirection = Random.insideUnitCircle.normalized;
            nextWanderTime = Time.time + wanderTime;
        }
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, wanderSpeed);

        // Move with the current direction
        rb.AddForce(wanderSpeed * Time.deltaTime * wanderDirection, ForceMode2D.Force);
        IsMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MapCollisions"))
        {
            // If collided with a wall, find a new direction
            FindNewWanderDirection();
        }
    }

    // Find a new direction to wander after collision with a wall
    private void FindNewWanderDirection()
    {
        // Generate a new random direction
        Vector2 newDirection = Random.insideUnitCircle.normalized;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, wanderSpeed);
        rb.AddForce(wanderSpeed * Time.deltaTime * newDirection, ForceMode2D.Force);
        wanderDirection = newDirection;

        // Update the wander time to prevent immediate direction changes
        nextWanderTime = Time.time + wanderTime;
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
    public abstract void UpdateAnimationState();
}
