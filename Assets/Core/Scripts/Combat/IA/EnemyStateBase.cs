using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyStateBase : MonoBehaviour
    {
        private EnemyAgentBase _currentAgent;

        public virtual void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent;
        }

        public virtual void OnStateEnter(){}

        public virtual void Execute(){}
    }
}
