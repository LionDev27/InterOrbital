using InterOrbital.Combat.IA;
using InterOrbital.Player;
using InterOrbital.Utils;
using InterOrbital.WorldSystem;
using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private DifficultyArea _difficultyArea;
        [SerializeField] private Transform player; // Referencia al transform del jugador
        [SerializeField] private LayerMask _layer;
        [SerializeField] private int _maxEnemiesSpawn;
        [SerializeField] private float _distanceBetween = 3f;
        [SerializeField] private float _spawnRadius = 5f;
        [SerializeField] private float _spawnDelay = 3f;

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
                if (_spawnTimer >= 0)
                {
                    _spawnTimer -= Time.deltaTime;
                }

                if (_spawnTimer < 0)
                {
                    if (currentEnemiesSpawned < _maxEnemiesSpawn)
                    {
                        SpawnAllEnemies();
                        _spawnTimer = _spawnDelay;
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
                if (spawnPosition.x >= 0 && spawnPosition.x < GridLogic.Instance.width && spawnPosition.y >= 0 &&
                    spawnPosition.y < GridLogic.Instance.height)
                {
                    GameObject enemySpawned = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                    enemySpawned.GetComponent<EnemyAgentBase>().SetEnemySpawner(this);
                    currentEnemiesSpawned++;
                }
            }

            yield return null;
        }

        private void SpawnAllEnemies()
        {
            var canSpawnCount = _maxEnemiesSpawn - currentEnemiesSpawned;
            if (canSpawnCount <= 0) return;
            for (int i = 0; i < canSpawnCount; i++)
            {
                Vector2 spawnPosition = new Vector2();
                for (int cont = 0; cont < 1000; cont++)
                {
                    spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, _distanceBetween, _layer.value);
                    if (colliders.Length > 1)
                        continue;
                    break;
                }

                Vector3Int spawnPositionInt = new Vector3Int((int)spawnPosition.x, (int)spawnPosition.y, 0);

                if (spawnPositionInt.x >= 0 && spawnPositionInt.x < GridLogic.Instance.width &&
                    spawnPositionInt.y >= 0 && spawnPositionInt.y < GridLogic.Instance.height)
                {
                    GameObject enemySpawned = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
                    enemySpawned.GetComponent<EnemyAgentBase>().SetEnemySpawner(this);
                    currentEnemiesSpawned++;
                }
            }
        }

        public DifficultyArea GetDifficultyArea()
        {
            return _difficultyArea;
        }

        public void EnemyDead()
        {
            currentEnemiesSpawned--;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
    }
}