using InterOrbital.UI;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Player
{
    public class PlayerEnergy : MonoBehaviour
    {
        public bool EnergyEmpty { get; private set; }

        [SerializeField] private int _currentEnergy;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private int _loseEnergyValue;
        [SerializeField] private float _loseEnergyTimerDefaultValue;
        private float _loseEnergyTimer;
        private bool _loseEnegrgyOverTime;

        private void Start()
        {
            _currentEnergy = _maxEnergy;
            EnergyEmpty = false;
            _loseEnegrgyOverTime = true;
            ResetTimer();
        }

        private void Update()
        {
            LoseEnergyOverTime();
        }


        public void RestoreEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy + energyAmount, 0, _maxEnergy);
            UIManager.Instance.UpdateEnergyUI(_maxEnergy, _currentEnergy);
            ResetTimer();
            CheckEnergy();
        }

        public void LoseEnergy(int energyAmount)
        {
            _currentEnergy = Mathf.Clamp(_currentEnergy - energyAmount, 0, _maxEnergy);
            UIManager.Instance.UpdateEnergyUI(_maxEnergy, _currentEnergy);
            ResetTimer();
            CheckEnergy();
        }

        public int GetCurrentPlayerEnergy()
        {
            return _currentEnergy;
        }

        public void StopLoseEnergyOverTime()
        {
            _loseEnegrgyOverTime = false;
        }
        public void ResumeLoseEnergyOverTime()
        {
            _loseEnegrgyOverTime = true;
        }

        public void ToggleLoseEnergy()
        {
            if (Application.isEditor)
                _loseEnegrgyOverTime = !_loseEnegrgyOverTime;
        }

        public void UpgradeEnergy(int energyAmount)
        {
            _maxEnergy += energyAmount;
            _currentEnergy = _maxEnergy;
            UIManager.Instance.UpgradeEnergyUI();
            UIManager.Instance.UpdateEnergyUI(_maxEnergy, _currentEnergy);
            ResetTimer();
        }

        public void ResetEnergy()
        {
            _currentEnergy = _maxEnergy;
            UIManager.Instance.UpdateEnergyUI(_maxEnergy, _currentEnergy);
            ResetTimer();
        }

        private void LoseEnergyOverTime()
        {
            if (!EnergyEmpty && _loseEnegrgyOverTime)
            {
                if (_loseEnergyTimer > 0)
                {
                    _loseEnergyTimer -= Time.deltaTime;
                }
                else
                {
                    LoseEnergy(_loseEnergyValue);
                }
            }
        }

        private void CheckEnergy()
        {
            if(_currentEnergy <= (_maxEnergy * 0.2f))
            {
                UIManager.Instance.EnergyBlink(true);
            }
            else
            {
                UIManager.Instance.EnergyBlink(false);
            }

            if (_currentEnergy <= 0)
            {
                EnergyEmpty = true;
                ResetTimer();
            }
            else
            {
                EnergyEmpty = false;
            }
        }

        private void ResetTimer()
        {
            _loseEnergyTimer = _loseEnergyTimerDefaultValue;
        }
    }
}
