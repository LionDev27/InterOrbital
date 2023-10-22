using System.Collections;
using System.Collections.Generic;
using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAgent : EnemyAgentBase
    {
        public BossDamageable Damageable => _damageable;
        
        [SerializeField] private List<BossPhase> _phases;
        [SerializeField] private float _changePhaseTime;
        private int _currentAttackIndex;
        private BossDamageable _damageable;
        private bool _changingPhase;
        private bool _dying;

        protected override void Awake()
        {
            base.Awake();
            _damageable = GetComponent<BossDamageable>();
        }

        protected override void Start()
        {
            ChangeFillColor(_phases[_currentAttackIndex]);
            base.Start();
        }

        public bool CanAttack()
        {
            return !CurrentAttacks().Attacking && !_changingPhase && !_dying;
        }

        public BossAttacks CurrentAttacks()
        {
            return _phases[_currentAttackIndex].attacks;
        }

        public void DownPhase()
        {
            if (_currentAttackIndex == 0) return;
            var currentPhase = _phases[_currentAttackIndex];
            if (_damageable.CurrentHealth > currentPhase.healthToChange)
            {
                _currentAttackIndex--;
                ChangeFillColor(currentPhase);
            }
        }

        public void UpPhase()
        {
            if (_currentAttackIndex == _phases.Count - 1) return;
            var nextPhase = _phases[_currentAttackIndex + 1];
            if (_damageable.CurrentHealth <= nextPhase.healthToChange)
            {
                StartCoroutine(ChangePhaseTimer());
                CurrentAttacks().DeactivateAttacks();
                CurrentAttacks().EndAttack();
                _currentAttackIndex++;
                ChangeFillColor(nextPhase);
            }
        }

        public override void Death()
        {
            CameraShake.Instance.Shake(10);
            _dying = true;
            CurrentAttacks().DeactivateAttacks();
            CurrentAttacks().EndAttack();
        }

        private void ChangeFillColor(BossPhase phase)
        {
            BossInfoBar.OnChangeFillColor?.Invoke(phase.barFillColor);
        }

        private IEnumerator ChangePhaseTimer()
        {
            _changingPhase = true;
            yield return new WaitForSeconds(_changePhaseTime);
            _changingPhase = false;
        }
    }
}