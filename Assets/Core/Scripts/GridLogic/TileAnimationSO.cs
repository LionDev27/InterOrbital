using InterOrbital.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TileAnimationSO", order = 2)]

    public class TileAnimationSO : ScriptableObject
    {
        public Texture2D textureToChangeRuleTile;
        [SerializeField]
        public List<SpriteAnimatedTile> spriteToAnimatedTiles;
    }
}
