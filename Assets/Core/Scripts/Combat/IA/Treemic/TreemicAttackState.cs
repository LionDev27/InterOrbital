using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class TreemicAttackState : EnemyStateBase
    {
        [SerializeField] protected GameObject _bulletPrefab;
        [SerializeField] protected Transform _bulletTransformSpawn;
        [SerializeField] private int _bulletDamage;
        [SerializeField] private AnimationClip _attackAnimation;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _animationSpeed;
        [SerializeField] private AudioClip _sfx;
        private EnemyAgentBase _agent;
        private float _timer;
        
        public override void Setup(EnemyAgentBase agent)
        {
            _agent = agent;
            _bulletPrefab.GetComponent<DamageDealer>().damage = _bulletDamage;
        }

        public override void OnStateEnter()
        {
            SetAttack();
            _agent.Animator.SetBool("Idle", false);
            _agent.Animator.speed = _animationSpeed;
        }

        public override void Execute()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                if (_agent.IsDetectingPlayer())
                    SetAttack();
                else
                {
                    _agent.Animator.speed = 1;
                    _agent.ChangeState(_agent.States[0]);
                }
            }
        }

        private void SetAttack()
        {
            _timer = (_attackAnimation.length / _animationSpeed) + _attackCooldown;
            _agent.Animator.SetTrigger("Attack");
        }

        protected Vector2 TargetDir()
        {
            return (_agent.Target.position - _bulletTransformSpawn.position).normalized;
        }

        public virtual void Attack()
        {
            if (_sfx != null)
                AudioManager.Instance.PlaySFX(_sfx);
        }
    }
}