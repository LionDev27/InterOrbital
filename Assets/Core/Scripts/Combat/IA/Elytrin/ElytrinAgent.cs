using System;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAgent : EnemyAgentBase
    {
        [SerializeField] private Transform _damageDealer;
        private SpriteFlipper _spriteFlipper;
        private float _timer;
        private float _attackRange;
        
        public float timeToIdle = 5f;

        public Transform DamageDealer => _damageDealer;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;
        public float AttackRange => _attackRange;

        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
        }

        protected override void Start()
        {
            base.Start();
            _attackRange = Vector3.Distance(transform.position, _damageDealer.position);
        }

        protected override void Update()
        {
            if (Animator.GetBool("Hit") && _currentState == _states[0])
                ChangeState(_states[1]);
            base.Update();
        }

        public void RunTimer()
        {
            _timer -= Time.deltaTime;
        }
        
        public void ResetTimer()
        {
            _timer = timeToIdle;
        }
        
        public bool TimerEnded()
        {
            return _timer <= 0f;
        }

        public bool TimerChanged()
        {
            return _timer < timeToIdle;
        }

        protected override void EndHit()
        {
            if (!_animator.GetBool("Running"))
            {
                _animator.SetBool("Running", true);
                ChangeState(_states[1]);
            }
            base.EndHit();
        }
    }
}
