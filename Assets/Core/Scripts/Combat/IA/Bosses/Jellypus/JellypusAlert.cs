using System;
using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusAlert : JellypusStateBase
    {
        [SerializeField] private float _losePlayerTime;
        [SerializeField] private float _closeAttackDistance;
        [Tooltip("Tiempo que tarda en atacar")]
        [SerializeField] private float _attackTime;
        private BossAttacks _bossAttacks;
        private float _losePlayerTimer;

        public override void Setup(EnemyAgentBase agent)
        {
            base.Setup(agent);
            _bossAttacks = GetComponentInChildren<BossAttacks>();
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
                StartCoroutine(Attack());
            }
            else
            {
                _losePlayerTimer -= Time.deltaTime;
                if (_losePlayerTimer <= 0f)
                    _currentAgent.ChangeState(_currentAgent.States[0]);
            }
        }

        private IEnumerator Attack()
        {
            _bossAttacks.StartAttack();
            yield return new WaitForSeconds(_attackTime);
            if (Vector2.Distance(_currentAgent.Target.position, transform.position) <= _closeAttackDistance)
                _bossAttacks.CloseAttack();
            else
                _bossAttacks.RandomAttack();
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