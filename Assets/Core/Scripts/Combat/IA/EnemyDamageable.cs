using System;
using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyDamageable : Damageable
    {
        [SerializeField] private ParticleSystem _deathParticles;
        [SerializeField] private float _deathTime;
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
            StartCoroutine(nameof(DeathSequence));
        }

        private IEnumerator DeathSequence()
        {
            yield return new WaitForSeconds(_deathTime);
            Instantiate(_deathParticles, transform.position, _deathParticles.transform.rotation).Play();
            _agent.Death();
            Destroy(gameObject);
        }
    }
}
