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
        public List<int> dropRates;
    }
}