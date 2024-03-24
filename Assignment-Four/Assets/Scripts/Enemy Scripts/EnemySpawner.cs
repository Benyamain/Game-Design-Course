using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject newEnemy;
    [SerializeField]
    private Transform[] spawnPosition;
    [SerializeField]
    private int enemySpawnLimit = 10;
    [SerializeField]
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField]
    private float minSpawnTime = 2f, maxSpawnTime = 5f;
    private bool isReturningToSpawn = false;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
        Invoke("SpawnEnemy", Random.Range(minSpawnTime, maxSpawnTime));
    }

    private void SpawnEnemy() {
        // Timed spawner
        Invoke("SpawnEnemy", Random.Range(minSpawnTime, maxSpawnTime));

        // Do not spawn more than the max amount set
        if (spawnedEnemies.Count == enemySpawnLimit) return;
        
        // Keep respawning as long as the game is still active (the player is not dead yet)
        if (!GameManager.IsPlayerDead) {
            newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
            
            // Check if the enemy is already returning to spawn before starting a new coroutine
            // TODO: Ask about spawn not working properly
            if (!isReturningToSpawn)
            {
                StartCoroutine(ReturnToSpawn(newEnemy));
            }
        }
    }

    private IEnumerator ReturnToSpawn(GameObject enemy)
    {
        isReturningToSpawn = true;

        yield return new WaitForSeconds(10f);

        // Enemy goes back to one of the spawn points
        if (enemy != null && !GameManager.IsPlayerDead)
        {
            enemy.transform.position = GetRandomSpawnPosition();
        }

        // Reset the flag to indicate coroutine has finished
        isReturningToSpawn = false;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return spawnPosition[Random.Range(0, spawnPosition.Length)].position;
    }

    public void EnemyDied(GameObject enemy) {
        spawnedEnemies.Remove(enemy);
    }
}