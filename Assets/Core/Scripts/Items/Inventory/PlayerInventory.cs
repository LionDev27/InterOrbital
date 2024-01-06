using UnityEngine;
using InterOrbital.UI;
using UnityEngine.UI;
using TMPro;
using InterOrbital.Item;
using InterOrbital.Utils;
using DG.Tweening;
using System.Collections.Generic;


namespace InterOrbital.Player
{
    public class PlayerInventory : Inventory
    {
        private int _level;
        private int _actualItemEquiped;
        private int _slotClicked1, _slotClicked2; //Para inventario mochila
        private int _slotChestClicked1, _slotChestClicked2; //Para inventario cofre
        [SerializeField] private Sprite _backgroundSelectedImage;
        [SerializeField] private Sprite _backgroundClickedImage;
        [SerializeField] private Sprite _backgroundDefaultChestImage;

        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
        public GameObject gridBullets;
        public ItemScriptableObject itemTest;
        public ItemScriptableObject itemTest2;
        public ItemScriptableObject itemTest3;

        void Start()
        {
            _level = 1;
            _actualItemEquiped = 2;
            InitSlots();
            StartInventory();
            _sizeInventory += gridBullets.transform.childCount;
            _slotClicked1 = -1;
            _slotClicked2 = -1;
            _slotChestClicked1 = -1;
            _slotChestClicked2 = -1;
        }
        

        void Update()
        {
            Vector3 direction = PlayerComponents.Instance.PlayerAttack.attackPoint.position - transform.position;
            Vector3 pointToDraw = (direction.normalized * dropForce) +
                                  PlayerComponents.Instance.PlayerAttack.attackPoint.position;
            Debug.DrawRay(transform.position, pointToDraw, Color.yellow);

            ScrollFastInventory();
        }

        public new void ResetInventory()
        {
            base.ResetInventory();
        }

        public void UpdateLevel()
        {
            if (_level < 3)
            {
                _level++;
                var grid = _level == 2 ? gridLeftPocket : gridRightPocket;
                grid.SetActive(true);
                if (_level == 2)
                    _sizeInventory += gridLeftPocket.transform.childCount;
                else
                {
                    _sizeInventory += gridRightPocket.transform.childCount;
                }
            }
        }

        private void ScrollFastInventory()
        {
            if (isHide && PlayerComponents.Instance.InputHandler.InventoryPositionValue != _actualItemEquiped)
            {
                if (BuildGrid.Instance.IsBuilding())
                    BuildGrid.Instance.DesactivateBuildMode();

                _itemsSlotBackGround[_actualItemEquiped - 1].sprite = _backgroundDefaultImage;
                _actualItemEquiped = PlayerComponents.Instance.InputHandler.InventoryPositionValue;
                _itemsSlotBackGround[_actualItemEquiped - 1].sprite = _backgroundSelectedImage;
                UpdateActionUI();
            }
        }


        public void UseItem()
        {
            int index = PlayerComponents.Instance.InputHandler.InventoryPositionValue - 1;
            var itemData = _items[index].itemSo;

            switch (itemData.type)
            {
                case ItemType.Build:
                    if (BuildGrid.Instance.IsBuilding())
                        BuildGrid.Instance.DesactivateBuildMode();
                    else
                        BuildGrid.Instance.ActivateBuildMode(_items[index].itemSo);
                    break;
                case ItemType.Consumable:
                    if (itemData.consumableValues.consumableType == ConsumableType.Elytrum)
                    {
                        PlayerComponents.Instance.GetComponent<PlayerEnergy>()
                            .RestoreEnergy(itemData.consumableValues.amountToRestore);
                        SubstractUsedItem();
                    }

                    if (itemData.consumableValues.consumableType == ConsumableType.Health)
                    {
                        PlayerComponents.Instance.GetComponent<PlayerDamageable>()
                            .RestoreHealth(itemData.consumableValues.amountToRestore);
                        SubstractUsedItem();
                    }
                    
                    if (itemData.consumableValues.consumableType == ConsumableType.Recollector)
                    {
                        RecollectorUpgrades.OnUpgradeRecollector?.Invoke(itemData.consumableValues.amountToRestore);
                        SubstractUsedItem();
                    }

                    break;
                case ItemType.Bullet:

                    int bulletSelectorIndex = PlayerComponents.Instance.Inventory.GetStartIndexBulletSlot() + BulletSelector.Instance.SelectedBulletIndex;
                    GameObject dropped = _itemsSlot[index].gameObject;
                    GameObject goal = _itemsSlot[bulletSelectorIndex].transform.parent.gameObject;
                    PlayerComponents.Instance.Inventory.ChangeSlots(dropped, goal, true);

                    _itemsSlot[index].sprite = _items[index].itemSo.itemSprite;
                    _textAmount[index].text = "";
                    _itemsSlot[bulletSelectorIndex].sprite = _items[bulletSelectorIndex].itemSo.itemSprite;
                    _textAmount[bulletSelectorIndex].text =  _items[bulletSelectorIndex].amount.ToString();

                    break;
                case ItemType.Upgrade:
                    if (itemData.upgradeType == UpgradeType.Elytrum)
                    {
                        PlayerComponents.Instance.GetComponent<PlayerEnergy>().UpgradeEnergy(20);
                        SubstractUsedItem();
                    }

                    if (itemData.upgradeType == UpgradeType.Health)
                    {
                        PlayerComponents.Instance.GetComponent<PlayerDamageable>().UpgradeHealth(8);
                        SubstractUsedItem();
                    }

                    break;
                case ItemType.None:
                    //No se puede usar item
                    break;
            }
        }

        public override void InitSlots()
        {
            var sizeMain = gridMain.transform.childCount;
            var sizeLeft = gridLeftPocket.transform.childCount;
            var sizeRight = gridRightPocket.transform.childCount;
            var sizeBullets = gridBullets.transform.childCount;

            _totalNumberOfSlots = sizeMain + sizeLeft + sizeRight + sizeBullets;
            _itemsSlot = new Image[_totalNumberOfSlots];
            _itemsSlotBackGround = new Image[_totalNumberOfSlots];
            _textAmount = new TextMeshProUGUI[_totalNumberOfSlots];

            RelateSlots(gridMain, 0, sizeMain, _itemsSlot, true);
            RelateSlots(gridBullets, sizeMain, sizeBullets, _itemsSlot, true);
            RelateSlots(gridLeftPocket, sizeMain + sizeBullets, sizeLeft, _itemsSlot, true);
            RelateSlots(gridRightPocket, sizeMain + sizeBullets + sizeLeft, sizeRight, _itemsSlot, true);

            _backgroundDefaultImage = _itemsSlotBackGround[1].sprite;
        }

        public bool CanUseMore()
        {
            return _items[PlayerComponents.Instance.InputHandler.InventoryPositionValue - 1].amount >= 1;
        }

        public int GetTotalItemAmount(ItemScriptableObject item)
        {
            int amountOnInventory = 0;

            for (int i = 0; i < _items.Length; i++)
            {
                if (item == _items[i].itemSo)
                {
                    amountOnInventory += _items[i].amount;
                }
            }

            return amountOnInventory;
        }

        public int GetItemAmountByIndex(int index)
        {
            return _items[index].amount;
        }


        public bool CanCraft(ItemCraftScriptableObject itemCraft, int amount)
        {
            for (int i = 0; i < itemCraft.itemsRequired.Count; i++)
            {
                var actualAmountItem = GetTotalItemAmount(itemCraft.itemsRequired[i].item);
                if (actualAmountItem < itemCraft.itemsRequired[i].amountRequired * amount)
                    return false;
            }

            return true;
        }


        public void SubstractItems(ItemScriptableObject item, int amount)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (item == _items[i].itemSo)
                {
                    var auxRest = _items[i].amount - amount;

                    if (auxRest <= 0)
                    {
                        _items[i].itemSo = itemVoid;
                        _itemsSlot[i].sprite = itemVoid.itemSprite;
                        _items[i].amount = 0;
                        _textAmount[i].text = "";
                        amount = auxRest * -1;
                    }
                    else
                    {
                        _items[i].amount -= amount;
                        _textAmount[i].text = _items[i].amount.ToString();
                        break;
                    }
                }
            }

            UpdateActionUI();
        }

        public void SubstractBulletInInventory(int indexInBulletSelector)
        {
            int index = GetStartIndexBulletSlot() + indexInBulletSelector;
            _items[index].amount--;
            if (_items[index].amount <= 0)
            {
                _items[index].itemSo = itemVoid;
                _itemsSlot[index].sprite = itemVoid.itemSprite;
                _items[index].amount = 0;
                _textAmount[index].text = "";
            }
            else
            {
                _textAmount[index].text = _items[index].amount.ToString();
            }
        }

        public void SubstractUsedItem()
        {
            int i = PlayerComponents.Instance.InputHandler.InventoryPositionValue - 1;
            var auxRest = _items[i].amount - 1;
            if (auxRest <= 0)
            {
                _items[i].itemSo = itemVoid;
                _itemsSlot[i].sprite = itemVoid.itemSprite;
                _items[i].amount = 0;
                _textAmount[i].text = "";
            }
            else
            {
                _items[i].amount -= 1;
                _textAmount[i].text = _items[i].amount.ToString();
            }

            UpdateActionUI();
        }

        public int GetStartIndexBulletSlot()
        {
            return gridMain.transform.childCount;
        }


        public void ClickSwapInventory(int index, bool isChest)
        {
            if (!isChest)
            {
                if (_slotClicked1 == -1 && _slotClicked1 != index && _slotChestClicked1 == -1)
                {
                    _slotClicked1 = index;
                    if (_itemsSlotBackGround[_slotClicked1].sprite == _backgroundDefaultImage || _itemsSlotBackGround[_slotClicked1].sprite == _backgroundSelectedImage)
                    {
                        _itemsSlotBackGround[_slotClicked1].sprite = _backgroundClickedImage;
                    }
                    else if (_itemsSlotBackGround[_slotClicked1].color.a == 0)
                    {
                        _itemsSlotBackGround[_slotClicked1].ChangueAlphaColor(1);
                    }
                }
                else if (_slotClicked1 == index || _slotClicked1 != -1 && (_items[_slotClicked1].itemSo == itemVoid && _items[index].itemSo == itemVoid) || _slotChestClicked1 != -1 && (UIManager.Instance.chestInventory._items[_slotChestClicked1].itemSo == itemVoid && _items[index].itemSo == itemVoid))
                {
                    StopClickSelection();
                }
                else if(_slotClicked1 != -1 || _slotClicked1 == -1 && _slotChestClicked1 != -1)
                {
                    _slotClicked2 = index;
                    GameObject dropped = null;
                    if (_slotChestClicked1 != -1)
                    {
                        dropped = UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked1].gameObject;
                    }
                    else
                    {
                        dropped = _itemsSlot[_slotClicked1].gameObject;
                    }

                    GameObject goal = _itemsSlot[_slotClicked2].transform.parent.gameObject;
                    PlayerComponents.Instance.Inventory.ChangeSlots(dropped, goal, true);

                    if(_slotChestClicked1 != -1)
                    {
                        UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked1].sprite = UIManager.Instance.chestInventory._items[_slotChestClicked1].itemSo.itemSprite;
                        UIManager.Instance.chestInventory._textAmount[_slotChestClicked1].text = UIManager.Instance.chestInventory._items[_slotChestClicked1].amount == 0 ? "" : UIManager.Instance.chestInventory._items[_slotChestClicked1].amount.ToString();
                    }
                    else
                    {
                        _itemsSlot[_slotClicked1].sprite = _items[_slotClicked1].itemSo.itemSprite;
                        _textAmount[_slotClicked1].text = _items[_slotClicked1].amount == 0 ? "" : _items[_slotClicked1].amount.ToString();
                    }
                    _itemsSlot[_slotClicked2].sprite = _items[_slotClicked2].itemSo.itemSprite;
                    _textAmount[_slotClicked2].text = _items[_slotClicked2].amount == 0 ? "" : _items[_slotClicked2].amount.ToString();
                    StopClickSelection();
                }
            }
            else
            {
                if(_slotClicked1 == -1 && _slotChestClicked1 == -1 && _slotChestClicked1 != index)
                {
                    _slotChestClicked1 = index;
                    UIManager.Instance.chestInventory._itemsSlotBackGround[_slotChestClicked1].sprite = _backgroundClickedImage;
                }
                else if (_slotChestClicked1 != -1 &&  (_slotChestClicked1 == index || (UIManager.Instance.chestInventory._items[_slotChestClicked1].itemSo == itemVoid && UIManager.Instance.chestInventory._items[index].itemSo == itemVoid)))
                {
                    StopClickSelection();
                }
                else if (_slotClicked1 != -1 && (_items[_slotClicked1].itemSo == itemVoid && UIManager.Instance.chestInventory._items[index].itemSo == itemVoid))
                {
                    StopClickSelection();
                }
                else
                {
                    _slotChestClicked2 = index;
                    GameObject dropped = null;
                    GameObject goal = null;

                    if (_slotChestClicked1 != -1)
                    {
                        dropped= UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked1].gameObject;
                    }
                    else if(_slotClicked1 != -1)
                    {
                        dropped = _itemsSlot[_slotClicked1].gameObject;
                    }

                    if(_slotChestClicked2 != -1)
                    {
                        goal = UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked2].transform.parent.gameObject;
                    }
                    else if (_slotClicked2 != -1)
                    {
                        goal = _itemsSlot[_slotClicked2].transform.parent.gameObject;
                    }

                    PlayerComponents.Instance.Inventory.ChangeSlots(dropped, goal, true);

                    if (_slotChestClicked1 != -1)
                    {
                        UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked1].sprite = UIManager.Instance.chestInventory._items[_slotChestClicked1].itemSo.itemSprite;
                        UIManager.Instance.chestInventory._textAmount[_slotChestClicked1].text = UIManager.Instance.chestInventory._items[_slotChestClicked1].amount == 0 ? "" : UIManager.Instance.chestInventory._items[_slotChestClicked1].amount.ToString();
                    }
                    else if (_slotClicked1 != -1)
                    {
                        _itemsSlot[_slotClicked1].sprite = _items[_slotClicked1].itemSo.itemSprite;
                        _textAmount[_slotClicked1].text = _items[_slotClicked1].amount == 0 ? "" : _items[_slotClicked1].amount.ToString();
                    }

                    if (_slotChestClicked2 != -1)
                    {
                        UIManager.Instance.chestInventory._itemsSlot[_slotChestClicked2].sprite = UIManager.Instance.chestInventory._items[_slotChestClicked2].itemSo.itemSprite;
                        UIManager.Instance.chestInventory._textAmount[_slotChestClicked2].text = UIManager.Instance.chestInventory._items[_slotChestClicked2].amount == 0 ? "" : UIManager.Instance.chestInventory._items[_slotChestClicked2].amount.ToString();
                    }
                    else if (_slotClicked2 != -1)
                    {
                        _itemsSlot[_slotClicked2].sprite = _items[_slotClicked2].itemSo.itemSprite;
                        _textAmount[_slotClicked2].text = _items[_slotClicked2].amount == 0 ? "" : _items[_slotClicked2].amount.ToString();
                    }
                    StopClickSelection();
                }
            }
        }

        public void StopClickSelection()
        {
            if(_slotClicked1 != -1)
            {
                if (_itemsSlotBackGround[_slotClicked1].sprite == _backgroundClickedImage)
                {
                    _itemsSlotBackGround[_slotClicked1].sprite = _slotClicked1 == _actualItemEquiped - 1 ? _backgroundSelectedImage : _backgroundDefaultImage;
                }
                else if(_itemsSlotBackGround[_slotClicked1].color.a == 1)
                {
                    _itemsSlotBackGround[_slotClicked1].ChangueAlphaColor(0);
                }
            }
            else
            {
                UIManager.Instance.chestInventory._itemsSlotBackGround[_slotChestClicked1].sprite = _backgroundDefaultChestImage;
            }
            
            _slotClicked1 = -1;
            _slotClicked2 = -1;
            _slotChestClicked1 = -1;
            _slotChestClicked2 = -1;
        }

        public void FillDeathBag(DeathBag deathBag)
        {
            foreach (var item in _items)
            {
                if (item.itemSo != itemVoid)
                {
                    Items itemFromPlayer = new Items();
                    itemFromPlayer.item = item.itemSo;
                    itemFromPlayer.amountRequired = item.amount;
                    deathBag.playerItems.Add(itemFromPlayer);
                }
            }
        }

    }
}