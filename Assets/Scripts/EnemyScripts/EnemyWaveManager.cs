using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clasa prin care sunt definite wave-urile 
[System.Serializable]
public class Wave
{
    public EnemySO[] enemies;  // Array of enemies for this wave
    public int[] counts;       // Number of each enemy to spawn
    public float spawnInterval; // Time between each spawn
}

// Manager care se ocupa cu spawnarea de wave-uri 
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnerManager enemySpawner;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private float timeBetweenWaves = 15f; // Time in seconds between waves

    private int currentWaveIndex = 0;
    private float nextSpawnTime = 0;
    private int enemyCountToSpawn = 0;
    private int enemyTypeIndex = 0;

    void Start()
    {
        StartCoroutine(StartNextWaveAfterDelay());
    }

    void Update()
    {
        if (enemyCountToSpawn > 0 && Time.time >= nextSpawnTime)
        {
            SpawnNextEnemy();
        }
    }

    IEnumerator StartNextWaveAfterDelay()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartWave();
            while (enemyCountToSpawn > 0)
            {
                yield return null;
            }
            currentWaveIndex++;
        }
        UIManager.Instance.ShowWinScreen();
    }

    void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            return; 
        }

        Wave currentWave = waves[currentWaveIndex];
        enemyCountToSpawn = 0;
        for (int i = 0; i < currentWave.counts.Length; i++)
        {
            enemyCountToSpawn += currentWave.counts[i];
        }
        enemyTypeIndex = 0;
        nextSpawnTime = Time.time + currentWave.spawnInterval;
    }

    void SpawnNextEnemy()
    {
        Wave currentWave = waves[currentWaveIndex];
        if (currentWave.counts[enemyTypeIndex] > 0)
        {
            enemySpawner.SpawnEnemy(currentWave.enemies[enemyTypeIndex]);
            currentWave.counts[enemyTypeIndex]--;
            enemyCountToSpawn--;
        }
        else
        {
            enemyTypeIndex++;
        }
        if (enemyCountToSpawn > 0)
        {
            nextSpawnTime = Time.time + currentWave.spawnInterval;
        }
    }
}
