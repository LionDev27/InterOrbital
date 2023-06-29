using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InterOrbital.Item;
using UnityEngine.EventSystems;
using InterOrbital.Player;
using InterOrbital.Utils;
using TMPro;

namespace InterOrbital.Item
{
    public class CraftSlot : MonoBehaviour, IPointerClickHandler
    {
        private Image _image;
        private ItemCraftScriptableObject _item;
        private CraftingItem _currentCraftingItem;
        private CraftCreator _craftCreator;

        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }
   
        public void SetItemCraft(ItemCraftScriptableObject item)
        {
            _item = item;
            _image.sprite = item.itemSprite;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectCraft();
        }

        public void SetCurrentCraftingItem(CraftingItem item)
        {
            _currentCraftingItem = item;
        }
        
        public void SelectCraft()
        {
            _craftCreator.SetItemCraftCreator(_item);
            if (_currentCraftingItem == null) return;
            _currentCraftingItem.currentCraftSelected = _item;
        }

        public void CheckCanCraft()
        {
            if (!PlayerComponents.Instance.Inventory.CanCraft(_item,1))
            {
                _image.ChangueAlphaColor(0.5f);
            }
            else
            {
                _image.ChangueAlphaColor(1f);
            }
        }

        public void SetCraftCreator(CraftCreator cr)
        {
            _craftCreator = cr;
        }
    }
}
