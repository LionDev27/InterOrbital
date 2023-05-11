using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.UI;
using InterOrbital.Item;

namespace InterOrbital.Player
{      
    public class PlayerCraft : PlayerComponents
    {
        private ItemCraftScriptableObject _actualTableCraftSelected;
        public GameObject craftUI;
        public CraftGrid craftGrid;
        

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            UpdateCraft();
        }

        private void UpdateCraft()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                InputHandler.ChangeActionMap();
                UIManager.Instance.ActivateOrDesactivateUI(craftUI);
                if (craftUI.activeSelf)
                {
                    craftGrid.UpdateFeedback();
                    craftGrid.SelectLast();
                }
            }
        }  
        
        public void SetActualTableCraftSelected(ItemCraftScriptableObject item)
        {
            _actualTableCraftSelected = item;
        }
        public ItemCraftScriptableObject GetActualTableCraftSelectd()
        {
            return _actualTableCraftSelected;
        }
    }
}
