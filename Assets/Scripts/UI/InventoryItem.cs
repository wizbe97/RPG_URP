using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    private GameObject placeholder; // Placeholder object for maintaining slot state while dragging


    bool isDragging = false; // Track if dragging is occurring
    private InventoryManager inventoryManager;

    public int InventorySlotIndex;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(); // Find the InventoryManager in the scene
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging && (Mouse.current.leftButton.isPressed || (Mouse.current.leftButton.isPressed && Mouse.current.rightButton.isPressed)))
        {
            isDragging = true; // Start dragging if left button is pressed or both buttons are pressed
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;

            // Create a placeholder object
            placeholder = new GameObject("Placeholder");
            placeholder.transform.SetParent(transform.parent);
            RectTransform placeholderRect = placeholder.AddComponent<RectTransform>();
            placeholderRect.sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;

            // Set the position of the placeholder to match the dragged item
            placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            transform.SetParent(transform.root);
            countText.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            image.raycastTarget = true;

            //This is an ugly fix for when eventData.pointerEnter is pointing to the textbox on top of the inventory item
            GameObject targetObject = eventData.pointerEnter;
            if (targetObject != null && targetObject.GetComponent<Text>() != null)
                targetObject = targetObject.transform.parent.gameObject;

            InventoryItem targetItem = targetObject ? targetObject.GetComponent<InventoryItem>() : null;

            //This is a fix for when eventData.pointerEnter points to a slot rather than an item
            if(targetItem == null)
                targetItem = targetObject?.GetComponent<InventorySlot>()?.Item;

            if (targetItem != null)
            {
                if (targetItem != this && item == targetItem.item && item.stackable)
                {
                    // Snap the dragged item onto the target item slot
                    transform.SetParent(targetItem.transform.parent);
                    transform.position = targetItem.transform.position;
                    transform.SetAsLastSibling(); // Ensure the dragged item is rendered on top

                    // Merge the dragged item with the target item
                    targetItem.MergeWith(this);
                }
                else
                {
                    InventoryManager.Instance.SwitchItemSlots(this, targetItem);
                }
            }
            else
            {
                InventorySlot targetSlot = targetObject?.GetComponent<InventorySlot>();
                if (targetSlot == null)
                    targetSlot = InventoryManager.Instance.inventorySlots[InventorySlotIndex]; //Use index to return it to original slot
                InventoryManager.Instance.ChangeItemSlot(this, targetSlot.Index);
            }

            countText.raycastTarget = true;

            // Destroy the placeholder object
            Destroy(placeholder);
        }
    }

    public void MergeWith(InventoryItem otherItem)
    {
        int totalItemsCount = otherItem.count + count;
        if (totalItemsCount <= inventoryManager.maxStackedItems)
        {
            // If the total count of items in the stack doesn't exceed the maximum stack size,
            // merge the stacks completely
            otherItem.count = totalItemsCount;
            otherItem.RefreshCount();
            Destroy(gameObject); // Destroy the dragged item
        }
        else
        {
            // If the target slot can't accommodate the entire stack, partially merge them
            int spaceLeftInStack = inventoryManager.maxStackedItems - otherItem.count;
            otherItem.count += spaceLeftInStack;
            otherItem.RefreshCount();
            count -= spaceLeftInStack;
            RefreshCount();
        }
    }


}