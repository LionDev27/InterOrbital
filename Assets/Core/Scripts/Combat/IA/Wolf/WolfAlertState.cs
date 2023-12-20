using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfAlertState : EnemyStateBase
    {
        [SerializeField] private Vector2 _attackRange;
        [SerializeField] private float _speed;
        private WolfAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.NavMeshAgent.speed = _speed;
            _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
        }

        public override void Execute()
        {
            if (_currentAgent.IsDetectingPlayer())
            {
                if (IsTargetInAttackRange())
                {
                    //_currentAgent.ChangeState(_currentAgent.States[3]);
                    Debug.Log("AttackState");
                    return;
                }
                MoveToTarget();
            }
            else if (_currentAgent.ArrivedDestination())
                _currentAgent.ChangeState(_currentAgent.States[2]);
        }

        private bool IsTargetInAttackRange()
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

        private void MoveToTarget()
        {
            if (_currentAgent.NavMeshAgent.destination != _currentAgent.Target.position)
                _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, _attackRange);
        }
    }
}