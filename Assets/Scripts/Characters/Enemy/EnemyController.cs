using UnityEngine;
public abstract class EnemyController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform player;
    public float lineOfSight = 15f;
    public float idleTime = 2f;
    public float moveSpeed = 12500;
    public float wanderSpeed = 10000f; // Speed for wandering
    public float wanderTime = 5f; // Time interval for changing wander direction

    [SerializeField] private float moveDrag = 15f;
    [SerializeField] private float stopDrag = 25f;

    public bool canMove = true;

    public bool isMoving = false;
    private Vector2 wanderDirection;
    private float nextWanderTime;

    public EnemyStates currentStateValue;
    public enum EnemyStates
    {
        IDLE,
        WALK,
        DEATH,
        SHOOT
    }
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
        UpdateEnemyState();
        SetAnimationDirection();
        if (player.transform == null)
        {
            Wander();
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
        if (Time.time > nextWanderTime)
        {
            // Change wander direction after wanderTime interval
            wanderDirection = Random.insideUnitCircle.normalized;
            nextWanderTime = Time.time + wanderTime;
        }

        rb.AddForce(wanderSpeed * Time.deltaTime * wanderDirection, ForceMode2D.Force);
        IsMoving = true;
    }

    public virtual void DisableMovement()
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
            Vector2 direction = rb.velocity.normalized;
            animator.SetFloat("xMove", direction.x);
            animator.SetFloat("yMove", direction.y);
        }
    }

    public virtual EnemyStates CurrentState
    {
        set
        {
            currentStateValue = value;
            switch (currentStateValue)
            {
                case EnemyStates.IDLE:
                    animator.Play("Idle");
                    break;
                case EnemyStates.WALK:
                    animator.Play("Walk");
                    break;
                case EnemyStates.DEATH:
                    animator.Play("Death");
                    break;
                case EnemyStates.SHOOT:
                    animator.Play("Shoot");
                    break;
            }

        }
    }

    public virtual void UpdateEnemyState()
    {
        if (rb.velocity == Vector2.zero)
        {
            CurrentState = EnemyStates.IDLE;
        }
        else
        {
            CurrentState = EnemyStates.WALK;
        }
    }
}
