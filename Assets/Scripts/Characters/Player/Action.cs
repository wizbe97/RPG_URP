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
    public GameObject smgPrefab;
    private GameObject instantiatedSmg;
    public Item currentItem;
    private Shotgun shotgun;
    private SMG smg;
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
            if (currentItem != null && currentItem.itemName == "shotgun")
            {
                shotgun = FindObjectOfType<Shotgun>(); // Assuming there's only one shotgun in the scene
                if (shotgun != null && !Gun.IsAnyGunShooting()) // Check if shotgun exists and no gun is shooting
                {
                    shotgun.Shoot();
                }
                else
                {
                    Debug.Log("Cannot shoot. Either shotgun not found or another gun is already shooting.");
                }
            }
            else if (currentItem != null && currentItem.itemName == "smg")
            {
                smg = FindObjectOfType<SMG>(); // Assuming there's only one smg in the scene
                if (smg != null && !Gun.IsAnyGunShooting()) // Check if smg exists and no gun is shooting
                {
                    smg.Shoot();
                }
                else
                {
                    Debug.Log("Cannot shoot. Either SMG not found or another gun is already shooting.");
                }
            }
            else
            {
                Debug.Log("No gun item in slot");
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
                        // Deactivate other gun if exists
                        if (instantiatedSmg != null)
                        {
                            instantiatedSmg.SetActive(false);
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
                        // Deactivate other gun if exists
                        if (instantiatedShotgun != null)
                        {
                            instantiatedShotgun.SetActive(false);
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
            }
        }
        else
        {
            Debug.LogWarning("Inventory instance is null!");
        }
    }



}
