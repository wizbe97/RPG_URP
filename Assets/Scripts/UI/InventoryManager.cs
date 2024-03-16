using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxStackedItems = 250;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < inventorySlots.Length; i++)
            inventorySlots[i].Index = i;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
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
        // Deselect the previously selected slot
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
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
                break;
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

    public bool AddItem(Item item)
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
                SpawnNewItem(item, slotIndex);
                return true;
            }
            else if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                // If the item is stackable and matches the item in the slot, and the count is less than the maximum stack size
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        return false;
    }



    void SpawnNewItem(Item item, int slotIndex)
    {
        InventorySlot slot = inventorySlots[slotIndex];
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        slot.Item = newItemGO.GetComponent<InventoryItem>();
        slot.Item.InitialiseItem(item);
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
}
