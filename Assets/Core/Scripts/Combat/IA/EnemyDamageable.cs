using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyDamageable : Damageable
    {
        [SerializeField] private List<EnemyDrops> _dropsList;
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

    [Serializable]
    public struct EnemyDrops
    {
        public ItemScriptableObject item;
        public float dropRate;
        public int minAmount;
        public int maxAmount;
    }
}
