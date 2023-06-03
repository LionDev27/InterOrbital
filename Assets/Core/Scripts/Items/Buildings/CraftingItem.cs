using System;
using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using InterOrbital.Player;
using TMPro;
using System.Collections.Generic;

namespace InterOrbital.Item
{
    public class CraftingItem : MonoBehaviour
    {
        protected GameObject _craftUI;
        protected CraftGrid _craftGrid;
        protected CraftCreator _craftCreator;

        [HideInInspector]
        public ItemCraftScriptableObject currentCraftSelected;


        protected virtual void Start()
        {
            _craftGrid = _craftUI.GetComponentInChildren<CraftGrid>();
            _craftCreator = _craftUI.GetComponentInChildren<CraftCreator>();
        }

        public virtual void Craft(ItemCraftScriptableObject itemCraft, int amount) { }

        public void ProccessSelection(){
            _craftCreator.SetCraftingItem(this);
            _craftGrid.currentCraftingItem = this;
            _craftGrid.UpdateFeedback();
            _craftGrid.SelectLast();
        }

        
    }
}
