using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    public EnemyStates currentStateValue;
    public enum EnemyStates
    {
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DEATH
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
                    break;
                case EnemyStates.WALK:
                    animator.Play("Walk");
                    break;
                case EnemyStates.RUN:
                    animator.Play("Run");
                    break;
                case EnemyStates.DEATH:
                    animator.Play("Death");
                    break;
            }
        }
    }
    public override void Update()
    {
        base.Update();
        // Check if the player object is null

        if (IsPlayerInLineOfSight())
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Wander if player is not in line of sight
            Wander();
        }

    }

    public override void UpdateAnimationState()
    {
        int stateIdentifier;
        if (isMoving)
        {
            // If moving, check if player is in line of sight. 
            stateIdentifier = !IsPlayerInLineOfSight() ? 1 : 2;
        }
        else
        {
            // Idle
            stateIdentifier = 3;
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
        }
    }
}
