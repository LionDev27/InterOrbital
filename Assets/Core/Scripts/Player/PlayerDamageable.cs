using System;
using InterOrbital.Combat;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField] private float _invencibilityTime;
        [SerializeField] private float _loseHealthTimerDefaultValue;
        private float _loseHealthTimer;
        private float _invencibilityTimer;
        
        private PlayerComponents _playerComponents;

        private void Awake()
        {
            _playerComponents = PlayerComponents.Instance;
        }

        private void Update()
        {
            LoseHealthOverTime();
            RunInvencibilityTimer();
        }

        private void LoseHealthOverTime()
        {
            bool energyEmpty = _playerComponents.PlayerEnergy.EnergyEmpty;
            if (energyEmpty)
            {
                if (_loseHealthTimer > 0)
                {
                    _loseHealthTimer -= Time.deltaTime;
                }
                else
                {
                    _currentHealth = Mathf.Clamp(_currentHealth - 1, 0, _maxHealth);
                    UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
                    ResetTimer();
                    CheckHealth();
                }
            }
        }
        
        private void ResetTimer()
        {
            _loseHealthTimer = _loseHealthTimerDefaultValue;
        }

        #region Invencibility

        private void RunInvencibilityTimer()
        {
            _invencibilityTimer -= Time.deltaTime;
        }
        
        private bool CanTakeDamage()
        {
            //Recibira daño si ha terminado su cooldown de invencibilidad y no está realizando un dash.
            return _invencibilityTimer <= 0 && !_playerComponents.PlayerDash.IsDashing();
        }

        #endregion
        
        public override void GetDamage(int damage)
        {
            if (CanTakeDamage())
            {
                Debug.Log("Recibiendo daño");
                base.GetDamage(damage);
                _invencibilityTimer = _invencibilityTime;
            }
        }

        public override void RestoreHealth(int healthAmount)
        {
            base.RestoreHealth(healthAmount);
            ResetTimer();
        }

        protected override void Death()
        {
            Debug.Log("Player Dead");
            //TODO: Desactivar colisiones y animacion de muerte.
            _playerComponents.InputHandler.DeactivateControls();
        }

    }
}
