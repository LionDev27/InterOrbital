using InterOrbital.Combat.IA;
using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform player; // Referencia al transform del jugador
        [SerializeField] private int _maxEnemiesSpawn;
        [SerializeField] private float _spawnRadius = 5f;
        [SerializeField] private float _spawnDelay = 3f;
        [SerializeField] public float _spawnMinActivationDistance = 15f; // Distancia de activación

        private bool _canSpawn;
        private float _playerNearSpawnTimer = -1;
        private float _spawnTimer;
        private int currentEnemiesSpawned = 0; // Contador de enemigos actual
        private bool playerInRangeToSpawn = false;

        private void Start()
        {
            player = PlayerComponents.Instance.transform;
        }

        private void Update()
        {
            SpawnEnemies();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _canSpawn = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _canSpawn = false;
                if (_playerNearSpawnTimer < 0)
                {
                    _playerNearSpawnTimer = 20f;
                }
            }
        }

        private void SpawnEnemies()
        {
            if (_canSpawn && _playerNearSpawnTimer < 0)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                playerInRangeToSpawn = distance > _spawnMinActivationDistance;

                if (playerInRangeToSpawn)
                {
                    if (_spawnTimer >= 0)
                    {
                        _spawnTimer -= Time.deltaTime;
                    }

                    if (_spawnTimer < 0)
                    {
                        if (currentEnemiesSpawned < _maxEnemiesSpawn)
                        {
                            StartCoroutine(SpawnEnemy());
                            _spawnTimer = _spawnDelay;
                        }
                    }
                }
            }

            if (_playerNearSpawnTimer >= 0)
            {
                _playerNearSpawnTimer -= Time.deltaTime;
            }
        }

        private IEnumerator SpawnEnemy()
        {
            if (currentEnemiesSpawned < _maxEnemiesSpawn)
            {
                Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;

                GameObject enemySpawned = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                enemySpawned.GetComponent<EnemyAgentBase>().SetEnemySpawner(this);
                currentEnemiesSpawned++;
            }
            yield return null;
        }

        public void EnemyDead()
        {
            currentEnemiesSpawned--;
        }
    }
}
