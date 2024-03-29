﻿using System.Collections;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    public Inventory inventoryPrefab;
    private Inventory inventoryManager;
    
    private void Start()
    {
        inventoryManager = FindObjectOfType<Inventory>();
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
                        shouldDisappear = inventoryManager.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    case Item.ItemType.GUN:
                        shouldDisappear = inventoryManager.AddItem(hitObject);
                        break;
                    default:
                        break;
                }

                if (shouldDisappear)
                {
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
        inventoryManager = Instantiate(inventoryPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;
    }
}