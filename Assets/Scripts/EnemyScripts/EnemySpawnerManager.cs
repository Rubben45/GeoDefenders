using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Acest manager spawneaza efectiv inamicii dupa instructiunile primite de EnemyWaveManager
public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Transform[] firstTargetLocations;
    [SerializeField] private Transform[] secondTargetLocations;
    [SerializeField] private Transform[] thirdTargetLocations;
    [SerializeField] private Transform[] fourthTargetLocations;
    [SerializeField] private Transform[] fifthTargetLocations;
    [SerializeField] private Transform[] sixthTargetLocations;
    [SerializeField] private Transform[] lastTargetLocations;

    public void SpawnEnemy(EnemySO enemyType)
    {
        int randomSpawnLocationNum = Random.Range(0, spawnLocations.Length);

        GameObject spawnedEnemy = Instantiate(enemyType.EnemyPrefab, spawnLocations[randomSpawnLocationNum].position, Quaternion.identity);
        Enemy spawnedEnemyBrain = spawnedEnemy.GetComponent<Enemy>();

        SetSpawnedEnemyTargetLocations(spawnedEnemyBrain);
        spawnedEnemyBrain.EnemySetUp(enemyType);    
    }

    private void SetSpawnedEnemyTargetLocations(Enemy enemyToSetLocations)
    {
        List<Transform> tempLocations = new();

        tempLocations.Add(firstTargetLocations[Random.Range(0, firstTargetLocations.Length)]);
        tempLocations.Add(secondTargetLocations[Random.Range(0, secondTargetLocations.Length)]);
        tempLocations.Add(thirdTargetLocations[Random.Range(0, thirdTargetLocations.Length)]);
        tempLocations.Add(fourthTargetLocations[Random.Range(0, fourthTargetLocations.Length)]);
        tempLocations.Add(fifthTargetLocations[Random.Range(0, fifthTargetLocations.Length)]);
        tempLocations.Add(sixthTargetLocations[Random.Range(0, sixthTargetLocations.Length)]);
        tempLocations.Add(lastTargetLocations[Random.Range(0, lastTargetLocations.Length)]);

        foreach (Transform location in tempLocations)
        {
            enemyToSetLocations.TargetLocations.Add(location);
        }
    }
}
