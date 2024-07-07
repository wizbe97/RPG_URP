
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private Action action;
    public int maxStackedItems = 250;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        // else
        // {
        //     Destroy(this.gameObject);
        // }
    }

    private void Start()
    {
        action = FindAnyObjectByType<Action>();
        ChangeSelectedSlot(0);
    }

    public void LoadInventoryData(int slot)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
            inventorySlots[i].Index = i;

        SaveManager.Instance.LoadInventory(inventorySlots, slot);

        ChangeSelectedSlot(0);
    }

    public void SaveInventoryData(int slot)
    {
        SaveManager.Instance.SaveInventory(inventorySlots, slot);
    }


    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 5)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        // Check if any gun is shooting before changing the selected slot
        if (!PlayerGun.IsAnyGunShooting())
        {
            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
            {
                // Deactivate existing current item
                action.DeactivateCurrentItem();
                action.isHoldingGun = false;
                inventorySlots[selectedSlot].Deselect();
            }
            // Find the slot with the specified index in the parent's list of children
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].transform.GetSiblingIndex() == newValue)
                {
                    // Select the found slot
                    inventorySlots[i].Select();
                    selectedSlot = i;

                    // Call CurrentItem() method here
                    action.CurrentItem();

                    break;
                }
            }
        }
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        else
        {
            return null;
        }
    }

    public bool HasItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                return true;
            }
        }
        return false;
    }

    public void ConsumeItem(Item item)
    {
        // Find the slot containing the specified item
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count > 0)
            {
                // Consume one item
                itemInSlot.count--;
                itemInSlot.RefreshCount();

                // If the count reaches zero, destroy the item
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                return; // Exit the method after consuming one item
            }
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int slotIndex = i;
            InventorySlot slot = inventorySlots[slotIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            // Check if there is no item in the slot and no placeholder exists
            if (itemInSlot == null && slot.transform.childCount == 0)
            {
                // Spawn a new item in the slot
                SpawnNewItem(item, slotIndex, quantity); // Pass the quantity here
                // SaveManager.Instance.SaveInventory(inventorySlots);
                return true;
            }
            else if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                // If the item is stackable and matches the item in the slot, and the count is less than the maximum stack size
                int spaceLeftInStack = maxStackedItems - itemInSlot.count;
                if (quantity <= spaceLeftInStack)
                {
                    // If there's enough space in the stack to accommodate the entire quantity
                    itemInSlot.count += quantity;
                    itemInSlot.RefreshCount();
                    // SaveManager.Instance.SaveInventory(inventorySlots);
                    return true;
                }
                else
                {
                    // If there's not enough space in the stack to accommodate the entire quantity
                    itemInSlot.count = maxStackedItems; // Fill the stack to its maximum
                    itemInSlot.RefreshCount();
                    quantity -= spaceLeftInStack; // Subtract the space filled from the remaining quantity
                                                  // Check the next slot or create a new stack if available slots are exhausted
                }
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, int slotIndex, int quantity)
    {
        InventorySlot slot = inventorySlots[slotIndex];
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        slot.Item = newItemGO.GetComponent<InventoryItem>();
        slot.Item.InitialiseItem(item, quantity); // Pass the quantity here
        slot.Item.InventorySlotIndex = slotIndex;
    }

    public void ChangeItemSlot(InventoryItem item, int slotIndex, bool emptyOriginalSlot = true)
    {
        InventorySlot slot = inventorySlots[slotIndex];
        if (item.InventorySlotIndex != slotIndex && emptyOriginalSlot)
            inventorySlots[item.InventorySlotIndex].Item = null;
        slot.Item = item;
        item.InventorySlotIndex = slotIndex;
        item.transform.SetParent(slot.transform);
    }

    public void SwitchItemSlots(params InventoryItem[] items)
    {
        int item0Slot = items[0].InventorySlotIndex;
        int item1Slot = items[1].InventorySlotIndex;
        ChangeItemSlot(items[0], item1Slot, false);
        ChangeItemSlot(items[1], item0Slot, false);
    }


    // Store collected item IDs in a list
    public List<string> collectedItems = new List<string>();

    // Call this method when an item is collected
    public void CollectItem(GameObject item)
    {
        Consumable uniqueID = item.GetComponent<Consumable>();
        if (uniqueID != null)
        {
            collectedItems.Add(uniqueID.ID);
            item.SetActive(false);
        }
    }
}
