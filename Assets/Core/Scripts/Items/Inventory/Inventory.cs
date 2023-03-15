using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using InterOrbital.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class Inventory : PlayerComponents
    {
        private int _totalNumberOfSlots;
        public ItemScriptableObject[] _items;
        private Image[] _itemsSlot;
        private int _level;
        private int sizeInventory;
        
        public GameObject gridMain;
        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
        
        public GameObject bagUI;
        public ItemScriptableObject itemVoid;
        public ItemScriptableObject itemTest;

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
            sizeInventory = gridMain.transform.childCount;
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                AddItem(itemTest);
            }
        }

        private void UpdateInventory()
        {
            UIManager.Instance.ActivateOrDesactivateUI(bagUI);
            UpdateImages();
        }

        private void UpdateImages()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                Debug.Log(_items[i].name);
                _itemsSlot[i].sprite = _items[i].itemSprite;
            }
        }

        private void UpdateLevel()
        {
            if (_level < 3)
            {
                _level++;
                UIManager.Instance.ActivateOrDesactivateUI(_level==2 ? gridLeftPocket : gridRightPocket);
                if (_level == 2)
                    sizeInventory += gridLeftPocket.transform.childCount;
                else
                {
                    sizeInventory += gridRightPocket.transform.childCount;
                }
            }
        }


        public void AddItem(ItemScriptableObject item)
        {
            
            for(int i=0; i< sizeInventory; i++)
            {
                if (_items[i] == itemVoid)
                {
                    _items[i] = item;
                    break;
                }
            }

            if(bagUI.activeSelf)
            {
                UpdateImages();
            }
        }

        public void SwitchItems(int indexA, int indexB)
        {
           ItemScriptableObject auxItem= _items[indexB];
           _items[indexB] = _items[indexA];
           _items[indexA] = auxItem;
           //Debug.Log("Intercambiamos "+indexA+", "+indexB);
        }
        
    }
}



