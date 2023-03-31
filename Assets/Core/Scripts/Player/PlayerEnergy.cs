using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        public bool EnergyEmpty { get; private set; }

        [SerializeField] private int _currentEnergy;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private float _loseEnergyTimerDefaultValue;
        private float _loseEnergyTimer;

        public Image img;
        private void Start()
        {
            _currentEnergy = _maxEnergy;
            EnergyEmpty = false;
            ResetTimer();
        }

        private void Update()
        {
            LoseEnergyOverTime();
            img.fillAmount = (float)_currentEnergy / _maxEnergy;

            if (Input.GetKeyDown(KeyCode.R))
                RestoreEnergy(5);

            if (Input.GetKeyDown(KeyCode.L))
                LoseEnergy(3);
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
