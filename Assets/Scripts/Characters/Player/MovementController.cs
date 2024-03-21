using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour
{
    UpdateAnimationState animationState;
    private Inventory inventory;

    public float movementSpeed = 3.0f;
    public Item currentItem;
    [HideInInspector] public Vector2 moveInput = Vector2.zero;
    public bool isHoldingGun = false;


    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = GetComponent<UpdateAnimationState>();
        inventory = Inventory.Instance;
    }

    private void FixedUpdate()
    {
        if (!animationState.stateLock)
        {
            rb.velocity = moveInput * movementSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        animationState.UpdateCharacterAnimationState(moveInput);
    }

    private void Update() {
        CurrentItem();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        animationState.UpdateCharacterAnimationState(moveInput);
    }
    private void CurrentItem()
    {
        Debug.Log("Inventory.Instance is: " + (Inventory.Instance != null ? "not null" : "null"));

        currentItem = Inventory.Instance.GetSelectedItem(false);
        if (currentItem != null && currentItem.itemType == Item.ItemType.GUN)
        {
            isHoldingGun = true;
        }
        else
        {
            isHoldingGun = false;
        }
    }

}
