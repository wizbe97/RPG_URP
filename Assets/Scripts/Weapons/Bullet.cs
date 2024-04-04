using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance;
    public int minimumBulletDamage;
    public int maximumBulletDamage;

    public Vector3 spawnPosition; // Position where the bullet was instantiated

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

    protected virtual void OnTriggerEnter2D(Collider2D other)
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
