using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animationState.UpdateCharacterAnimationState(moveInput);

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
    private UpdateAnimationState animationState;
    private Inventory inventory;
    private Rigidbody2D rb;


    [SerializeField] private float moveDrag = 15f;
    [SerializeField] private float stopDrag = 25f;
    public bool isMoving = false;
    private bool canMove = true;

    public float movementSpeed = 1250f;
    [HideInInspector] public Vector2 moveInput = Vector2.zero;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = GetComponent<UpdateAnimationState>();
        inventory = Inventory.Instance;
    }

    private void FixedUpdate()
    {

        MoveCharacter();
    }


    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void MoveCharacter()
    {
        if (canMove == true && moveInput != Vector2.zero)
        {
            rb.AddForce(moveInput * movementSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            IsMoving = true;

        }
        else
        {
            IsMoving = false;
        }
    }
}