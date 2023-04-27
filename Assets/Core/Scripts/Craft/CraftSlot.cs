using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Item;

namespace InterOrbital.Item
{
    public class CraftSlot : MonoBehaviour
    {
        private Image _image;
        private ItemCraftScriptableObject _item;

        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }
   
        public void SetItemCraft(ItemCraftScriptableObject item)
        {
            _item = item;
            _image.sprite = item.itemSprite;
        }
    }
}
