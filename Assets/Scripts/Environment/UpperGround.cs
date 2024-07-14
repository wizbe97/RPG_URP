using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperGround : MonoBehaviour
{
    private int originalSortingOrder;
    private SpriteRenderer parentSpriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollisions"))
        {
            // Get the parent GameObject of the collider
            GameObject parentObject = other.transform.parent.gameObject;

            // Get the SpriteRenderer component from the parent
            parentSpriteRenderer = parentObject.GetComponent<SpriteRenderer>();
            if (parentSpriteRenderer != null)
            {
                // Store the original sorting order
                originalSortingOrder = parentSpriteRenderer.sortingOrder;
                // Increase the sorting order by 1
                parentSpriteRenderer.sortingOrder += 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollisions"))
        {
            // Ensure we have a reference to the parent's SpriteRenderer
            if (parentSpriteRenderer != null)
            {
                // Reset to the original sorting order
                parentSpriteRenderer.sortingOrder = originalSortingOrder;
                // Clear the reference to avoid potential issues
                parentSpriteRenderer = null;
            }
        }
    }
}
