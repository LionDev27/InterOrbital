using System;
using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusAlert : JellypusStateBase
    {
        [SerializeField] private float _losePlayerTime;
        [SerializeField] private float _closeAttackDistance;
        private float _losePlayerTimer;

        public override void Setup(EnemyAgentBase agent)
        {
            base.Setup(agent);
            _currentAgent.ChangePhase += StopAllCoroutines;
        }

        private void OnEnable()
        {
            if (_currentAgent)
                _currentAgent.ChangePhase += StopAllCoroutines;
        }

        private void OnDisable()
        {
            _currentAgent.ChangePhase -= StopAllCoroutines;
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
                if (!CanAttack()) return;
                StartCoroutine(Attack(_currentAgent.CurrentAttacks()));
            }
            else
            {
                _losePlayerTimer -= Time.deltaTime;
                if (_losePlayerTimer <= 0f)
                    _currentAgent.ChangeState(_currentAgent.States[0]);
            }
        }

        private IEnumerator Attack(BossAttacks attacks)
        {
            attacks.StartAttack();
            yield return new WaitForSeconds(_currentAgent.CurrentAttacks().attackTime);
            if (Vector2.Distance(_currentAgent.Target.position, transform.position) <= _closeAttackDistance)
                attacks.CloseAttack();
            else
                attacks.RandomAttack();
        }

        private bool CanAttack()
        {
            return _currentAgent.CanAttack();
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