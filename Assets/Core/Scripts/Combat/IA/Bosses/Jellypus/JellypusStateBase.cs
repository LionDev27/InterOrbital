using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusStateBase : EnemyStateBase
    {
        protected BossAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as BossAgent;
        }

        public override void OnStateEnter()
        {
        }

        public override void Execute()
        {
        }
    }
}
