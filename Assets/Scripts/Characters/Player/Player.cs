﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public HitPoints hitPoints;
    public GameObject playerSpawnPoint;
    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    public Inventory inventoryPrefab;
    private Inventory inventory;
    private Action action;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        action = GetComponent<Action>();
        SetupCharacter();
    }

    private void SetupCharacter()
    {
        healthBar = Instantiate(healthBarPrefab);
        inventory = Instantiate(inventoryPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;
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
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    case Item.ItemType.COIN:
                    case Item.ItemType.BULLET:
                    case Item.ItemType.GUN:
                        shouldDisappear = AddItemToInventory(hitObject, hitObject.quantity);
                        action.CurrentItem();
                        break;
                }

                if (shouldDisappear)
                {
                    // Notify ObjectManager when an object is destroyed
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    bool AddItemToInventory(Item item, int quantity)
    {
        return inventory.AddItem(item, quantity);
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
        if (hitPoints.value <= 0)
        {
            yield break; // Exit the coroutine if the enemy is already dead
        }

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
        gameObject.SetActive(false);
        RespawnManager respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager != null)
        {
            respawnManager.StartRespawnCoroutine(gameObject, 2f);
        }
    }


    private IEnumerator ResetCharacterAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        transform.position = playerSpawnPoint.transform.position;
        hitPoints.value = startingHitPoints;
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
