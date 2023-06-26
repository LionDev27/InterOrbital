using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterOrbital.Spaceship
{
    public class SpaceshipFuelSlot : MonoBehaviour, IPointerClickHandler
    {
        private Image _image;
        [HideInInspector] public ItemFuelScriptableObject item;
        private SpaceshipEnergyManager _spaceshipEnergyManager;
        private bool _canBeSelected;
        [SerializeField] TextMeshProUGUI _amountText; 


        private void Awake()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
            _canBeSelected = true;
        }

        public void SetFuelItem(ItemFuelScriptableObject item)
        {
            this.item = item;
            _image.sprite = item.itemSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_canBeSelected)
            {
                SubstractSlot();
                _spaceshipEnergyManager.SetFuelSelected(this);
                FindInInventory();
            }
        }

        public void SubstractSlot()
        {
            PlayerComponents.Instance.Inventory.SubstractItems(item, 1);
            FindInInventory();
        }

        public void AddSlot()
        {
            PlayerComponents.Instance.Inventory.AddOneItemSO(item);
            FindInInventory();
        }

        public void SetSpaceShipManager(SpaceshipEnergyManager sem)
        {
            _spaceshipEnergyManager = sem;
        }

        public void FindInInventory()
        {
            int amount = PlayerComponents.Instance.Inventory.GetTotalItemAmount(item);
            if( amount > 0)
            {
                _amountText.text = amount.ToString();
                var fuelSlotSelected= _spaceshipEnergyManager.GetFuelSlotSelected();

                if (fuelSlotSelected != null  && item.id == fuelSlotSelected.item.id ||
                    SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy() == SpaceshipComponents.Instance.SpaceshipEnergy.GetMaxEnergy())
                {
                    _canBeSelected = false;
                    _image.ChangueAlphaColor(0.5f);
                }
                else 
                {
                    _canBeSelected = true;
                    _image.ChangueAlphaColor(1f);
                }

            }
            else
            {
                _canBeSelected = false;
                _amountText.text = "0";
                _image.ChangueAlphaColor(0.5f);
            }

            _spaceshipEnergyManager.UpdateEnergyGiven();

        }

        public void RestoreSlot()
        {
            if(PlayerComponents.Instance.Inventory.GetTotalItemAmount(item) > 0 && 
                    SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy() != SpaceshipComponents.Instance.SpaceshipEnergy.GetMaxEnergy())
            {
                _canBeSelected = true;
                _image.ChangueAlphaColor(1f);
            }
        }
    }
}
