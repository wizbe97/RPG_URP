using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Action : MonoBehaviour
{
    private Inventory inventory;
    public bool isHoldingGun = false;
    public GameObject shotgunPrefab;
    private GameObject instantiatedShotgun;
    public GameObject sniperPrefab;
    private GameObject instantiatedSniper;
    public GameObject smgPrefab;
    private GameObject instantiatedSmg;
    public Item currentItem;
    private PlayerGun playerGun;
    private bool overUI;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    void Update()
    {
        overUI = IsPointerOverUI();
        if (overUI != true && Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            OnUseItem();
        }
    }

    void LateUpdate()
    {
        CurrentItem();
    }

    private bool IsPointerOverUI()
    {
        // Check if the pointer is over a UI element
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void OnUseItem()
    {
        if (overUI != true)
        {
            if (currentItem != null && currentItem.itemType == Item.ItemType.GUN)
            {
                playerGun = FindObjectOfType<PlayerGun>();
                if (playerGun != null && !PlayerGun.IsAnyGunShooting()) // Check if shotgun exists and no gun is shooting
                {
                    playerGun.Shoot();
                }
                else
                {
                    Debug.Log("Cannot shoot. Either shotgun not found or another gun is already shooting.");
                }

            }
        }
    }

    private void OnDropItem()
    {
        currentItem = inventory.GetSelectedItem(true);
        if (currentItem != null)
        {
            Debug.Log("Dropping item: " + currentItem);
        }
        else
        {
            Debug.Log("No item in slot");
        }
    }

    private void CurrentItem()
    {
        if (Inventory.Instance != null)
        {
            currentItem = Inventory.Instance.GetSelectedItem(false);
            if (currentItem != null)
            {
                if (currentItem.itemType == Item.ItemType.GUN)
                {
                    if (currentItem.itemName == "shotgun")
                    {
                        isHoldingGun = true;
                        // If shotgun prefab is not instantiated, instantiate it and set its parent to the player
                        if (instantiatedShotgun == null && shotgunPrefab != null)
                        {
                            instantiatedShotgun = Instantiate(shotgunPrefab, transform.position, Quaternion.identity);
                            instantiatedShotgun.transform.parent = transform; // Set player as parent
                        }
                        // If instantiated, set active
                        if (instantiatedShotgun != null)
                        {
                            instantiatedShotgun.SetActive(true);
                        }
                        // Deactivate other guns if exist
                        if (instantiatedSmg != null)
                        {
                            instantiatedSmg.SetActive(false);
                        }
                        if (instantiatedSniper != null)
                        {
                            instantiatedSniper.SetActive(false);
                        }
                    }
                    else if (currentItem.itemName == "smg")
                    {
                        isHoldingGun = true;
                        // If smg prefab is not instantiated, instantiate it and set its parent to the player
                        if (instantiatedSmg == null && smgPrefab != null)
                        {
                            instantiatedSmg = Instantiate(smgPrefab, transform.position, Quaternion.identity);
                            instantiatedSmg.transform.parent = transform; // Set player as parent
                        }
                        // If instantiated, set active
                        if (instantiatedSmg != null)
                        {
                            instantiatedSmg.SetActive(true);
                        }
                        // Deactivate other guns if exist
                        if (instantiatedShotgun != null)
                        {
                            instantiatedShotgun.SetActive(false);
                        }
                        if (instantiatedSniper != null)
                        {
                            instantiatedSniper.SetActive(false);
                        }
                    }
                    else if (currentItem.itemName == "sniper")
                    {
                        isHoldingGun = true;
                        // If sniper prefab is not instantiated, instantiate it and set its parent to the player
                        if (instantiatedSniper == null && sniperPrefab != null)
                        {
                            instantiatedSniper = Instantiate(sniperPrefab, transform.position, Quaternion.identity);
                            instantiatedSniper.transform.parent = transform; // Set player as parent
                        }
                        // If instantiated, set active
                        if (instantiatedSniper != null)
                        {
                            instantiatedSniper.SetActive(true);
                        }
                        // Deactivate other guns if exist
                        if (instantiatedShotgun != null)
                        {
                            instantiatedShotgun.SetActive(false);
                        }
                        if (instantiatedSmg != null)
                        {
                            instantiatedSmg.SetActive(false);
                        }
                    }
                }
                else
                {
                    isHoldingGun = false;
                    // If instantiated, set inactive
                    if (instantiatedShotgun != null)
                    {
                        instantiatedShotgun.SetActive(false);
                    }
                    if (instantiatedSmg != null)
                    {
                        instantiatedSmg.SetActive(false);
                    }
                    if (instantiatedSniper != null)
                    {
                        instantiatedSniper.SetActive(false);
                    }
                }
            }
            else
            {
                isHoldingGun = false;
                // If instantiated, set inactive
                if (instantiatedShotgun != null)
                {
                    instantiatedShotgun.SetActive(false);
                }
                if (instantiatedSmg != null)
                {
                    instantiatedSmg.SetActive(false);
                }
                if (instantiatedSniper != null)
                {
                    instantiatedSniper.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("Inventory instance is null!");
        }
    }
}
