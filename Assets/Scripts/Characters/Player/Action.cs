using UnityEngine;
using UnityEngine.EventSystems;

public class Action : MonoBehaviour
{
    private Inventory inventory;
    public bool isHoldingGun = false;
    private GameObject instantiatedGun;
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
        if (overUI || currentItem == null)
            return;

        if (currentItem.itemType == Item.ItemType.GUN)
        {
            playerGun = FindObjectOfType<PlayerGun>();
            if (playerGun != null && !PlayerGun.IsAnyGunShooting())
            {
                playerGun.Shoot();
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
            if (currentItem != null && currentItem.holdable == true)
            {
                if (currentItem.itemType == Item.ItemType.GUN)
                {
                    isHoldingGun = true;
                    Debug.Log("Instantiating Gun");

                    // Only instantiate if the shotgun prefab is not already instantiated
                    if (instantiatedGun == null)
                    {
                        Debug.Log("Instantiating " +currentItem.itemName);
                        instantiatedGun = Instantiate(currentItem.instantiatedPrefab, transform.position, Quaternion.identity);
                        instantiatedGun.transform.parent = transform; // Set player as parent
                    }

                    // If instantiated, set active
                    if (instantiatedGun != null)
                    {
                        Debug.Log("Activating "+currentItem.itemName);
                        instantiatedGun.SetActive(true);
                    }
                }
            }
        }
    }
}
//     }
//                         // Deactivate other guns if exist
//                         if (instantiatedSmg != null)
//                         {
//                             instantiatedSmg.SetActive(false);
//                         }
//                         if (instantiatedSniper != null)
//                         {
//                             instantiatedSniper.SetActive(false);
//                         }
//                     }
//                     else if (currentItem.itemName == "smg")
//                     {
//                         // If smg prefab is not instantiated, instantiate it and set its parent to the player
//                         if (instantiatedSmg == null && smgPrefab != null)
//                         {
//                             instantiatedSmg = Instantiate(smgPrefab, transform.position, Quaternion.identity);
//                             instantiatedSmg.transform.parent = transform; // Set player as parent
//                         }
//                         // If instantiated, set active
//                         if (instantiatedSmg != null)
//                         {
//                             instantiatedSmg.SetActive(true);
//                         }
//                         // Deactivate other guns if exist
//                         if (instantiatedShotgun != null)
//                         {
//                             instantiatedShotgun.SetActive(false);
//                         }
//                         if (instantiatedSniper != null)
//                         {
//                             instantiatedSniper.SetActive(false);
//                         }
//                     }
//                     else if (currentItem.itemName == "sniper")
//                     {
//                         // If sniper prefab is not instantiated, instantiate it and set its parent to the player
//                         if (instantiatedSniper == null && sniperPrefab != null)
//                         {
//                             instantiatedSniper = Instantiate(sniperPrefab, transform.position, Quaternion.identity);
//                             instantiatedSniper.transform.parent = transform; // Set player as parent
//                         }
//                         // If instantiated, set active
//                         if (instantiatedSniper != null)
//                         {
//                             instantiatedSniper.SetActive(true);
//                         }
//                         // Deactivate other guns if exist
//                         if (instantiatedShotgun != null)
//                         {
//                             instantiatedShotgun.SetActive(false);
//                         }
//                         if (instantiatedSmg != null)
//                         {
//                             instantiatedSmg.SetActive(false);
//                         }
//                     }
//                 }
//                 else
//                 {
//                     isHoldingGun = false;
//                     // If instantiated, set inactive
//                     if (instantiatedShotgun != null)
//                     {
//                         instantiatedShotgun.SetActive(false);
//                     }
//                     if (instantiatedSmg != null)
//                     {
//                         instantiatedSmg.SetActive(false);
//                     }
//                     if (instantiatedSniper != null)
//                     {
//                         instantiatedSniper.SetActive(false);
//                     }
//                 }
//             }
//             else
//             {
//                 isHoldingGun = false;
//                 // If instantiated, set inactive
//                 if (instantiatedShotgun != null)
//                 {
//                     instantiatedShotgun.SetActive(false);
//                 }
//                 if (instantiatedSmg != null)
//                 {
//                     instantiatedSmg.SetActive(false);
//                 }
//                 if (instantiatedSniper != null)
//                 {
//                     instantiatedSniper.SetActive(false);
//                 }
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Inventory instance is null!");
//         }
//     }
// }
