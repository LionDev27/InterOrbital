using System;
using InterOrbital.Combat.IA;
using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InterOrbital.Recollectables.Spawner
{
    public class ResourcesSpawner : MonoBehaviour
    {
        [HideInInspector] public bool canSpawn;
        [SerializeField] private List<GameObject> _resourcePrefabs;
        [SerializeField] private Transform player; // Referencia al transform del jugador
        [SerializeField] private LayerMask _layer;
        [SerializeField] private int _maxResourcesSpawn;
        [SerializeField] private float _distanceBetween = 3f;
        [SerializeField] private float _spawnRadius = 15f;
        [SerializeField] private float _visibleDistance = 15f;

        private bool _playerInRadius;
        private float _playerNearSpawnTimer = -1;
        private float _spawnTimer;
        private int currentResourcesSpawned = 0; // Contador de enemigos actual
        private Vector2 resourceDimensions;

        private void Start()
        {
            player = PlayerComponents.Instance.transform;
            canSpawn = true;
        }

        private void Update()
        {
            SpawnResources();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInRadius = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInRadius = false;
                if (_playerNearSpawnTimer < 0)
                    _playerNearSpawnTimer = 20f;
            }
        }

        private bool CanSpawn()
        {
            return canSpawn && _playerNearSpawnTimer < 0 && NotVisibleInCamera();
        }

        private bool NotVisibleInCamera()
        {
            return (Vector3.Distance(transform.position, player.position) >= _spawnRadius + _visibleDistance) &&
                   _playerInRadius;
        }

        private void SpawnResources()
        {
            if (CanSpawn())
            {
                if (currentResourcesSpawned < _maxResourcesSpawn)
                    SpawnAllResources();
                else
                    canSpawn = false;
            }

            if (_playerNearSpawnTimer >= 0)
            {
                _playerNearSpawnTimer -= Time.deltaTime;
            }
        }

        // private IEnumerator SpawnResource()
        // {
        //     if (currentResourcesSpawned < _maxResourcesSpawn)
        //     {
        //         Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;
        //         Vector3Int spawnPositionInt = new Vector3Int((int)spawnPosition.x, (int)spawnPosition.y, 0);
        //
        //         if (spawnPositionInt.x >= 0 && spawnPositionInt.x < GridLogic.Instance.width &&
        //             spawnPositionInt.y >= 0 && spawnPositionInt.y < GridLogic.Instance.height)
        //         {
        //             int resourceIndex = UnityEngine.Random.Range(0, _resourcePrefabs.Count);
        //             resourceDimensions = _resourcePrefabs[resourceIndex].GetComponent<Recollectable>().GetDimensions();
        //             if (IsPosibleToSpawn(spawnPositionInt.x, spawnPositionInt.y, resourceDimensions))
        //             {
        //                 GameObject resource = Instantiate(_resourcePrefabs[resourceIndex], spawnPositionInt,
        //                     Quaternion.identity);
        //                 LockCellsOnSpawn(spawnPositionInt.x, spawnPositionInt.y);
        //                 currentResourcesSpawned++;
        //                 resource.GetComponent<Recollectable>().SetSpawnerRef(this);
        //             }
        //         }
        //     }
        //
        //     yield return null;
        // }

        private void SpawnAllResources()
        {
            canSpawn = false;
            var canSpawnCount = _maxResourcesSpawn - currentResourcesSpawned;
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
                    int resourceIndex = Random.Range(0, _resourcePrefabs.Count);
                    resourceDimensions = _resourcePrefabs[resourceIndex].GetComponent<Recollectable>().GetDimensions();
                    if (IsPosibleToSpawn(spawnPositionInt.x, spawnPositionInt.y, resourceDimensions))
                    {
                        GameObject resource = Instantiate(_resourcePrefabs[resourceIndex], spawnPositionInt,
                            Quaternion.identity);
                        LockCellsOnSpawn(spawnPositionInt.x, spawnPositionInt.y);
                        currentResourcesSpawned++;
                        resource.GetComponent<Recollectable>().SetSpawnerRef(this);
                    }
                }
            }
        }

        private bool IsPosibleToSpawn(int x, int y, Vector2 dimensions)
        {
            if (GridLogic.Instance.IsCellAreaLocked(x, y, dimensions))
            {
                return false;
            }

            if (GridLogic.Instance.IsCellAreaSpaceshipArea(x, y, dimensions))
            {
                return false;
            }

            return true;
        }

        private void LockCellsOnSpawn(int x, int y)
        {
            GridLogic.Instance.LockCell(x, y);
        }

        public void ResourceObtained()
        {
            currentResourcesSpawned--;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
    }
}