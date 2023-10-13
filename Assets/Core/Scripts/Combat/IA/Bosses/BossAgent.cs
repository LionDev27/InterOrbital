using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAgent : EnemyAgentBase
    {
        public BossDamageable Damageable => _damageable;
        
        private BossDamageable _damageable;

        protected override void Awake()
        {
            base.Awake();
            _damageable = GetComponent<BossDamageable>();
        }
    }
}