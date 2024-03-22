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
    public GameObject shotgunPrefab;
    private GameObject instantiatedShotgun;


    Rigidbody2D rb;
    void Awake()
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
        CurrentItem();
    }


    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        animationState.UpdateCharacterAnimationState(moveInput);
    }
    private void CurrentItem()
    {
        if (Inventory.Instance != null)
        {
            currentItem = Inventory.Instance.GetSelectedItem(false);
            if (currentItem != null && currentItem.itemType == Item.ItemType.GUN)
            {
                isHoldingGun = true;
                // If shotgun prefab is not instantiated, instantiate it and set its parent to the player
                if (instantiatedShotgun == null && shotgunPrefab != null)
                {
                    instantiatedShotgun = Instantiate(shotgunPrefab, transform.position, Quaternion.identity);
                    instantiatedShotgun.transform.parent = transform; // Set player as parent
                }
                // If instantiated, set active
                if (instantiatedShotgun != null)
                {
                    instantiatedShotgun.SetActive(true);
                }
            }
            else
            {
                isHoldingGun = false;
                // If instantiated, set inactive
                if (instantiatedShotgun != null)
                {
                    instantiatedShotgun.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("Inventory instance is null!");
        }
    }
}