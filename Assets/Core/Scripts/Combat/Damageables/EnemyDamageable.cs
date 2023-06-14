using System;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyDamageable : Damageable
    {
        private EnemyAgentBase _agent;

        private void Awake()
        {
            _agent = GetComponent<EnemyAgentBase>();
        }

        public override void GetDamage(int damage)
        {
            _agent.HitEnemy();
            base.GetDamage(damage);
        }

        protected override void Death()
        {
            Destroy(gameObject);
        }
    }
}
