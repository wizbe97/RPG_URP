using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAnimationState : MonoBehaviour
{

    [HideInInspector] public bool stateLock = false;
    public Animator animator;
    private MovementController movementController;

    public PlayerStates currentStateValue;
    public enum PlayerStates
    {
        IDLE,
        IDLE_HOLDING_GUN,
        WALK,
        WALK_HOLDING_GUN,
        RUN,
        RUN_HOLDING_GUN,
        ATTACK,
        DIE
    }

    public void Start()
    {
        movementController = GetComponent<MovementController>();
    }
    public PlayerStates currentState
    {
        set
        {
            if (stateLock == false)
            {
                currentStateValue = value;
                switch (currentStateValue)
                {
                    case PlayerStates.IDLE:
                        animator.Play("Idle");
                        break;
                    case PlayerStates.IDLE_HOLDING_GUN:
                        animator.Play("Idle_Holding_Gun");
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Walk");
                        break;
                    case PlayerStates.WALK_HOLDING_GUN:
                        animator.Play("Walk_Holding_Gun");
                        break;
                    case PlayerStates.RUN:
                        animator.Play("Run");
                        break;
                    case PlayerStates.RUN_HOLDING_GUN:
                        animator.Play("Run_Holding_Gun");
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Attack");
                        stateLock = true;
                        break;
                    case PlayerStates.DIE:
                        animator.Play("Die");
                        stateLock = true;
                        break;
                }
            }
        }
    }

    public void UpdateCharacterAnimationState(Vector2 moveInput)
    {
        bool isMoving = moveInput != Vector2.zero;

        int stateIdentifier;

        if (isMoving)
        {
            if (movementController.isHoldingGun == true) // Check if the player is holding a gun
            {
                // If holding a gun, check if the movement speed is greater than or equal to 3
                stateIdentifier = movementController.movementSpeed >= 3 ? 1 : 2;
            }
            else
            {
                // If not holding a gun, check if the movement speed is greater than or equal to 3
                stateIdentifier = movementController.movementSpeed >= 3 ? 3 : 4;
            }
        }
        else
        {
            // If not moving, check if holding a gun
            stateIdentifier = movementController.isHoldingGun ? 5 : 6;
        }

        switch (stateIdentifier)
        {
            case 1: // PLAYER IS MOVING AND HOLDING A GUN, MOVE SPEED IS GREATER THAN 3
                PlayerFollowMouse();
                currentState = PlayerStates.RUN_HOLDING_GUN;
                break;

            case 2: // PLAYER IS MOVING AND HOLDING A GUN, MOVE SPEED IS LESS THAN 3
                PlayerFollowMouse();
                currentState = PlayerStates.WALK_HOLDING_GUN;
                break;

            case 3: // PLAYER IS MOVING, NOT HOLDING A GUN, MOVE SPEED IS GREATER THAN 3
                PlayerFaceMovementDirection();
                currentState = PlayerStates.RUN;
                break;

            case 4: // PLAYER IS MOVING, NOT HOLDING A GUN, MOVE SPEED IS LESS THAN 3
                PlayerFaceMovementDirection();
                currentState = PlayerStates.WALK;
                break;

            case 5: // PLAYER IS IDLE AND HOLDING A GUN
                PlayerFollowMouse();
                currentState = PlayerStates.IDLE_HOLDING_GUN;
                break;

            case 6: // PLAYER IS IDLE AND NOT HOLDING A GUN
                currentState = PlayerStates.IDLE;
                break;
        }
    }
    void PlayerFollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToMouse = (mousePosition - (Vector2)transform.position).normalized;
        animator.SetFloat("mouseX", playerToMouse.x);
        animator.SetFloat("mouseY", playerToMouse.y);
    }

    void PlayerFaceMovementDirection()
    {
        animator.SetFloat("xMove", movementController.moveInput.x);
        animator.SetFloat("yMove", movementController.moveInput.y);
    }
}
