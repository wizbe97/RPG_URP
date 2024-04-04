using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Consumable : MonoBehaviour
{
    public Item item;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Assign the sprite from the item.droppedImage to the SpriteRenderer's sprite
        if (spriteRenderer != null && item != null && item.droppedImage != null)
        {
            spriteRenderer.sprite = item.droppedImage;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer or Item or Dropped Image is not set properly.");
        }
    }
}
