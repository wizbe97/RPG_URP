using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Action action;
    public Animator animator;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    private AudioSource audioSource;
    [HideInInspector] public bool isShooting;
    
    private void Start()
    {
        action = GetComponent<Action>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
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
}
