using InterOrbital.Combat.IA;
using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InterOrbital.Recollectables.Spawner
{
    public class ResourcesSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _resourcePrefabs;
        [SerializeField] private Transform player; // Referencia al transform del jugador
        [SerializeField] private int _maxResourcesSpawn;
        [SerializeField] private float _spawnRadius = 15f;
        [SerializeField] private float _spawnDelay = 2f;
        [SerializeField] public float _spawnMinActivationDistance = 20f; // Distancia de activación

        private bool _canSpawn;
        private float _playerNearSpawnTimer = -1;
        private float _spawnTimer;
        private int currentResourcesSpawned = 0; // Contador de enemigos actual
        private bool playerInRangeToSpawn = false;
        private Vector2 resourceDimensions;

        private void Start()
        {
            player = PlayerComponents.Instance.transform;
        }
        private void Update()
        {
            SpawnResources();
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

        private void SpawnResources()
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
                        if (currentResourcesSpawned < _maxResourcesSpawn)
                        {
                            StartCoroutine(SpawnResource());
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

        private IEnumerator SpawnResource()
        {
            if (currentResourcesSpawned < _maxResourcesSpawn)
            {
                Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;
                Vector3Int spawnPositionInt = new Vector3Int((int)spawnPosition.x, (int)spawnPosition.y,0);

                if (spawnPositionInt.x >= 0 && spawnPositionInt.x < GridLogic.Instance.width && spawnPositionInt.y >= 0 && spawnPositionInt.y < GridLogic.Instance.height)
                {
                    int resourceIndex = UnityEngine.Random.Range(0, _resourcePrefabs.Count);
                    resourceDimensions = _resourcePrefabs[resourceIndex].GetComponent<Recollectable>().GetDimensions();
                    if (!GridLogic.Instance.IsCellAreaLocked(spawnPositionInt.x, spawnPositionInt.y, resourceDimensions))
                    {
                        Instantiate(_resourcePrefabs[resourceIndex], spawnPositionInt, Quaternion.identity);
                        currentResourcesSpawned++;
                    }
                }

            }
            yield return null;
        }

        public void ResourceObtained()
        {
            currentResourcesSpawned--;
        }

    }
}