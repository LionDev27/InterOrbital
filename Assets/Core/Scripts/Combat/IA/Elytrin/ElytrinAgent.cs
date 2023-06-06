using System;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAgent : EnemyAgentBase
    {
        [SerializeField] private Vector2 _detectionRange;
        [SerializeField] private float _attackRange;
        private Transform _target;

        public Transform Target => _target;

        private void Start()
        {
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
                    _target = col.transform;
                    return true;
                }
            }
            return false;
        }

        public bool CanAttackPlayer()
        {
            return false;
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
