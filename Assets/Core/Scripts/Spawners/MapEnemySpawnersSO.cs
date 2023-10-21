using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace InterOrbital.Combat.Spawner
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapEnemySpawnersSO", order = 3)]

    public class MapEnemySpawnersSO : ScriptableObject
    {
        public int easySpawnersAmount;
        public int mediumSpawnersAmount;
        public int hardSpawnersAmount;
        public float distanceBetweenSpawners;
        [SerializeField] private List<GameObject> _EasyEnemySpawners;
        private List<GameObject> _EasyEnemySpawnersSpawned;
        [SerializeField] private List<GameObject> _MediumEnemySpawners;
        private List<GameObject> _MediumEnemySpawnersSpawned;
        [SerializeField] private List<GameObject> _HardEnemySpawners;
        private List<GameObject> _HardEnemySpawnersSpawned;

        public void ResetSpawners()
        {
            if(_EasyEnemySpawners.Count == 0 && _EasyEnemySpawnersSpawned.Count > 0)
            {
                foreach(GameObject obj in _EasyEnemySpawnersSpawned)
                {
                    _EasyEnemySpawners.Add(obj);
                }
                _EasyEnemySpawnersSpawned = new List<GameObject>();
            }

            if (_MediumEnemySpawners.Count == 0 && _MediumEnemySpawnersSpawned.Count > 0)
            {
                foreach (GameObject obj in _MediumEnemySpawnersSpawned)
                {
                    _MediumEnemySpawners.Add(obj);
                }
                _MediumEnemySpawnersSpawned = new List<GameObject>();
            }

            if (_HardEnemySpawners.Count == 0 && _HardEnemySpawnersSpawned.Count > 0)
            {
                foreach (GameObject obj in _HardEnemySpawnersSpawned)
                {
                    _HardEnemySpawners.Add(obj);
                }
                _HardEnemySpawnersSpawned = new List<GameObject>();
            }
        }

        public GameObject GetEnemySpawnerByDifficultArea(DifficultyArea area)
        {
            if (area == DifficultyArea.Easy)
            {
                return GetEasyEnemySpawner();
            }
            else if(area == DifficultyArea.Medium)
            {
                return GetMediumEnemySpawner();
            }
            else
            {
                return GetHardEnemySpawner();
            }
        }

        private GameObject GetEasyEnemySpawner()
        {
            if (_EasyEnemySpawners.Count > 0)
            {
                GameObject enemySpawner = _EasyEnemySpawners[0];
                _EasyEnemySpawnersSpawned.Add(enemySpawner);
                _EasyEnemySpawners.RemoveAt(0);

                return enemySpawner;
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, _EasyEnemySpawnersSpawned.Count);
                return _EasyEnemySpawnersSpawned[randomIndex];
            }
        }

        private GameObject GetMediumEnemySpawner()
        {
            if (_MediumEnemySpawners.Count > 0)
            {
                GameObject enemySpawner = _MediumEnemySpawners[0];
                _MediumEnemySpawnersSpawned.Add(enemySpawner);
                _MediumEnemySpawners.RemoveAt(0);

                return enemySpawner;
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, _MediumEnemySpawnersSpawned.Count);
                return _MediumEnemySpawnersSpawned[randomIndex];
            }
        }

        private GameObject GetHardEnemySpawner()
        {
            if (_HardEnemySpawners.Count > 0)
            {
                GameObject enemySpawner = _HardEnemySpawners[0];
                _HardEnemySpawnersSpawned.Add(enemySpawner);
                _HardEnemySpawners.RemoveAt(0);

                return enemySpawner;
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, _HardEnemySpawnersSpawned.Count);
                return _HardEnemySpawnersSpawned[randomIndex];
            }
        }
    }
}

