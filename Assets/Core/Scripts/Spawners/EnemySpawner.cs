using InterOrbital.Combat.IA;
using InterOrbital.Player;
using InterOrbital.Utils;
using InterOrbital.WorldSystem;
using InterOrbital.Spawners;
using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.Spawner
{
    public class EnemySpawner : InterOrbital.Spawners.Spawner
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private DifficultyArea _difficultyArea;   

        // private IEnumerator SpawnEnemy()
        // {
        //     if (currentEnemiesSpawned < _maxEnemiesSpawn)
        //     {
        //         Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;
        //         if (spawnPosition.x >= 0 && spawnPosition.x < GridLogic.Instance.width && spawnPosition.y >= 0 &&
        //             spawnPosition.y < GridLogic.Instance.height)
        //         {
        //             GameObject enemySpawned = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        //             enemySpawned.GetComponent<EnemyAgentBase>().SetEnemySpawner(this);
        //             currentEnemiesSpawned++;
        //         }
        //     }
        //
        //     yield return null;
        // }

        protected override void SpawnAllEntities()
        {
            canSpawn = false;
            var canSpawnCount = _maxEntitiesSpawn - currentEntitiesSpawned;
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
                    currentEntitiesSpawned++;
                }
            }
        }

        public DifficultyArea GetDifficultyArea()
        {
            return _difficultyArea;
        }

        public void EnemyDead()
        {
            currentEntitiesSpawned--;
        }

        
    }
}