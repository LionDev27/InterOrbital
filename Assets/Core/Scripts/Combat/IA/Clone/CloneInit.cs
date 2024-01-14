namespace InterOrbital.Combat.IA
{
    public class CloneInit : EnemyStateBase
    {
        private CloneAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as CloneAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.ShowGun(false);
        }

        public override void Execute()
        {
            
        }

        public void EndInit()
        {
            _currentAgent.ChangeState(1);
        }
    }
}