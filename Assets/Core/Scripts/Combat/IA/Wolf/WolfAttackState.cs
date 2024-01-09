using DG.Tweening;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfAttackState : EnemyStateBase
    {
        [SerializeField] private AudioClip _attackSfx;
        [SerializeField] private float _attackTime;
        [Tooltip("Multiplicador para saber a qué distancia del jugador se queda después del ataque.")]
        [Range(1f, 2f)] [SerializeField] private float _attackDistanceMultiplier;
        [SerializeField] private float _minAttackDistance = 8f;
        private WolfAgent _currentAgent;
        private Rigidbody2D _rigidbody;
        private bool _attacking;
        
        private const string AttackingBoolAnim = "Attacking";

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
            _currentAgent.Animator.SetBool(AttackingBoolAnim, true);
        }

        public override void Execute()
        {
            if (!_attacking)
                _currentAgent.FlipSprite();
        }

        public void Attack()
        {
            _attacking = true;
            var currentPos = transform.position;
            var dir = _currentAgent.Target.position - currentPos;
            dir.Normalize();
            _rigidbody.DOMove(currentPos + (dir * (GetDistance() * _attackDistanceMultiplier)), _attackTime);
            _currentAgent.FlipSprite();
            AudioManager.Instance.PlaySFX(_attackSfx);
            StartCoroutine(_currentAgent.AttackCooldown());
        }

        public void EndAttack()
        {
            _attacking = false;
            _rigidbody.Sleep();
            _currentAgent.Animator.SetBool(AttackingBoolAnim, false);
            _currentAgent.ChangeState(_currentAgent.States[1]);
        }

        private float GetDistance()
        {
            var distance = Vector2.Distance(_currentAgent.Target.position, transform.position);
            return distance < _minAttackDistance ? _minAttackDistance : distance;
        }
    }
}