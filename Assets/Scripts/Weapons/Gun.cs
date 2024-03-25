using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Action action;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    protected AudioSource audioSource;
    private bool isShooting;

    protected virtual void Start()
    {
        action = GetComponent<Action>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Shoot()
    {
        isShooting = true;
        animator.SetBool("isShooting", true);
        audioSource.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
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
