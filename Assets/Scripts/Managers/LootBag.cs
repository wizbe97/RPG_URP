using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Item> lootList = new List<Item>();

    public List<Item> GetDroppedItems()
    {
        List<Item> possibleItems = new List<Item>();

        // Iterate through each item
        foreach (Item item in lootList)
        {
            // Generate a random number to determine if the item should be dropped
            int randomNumber = Random.Range(1, 101);
            // Check if the random number is less than or equal to the drop chance of the item
            if (randomNumber <= item.dropChance)
            {
                // Add the item to the list of possible drops
                possibleItems.Add(item);
            }
        }

        return possibleItems;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Item> droppedItems = GetDroppedItems();

        // Instantiate each dropped item once
        foreach (Item droppedItem in droppedItems)
        {
            Instantiate(droppedItem.droppedItem, spawnPosition, Quaternion.identity);
        }
    }


}
