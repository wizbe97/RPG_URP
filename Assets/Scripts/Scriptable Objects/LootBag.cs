using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Item> lootList = new List<Item>();

    // List<Item> GetDroppedItems() // THIS IS FOR DROPPING MULTIPLE ITEMS
    // {
    //     int randomNumber = Random.Range(1, 101);
    //     List<Item> possibleItems = new List<Item>();
    //     foreach (Item item in lootList)
    //     {
    //         if (randomNumber <= item.dropChance)
    //         {
    //             possibleItems.Add(item);
    //             return possibleItems;
    //         }
    //     }
    //     Debug.Log("No item dropped");
    //     return null;
    // }

    Item GetDroppedItem()  // THIS IS FOR DROPPING ONE ITEM ONLY (RANDOM ITEM PICKED)
    {
        int randomNumber = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();
        foreach (Item item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No item dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition) {
        Item droppedItem = GetDroppedItem();
        if(droppedItem != null) {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);)
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.droppedImage;
        }
    }

}
