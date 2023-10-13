using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InterOrbital.Combat.IA
{
    public class BossDamageable : EnemyDamageable
    {
        [SerializeField] private UnityEvent _onDeath;
        [SerializeField] private float _timePerRecover = 0.5f;
        [SerializeField] private string _name;
        [SerializeField] private ParticleSystem _healParticles;

        protected override void UpdateLifeBar()
        {
            BossInfoBar.OnUpdateLifeBar?.Invoke(_currentHealth);
        }

        protected override void HitReceived(){}

        protected override void CheckHitTimer(){}

        protected override void Death()
        {
            _onDeath?.Invoke();
        }

        private IEnumerator Recover()
        {
            _healParticles.Play();
            while (_hitted)
            {
                if (_currentHealth >= _maxHealth)
                {
                    _currentHealth = _maxHealth;
                    _hitted = false;
                    break;
                }
                yield return new WaitForSeconds(_timePerRecover);
                _currentHealth++;
            }
            _healParticles.Stop();
        }

        public void DeactivateBoss()
        {
            _hitted = true;
            BossInfoBar.OnDeactivateBoss.Invoke();
            StartCoroutine(Recover());
        }

        public void ActivateBoss()
        {
            _hitted = false;
            BossInfoBar.OnActivateBoss?.Invoke(_name, _currentHealth, _maxHealth);
        }
    }
}
