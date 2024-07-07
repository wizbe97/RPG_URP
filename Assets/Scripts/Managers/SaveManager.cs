using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; set; }
    private string saveDirectory;


    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        else Instance = this;

        DontDestroyOnLoad(this.gameObject);

        saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public bool IsDataSaved(int slot)
    {
        string inventoryPath = GetInventoryPath(slot);
        string playerStatsPath = GetPlayerStatsPath(slot);
        return File.Exists(inventoryPath) || File.Exists(playerStatsPath);
    }

    public void SaveInventory(InventorySlot[] inventorySlots, int slot)
    {
        //create a new inventory data
        InventoryData inventoryData = new InventoryData();
        //grab all the slots from the inventory script
        foreach (var slots in inventorySlots)
        {
            //grab all the items from the slots
            InventoryItem itemInSlot = slots.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                //grab the items name and count
                InventoryItemData itemData = new InventoryItemData
                {
                    itemName = itemInSlot.item.itemName,
                    count = itemInSlot.count
                };
                //finally put each item into the inventorydata list
                inventoryData.items.Add(itemData);
            }
        }

        //this will keep track of the IDs of the collected items so it disables them when playing scene
        inventoryData.collectedItemIDs = Inventory.Instance.collectedItems;
        string json = JsonUtility.ToJson(inventoryData, true);
        string inventoryPath = GetInventoryPath(slot);
        File.WriteAllText(inventoryPath, json);
    }

    public void LoadInventory(InventorySlot[] inventorySlots, int slot)
    {
        string inventoryPath = GetInventoryPath(slot);
        //check if inventory file exist
        if (File.Exists(inventoryPath))
        {
            //grab the inventory saved file
            string json = File.ReadAllText(inventoryPath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

            foreach (var slots in inventorySlots)
            {
                // Clear existing items in the inventory slots
                InventoryItem itemInSlot = slots.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null)
                {
                    Destroy(itemInSlot.gameObject);
                }
            }

            // Populate inventory slots with loaded data
            foreach (var itemData in inventoryData.items)
            {
                Item item = GetInventoryItemByName(itemData.itemName);
                if (item != null)
                {
                    Inventory.Instance.AddItem(item, itemData.count);
                }
            }

            Inventory.Instance.collectedItems = inventoryData.collectedItemIDs; // Load collected item IDs from the data

            // Disable collected items in the scene
            Consumable[] allCollectibles = FindObjectsOfType<Consumable>();
            foreach (var collectible in allCollectibles)
            {
                if (Inventory.Instance.collectedItems.Contains(collectible.ID))
                {
                    collectible.gameObject.SetActive(false);
                }
            }
        }
    }

    // Get the item by it's name that you set on ItemName on scriptableObject
    private Item GetInventoryItemByName(string itemName)
    {
        Item[] allInventoryItems = Resources.LoadAll<Item>("Items");
        foreach (Item item in allInventoryItems)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null;
    }


    public void SavePlayerStats(PlayerData playerStats, int slot)
    {
        playerStats.sceneName = SceneManager.GetActiveScene().name;
        string json = JsonUtility.ToJson(playerStats, true);
        string playerStatsPath = GetPlayerStatsPath(slot);
        File.WriteAllText(playerStatsPath, json);
    }

    public PlayerData LoadPlayerStats(int slot)
    {
        string playerStatsPath = GetPlayerStatsPath(slot);
        if (File.Exists(playerStatsPath))
        {
            string json = File.ReadAllText(playerStatsPath);
            PlayerData playerStats = JsonUtility.FromJson<PlayerData>(json);
            return playerStats;
        }
        else
        {
            return null;
        }
    }

    private string GetInventoryPath(int slot)
    {
        return Path.Combine(saveDirectory, $"inventory_slot{slot}.json");
    }

    private string GetPlayerStatsPath(int slot)
    {
        return Path.Combine(saveDirectory, $"playerstats_slot{slot}.json");
    }

    public void RemoveSlot(int slot)
    {
        string inventoryPath = GetInventoryPath(slot);
        string playerStatsPath = GetPlayerStatsPath(slot);

        if (File.Exists(inventoryPath))
        {
            File.Delete(inventoryPath);
        }
        if (File.Exists(playerStatsPath))
        {
            File.Delete(playerStatsPath);
        }
    }
}


[Serializable]
public class InventoryItemData
{
    public string itemName;
    public int count;
}

[Serializable]
public class InventoryData // This will grab the data and put them on a list
{
    public List<InventoryItemData> items = new List<InventoryItemData>();
    public List<string> collectedItemIDs = new List<string>();
}

[Serializable]
public class PlayerData
{
    public float health;
    public Vector3 position;
    public string sceneName;
    // Add other player stats here
}