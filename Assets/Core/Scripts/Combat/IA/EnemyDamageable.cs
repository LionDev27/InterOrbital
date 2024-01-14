using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InterOrbital.Combat.IA
{
    public class EnemyDamageable : Damageable
    {
        [SerializeField] private GameObject _dropItemPrefab;
        [SerializeField] private List<EnemyDrops> _dropsList;
        [SerializeField] protected ParticleSystem _deathParticles;
        [SerializeField] protected float _deathTime;
        [SerializeField] private float _dropForce = 1.5f;
        protected EnemyAgentBase _agent;
        [SerializeField] private Image _lifeBar;
        [SerializeField] private CanvasGroup _lifeBarCG;

        protected float _noHitTime = 60f;
        protected float _noHitTimer;
        protected bool _hitted;

        protected virtual void Awake()
        {
            _agent = GetComponent<EnemyAgentBase>();
        }

        private void Update()
        {
            CheckHitTimer();
        }

        public override void GetDamage(int damage)
        {
            if (!_agent.Animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
            {
                _agent.HitEnemy();
                base.GetDamage(damage);
                HitReceived();
                UpdateLifeBar();
            }
        }

        public void ExplosionDeath()
        {
            StartCoroutine(nameof(DeathSequence));
        }

        protected override void Death()
        {
            StartCoroutine(nameof(DeathSequence));
        }

        private IEnumerator DeathSequence()
        {
            yield return new WaitForSeconds(_deathTime);
            Instantiate(_deathParticles, transform.position, _deathParticles.transform.rotation).Play();
            DropAllItems();
            _agent.Death();
            AfterDeath();
            Destroy(gameObject);
        }

        protected virtual void AfterDeath(){}

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

        protected Vector2 RandomDropDir()
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

        protected virtual void UpdateLifeBar()
        {
            float lifeAmount = _currentHealth / (float)_maxHealth;
            if (_currentHealth < _maxHealth && HasLifeBar())
            {
                _lifeBarCG.alpha = 1;
                _lifeBar.fillAmount = lifeAmount;
            }
        }

        protected virtual void HitReceived()
        {
            if (!_hitted)
                _hitted = true;
            _noHitTimer = _noHitTime;
        }

        protected virtual void CheckHitTimer()
        {
            if (_hitted)
            {
                if (_noHitTimer <= 0 && _currentHealth > 0)
                {
                    if (HasLifeBar())
                        _lifeBarCG.alpha = 0;
                    _currentHealth = _maxHealth;
                    _hitted = false;
                }

                if (_noHitTimer > 0 && _currentHealth > 0)
                {
                    _noHitTimer -= Time.deltaTime;
                }
            }
        }

        private bool HasLifeBar()
        {
            return _lifeBar != null && _lifeBarCG != null;
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
