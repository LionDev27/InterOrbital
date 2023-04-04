using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField] private float _loseHealthTimerDefaultValue;
        private float _loseHealthTimer;

        private void Update()
        {
            LoseHealthOverTime();
            //TODO: EVENTOS DE ACTUALIZACION DE HUD
        }

        private void LoseHealthOverTime()
        {
            bool energyEmpty = PlayerComponents.Instance.PlayerEnergy.EnergyEmpty;
            if (energyEmpty)
            {
                if (_loseHealthTimer > 0)
                {
                    _loseHealthTimer -= Time.deltaTime;
                }
                else
                {
                    _currentHealth = Mathf.Clamp(_currentHealth - 1, 0, _maxHealth);
                    ResetTimer();
                    CheckHealth();
                }
            }
        }

        //TODO: TIEMPO DE INVENCIBILIDAD DESPUES DE RECIBIR DAÑO
        public override void GetDamage(int damage)
        {
            base.GetDamage(damage);
        }

        public override void RestoreHealth(int healthAmount)
        {
            base.RestoreHealth(healthAmount);
            ResetTimer();
        }

        private void ResetTimer()
        {
            _loseHealthTimer = _loseHealthTimerDefaultValue;
        }

        protected override void Death()
        {
            Debug.Log("Player Dead");
            PlayerComponents.Instance.InputHandler.DeactivateControls();
        }

    }
}
