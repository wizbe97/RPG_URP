using System.Collections;
using UnityEngine;

public class Player : Character
{
    private InventoryManager inventoryManager;

    public HitPoints hitPoints;

    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        ResetCharacter();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                bool shouldDisappear = false;

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        Debug.Log("Coin picked up!");
                        shouldDisappear = inventoryManager.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        Debug.Log("Health picked up!");
                        break;
                    case Item.ItemType.GUN:
                        Debug.Log("Gun picked up!");
                        shouldDisappear = inventoryManager.AddItem(hitObject);
                        break;
                    default:
                        break;
                }

                if (shouldDisappear)
                {
                    //collision.gameObject.SetActive(false);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
    }

    public override void ResetCharacter()
    {
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;
    }
}