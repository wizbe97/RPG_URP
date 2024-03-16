using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void OnUseItem()
    {
        Item currentItem = inventoryManager.GetSelectedItem(false);
        if (currentItem != null)
        {
            Debug.Log("Current item: " + currentItem);
        }
        else
        {
            Debug.Log("No item in slot");
        }
    }

    private void OnDropItem()
    {
        Item currentItem = inventoryManager.GetSelectedItem(true);
        if (currentItem != null)
        {
            Debug.Log("Current item: " + currentItem);
        }
        else
        {
            Debug.Log("No item in slot");
        }
    }
}
