using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectManager : MonoBehaviour
{
    public GameObject[] parentToDeactivate;
    private WaveSpawner waveSpawner;

    void Awake()
    {
        // Ensure this GameObject persists between scenes
        DontDestroyOnLoad(gameObject);
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        // Check if the loaded scene is named "Planet"
        if (scene.name == "Planet")
        {

            SetParentsActive(true);
            waveSpawner.startNextWave = true;
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
            if (parent != null)
            {
                parent.SetActive(activeState);
            }
        }
    }

}
