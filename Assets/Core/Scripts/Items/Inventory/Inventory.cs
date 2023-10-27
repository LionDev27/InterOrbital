
using InterOrbital.Item;
using InterOrbital.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using InterOrbital.Utils;
using InterOrbital.Mission;

namespace InterOrbital.Player
{
    public class Inventory : MonoBehaviour
    {
        private MissionCreator _missionCreator;
        protected int _totalNumberOfSlots;
        protected ItemObject [] _items;
        protected Image[] _itemsSlotBackGround;
        protected Image[] _itemsSlot;
        protected TextMeshProUGUI[] _textAmount;
        protected int _sizeInventory;
        protected Sprite _backgroundDefaultImage;
        
        public GameObject gridMain;
        public ItemScriptableObject itemVoid;
        

        public GameObject dropItemPrefab;
        public float dropForce;

        public bool isHide;

        public virtual void Awake()
        {
            _missionCreator = FindObjectOfType<MissionCreator>();
        }

        
        protected void StartInventory()
        {
            _items = new ItemObject[_totalNumberOfSlots];
            for (int i = 0; i < _items.Length; i++)
            {
                GameObject obj = new GameObject();
                ItemObject item = obj.AddComponent<ItemObject>();
                Destroy(obj);
                _items[i] = item;
                _items[i].itemSo = itemVoid;
                _itemsSlot[i].sprite = _items[i].itemSo.itemSprite;
            }
            _sizeInventory = gridMain.transform.childCount;
            isHide = true;
        }


        protected void RelateSlots(GameObject grid,int startSize, int size, Image[] imagesSlot, bool relateAmounts)
        {
            for (var i = 0; i < size; i++)
            { 
                imagesSlot[i + startSize] = grid.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                _itemsSlotBackGround[i + startSize] = grid.transform.GetChild(i).GetComponent<Image>();
                if (relateAmounts)
                {
                    grid.transform.GetChild(i).GetChild(0).GetComponent<DraggableItem>().inventoryIndex = i + startSize;
                    _textAmount[i+ startSize] = grid.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
            }
        }

        protected void UpdateActionUI()
        {
            int _actualItemEquiped = PlayerComponents.Instance.InputHandler.InventoryPositionValue;
            UIManager.Instance.ChangeActionUI(_items[_actualItemEquiped - 1].itemSo);
        }

        private void SetAmount(int index, int num)
        {
            _items[index].amount = num;
            _textAmount[index].text = _items[index].amount.ToString();
        }

        public bool IsInventoryFull()
        {
            for(int i=0; i<_sizeInventory; i++)
            {
                if (_items[i].itemSo.isStackable && _items[i].amount == _items[i].itemSo.maxAmount)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public virtual void InitSlots()
        {
            var sizeMain= gridMain.transform.childCount;
            
            _totalNumberOfSlots = sizeMain;
            _itemsSlot = new Image[_totalNumberOfSlots];
            _itemsSlotBackGround = new Image[_totalNumberOfSlots];
            _textAmount = new TextMeshProUGUI[_totalNumberOfSlots];

            RelateSlots(gridMain, 0,sizeMain, _itemsSlot, true);
            _backgroundDefaultImage = _itemsSlotBackGround[1].sprite;          
        }

        public void AddOneItemSO(ItemScriptableObject item)
        {
            GameObject obj = Instantiate(dropItemPrefab);
            ItemObject itemAux= obj.AddComponent<ItemObject>();
            itemAux.ObtainComponents();
            Destroy(obj);
            itemAux.SetItem(item);
            itemAux.amount = 1;
            PlayerComponents.Instance.Inventory.AddItem(itemAux);
        }

        public void AddItem(ItemObject item)
        {
            var rest = 0;

            for(int i=0; i < _sizeInventory; i++)
            {
                if (_items[i].itemSo == item.itemSo && _items[i].itemSo.isStackable && _items[i].amount <= _items[i].itemSo.maxAmount)
                {
                    AudioManager.Instance.PlaySFX("ObtainItem");
                    int sum = _items[i].amount + item.amount;
                    if(sum <= _items[i].itemSo.maxAmount)
                    {
                        SetAmount(i, sum);     
                        if(item.itemSo.type == ItemType.Bullet)
                        {
                            BulletSelector.Instance.UpdateBulletSelectorUI();
                        }
                        _missionCreator.UpdateMission(item.amount, item.itemSo.itemName);
                        UpdateActionUI();

                        return;
                    }
                    else
                    {
                        rest = sum - _items[i].itemSo.maxAmount;
                        SetAmount(i, _items[i].itemSo.maxAmount);
                        if (item.itemSo.type == ItemType.Bullet)
                        {
                            BulletSelector.Instance.UpdateBulletSelectorUI();
                        }
                        _missionCreator.UpdateMission(item.amount - rest, item.itemSo.itemName);
                        item.amount = rest;
                    }
                }
            }

            for (int i=0; i< _sizeInventory; i++)
            {
                if (_items[i].itemSo == itemVoid && !(item.itemSo.type != ItemType.Bullet && i >= PlayerComponents.Instance.Inventory.GetStartIndexBulletSlot() && i <= PlayerComponents.Instance.Inventory.GetStartIndexBulletSlot() + 3))
                {
                    _items[i] = item;
                    SetAmount(i, item.amount);
                    AudioManager.Instance.PlaySFX("ObtainItem");
                    if (item.itemSo.type == ItemType.Bullet)
                    {
                        BulletSelector.Instance.UpdateBulletSelectorUI();
                    }
                    _itemsSlot[i].sprite = null;
                    _itemsSlot[i].sprite = _items[i].itemSo.itemSprite;
                    UpdateActionUI();
                    _missionCreator.UpdateMission(item.amount, item.itemSo.itemName);
                    return;
                }
            }

            //Si llegamos aqui, es porque falta espacio para meter el resto de un item.
            DropItem(PlayerComponents.Instance.PlayerAttack.attackPoint.position, transform.position,- 1, item.itemSo);
           
        }

        public void SwitchItems(int indexA, int indexB)
        {
            //Debug.Log("Intercambiamos inventario" + indexA + "por inventario" + indexB);
            (_textAmount[indexB], _textAmount[indexA]) = (_textAmount[indexA], _textAmount[indexB]);
            (_itemsSlot[indexB], _itemsSlot[indexA]) = (_itemsSlot[indexA], _itemsSlot[indexB]);
            (_items[indexB], _items[indexA]) = (_items[indexA], _items[indexB]);
            
        }


        public void SwitchItemWithChest(int indexInventory, int indexChest)
        {
            //Debug.Log("Intercambiamos cofre" + indexChest + "por inventario" + indexInventory);
            (UIManager.Instance.chestInventory._textAmount[indexChest], PlayerComponents.Instance.Inventory._textAmount[indexInventory]) = (PlayerComponents.Instance.Inventory._textAmount[indexInventory], UIManager.Instance.chestInventory._textAmount[indexChest]);
            (UIManager.Instance.chestInventory._itemsSlot[indexChest], PlayerComponents.Instance.Inventory._itemsSlot[indexInventory]) = (PlayerComponents.Instance.Inventory._itemsSlot[indexInventory], UIManager.Instance.chestInventory._itemsSlot[indexChest]);
            (UIManager.Instance.chestInventory._items[indexChest], PlayerComponents.Instance.Inventory._items[indexInventory]) = (PlayerComponents.Instance.Inventory._items[indexInventory], UIManager.Instance.chestInventory._items[indexChest]);
        }

        public void DropItem(Vector3 spawnPosition, Vector3 droperPosition, int index=-1, ItemScriptableObject item=null)
        {   
            GameObject p = Instantiate(dropItemPrefab, spawnPosition, Quaternion.identity);
            ItemObject auxItem = p.GetComponent<ItemObject>();
            auxItem.ObtainComponents();
            if (index >= 0)
            {
                auxItem.SetItem(_items[index].itemSo);
                auxItem.amount = _items[index].amount;
            }
            else if(item != null)
            {
                auxItem.SetItem(item);
                
            }
            
            Vector3 direction = spawnPosition - droperPosition;
            auxItem.DropItem((direction.normalized * dropForce) + spawnPosition);
          
            if(index >= 0)
            {
                _items[index].itemSo = itemVoid;
                _items[index].amount = 0;
                _itemsSlot[index].sprite = itemVoid.itemSprite;
                _textAmount[index].text = "";
            }

            BulletSelector.Instance.UpdateBulletSelectorUI();
        }

        public ItemObject GetItemObjectByIndex(int index)
        {
            return _items[index];
        }
        

    }
}



