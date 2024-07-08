using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    //HERE ARE ASSIGNED THE PREFABS WHICH WILL BE SPAWNED
    public GameplayUI gameplayUIPrefab;
    public SaveManager saveManagerPrefab;
    public Inventory inventoryManagerPrefab;
    public HealthBar healthBarManagerPrefab;
    public Player playerManagerPrefab;

    //THIS WILL BE THE INITIAL POSITION WHERE THE PLAYER WILL SPAWN WHEN YOU FIRST START THE SCENE
    public Transform playerSpawnPosition;
    public Vector3 scenePlayerSpawnPosition;


    //THESE WILL HOLD REFERENCES FOR THE OTHER MANAGERS WHICH YOU CAN CALL FROM ANY SCRIPT USING GameManager.Instance....
    [HideInInspector] public GameplayUI gameplayUI;
    [HideInInspector] public SaveManager saveManager;
    [HideInInspector] public Inventory inventoryManager;
    [HideInInspector] public HealthBar healthBarManager;
    [HideInInspector] public Player playerManager;

    public int currentSlot;
    private bool isOnMenu = false;
    private bool isLocal = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            if(currentSlot == 0) currentSlot = 1;
            scenePlayerSpawnPosition = playerSpawnPosition.position;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            SpawnMenuManagers();
            isOnMenu = true;
        }
        else
        {
            SpawnManagers();
            Invoke(nameof(LoadAllData), 0.1f);
        }
    }


    //SPAWN ALL PREFABS AND ASSIGN THE MANAGERS TO THEM
    private void SpawnManagers()
    {
        if (gameplayUI == null)
            gameplayUI = Instantiate(gameplayUIPrefab);

        if (saveManager == null)
            saveManager = Instantiate(saveManagerPrefab);

        if (inventoryManager == null)
            inventoryManager = Instantiate(inventoryManagerPrefab);

        if (playerManager == null)
            playerManager = Instantiate(playerManagerPrefab, scenePlayerSpawnPosition, Quaternion.identity);

        if (healthBarManager == null)
            healthBarManager = Instantiate(healthBarManagerPrefab);


        CinemachineVirtualCamera vCamGameObject = GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        vCamGameObject.Follow = playerManager.transform;
    }



    //IF IT IS ON MENU IT DOESN"T NEED TO SPAWN PLAYER AND OTHER MANAGERS
    //AND ALSO DESTROY MANAGERS SUCH AS PLAYER,INVENTORY...
    private void SpawnMenuManagers()
    {
        if (playerManager != null)
            Destroy(playerManager.gameObject);

        if (inventoryManager != null)
            Destroy(inventoryManager.gameObject);

        if (gameplayUI != null)
            Destroy(gameplayUI.gameObject);

        if (healthBarManager != null)
            Destroy(healthBarManager.gameObject);


        if (saveManager == null)
            saveManager = Instantiate(saveManagerPrefab);
    }

    private Vector3 GetPlayerPosition()
    {
        PlayerData loadedData = SaveManager.Instance.LoadPlayerStats(currentSlot);
        if (loadedData != null)
        {
            return loadedData.position;
        }
        return playerSpawnPosition.position;
    }

    //HERE YOU WILL PUT OTHER THINGS THAT YOU WANT TO SAVE IN THE FUTURE
    //CURRENTLY ONLY HAS INVENTORY AND PLAYER STATS
    public void SaveAllData(bool isLocal)
    {
        this.isLocal = isLocal;

        int slotToSave = isLocal ? 0 : currentSlot;
        playerManager.SavePlayerData(slotToSave);
        inventoryManager.SaveInventoryData(slotToSave);
    }


    public void LoadAllData()
    {
        if(isOnMenu)
        {
            isLocal = false;
        }
        
        int slotToSave = isLocal ? 0 : currentSlot;

        playerManager.LoadPlayerData(slotToSave);
        inventoryManager.LoadInventoryData(slotToSave);

        if (!isOnMenu)
        {
            playerManager.transform.position = scenePlayerSpawnPosition;
        }
        else
        {
            playerManager.transform.position = GetPlayerPosition();
        }

        isOnMenu = false;

    }

    public void RemoveSlot(int slot)
    {
        saveManager.RemoveSlot(slot);
    }

    //RELOADING THE SAME SCENE
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        //If there are no saved data then load Planet scene by default
        string sceneToLoad = "Planet";

        PlayerData loadedData = SaveManager.Instance.LoadPlayerStats(currentSlot);
        if (loadedData != null)
        {
            sceneToLoad = loadedData.sceneName;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
