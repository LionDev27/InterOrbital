using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace InterOrbital.Spaceship
{
    public class SpaceshipEnergy : MonoBehaviour
    {
        [SerializeField] private int _currentEnergy;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private Image _spaceshipEnergyImg;


        private void Awake()
        {

        }

        private void Start()
        {
            _currentEnergy = _maxEnergy;
        }

        public void RestoreEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy + energyAmount, 0, _maxEnergy);
            UpdateSpaceshipEnergyUI();
        }

        public void LoseEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy - energyAmount, 0, _maxEnergy);
            UpdateSpaceshipEnergyUI();
        }

        public void UpgradeEnergy(int energyAmount)
        {
            _maxEnergy += energyAmount;
        }

        public int GetCurrentSpaceshipEnergy()
        {
            return _currentEnergy;
        }

        public int GetMaxEnergy()
        {
            return _maxEnergy;
        }

        private void UpdateSpaceshipEnergyUI()
        {
            float fillAmount = _currentEnergy / (float)_maxEnergy;
            _spaceshipEnergyImg.fillAmount = fillAmount;
        }
    }
}
