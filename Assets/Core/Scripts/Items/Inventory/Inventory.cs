using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using InterOrbital.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class Inventory : PlayerComponents
    {
        private int _totalNumberOfSlots;
        private ItemScriptableObject[] _items;
        private Image[] _itemsSlot;
        private Image[] _itemsFastInventory;
        private int _level;
        private int _sizeInventory;
        private int _numSlotsFastInventory;
        
        //Como el vector items esta ordenado, solo basta con usar un numero del 0 al _numSlotsFastInventory para saber que items usar
        //Me ahorro crear asi otros arrays para la gestion del inventario rapido. Solo me hace falta el de las imagenes a mostrar
        private int _actualItemEquiped; 
        
        public GameObject gridMain;
        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
        public GameObject gridFastInventory;
            
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
            _numSlotsFastInventory = gridFastInventory.transform.childCount;
            InitSlots();
            _items = new ItemScriptableObject[_totalNumberOfSlots];
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = itemVoid;
            }
            _sizeInventory = gridMain.transform.childCount;
        }

        private void InitSlots()
        {
            var sizeMain= gridMain.transform.childCount;
            var sizeLeft= gridLeftPocket.transform.childCount;
            var sizeRight = gridRightPocket.transform.childCount;
           
            _totalNumberOfSlots = sizeMain + sizeLeft  +  sizeRight;
            _itemsSlot = new Image[_totalNumberOfSlots];
            _itemsFastInventory = new Image[_numSlotsFastInventory];
            
            
            RelateSlots(gridMain, 0,sizeMain, _itemsSlot);
            RelateSlots(gridLeftPocket, sizeMain,sizeLeft,_itemsSlot );
            RelateSlots(gridRightPocket, sizeMain + sizeLeft,sizeRight,_itemsSlot);
            RelateSlots(gridFastInventory, 0, 5, _itemsFastInventory);
            
          
        }

        private void RelateSlots(GameObject grid,int startSize, int size, Image[] imagesSlot)
        {
            for (var i = 0; i < size; i++)
            { 
                imagesSlot[i + startSize] = grid.transform.GetChild(i).GetChild(0).GetComponent<Image>();
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
            
        }
        
        private void UpdateLevel()
        {
            if (_level < 3)
            {
                _level++;
                UIManager.Instance.ActivateOrDesactivateUI(_level==2 ? gridLeftPocket : gridRightPocket);
                if (_level == 2)
                    _sizeInventory += gridLeftPocket.transform.childCount;
                else
                {
                    _sizeInventory += gridRightPocket.transform.childCount;
                }
            }
        }
        
        private void UpdateFastInventory()
        {
            for (int i = 0; i < _numSlotsFastInventory; i++)
            {
                _itemsFastInventory[i].sprite = _items[i].itemSprite;
            }
        }


        public void AddItem(ItemScriptableObject item)
        {
            
            for(int i=0; i< _sizeInventory; i++)
            {
                if (_items[i] == itemVoid)
                {
                    _items[i] = item;
                    _itemsSlot[i].sprite = _items[i].itemSprite;

                    if (i < _numSlotsFastInventory)
                    {
                        _itemsFastInventory[i].sprite = _items[i].itemSprite;
                    }
                    break;
                }
            }
        }

        public void SwitchItems(int indexA, int indexB)
        {
            (_itemsSlot[indexB], _itemsSlot[indexA]) = (_itemsSlot[indexA], _itemsSlot[indexB]);
            (_items[indexB], _items[indexA]) = (_items[indexA], _items[indexB]);
            if (indexA < _numSlotsFastInventory || indexB < _numSlotsFastInventory)
            {
                UpdateFastInventory();
            }
        }

        
        
    }
}



