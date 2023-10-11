using UnityEngine;
using UnityEngine.Events;

namespace InterOrbital.Combat.IA
{
    public class BossDamageable : EnemyDamageable
    {
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        
        [SerializeField] private UnityEvent _onDeath;
        [SerializeField] private float _timePerRecover = 0.5f;

        protected override void UpdateLifeBar()
        {
            BossInfoBar.OnUpdateLifeBar?.Invoke(_currentHealth);
        }

        protected override void HitReceived(){}

        protected override void CheckHitTimer()
        {
            if (_hitted)
            {
                if (_currentHealth >= _maxHealth)
                {
                    _currentHealth = _maxHealth;
                    UpdateLifeBar();
                    _hitted = false;
                }
                _currentHealth += (int)(Time.deltaTime / _timePerRecover);
            }
        }

        protected override void Death()
        {
            _onDeath?.Invoke();
        }

        public void DeactivateBoss()
        {
            _hitted = true;
        }
    }
}
