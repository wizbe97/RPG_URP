using System.Collections;
using System.Collections.Generic;
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
    private Action action;
    private Rigidbody2D rb;
    [SerializeField] private float moveDrag = 15f;
    [SerializeField] private float stopDrag = 25f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 3f;
    private bool dashOnCooldown = false; 
    public bool isMoving = false;
    private readonly bool canMove = true;

    public float movementSpeed = 1250f;
    [HideInInspector] public Vector2 moveInput = Vector2.zero;
    [HideInInspector] public bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = GetComponent<UpdateAnimationState>();
        action = GetComponent<Action>();
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
            rb.AddForce(movementSpeed * Time.fixedDeltaTime * moveInput, ForceMode2D.Force);
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void OnDash()
    {
        if (!dashOnCooldown && moveInput.magnitude > 0)
        {
            Vector2 dashDirection = moveInput.normalized;
            rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
            isDashing = true;
            dashOnCooldown = true;
            animationState.UpdateCharacterAnimationState(moveInput);
            action.DeactivateCurrentItem();
            StartCoroutine(DashCooldown()); 
        }
    }

    public void OnDashEnd()
    {
        isDashing = false;
        animationState.stateLock = false; 
        animationState.UpdateCharacterAnimationState(moveInput);
        action.CurrentItem();
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    private void OnOpenInventory()
    {
        Inventory inventory = Inventory.Instance;

        GameObject backpack = inventory.transform.Find("Backpack").gameObject;

        backpack.SetActive(!backpack.activeSelf);
    }
}
