using System;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneChase : EnemyStateBase
    {
        [SerializeField] private float _distanceToPlayer;
        private CloneAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as CloneAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.EnableNavigation(true);
        }

        public override void Execute()
        {
            Debug.Log("Chasing");
            // if (Vector3.Distance(transform.position, _currentAgent.Target.position) <= _distanceToPlayer)
            // {
            //     _currentAgent.ChangeState(_currentAgent.States[1]);
            //     return;
            // }
            // if (_currentAgent.NavMeshAgent.destination != _currentAgent.Target.position)
            //     _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distanceToPlayer);
        }
    }
}