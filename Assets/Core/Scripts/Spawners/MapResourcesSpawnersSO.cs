using InterOrbital.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.Combat.Spawner
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapResourcesSpawnersSO", order = 4)]

    public class MapResourcesSpawnersSO : ScriptableObject
    {
        public int spawnersAmount;
        public float distanceBetweenSpawners;
        public List<ResourceBiomeSpawner> _resourcesBiomeSpawners;

        public GameObject GetRandomResourceSpawnerFromBiome(string biome)
        {
            foreach (ResourceBiomeSpawner resourceBiomeSpawner in _resourcesBiomeSpawners)
            {
                if (resourceBiomeSpawner.biome == biome && resourceBiomeSpawner._resourcesSpawners.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, resourceBiomeSpawner._resourcesSpawners.Count);
                    return resourceBiomeSpawner._resourcesSpawners[randomIndex];
                }
            }

            return null;
        }
    }
}

