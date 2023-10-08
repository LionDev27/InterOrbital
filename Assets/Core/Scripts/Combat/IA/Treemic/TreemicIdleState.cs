namespace InterOrbital.Combat.IA
{
    public class TreemicIdleState : EnemyStateBase
    {
        private EnemyAgentBase _agent;
        
        public override void Setup(EnemyAgentBase agent)
        {
            _agent = agent;
        }

        public override void OnStateEnter()
        {
            _agent.Animator.SetBool("Idle", true);
        }

        public override void Execute()
        {
            if (_agent.IsDetectingPlayer())
                _agent.ChangeState(_agent.States[1]);
        }
    }
}
