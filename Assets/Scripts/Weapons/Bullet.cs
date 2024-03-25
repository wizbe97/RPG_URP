using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance;
    public int minimumBulletDamage;
    public int maximumBulletDamage;

    private Vector3 spawnPosition; // Position where the bullet was instantiated

    protected virtual void Start()
    {
        spawnPosition = transform.position;
    }

    private void Update()
    {
        float travelDistance = Vector3.Distance(spawnPosition, transform.position);
        if (travelDistance >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
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
}
