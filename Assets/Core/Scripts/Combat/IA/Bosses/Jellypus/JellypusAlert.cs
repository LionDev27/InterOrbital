using System;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusAlert : JellypusStateBase
    {
        [SerializeField] private float _losePlayerTime;
        [SerializeField] private float _closeAttackDistance;
        private BossAttacks _bossAttacks;
        private float _losePlayerTimer;

        public override void Setup(EnemyAgentBase agent)
        {
            base.Setup(agent);
            _bossAttacks = GetComponent<BossAttacks>();
        }

        public override void OnStateEnter()
        {
            ResetTimer();
        }

        public override void Execute()
        {
            if (_currentAgent.IsDetectingPlayer())
            {
                if (_losePlayerTimer < _losePlayerTime)
                    ResetTimer();
                if (_bossAttacks.Attacking) return;
                if (Vector2.Distance(_currentAgent.Target.position, transform.position) <= _closeAttackDistance)
                    _bossAttacks.CloseAttack();
                else
                    _bossAttacks.RandomAttack();
            }
            else
            {
                _losePlayerTimer -= Time.deltaTime;
                if (_losePlayerTimer <= 0f)
                    _currentAgent.ChangeState(_currentAgent.States[0]);
            }
        }

        private void ResetTimer()
        {
            _losePlayerTimer = _losePlayerTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _closeAttackDistance);
        }
    }
}