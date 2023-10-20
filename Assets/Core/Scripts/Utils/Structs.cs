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
        public Sprite minimapSprite;
    }

    [Serializable]
    public struct TilemapLayer
    {
        public string name;
        public Tilemap tilemap;
        public Tilemap minimapTilemap;
        public List<BiomeRuleTile> biomesTiles;
        public FillMode fillMode;
    }

    [Serializable]
    public struct AnimatedBiomeTiles
    {
        public string biome;
        public List<AnimatedTile> animatedTile;
    }

    [Serializable]
    public struct ResourceBiomeSpawner
    {
        public string biome;
        public List<GameObject> _resourcesSpawners;
    }

    [Serializable]

    public struct CraftAmountItem
    {
        public ItemCraftScriptableObject item;
        public int amount;

        public CraftAmountItem(ItemCraftScriptableObject item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    [Serializable]
    public struct ConsumableValues
    {
        public ConsumableType consumableType;
        public int amountToRestore;
    }

    [Serializable]
    public struct Region
    {
        public string biome;
        public Vector2Int startPos;
        public int extension;
    }

}
