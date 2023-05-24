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
        public TileAnimationSO animationTiles;
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
    public struct SpriteAnimatedTile
    {
        public Sprite sprite;
        public AnimatedTile animatedTile;
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

}
