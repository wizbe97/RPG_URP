using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    private bool isAttacking = false;
    private int damage = 15;
    private Coroutine damageCoroutine;

    public override void Update()
    {
        base.Update();

        if (IsPlayerInLineOfSight())
        {
            MoveTowardsPlayer();
        }
        else
        {
            Wander();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Player player = FindObjectOfType<Player>();
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damage, 2f));
                isAttacking = true;
                UpdateAnimationState();
            }
            UpdateAnimationState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
            isAttacking = false;
        }
    }


    private void OnAttackEnd()
    {
        isAttacking = false;
        canMove = true;
        UpdateAnimationState();
    }

    public override void UpdateAnimationState()
    {
        int stateIdentifier;
        if (isAttacking == false)
        {
            if (isMoving)
            {
                stateIdentifier = !IsPlayerInLineOfSight() ? 1 : 2;
            }
            else
            {
                stateIdentifier = 3;
            }
        }
        else
        {
            stateIdentifier = 4;
        }

        switch (stateIdentifier)
        {
            case 1:
                SetAnimationDirection();
                CurrentState = EnemyStates.WALK;
                break;

            case 2:
                SetAnimationDirection();
                CurrentState = EnemyStates.RUN;
                break;

            case 3:
                SetAnimationDirection();
                CurrentState = EnemyStates.IDLE;
                break;
            case 4:
                SetAnimationDirection();
                CurrentState = EnemyStates.ATTACK;
                break;
        }
    }
}
