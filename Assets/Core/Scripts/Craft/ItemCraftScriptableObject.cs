using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;

namespace InterOrbital.Item
{
    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemCraftScriptableObject", order = 1)]
    public class ItemCraftScriptableObject : ItemScriptableObject
    {
        public List<ItemRequired> itemsRequired;
        public TypeCraft craftType;
    }
}


