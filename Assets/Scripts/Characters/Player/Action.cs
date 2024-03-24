using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    private Inventory inventory;
    public bool isHoldingGun = false;
    public GameObject shotgunPrefab;
    private GameObject instantiatedShotgun;
    public Item currentItem;
    private Shotgun shotgun;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void FixedUpdate()
    {
        CurrentItem();
    }

    private void OnUseItem()
    {
        if (currentItem != null)
        {
            shotgun = FindAnyObjectByType<Shotgun>();
            if (currentItem.itemType == Item.ItemType.GUN)
            {
                if (shotgun.isShooting != true)
                {
                    shotgun.Shoot();
                }
            }
        }

        else
        {
            Debug.Log("No item in slot");
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

    private void CurrentItem()
    {
        if (Inventory.Instance != null)
        {
            currentItem = Inventory.Instance.GetSelectedItem(false);
            if (currentItem != null)
            {
                {
                    if (currentItem.objectName == "shotgun")
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
