using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        public bool EnergyEmpty { get; private set; }
        public Image img;

        [SerializeField] private int _currentEnergy;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private float _loseEnergyTimerDefaultValue;
        private float _loseEnergyTimer;

        private void Start()
        {
            _currentEnergy = _maxEnergy;
            EnergyEmpty = false;
            ResetTimer();
        }

        private void Update()
        {
            LoseEnergyOverTime();
            //TODO: EVENTOS DE ACTUALIZACION DE HUD
        }


        public void RestoreEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy + energyAmount, 0, _maxEnergy);
            ResetTimer();
            CheckEnergy();
        }

        public void LoseEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy - energyAmount, 0, _maxEnergy);
            ResetTimer();
            CheckEnergy();
        }

        public void UpgradeEnergy(int energyAmount)
        {
            _maxEnergy += energyAmount;
        }

        private void LoseEnergyOverTime()
        {
            if (!EnergyEmpty)
            {
                if (_loseEnergyTimer > 0)
                {
                    _loseEnergyTimer -= Time.deltaTime;
                }
                else
                {
                    _currentEnergy = Mathf.Clamp(_currentEnergy - 1, 0, _maxEnergy);
                    ResetTimer();
                    CheckEnergy();
                }
            }
        }

        private void CheckEnergy()
        {
            if (_currentEnergy <= 0)
            {
                EnergyEmpty = true;
                ResetTimer();
            }
            else
                EnergyEmpty = false;
        }

        private void ResetTimer()
        {
            _loseEnergyTimer = _loseEnergyTimerDefaultValue;
        }
    }
}
