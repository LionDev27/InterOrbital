
using InterOrbital.Item;
using InterOrbital.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InterOrbital.Player
{
    public class Inventory : PlayerComponents
    {
    
        private int _totalNumberOfSlots;
        private ItemObject [] _items;
        private Image[] _itemsSlot;
        private TextMeshProUGUI[] _textAmount;
        private int _level;
        private int _sizeInventory;
        private int _numSlotsFastInventory;
        
        //Como el vector items esta ordenado, solo basta con usar un numero del 0 al _numSlotsFastInventory para saber que items usar
        //Me ahorro crear asi otros arrays para la gestion del inventario rapido. Solo me hace falta el de las imagenes a mostrar
        private int _actualItemEquiped; 
        
        public GameObject gridMain;
        public GameObject gridLeftPocket;
        public GameObject gridRightPocket;
    
        public ItemScriptableObject itemVoid;
        public ItemScriptableObject itemTest;
        public ItemScriptableObject itemTest2;

        public GameObject dropItemPrefab;
        public float dropForce;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnOpenInventory += UpdateInventory;
        }

        private void Start()
        {
            _level = 1;
            InitSlots();
            _items = new ItemObject[_totalNumberOfSlots];
            for (int i = 0; i < _items.Length; i++)
            {
                GameObject obj = new GameObject();
                ItemObject item = obj.AddComponent<ItemObject>();
                Destroy(obj);
                _items[i] = item;
                
                _items[i].itemSo = itemVoid;
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
            _textAmount = new TextMeshProUGUI[_totalNumberOfSlots];

            RelateSlots(gridMain, 0,sizeMain, _itemsSlot, true);
            RelateSlots(gridLeftPocket, sizeMain,sizeLeft,_itemsSlot, true );
            RelateSlots(gridRightPocket, sizeMain + sizeLeft,sizeRight,_itemsSlot, true);
            
          
        }

        private void RelateSlots(GameObject grid,int startSize, int size, Image[] imagesSlot, bool relateAmounts)
        {
            for (var i = 0; i < size; i++)
            { 
                imagesSlot[i + startSize] = grid.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                if (relateAmounts)
                {
                    grid.transform.GetChild(i).GetChild(0).GetComponent<DraggableItem>().inventoryIndex = i + startSize;
                    _textAmount[i+ startSize] = grid.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
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
                DropItem(-1,itemTest);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                DropItem(-1, itemTest2);
            }

            Vector3 direction = PlayerAttack.attackPoint.position - transform.position;
            Vector3 pointToDraw = (direction.normalized * dropForce) + PlayerAttack.attackPoint.position;
            Debug.DrawRay(transform.position,pointToDraw, Color.yellow);
        }
        

        private void UpdateInventory()
        {
            UIManager.Instance.OpenInventory();
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

        public void AddItem(ItemObject item)
        {
            var rest = 0;

            for(int i=0; i < _sizeInventory; i++)
            {
                 if (_items[i].itemSo == item.itemSo && _items[i].itemSo.isStackable && _items[i].amount <= _items[i].itemSo.maxAmount)
                 {
                   
                    int sum = _items[i].amount + item.amount;
                    if(sum <= _items[i].itemSo.maxAmount)
                    {
                        SetAmount(i, sum);       
                        return;
                    }
                    else
                    {
                        rest = sum - _items[i].itemSo.maxAmount;
                        SetAmount(i, _items[i].itemSo.maxAmount);
                        item.amount = rest;
                    }
                 }
            }
            
            for(int i=0; i< _sizeInventory; i++)
            {
                if (_items[i].itemSo == itemVoid)
                {
                    _items[i] = item;
                    SetAmount(i, item.amount);
                    _itemsSlot[i].sprite = _items[i].itemSo.itemSprite;
                    
                    return;
                }
            }

            //Si llegamos aqui, es porque falta espacio para meter el resto de un item.
            DropItem(-1, item.itemSo);
           
        }

        public void SwitchItems(int indexA, int indexB)
        {
          
            (_textAmount[indexB], _textAmount[indexA]) = (_textAmount[indexA], _textAmount[indexB]);
            (_itemsSlot[indexB], _itemsSlot[indexA]) = (_itemsSlot[indexA], _itemsSlot[indexB]);
            (_items[indexB], _items[indexA]) = (_items[indexA], _items[indexB]);
            
        }

        public void DropItem(int index=-1, ItemScriptableObject item=null)
        {   
            GameObject p = Instantiate(dropItemPrefab, PlayerAttack.attackPoint.position, Quaternion.identity);
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
            
            Vector3 direction = PlayerAttack.attackPoint.position - transform.position;
            auxItem.DropItem((direction.normalized * dropForce) + PlayerAttack.attackPoint.position);
          
            if(index >= 0)
            {
                _items[index].itemSo = itemVoid;
                _itemsSlot[index].sprite = itemVoid.itemSprite;
                _textAmount[index].text = "";
            }
        }
      
    }
}



