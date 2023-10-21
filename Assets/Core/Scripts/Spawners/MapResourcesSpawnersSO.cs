using InterOrbital.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.Combat.Spawner
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapResourcesSpawnersSO", order = 4)]

    public class MapResourcesSpawnersSO : ScriptableObject
    {
        public int easySpawnersAmount;
        public int mediumSpawnersAmount;
        public int hardSpawnersAmount;
        public float distanceBetweenSpawners;
        [SerializeField] private List<ResourceBiomeSpawner> _EasyResourceSpawners;
        private List<ResourceBiomeSpawner> _EasyResourceSpawnersSpawned;
        [SerializeField] private List<ResourceBiomeSpawner> _MediumResourceSpawners;
        private List<ResourceBiomeSpawner> _MediumResourceSpawnersSpawned;
        [SerializeField] private List<ResourceBiomeSpawner> _HardResourceSpawners;
        private List<ResourceBiomeSpawner> _HardResourceSpawnersSpawned;

        public List<GameObject> GetRandomResourceSpawnerFromBiome(string biome, List<ResourceBiomeSpawner> resourcesBiomeSpawners)
        {
            foreach (ResourceBiomeSpawner resourceBiomeSpawner in resourcesBiomeSpawners)
            {
                if (resourceBiomeSpawner.biome == biome && resourceBiomeSpawner._resourcesSpawners.Count > 0)
                {
                    return resourceBiomeSpawner._resourcesSpawners;
                }
            }

            return null;
        }

        public bool BiomeExist(string biome, List<ResourceBiomeSpawner> resourcesBiomeSpawners)
        {
            foreach (ResourceBiomeSpawner resourceBiomeSpawner in resourcesBiomeSpawners)
            {
                if (resourceBiomeSpawner.biome == biome)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddSpawnerToBiomeList(GameObject spawner, string biome, List<ResourceBiomeSpawner> resourcesBiomeSpawners)
        {
            for (int i = 0; i < resourcesBiomeSpawners.Count; i++) 
            {
                if (resourcesBiomeSpawners[i].biome == biome)
                {
                    resourcesBiomeSpawners[i]._resourcesSpawners.Add(spawner);
                    break;
                }
            }
        }

        public void RemoveAtBiomeList(int index, string biome, List<ResourceBiomeSpawner> resourcesBiomeSpawners)
        {
            for (int i = 0; i < resourcesBiomeSpawners.Count; i++) 
            {
                if (resourcesBiomeSpawners[i].biome == biome)
                {
                    resourcesBiomeSpawners[i]._resourcesSpawners.RemoveAt(index);
                    break;
                }
            }
        }

        public void ClearBiomeList(string biome, List<ResourceBiomeSpawner> resourcesBiomeSpawners)
        {
            for (int i = 0; i < resourcesBiomeSpawners.Count; i++) 
            {
                if (resourcesBiomeSpawners[i].biome == biome)
                {
                    resourcesBiomeSpawners[i]._resourcesSpawners.Clear();
                    break;
                }
            }
        }

        

        public void ResetSpawners()
        {
            foreach (ResourceBiomeSpawner rbs in _EasyResourceSpawners)
            {
                List<GameObject> easySpawnedBiomeFiltered = GetRandomResourceSpawnerFromBiome(rbs.biome,_EasyResourceSpawnersSpawned);
                if(easySpawnedBiomeFiltered != null)
                {
                    foreach (GameObject obj in easySpawnedBiomeFiltered)
                    {
                        rbs._resourcesSpawners.Add(obj);
                    }
                    ClearBiomeList(rbs.biome, _EasyResourceSpawnersSpawned);
                }
            }

            foreach (ResourceBiomeSpawner rbs in _MediumResourceSpawners)
            {
                List<GameObject> mediumSpawnedBiomeFiltered = GetRandomResourceSpawnerFromBiome(rbs.biome, _MediumResourceSpawnersSpawned);
                if (mediumSpawnedBiomeFiltered != null)
                {
                    foreach (GameObject obj in mediumSpawnedBiomeFiltered)
                    {
                        rbs._resourcesSpawners.Add(obj);
                    }
                    ClearBiomeList(rbs.biome, _MediumResourceSpawnersSpawned);
                }
            }

            foreach (ResourceBiomeSpawner rbs in _HardResourceSpawners)
            {
                List<GameObject> hardSpawnedBiomeFiltered = GetRandomResourceSpawnerFromBiome(rbs.biome, _HardResourceSpawnersSpawned);
                if (hardSpawnedBiomeFiltered != null)
                {
                    foreach (GameObject obj in hardSpawnedBiomeFiltered)
                    {
                        rbs._resourcesSpawners.Add(obj);
                    }
                    ClearBiomeList(rbs.biome, _HardResourceSpawnersSpawned);
                }
            }
        }

        public GameObject GetResourceSpawnerByDifficultArea(DifficultyArea area, string biome)
        {
            if (area == DifficultyArea.Easy)
            {
                return GetEasyResourceSpawner(biome);
            }
            else if (area == DifficultyArea.Medium)
            {
                return GetMediumResourceSpawner(biome);
            }
            else
            {
                return GetHardResourceSpawner(biome);
            }
        }

        private GameObject GetEasyResourceSpawner(string biome)
        {
            if (_EasyResourceSpawners.Count > 0)
            {
                List<GameObject> resourceBiomeFilteredSpawner = GetRandomResourceSpawnerFromBiome(biome, _EasyResourceSpawners);
                if (resourceBiomeFilteredSpawner != null)
                {
                    GameObject resourceSpawner = resourceBiomeFilteredSpawner[0];
                    AddSpawnerToBiomeList(resourceSpawner, biome, _EasyResourceSpawnersSpawned);
                    RemoveAtBiomeList(0,biome, _EasyResourceSpawners);
                    
                    return resourceSpawner;
                }
                else
                {
                    List<GameObject> resourceBiomeSpawner = GetRandomResourceSpawnerFromBiome(biome, _EasyResourceSpawnersSpawned);

                    if (resourceBiomeSpawner != null)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, resourceBiomeSpawner.Count);
                        return resourceBiomeSpawner[randomIndex];
                    }
                }
            }
            return null;
        }

        private GameObject GetMediumResourceSpawner(string biome)
        {
            if (_MediumResourceSpawners.Count > 0)
            {
                List<GameObject> resourceBiomeFilteredSpawner = GetRandomResourceSpawnerFromBiome(biome, _MediumResourceSpawners);

                if (resourceBiomeFilteredSpawner != null)
                {
                    GameObject resourceSpawner = resourceBiomeFilteredSpawner[0];
                    AddSpawnerToBiomeList(resourceSpawner, biome, _MediumResourceSpawnersSpawned);
                    RemoveAtBiomeList(0, biome, _MediumResourceSpawners);

                    return resourceSpawner;
                }
                else
                {
                    List<GameObject> resourceBiomeSpawner = GetRandomResourceSpawnerFromBiome(biome, _MediumResourceSpawnersSpawned);

                    if (resourceBiomeSpawner != null)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, resourceBiomeSpawner.Count);
                        return resourceBiomeSpawner[randomIndex];
                    }
                }
            }
            return null;
        }

        private GameObject GetHardResourceSpawner(string biome)
        {
            if (_HardResourceSpawners.Count > 0)
            {
                List<GameObject> resourceBiomeFilteredSpawner = GetRandomResourceSpawnerFromBiome(biome, _HardResourceSpawners);
                if (resourceBiomeFilteredSpawner != null)
                {
                    GameObject resourceSpawner = resourceBiomeFilteredSpawner[0];
                    AddSpawnerToBiomeList(resourceSpawner, biome, _HardResourceSpawnersSpawned);
                    RemoveAtBiomeList(0, biome, _HardResourceSpawners);

                    return resourceSpawner;
                }
                else
                {
                    List<GameObject> resourceBiomeSpawner = GetRandomResourceSpawnerFromBiome(biome, _HardResourceSpawnersSpawned);
                    if (resourceBiomeSpawner != null)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, resourceBiomeSpawner.Count);
                        return resourceBiomeSpawner[randomIndex];
                    }
                }
            }
            return null;
        }
    }
}

