using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform player; // Referencia al transform del jugador
    [SerializeField] private int _maxEnemiesSpawn;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnDelay = 1f;
    [SerializeField ]public float _spawnActivationDistance = 10f; // Distancia de activación

    private float _spawnTimer;
    private int currentEnemies = 0; // Contador de enemigos actual
    private bool playerInRange = false;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= _spawnActivationDistance;

        if (playerInRange && currentEnemies < _maxEnemiesSpawn)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnDelay);

        if (currentEnemies < _maxEnemiesSpawn)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;

            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            currentEnemies++;
        }
    }
}
