using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Coroutine damageCoroutine;
    public float maxTravelDistance = 10f; // Maximum travel distance of the bullet
    public float destroyBulletDelay = 1f;
    public int minimumBulletDamage = 5; // Minimum damage strength of the bullet
    public int maximumBulletDamage = 30; // Maximum damage strength of the bullet

    private Vector3 spawnPosition; // Position where the bullet was instantiated

    void Start()
    {
        spawnPosition = transform.position;
    }
    private void Update()
    {
        StartCoroutine(DestroyAfterDelay(destroyBulletDelay));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Calculate damage based on travel distance
            float travelDistance = Vector3.Distance(spawnPosition, transform.position);
            int calculatedDamage = CalculateDamage(travelDistance);

            // Only call DamageCharacter on the Enemy if we don't currently have a DamageCharacter() Coroutine running.
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(enemy.DamageCharacter(calculatedDamage, 0f));
            }
        }
        Destroy(gameObject);
    }

    private int CalculateDamage(float travelDistance)
    {
        // Calculate damage based on travel distance
        float t = Mathf.Clamp01(travelDistance / maxTravelDistance);
        int damage = Mathf.RoundToInt(Mathf.Lerp(maximumBulletDamage, minimumBulletDamage, t));
        return damage;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
