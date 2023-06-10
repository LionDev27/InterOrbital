using System;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAgent : EnemyAgentBase
    {
        [SerializeField] private Vector2 _detectionRange;
        [SerializeField] private float _attackRange;
        private Transform _target;
        private SpriteFlipper _spriteFlipper;
        private float _timer;
        
        public float timeToIdle = 5f;
        public Transform Target => _target;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;

        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
        }

        protected override void Start()
        {
            base.Start();
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

        public bool CanAttackPlayer()
        {
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
