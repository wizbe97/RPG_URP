using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance;
    public float destroyBulletDelay;
    public int minimumBulletDamage;
    public int maximumBulletDamage;

    protected Vector3 spawnPosition; // Position where the bullet was instantiated

    protected virtual void Start()
    {
        spawnPosition = transform.position;
        StartCoroutine(DestroyAfterDelay(destroyBulletDelay)); // Start coroutine to destroy the bullet after delay
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    protected virtual int CalculateDamage(float travelDistance)
    {
        // Calculate damage based on travel distance
        float t = Mathf.Clamp01(travelDistance / maxTravelDistance);
        int damage = Mathf.RoundToInt(Mathf.Lerp(maximumBulletDamage, minimumBulletDamage, t));
        return damage;
    }

    protected virtual IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
