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


    private void Start()
    {
        action = GetComponent<Action>();
        audioSource = GetComponent<AudioSource>();
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

            animator.SetBool("isShooting", true);
            isShooting = false;
        }
        else // If mouse button is not held down
        {
            // Reset the variable and timer
            recoilActivated = false;
            holdStartTime = 0f;
        }
    }

    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            isShooting = true;
            animator.SetBool("isShooting", true);
            audioSource.Play();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Bullet Fired");
            if (recoilActivated == true)
            {
                Vector3 recoil = new Vector2(Random.Range(-recoilStrength, recoilStrength), Random.Range(-recoilStrength, recoilStrength));
                Vector2 fireDirection = (firePoint.up + recoil).normalized;
                bullet.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                Vector2 fireDirection = firePoint.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void OnShootingAnimationEnd()
    {
        animator.SetBool("isShooting", false);
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
}
