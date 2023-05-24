using InterOrbital.UI;
using InterOrbital.Utils;
using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Item
{
    public class StorageItem : MonoBehaviour, IInteractable
    {
        private GameObject _storageUI;
        private ChestInventory generalChest;
        private ChestInventory _myChestInventory;

        private void Awake()
        {
            generalChest = UIManager.Instance.storageUI.transform.GetChild(1).GetComponent<ChestInventory>();
        }

        private void Start()
        {
            _storageUI = UIManager.Instance.storageUI;
        }

        public void Interact()
        {
            UIManager.Instance.OpenInventory(true);
            generalChest.SetChest(_myChestInventory);
        }

        public void EndInteraction()
        {

            UIManager.Instance.OpenInventory(true);
            
        }
    }
}
