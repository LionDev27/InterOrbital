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
        [Tooltip("Multiplicador para saber a qué distancia del jugador se queda después del ataque.")]
        [Range(1f, 2f)] [SerializeField] private float _attackDistanceMultiplier;
        [SerializeField] private float _minAttackDistance = 8f;
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
            var dir = _currentAgent.Target.position - currentPos;
            dir.Normalize();
            _rigidbody.DOMove(currentPos + (dir * (GetDistance() * _attackDistanceMultiplier)), _attackTime)
                .OnComplete(() => StartCoroutine(EndAttack()));
            StartCoroutine(_currentAgent.AttackCooldown());
        }

        private IEnumerator EndAttack()
        {
            yield return new WaitForSeconds(_attackEndTime);
            _rigidbody.Sleep();
            _currentAgent.ChangeState(_currentAgent.States[1]);
        }

        private float GetDistance()
        {
            var distance = Vector2.Distance(_currentAgent.Target.position, transform.position);
            return distance < _minAttackDistance ? _minAttackDistance : distance;
        }
    }
}