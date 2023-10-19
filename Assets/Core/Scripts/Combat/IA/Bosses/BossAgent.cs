using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAgent : EnemyAgentBase
    {
        public BossDamageable Damageable => _damageable;
        
        [SerializeField] private List<BossPhase> _phases;
        private int _currentAttackIndex;
        private BossDamageable _damageable;

        protected override void Awake()
        {
            base.Awake();
            _damageable = GetComponent<BossDamageable>();
        }

        public bool IsAttacking()
        {
            return CurrentAttacks().Attacking;
        }

        public BossAttacks CurrentAttacks()
        {
            return _phases[_currentAttackIndex].attacks;
        }

        public void ChangePhase()
        {
            for (int i = 0; i < _phases.Count; i++)
            {
                var currentPhase = _phases[i];
                if (_damageable.CurrentHealth <= currentPhase.healthToChange && _currentAttackIndex != i)
                    _currentAttackIndex = i;
            }
        }
    }
}