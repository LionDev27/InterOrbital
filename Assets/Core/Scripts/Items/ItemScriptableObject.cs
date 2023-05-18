using UnityEngine;
using InterOrbital.Utils;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite itemSprite;
        public int id;
        public string itemName;
        public string itemDescription;
        public TypeCraft type;
        public bool isStackable;
        public int maxAmount;
    }
}
