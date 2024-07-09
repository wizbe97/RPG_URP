using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAnimationState : MonoBehaviour
{
    [HideInInspector] public bool stateLock = false;
    public Animator animator;
    private PlayerController playerController;
    private Action action;
    private Player player;
    public PlayerStates currentStateValue;
    public enum PlayerStates
    {
        IDLE,
        IDLE_HOLDING_GUN,
        WALK,
        WALK_HOLDING_GUN,
        RUN,
        RUN_HOLDING_GUN,
        DASH,
        ATTACK,
        DIE
    }

    public void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        action = GetComponent<Action>();
        player = GetComponent<Player>();
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
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.IDLE_HOLDING_GUN:
                        animator.Play("Idle_Holding_Gun");
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Walk");
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.WALK_HOLDING_GUN:
                        animator.Play("Walk_Holding_Gun");
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.RUN:
                        animator.Play("Run");
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.RUN_HOLDING_GUN:
                        animator.Play("Run_Holding_Gun");
                        playerController.isDashing = false;
                        stateLock = false;
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Attack");
                        playerController.isDashing = false;
                        stateLock = true;
                        break;
                    case PlayerStates.DASH:
                        animator.Play("Dash");
                        playerController.isDashing = true;
                        stateLock = true;
                        break;
                    case PlayerStates.DIE:
                        animator.Play("Die");
                        playerController.isDashing = false;
                        stateLock = true;
                        break;
                }
            }
        }
    }

    public void UpdateCharacterAnimationState(Vector2 moveInput)
    {
        int stateIdentifier;
        if (playerController.isDashing)
        {
            stateIdentifier = 7;
        }
        else if (playerController.isMoving)
        {
            if (action.isHoldingGun)
            {
                stateIdentifier = playerController.movementSpeed >= 2000 ? 1 : 2;
            }
            else
            {
                stateIdentifier = playerController.movementSpeed >= 2000 ? 3 : 4;
            }
        }
        else
        {
            stateIdentifier = action.isHoldingGun ? 5 : 6;
        }

        switch (stateIdentifier)
        {
            case 1:
                PlayerFollowMouse();
                currentState = PlayerStates.RUN_HOLDING_GUN;
                break;
            case 2:
                PlayerFollowMouse();
                currentState = PlayerStates.WALK_HOLDING_GUN;
                break;
            case 3:
                PlayerFaceMovementDirection();
                currentState = PlayerStates.RUN;
                break;
            case 4:
                PlayerFaceMovementDirection();
                currentState = PlayerStates.WALK;
                break;
            case 5:
                PlayerFollowMouse();
                currentState = PlayerStates.IDLE_HOLDING_GUN;
                break;
            case 6:
                currentState = PlayerStates.IDLE;
                break;
            case 7:
                PlayerFaceMovementDirection();
                currentState = PlayerStates.DASH;
                break;
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
            animator.SetFloat("xMove", playerController.moveInput.x);
            animator.SetFloat("yMove", playerController.moveInput.y);
        }
    }
}
