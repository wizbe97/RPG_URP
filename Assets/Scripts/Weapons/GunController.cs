using UnityEngine;

public class GunController : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer gunSpriteRenderer;
    private int playerSortingOrder;

    void Start()
    {
        playerTransform = transform.parent;
        gunSpriteRenderer = GetComponent<SpriteRenderer>();

        playerSortingOrder = playerTransform.GetComponent<SpriteRenderer>().sortingOrder;
    }

    void Update()
    {
        RotateGunTowardsMouse();
        AdjustSortingOrder();
    }

    void RotateGunTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (angle > 90 || angle < -90)
        {
            gunSpriteRenderer.flipY = true;
        }
        else
        {
            gunSpriteRenderer.flipY = false;
        }
    }

    void AdjustSortingOrder()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        int sortingOrder = playerSortingOrder; // Start with player's sorting order

        // Adjust sorting order based on angle
        switch (Mathf.RoundToInt(angle / 45f))
        {
            case 1: // 45 degrees
            case 2: // 90 degrees
            case 3: // 135 degrees
                sortingOrder -= 1; 
                break;
            default:
                sortingOrder += 1; 
                break;
        }

        // Apply the adjusted sorting order to the gun sprite renderer
        gunSpriteRenderer.sortingOrder = sortingOrder;
    }
}