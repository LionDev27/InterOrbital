using System.Collections.Generic;
using InterOrbital.Item;
using UnityEditor;
using UnityEngine;

namespace InterOrbital.Recollectables
{
    [System.Serializable]
    public class RecollectableConfig
    {
        public List<ItemScriptableObject> dropItems;
        public List<ItemSelector> itemSelectors;
        public List<int> dropRates;
        public int tier;
    }
}