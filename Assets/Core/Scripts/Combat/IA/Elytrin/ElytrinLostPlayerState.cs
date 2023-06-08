using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinLostPlayerState : EnemyStateBase
    {
        private ElytrinAgent _currentAgent;
        
        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool("PlayerLost", true);
        }

        public override void Execute()
        {
            _currentAgent.RunTimer();
            if (!_currentAgent.TimerEnded()) return;
            if (_currentAgent.Animator.GetBool("Running")) _currentAgent.Animator.SetBool("Running", false);
            _currentAgent.ChangeState(_currentAgent.States[0]);
        }
    }
}