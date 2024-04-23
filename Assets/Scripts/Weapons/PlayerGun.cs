using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    protected Action action;
    private float holdStartTime;
    private bool recoilActivated;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce;
    public float recoilStrength = 1f;
    public float recoilTimer = 0.5f;
    protected AudioSource audioSource;
    public bool isShooting;
    public float fireRate = 0.5f; // Adjust this value to control fire rate
    private float nextFireTime = 0f; // Tracks the next allowed time to fire

    private Transform playerTransform;
    private SpriteRenderer gunSpriteRenderer;
    private int playerSortingOrder;



    private void Start()
    {
        action = FindObjectOfType<Action>();
        audioSource = GetComponent<AudioSource>();
        playerTransform = transform.parent;
        gunSpriteRenderer = GetComponent<SpriteRenderer>();

        playerSortingOrder = playerTransform.GetComponent<SpriteRenderer>().sortingOrder;

        // Adjust the Y position of the gun by 2 units
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.23f, transform.localPosition.z);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is pressed
        {
            // Start the timer when mouse button is pressed
            holdStartTime = Time.time;
        }

        if (Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            float holdDuration = Time.time - holdStartTime;
            if (holdDuration >= recoilTimer) // Check if held down for X time
            {
                // Set your variable here, for example:
                recoilActivated = true;
            }
            else
            {
                recoilActivated = false;
            }

            isShooting = false;
        }
        else // If mouse button is not held down
        {
            // Reset the variable and timer
            recoilActivated = false;
            holdStartTime = 0f;
        }

        RotateGunTowardsMouse();
        AdjustSortingOrder();
    }

    public void Shoot()
    {

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            isShooting = true;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Inventory.Instance.ConsumeItem(action.currentItem.bullet);

            animator.SetBool("isShooting", true);
            audioSource.Play();

            if (recoilActivated == true)
            {
                Vector3 recoil = new Vector2(Random.Range(-recoilStrength, recoilStrength), Random.Range(-recoilStrength, recoilStrength));
                Vector2 fireDirection = (firePoint.up + recoil).normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 fireDirection = firePoint.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
            }
        }
    }


    public void OnShootingAnimationEnd()
    {
        animator.SetBool("isShooting", false);
        animator.Play("idle");
        isShooting = false;
    }

    public static bool IsAnyGunShooting()
    {
        PlayerGun[] guns = FindObjectsOfType<PlayerGun>();
        foreach (PlayerGun gun in guns)
        {
            if (gun.isShooting)
            {
                return true;
            }
        }
        return false;
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
