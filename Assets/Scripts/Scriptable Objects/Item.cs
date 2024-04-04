using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite droppedImage;
    public Sprite inventoryImage;
    public int quantity;
    public bool stackable;
    public int dropChance;

    public enum ItemType {
        COIN,
        HEALTH,
        GUN
    }
    public ItemType itemType;

    public Item(string itemName, int dropChance){ 
        this.itemName = itemName;
        this.dropChance = dropChance;
    }
}
