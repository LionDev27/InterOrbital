using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterOrbital.Player.Inventory
{
    public class Inventory : PlayerComponents
    {
        private int _totalNumberOfSlots;
        public ItemScriptableObject[] _items;
        private Image[] _itemsSlot;
        private int _level;
        
        public GameObject gridMain;
        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
        
        public GameObject bagUI;
        public ItemScriptableObject itemVoid;
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnOpenInventory += UpdateInventory;
            
        }

        private void Start()
        {
            _level = 1;
            InitSlots();
            _items = new ItemScriptableObject[_totalNumberOfSlots];
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = itemVoid;
            }
        }

        private void InitSlots()
        {
            var sizeMain= gridMain.transform.childCount;
            var sizeLeft= gridLeftPocket.transform.childCount;
            var sizeRight = gridRightPocket.transform.childCount;
            
            _totalNumberOfSlots = sizeMain + sizeLeft  +  sizeRight;
            _itemsSlot = new Image[_totalNumberOfSlots];
            RelateSlots(gridMain, 0,sizeMain);
            RelateSlots(gridLeftPocket, sizeMain,sizeLeft );
            RelateSlots(gridRightPocket, sizeMain + sizeLeft,sizeRight);
          
        }

        private void RelateSlots(GameObject grid,int startSize, int size)
        {
            for (var i = 0; i < size; i++)
            { 
                _itemsSlot[i + startSize] = grid.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            }
        }

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateLevel();
            }
        }

        private void UpdateInventory()
        {
            UIManager.Instance.ActivateOrDesactivateUI(bagUI);
            for (int i = 0; i < _items.Length; i++)
            {
                _itemsSlot[i].sprite = _items[i].itemSprite;
            }
        }

        private void UpdateLevel()
        {
            if (_level < 3)
            {
                _level++;
                UIManager.Instance.ActivateOrDesactivateUI(_level==2 ? gridLeftPocket : gridRightPocket); 
            }
        }
        
    }
}



