using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Action : MonoBehaviour
{
    private Inventory inventory;
    public bool isHoldingGun = false;
    [HideInInspector] public GameObject instantiatedCurrentItem;
    public Item currentItem;
    private PlayerGun playerGun;
    private bool overUI;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void Update()
    {
        overUI = IsPointerOverUI();
        if (overUI != true && Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            OnUseItem();
        }
    }

    private bool IsPointerOverUI()  // Check if the pointer is over a UI element
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void OnUseItem()
    {
        if (overUI || currentItem == null)
            return;

        if (currentItem.itemType == Item.ItemType.GUN)
        {
            if (currentItem.bullet != null)
            {
                // Check if the inventory has the required bullet type for this gun
                if (inventory.HasItem(currentItem.bullet))
                {
                    playerGun = FindObjectOfType<PlayerGun>();
                    if (playerGun != null && !PlayerGun.IsAnyGunShooting())
                    {
                        // Allow shooting only when the left mouse button is pressed down
                        playerGun.Shoot();
                    }
                }
            }
        }
    }


    private void OnDropItem()
    {
        currentItem = inventory.GetSelectedItem(true);
        if (currentItem != null && !PlayerGun.IsAnyGunShooting())
        {

            // Calculate drop direction based on the mouse position if holding a gun
            Vector3 dropDirection;
            if (isHoldingGun)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dropDirection = (mousePosition - transform.position).normalized;
            }
            else
            {
                // Otherwise, use the player's movement direction
                PlayerController playerController = GetComponent<PlayerController>();
                dropDirection = playerController.moveInput.normalized;
            }

            Vector3 dropPosition = transform.position + dropDirection * 2f;

            GameObject droppedItem = Instantiate(currentItem.droppedItem, dropPosition, Quaternion.identity);

            // Disable collider temporarily to prevent instant pickup
            if (droppedItem.TryGetComponent<Collider2D>(out var itemCollider))
            {
                itemCollider.enabled = false;
                StartCoroutine(EnableColliderAfterDelay(itemCollider));
            }

            DeactivateCurrentItem();
        }

    }

    private IEnumerator EnableColliderAfterDelay(Collider2D collider)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay as needed
        collider.enabled = true;
    }

    public void CurrentItem()
    {
        if (Inventory.Instance == null)
        {
            Debug.LogWarning("Inventory instance is null!");
            return;
        }

        currentItem = Inventory.Instance.GetSelectedItem(false);

        if (currentItem == null || !currentItem.holdable)
        {
            DeactivateCurrentItem();
            return;
        }

        if (currentItem.itemType == Item.ItemType.GUN)
        {
            isHoldingGun = true;

            Transform existingItemTransform = transform.Find(currentItem.itemName);
            instantiatedCurrentItem = existingItemTransform != null ? existingItemTransform.gameObject :
                                  Instantiate(currentItem.instantiatedPrefab, transform.position, Quaternion.identity);
            instantiatedCurrentItem.name = currentItem.itemName;
            instantiatedCurrentItem.transform.parent = transform;
            instantiatedCurrentItem.SetActive(true);
        }
        else
        {
            DeactivateCurrentItem();
        }
    }

    public void DeactivateCurrentItem()
    {
        if (instantiatedCurrentItem != null)
        {
            isHoldingGun = false;
            instantiatedCurrentItem.SetActive(false);
        }
    }
}