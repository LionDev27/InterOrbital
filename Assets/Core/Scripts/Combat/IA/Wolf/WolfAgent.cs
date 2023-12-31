using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfAgent : EnemyAgentBase
    {
        [SerializeField] private Vector2 _attackRange;
        [SerializeField] private float _attackCooldown;
        private SpriteFlipper _spriteFlipper;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;
        public bool CanAttack => _canAttack;
        private bool _canAttack;

        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
        }

        protected override void Start()
        {
            base.Start();
            _canAttack = true;
        }

        public void FlipSprite()
        {
            if (Vector2.Distance(NavMeshAgent.destination, transform.position) < 0.5f) return;
            switch (Mathf.Sign(NavMeshAgent.destination.x - transform.position.x))
            {
                case > 0:
                    SpriteFlipper.FlipX(1);
                    break;
                case < 0:
                    SpriteFlipper.FlipX(0);
                    break;
            }
        }
        
        public bool IsTargetInAttackRange()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _attackRange, 0f);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, _attackRange);
        }

        public IEnumerator AttackCooldown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }
    }
}