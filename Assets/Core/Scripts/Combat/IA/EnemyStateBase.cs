using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public abstract class EnemyStateBase : MonoBehaviour
    {
        public abstract void Setup(EnemyAgentBase agent);
        public abstract void OnStateEnter();
        public abstract void Execute();
    }
}
