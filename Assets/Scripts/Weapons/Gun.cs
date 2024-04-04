using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Action action;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce;
    protected AudioSource audioSource;
    public bool isShooting;
    public float fireRate = 0.5f; // Adjust this value to control fire rate
    private float nextFireTime = 0f; // Tracks the next allowed time to fire


    protected virtual void Start()
    {
        action = GetComponent<Action>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Update()
    {
        if (Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            animator.SetBool("isShooting", true);
            isShooting = false;
        }
    }
    public virtual void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            isShooting = true;
            animator.SetBool("isShooting", true);
            audioSource.Play();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            nextFireTime = Time.time + fireRate; // Update next allowed time to fire
        }
    }

    public void OnShootingAnimationEnd()
    {
        animator.SetBool("isShooting", false);
        isShooting = false;
    }

    public static bool IsAnyGunShooting()
    {
        Gun[] guns = FindObjectsOfType<Gun>();
        foreach (Gun gun in guns)
        {
            if (gun.isShooting)
            {
                return true;
            }
        }
        return false;
    }
}
