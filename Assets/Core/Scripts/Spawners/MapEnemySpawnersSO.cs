using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.Spawner
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapSpawnersSO", order = 3)]

    public class MapEnemySpawnersSO : ScriptableObject
    {
        public int spawnersAmount;
        public float distanceBetweenSpawners;
        public List<GameObject> _enemySpawners;
    }
}

