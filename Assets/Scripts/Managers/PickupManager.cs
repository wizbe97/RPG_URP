using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupManager : MonoBehaviour
{
    public GameObject pickupsParent;

    void Awake()
    {
        // Ensure this GameObject persists between scenes
        DontDestroyOnLoad(gameObject);

        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is named "Planet"
        if (scene.name == "Planet")
        {
            // Enable the pickups parent GameObject only if the scene is named "Planet"
            pickupsParent.SetActive(true);
        }
        else
        {
            // Disable the pickups parent GameObject if the scene is not named "Planet"
            pickupsParent.SetActive(false);
        }
    }
}
