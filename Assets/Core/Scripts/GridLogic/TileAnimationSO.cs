using InterOrbital.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TileAnimationSO", order = 2)]

    public class TileAnimationSO : ScriptableObject
    {
        [SerializeField]
        public List<AnimatedBiomeTiles> animatedBiomeTiles;

        public AnimatedTile GetRandomTileFromBiome(string biome)
        {
            foreach (AnimatedBiomeTiles biomeAnimatedTiles in animatedBiomeTiles)
            {
                if (biomeAnimatedTiles.biome == biome && biomeAnimatedTiles.animatedTile.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, biomeAnimatedTiles.animatedTile.Count);
                    return biomeAnimatedTiles.animatedTile[randomIndex];
                }
            }

            return null;
        }
    }
}
