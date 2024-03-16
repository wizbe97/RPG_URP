using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour
{
    UpdateAnimationState animationState;

    public float movementSpeed = 3.0f;
    [HideInInspector] public Vector2 moveInput = Vector2.zero;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = GetComponent<UpdateAnimationState>();
    }

    void FixedUpdate()
    {
        if (!animationState.stateLock)
        {
            rb.velocity = moveInput * movementSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // if (currentStateValue != PlayerStates.ATTACK)
        // {
        if (moveInput != Vector2.zero)
        {
            if (movementSpeed <= 3.01)
            {
                animationState.currentState = UpdateAnimationState.PlayerStates.WALK;
                PlayerFaceMovementDirection();
            }
            else
            {
                animationState.currentState = UpdateAnimationState.PlayerStates.RUN;
                PlayerFaceMovementDirection();
            }
        }
        else
        {
            animationState.currentState = UpdateAnimationState.PlayerStates.IDLE;
        }
        // }

    }

    void PlayerFollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToMouse = (mousePosition - (Vector2)transform.position).normalized;
        animationState.animator.SetFloat("mouseX", playerToMouse.x);
        animationState.animator.SetFloat("mouseY", playerToMouse.y);
    }

    void PlayerFaceMovementDirection()
    {
        animationState.animator.SetFloat("xMove", moveInput.x);
        animationState.animator.SetFloat("yMove", moveInput.y);
    }
}
