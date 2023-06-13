using System;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAgent : EnemyAgentBase
    {
        [SerializeField] private Vector2 _detectionRange;
        [SerializeField] private Transform _damageDealer;
        private Transform _target;
        private SpriteFlipper _spriteFlipper;
        private float _timer;
        private float _attackRange;
        
        public float timeToIdle = 5f;

        public Transform DamageDealer => _damageDealer;
        public Transform Target => _target;
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
            Debug.Log(_attackRange);
            ChangeState(_states[0]);
        }

        protected override void Update()
        {
            base.Update();
        }

        public bool IsDetectingPlayer()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _detectionRange, 0f);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    if (!_target) _target = col.transform;
                    return true;
                }
            }
            return false;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _detectionRange);
        }
    }
}
