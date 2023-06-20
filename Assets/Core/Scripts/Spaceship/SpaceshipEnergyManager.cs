using InterOrbital.Item;
using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using InterOrbital.Utils;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace InterOrbital.Spaceship
{
    public class SpaceshipEnergyManager : MonoBehaviour
    {
        private SpaceshipFuelSlot _fuelSlotSelected;
        private int _amountSelected;
        [SerializeField] private Sprite _transparentImage;
        [SerializeField] private Image _fuelSelectedImage;
        [SerializeField] private TextMeshProUGUI _fuelSelectedText;
        [SerializeField] private Image _energyBarEnergy;
        [SerializeField] private Button _buttonPlus;
        [SerializeField] private Button _buttonSubstract;
        [SerializeField] private Button _takeAll;
        [SerializeField] private Button _returnAll;
        [SerializeField] private Button _useEnergy;
        [SerializeField] private TextMeshProUGUI _energyGivenText;
        [SerializeField] private TextMeshProUGUI _currentSpaceshipEnergyText;

        private void Start()
        {
            UpdateButtons();
        }

        private Color GetTextColor(int currentValue, int maxValue)
        {
            float percentage = (float)currentValue / maxValue;

            if (percentage <= 0.2f) 
            {
                return Color.red;
            }
            else if (percentage <= 0.6f)
            { 
                return Color.yellow;
            }
            else 
            {
                return Color.green;
            }
        }

        public void UpdateEnergyBar()
        {
            int currentEnergy = SpaceshipComponents.Instance.SpaceshipEnergy.GetCurrentSpaceshipEnergy();
            int maxValue = SpaceshipComponents.Instance.SpaceshipEnergy.GetMaxEnergy();
            Color textColor = GetTextColor(currentEnergy, maxValue);
            _currentSpaceshipEnergyText.color = textColor;
            _currentSpaceshipEnergyText.text= currentEnergy.ToString();
            var calcValue = (float) currentEnergy / maxValue;
            _energyBarEnergy.DOFillAmount(calcValue, 1).SetEase(Ease.Linear).Play();
        }

        public void UpdateEnergyGiven()
        {
            if (_fuelSlotSelected)
            {
                int energyGiven = _amountSelected * _fuelSlotSelected.item.energyProvided;
                _energyGivenText.text = "+" + energyGiven.ToString();
            }
            else
            {
                _energyGivenText.text = "+0";
            }
        }

        public void SetFuelSelected(SpaceshipFuelSlot _itemFuelSlot)
        {
            _fuelSlotSelected = _itemFuelSlot;
            _amountSelected = 1;
            _fuelSelectedText.text = _amountSelected.ToString();
            _fuelSelectedImage.sprite = _fuelSlotSelected.item.itemSprite;
            UpdateButtons();
        }

        public void UpdateButtons()
        {
            if (!_fuelSlotSelected)
            {
                _buttonPlus.image.ChangueAlphaColor(0.5f);
                _buttonSubstract.image.ChangueAlphaColor(0.5f);
                _takeAll.image.ChangueAlphaColor(0.5f);
                _returnAll.image.ChangueAlphaColor(0.5f);
                _useEnergy.image.ChangueAlphaColor(0.5f);
            }
            else
            {
                _buttonPlus.image.ChangueAlphaColor(1f);
                _buttonSubstract.image.ChangueAlphaColor(1f);
                _takeAll.image.ChangueAlphaColor(1f);
                _returnAll.image.ChangueAlphaColor(1f);
                _useEnergy.image.ChangueAlphaColor(1f);
            }
        }

        public void IncreaseAmount()
        {
            if (_fuelSlotSelected)
            {
                if(PlayerComponents.Instance.Inventory.GetTotalItemAmount(_fuelSlotSelected.item) > 0)
                {
                    _amountSelected++;
                    _fuelSelectedText.text = _amountSelected.ToString();
                    _fuelSlotSelected.SubstractSlot();
                }
            }
        }

        public void TakeAll()
        {
            int count = PlayerComponents.Instance.Inventory.GetTotalItemAmount(_fuelSlotSelected.item);
            for (int i=0; i<count ; i++)
            {
                _amountSelected++;
                _fuelSelectedText.text = _amountSelected.ToString();
                _fuelSlotSelected.SubstractSlot();
            }
        }

        public void DecreaseAmount()
        {
            if (_fuelSlotSelected)
            {
                if (_amountSelected > 1)
                {
                    _amountSelected--;
                    _fuelSelectedText.text = _amountSelected.ToString();
                    _fuelSlotSelected.AddSlot();
                }
                else
                {
                    RegretToInventory();
                }
            }
        }

        public void RegretToInventory()
        { 
            for(int i=0; i<_amountSelected; i++)
            {
                _fuelSlotSelected.AddSlot();
            }

            _amountSelected = 0;
            _fuelSelectedText.text = "";
            _fuelSlotSelected.RestoreSlot();
            _fuelSlotSelected = null;
            _fuelSelectedImage.sprite = _transparentImage;
            UpdateButtons();
            UpdateEnergyGiven();
            

        }


        public SpaceshipFuelSlot GetFuelSlotSelected()
        {
            return _fuelSlotSelected;
        }


        public void UseFuel()
        {
            int energyObtained = _amountSelected * _fuelSlotSelected.item.energyProvided;
            SpaceshipComponents.Instance.SpaceshipEnergy.RestoreEnergy(energyObtained);
            _amountSelected = 0;
            _fuelSelectedText.text = "";
            _fuelSlotSelected = null;
            _fuelSelectedImage.sprite = _transparentImage;
            UpdateButtons();
            UpdateEnergyGiven();
            UpdateEnergyBar();
        }
        

    }
}
   
