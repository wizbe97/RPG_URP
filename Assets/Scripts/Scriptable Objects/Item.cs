using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public GameObject droppedItem;
    public GameObject instantiatedPrefab = null;
    public Sprite inventoryImage;
    public int quantity;
    public bool stackable;
    public int dropChance;
    public Item bullet;

    public bool holdable;

    public enum ItemType
    {
        COIN,
        HEALTH,
        GUN,
        BULLET
    }
    public ItemType itemType;
}
