using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject[] parentToDeactivate;

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

            SetParentsActive(true);
        }
        else
        {
            // Disable the pickups parent GameObject if the scene is not named "Planet"
            SetParentsActive(false);
        }
    }

    void SetParentsActive(bool activeState)
    {
        foreach (GameObject parent in parentToDeactivate)
        {
            parent.SetActive(activeState);
        }
    }
}