using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Inventory inventory;
    public Item[] itemsToPickup;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    private void OnUseItem()
    {
        Item currentItem = inventory.GetSelectedItem(false);
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
        Item currentItem = inventory.GetSelectedItem(true);
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
