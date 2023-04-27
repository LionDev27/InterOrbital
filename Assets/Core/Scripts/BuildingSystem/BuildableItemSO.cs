using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.BuildingSystem
{
    [CreateAssetMenu(fileName = "Data",menuName ="ScriptableObjects/New Building Item" , order = 2)]
    public class BuildableItemSO : ScriptableObject
    {
        [field:SerializeField] 
        public string Name { get; private set; }
        [field:SerializeField]
        public TileBase Tile { get; private set; }
    }
}
