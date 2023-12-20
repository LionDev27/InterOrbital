using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfAttackState : EnemyStateBase
    {
        [SerializeField] private float _attackStartTime;
        [SerializeField] private float _attackTime;
        [SerializeField] private float _attackEndTime;
        [Range(1f, 2f)] [SerializeField] private float _attackDistanceMultiplier;
        private WolfAgent _currentAgent;
        private Rigidbody2D _rigidbody;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.Sleep();
        }

        public override void OnStateEnter()
        {
            _currentAgent.EnableNavigation(false);
            _rigidbody.WakeUp();
            StartCoroutine(Attack());
        }

        public override void Execute(){}

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(_attackStartTime);
            var currentPos = transform.position;
            var distance = Vector2.Distance(_currentAgent.Target.position, currentPos);
            var dir = _currentAgent.Target.position - currentPos;
            dir.Normalize();
            _rigidbody.DOMove(currentPos + (dir * distance * _attackDistanceMultiplier), _attackTime)
                .OnComplete(() => StartCoroutine(EndAttack()));
            StartCoroutine(_currentAgent.AttackCooldown());
        }

        private IEnumerator EndAttack()
        {
            yield return new WaitForSeconds(_attackEndTime);
            _rigidbody.Sleep();
            _currentAgent.ChangeState(_currentAgent.States[1]);
        }
    }
}