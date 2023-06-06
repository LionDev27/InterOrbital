using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinIdleState : EnemyStateBase
    {
        private ElytrinAgent _currentAgent;
        
        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            Debug.Log("Estado Idle");
        }

        public override void Execute()
        {
            if (_currentAgent.IsDetectingPlayer())
            {
                _currentAgent.ChangeState(_currentAgent.States[1]);
            }
        }
    }
}
