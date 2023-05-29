using InterOrbital.UI;
using InterOrbital.Utils;
using InterOrbital.Player;
using UnityEngine;
using System.Collections;

namespace InterOrbital.Item
{
    public class StorageItem : MonoBehaviour, IInteractable
    {
        private ItemObject[] _itemsChest;
        [SerializeField] private SizeChest _size;


        private void Start()
        {
 
            int size = _size == SizeChest.Small ? 10 : 19;
            _itemsChest = new ItemObject[size];
            
            for(int i=0; i<_itemsChest.Length; i++)
            {
                GameObject obj = new GameObject();
                ItemObject item = obj.AddComponent<ItemObject>();
                Destroy(obj);
                _itemsChest[i] = item;
                _itemsChest[i].itemSo = PlayerComponents.Instance.Inventory.itemVoid;
            }
        }

        public void Interact()
        {
           UIManager.Instance.chestInventory.SetChest(_itemsChest);
           UIManager.Instance.OpenInventory(true);
           UIManager.Instance.chestInventory.isHide = false;
        }

        public void EndInteraction()
        {
         
            UIManager.Instance.OpenInventory(true);
           
            _itemsChest = UIManager.Instance.chestInventory.GetItems();

            UIManager.Instance.chestInventory.isHide = true;
        }
    }
}
