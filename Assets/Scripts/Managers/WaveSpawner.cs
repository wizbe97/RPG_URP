using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    private int spawnIndex;
    public int difficulty;
    public float spawnDelay = 1.0f;

    private float waveDuration;
    private float waveTimer;
    private float spawnInterval;
    public bool startNextWave;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        GenerateWave();
        startNextWave = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedEnemies.Count <= 0 && waveTimer <= 0)
        {
            // Increment the wave number to start the next wave
            currWave++;
            GenerateWave();
        }

    }

    public void GenerateWave()
    {
        waveValue = currWave * difficulty;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only

        StartCoroutine(SpawnEnemiesWithDelay());
    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        foreach (GameObject enemyPrefab in enemiesToSpawn)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnLocation[spawnIndex].position, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            // Update spawnIndex for next spawn
            spawnIndex = (spawnIndex + 1) % spawnLocation.Length;

            yield return new WaitForSeconds(spawnDelay); // Use the explicit spawn delay
        }
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}