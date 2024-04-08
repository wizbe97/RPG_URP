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

    private bool IsPointerOverUI()
    {
        // Check if the pointer is over a UI element
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void OnUseItem()
    {
        if (overUI || currentItem == null)
            return;

        if (currentItem.itemType == Item.ItemType.GUN)
        {
            playerGun = FindObjectOfType<PlayerGun>();
            if (playerGun != null && !PlayerGun.IsAnyGunShooting())
            {
                playerGun.Shoot();
            }
        }
    }

    private void OnDropItem()
    {
        currentItem = inventory.GetSelectedItem(true);
        if (currentItem != null)
        {
            Debug.Log("Dropping item: " + currentItem);
        }
        else
        {
            Debug.Log("No item in slot");
        }
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
            if (existingItemTransform != null)
            {
                // If an item with the same name exists, set it as the current item and activate it
                instantiatedCurrentItem = existingItemTransform.gameObject;
            }
            else
            {
                // If no item with the same name exists, instantiate a new one
                instantiatedCurrentItem = Instantiate(currentItem.instantiatedPrefab, transform.position, Quaternion.identity);
                instantiatedCurrentItem.name = currentItem.itemName; // Set the name to make it easier to find later
                instantiatedCurrentItem.transform.parent = transform; // Set player as parent
            }

            // Activate the current item
            instantiatedCurrentItem.SetActive(true);
        }
        else
        {
            // Deactivate current item if exists
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