using System;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;
using static InterOrbital.WorldSystem.GridLogic;
using UnityEngine.Tilemaps;

namespace InterOrbital.Utils
{
    [Serializable]
    public struct ItemRequired
    {
        public ItemScriptableObject item;
        public int amountRequired;
    }

    [Serializable]
    public struct BiomeRuleTile
    {
        public string biome;
        public RuleTile tiles;
    }

    [Serializable]
    public struct TilemapLayer
    {
        public string name;
        public Tilemap tilemap;
        public List<BiomeRuleTile> biomesTiles;
        public FillMode fillMode;
    }

    [Serializable]
    public struct SpriteAnimatedTile
    {
        public Sprite sprite;
        public AnimatedTile animatedTile;
    }
}
