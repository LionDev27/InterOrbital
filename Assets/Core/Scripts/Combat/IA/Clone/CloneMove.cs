using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneMove : EnemyStateBase
    {
        private CloneAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as CloneAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.ShowGun(true);
            _currentAgent.Animator.SetBool("PlayerRunning", true);
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (Vector2.Distance(transform.position, _currentAgent.Target.position) <= _currentAgent.RandomDistance)
                _currentAgent.ChangeState(2);
            if (_currentAgent.NavMeshAgent.destination != _currentAgent.Target.position)
                _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
        }
    }
}