using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Player;
using InterOrbital.UI;
using UnityEngine.UI;
using TMPro;
using InterOrbital.Item;
using InterOrbital.Utils;

namespace InterOrbital.Player
{
    public class PlayerInventory : Inventory
    {
        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
        private int _level;
        private int _actualItemEquiped;
        [SerializeField] private Sprite _backgroundSelectedImage;

        public ItemScriptableObject itemTest;
        public ItemScriptableObject itemTest2;
        public ItemScriptableObject itemTest3;


        // Start is called before the first frame update
        void Start()
        {
            _level = 1;
            _actualItemEquiped = 2;
            InitSlots();
            StartInventory();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateLevel();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                DropItem(PlayerComponents.Instance.PlayerAttack.attackPoint.position, transform.position, -1, itemTest);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                DropItem(PlayerComponents.Instance.PlayerAttack.attackPoint.position, transform.position, -1, itemTest2);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                DropItem(PlayerComponents.Instance.PlayerAttack.attackPoint.position, transform.position, -1, itemTest3);
            }

            Vector3 direction = PlayerComponents.Instance.PlayerAttack.attackPoint.position - transform.position;
            Vector3 pointToDraw = (direction.normalized * dropForce) + PlayerComponents.Instance.PlayerAttack.attackPoint.position;
            Debug.DrawRay(transform.position, pointToDraw, Color.yellow);

            ScrollFastInventory();

        }

        private void UpdateLevel()
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
            }
        }

        public void UseItem()
        {
            int index = PlayerComponents.Instance.InputHandler.InventoryPositionValue - 1;

            switch (_items[index].itemSo.type)
            {
                case ItemType.Build:
                    if (BuildGrid.Instance.IsBuilding())
                        BuildGrid.Instance.DesactivateBuildMode();
                    else
                        BuildGrid.Instance.ActivateBuildMode(_items[index].itemSo);
                    break;
                case ItemType.Consumable:
                    //_items[actualNumInventoryIndex] consumir item
                    break;
                case ItemType.Bullet:
                    //TODO equipar al menu de balas si hay huecos libres
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

            _totalNumberOfSlots = sizeMain + sizeLeft + sizeRight;
            _itemsSlot = new Image[_totalNumberOfSlots];
            _itemsSlotBackGround = new Image[_totalNumberOfSlots];
            _textAmount = new TextMeshProUGUI[_totalNumberOfSlots];

            RelateSlots(gridMain, 0, sizeMain, _itemsSlot, true);
            RelateSlots(gridLeftPocket, sizeMain, sizeLeft, _itemsSlot, true);
            RelateSlots(gridRightPocket, sizeMain + sizeLeft, sizeRight, _itemsSlot, true);

            _backgroundDefaultImage = _itemsSlotBackGround[1].sprite;
        }

        public bool CanUseMore()
        {
            return _items[PlayerComponents.Instance.InputHandler.InventoryPositionValue - 1].amount >= 1;
        }

        public int GetItemAmount(ItemScriptableObject item)
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

        public bool CanCraft(ItemCraftScriptableObject itemCraft, int amount)
        {
            for (int i = 0; i < itemCraft.itemsRequired.Count; i++)
            {
                var actualAmountItem = GetItemAmount(itemCraft.itemsRequired[i].item);
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

        }
    }
}
  
