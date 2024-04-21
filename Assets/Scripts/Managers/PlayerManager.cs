using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private Vector3 spawnPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetSpawnPoint(Vector3 position)
    {
        spawnPosition = position;
    }

    // Method to respawn the player at the spawn point position
    public void RespawnPlayer()
    {
        transform.position = spawnPosition;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RespawnPlayer();
    }

}
