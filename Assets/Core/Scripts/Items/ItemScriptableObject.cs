using UnityEngine;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
    public class ItemScriptableObject : ScriptableObject
    {
        public int id;
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool isStackable;
        public int maxAmount;
    }
}
