using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab here
    public GameObject[] enemyShootingPositions; // Assign your shooting positions here
    public GameObject[] enemySpawnPositions; // Assign your spawn positions here
    public GameObject playerPositionObject;

    private int enemiesDestroyed = 0;
    private int shotsFiredByPlayer = 0;

    void Start()
    {

        // InvokeRepeating("SpawnEnemy", 1f, 5f);

        SpawnEnemy();
    }

    public void PlayerShotFired()
    {
        shotsFiredByPlayer++;
    }

    public void EnemyDestroyed()
    {
        enemiesDestroyed++;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null && enemyShootingPositions.Length > 0 && enemySpawnPositions.Length > 0)
        {
            // Randomly select one of the shooting positions
            GameObject selectedShootingPosition = enemyShootingPositions[Random.Range(0, enemyShootingPositions.Length)];

            // Randomly select one of the spawn positions
            Transform selectedSpawnPosition = enemySpawnPositions[Random.Range(0, enemySpawnPositions.Length)].transform;

            GameObject enemy = Instantiate(enemyPrefab, selectedSpawnPosition.position, Quaternion.identity);

            // Assign the shooting position to the enemy
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.shootingPositionObject = selectedShootingPosition;
            enemyController.playerPositionObject = playerPositionObject;

            // Now, set the shooting destination
            enemyController.SetShootingDestination(selectedShootingPosition);
        }
    }

    public float GetPlayerAccuracy()
    {
        if (shotsFiredByPlayer == 0) return 0; // Avoid division by zero
        return ((float)enemiesDestroyed / shotsFiredByPlayer) * 100; // This returns the accuracy as a percentage
    }

    // Additional utility functions if you need them
    public int GetShotsFiredByPlayer()
    {
        return shotsFiredByPlayer;
    }

    public int GetEnemiesDestroyedCount()
    {
        return enemiesDestroyed;
    }
}
