using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    public InventoryItem Item;
    public int Index;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            // Get the dragged item
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            // Check if the slot already contains an item
            if (inventoryItem.parentAfterDrag != transform)
            {
                // If the slot is empty or does not contain the dragged item, proceed with dropping the item into the slot
                inventoryItem.parentAfterDrag = transform; // Set the parent after drag to this slot
                inventoryItem.transform.SetParent(transform); // Set the parent of the dragged item to this slot
            }
        }
    }

}