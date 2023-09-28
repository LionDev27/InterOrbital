using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAttackState : EnemyStateBase
    {
        [SerializeField] private AnimationClip _attackAnimation;
        [SerializeField] private float _attackCooldown;
        protected ElytrinAgent _currentAgent;
        private float _timer;
        
        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.EnableNavigation(false);
            _currentAgent.Animator.SetBool("Running", false);
            Attack();
        }

        public override void Execute()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (Vector3.Distance(transform.position, _currentAgent.Target.position) <= _currentAgent.AttackRange)
                {
                    Attack();
                    return;
                }
                _currentAgent.Animator.SetBool("Running", true);
                _currentAgent.ChangeState(_currentAgent.States[2]);
            }
            else
                SetDamageableDir();
        }

        private void Attack()
        {
            _timer = _attackAnimation.length + _attackCooldown;
            _currentAgent.Animator.SetTrigger("Attack");
        }

        protected virtual void SetDamageableDir()
        {
            Vector3 targetDir = (_currentAgent.Target.position - transform.position).normalized;
            _currentAgent.DamageDealer.transform.position = transform.position + (targetDir * _currentAgent.AttackRange);
        }
    }
}
