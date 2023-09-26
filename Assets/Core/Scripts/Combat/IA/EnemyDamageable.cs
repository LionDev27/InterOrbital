using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using InterOrbital.Mission;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InterOrbital.Combat.IA
{
    public class EnemyDamageable : Damageable
    {
        [SerializeField] private GameObject _dropItemPrefab;
        [SerializeField] private List<EnemyDrops> _dropsList;
        [SerializeField] private ParticleSystem _deathParticles;
        [SerializeField] private float _deathTime;
        [SerializeField] private float _dropForce = 1.5f;
        private MissionCreator _missionCreator;
        private EnemyAgentBase _agent;

        private void Awake()
        {
            _missionCreator = FindObjectOfType<MissionCreator>();
            _agent = GetComponent<EnemyAgentBase>();
        }

        public override void GetDamage(int damage)
        {
            _agent.HitEnemy();
            base.GetDamage(damage);
        }

        public void ExplosionDeath()
        {
            StartCoroutine(nameof(DeathSequence));
        }

        protected override void Death()
        {
            _missionCreator.UpdateMission(1, null);
            StartCoroutine(nameof(DeathSequence));
        }

        private IEnumerator DeathSequence()
        {
            yield return new WaitForSeconds(_deathTime);
            Instantiate(_deathParticles, transform.position, _deathParticles.transform.rotation).Play();
            DropAllItems();
            _agent.Death();
            Destroy(gameObject);
        }

        private void DropAllItems()
        {
            foreach (var drop in _dropsList)
            {
                DropItem(drop);
            }
        }

        private void DropItem(EnemyDrops drop)
        {
            if (Random.Range(0, 101) > drop.dropRate) return;
            int dropCount = Random.Range(drop.minAmount, drop.maxAmount + 1);

            for (int i = 0; i < dropCount; i++)
            {
                Vector2 dropItemOriginPos = transform.position;
                GameObject tempDroppingObject = Instantiate(_dropItemPrefab, dropItemOriginPos, Quaternion.identity);
                ItemObject tempDroppingItem = tempDroppingObject.GetComponent<ItemObject>();
            
                if (tempDroppingItem == null) return;
                tempDroppingItem.ObtainComponents();
                tempDroppingItem.SetItem(drop.item);
                tempDroppingItem.DropItem(RandomDropDir() * _dropForce + dropItemOriginPos);
            }
        }

        private Vector2 RandomDropDir()
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 0f);
            while (x == 0 && y == 0)
            {
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 0f);
            }
            return new Vector2(x, y).normalized;
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
