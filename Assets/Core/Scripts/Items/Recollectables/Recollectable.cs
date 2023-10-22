using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InterOrbital.Item;
using InterOrbital.Others;
using InterOrbital.Recollectables.Spawner;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InterOrbital.Recollectables
{
    public class Recollectable : MonoBehaviour
    {
        [SerializeField] private Vector2 _dimensions;
        [SerializeField] private RecollectableScriptableObject _scriptableObject;
        [SerializeField] private HitShaderController _hitShaderController;
        [Tooltip("How many times can the player recollect from this recollectable.")]
        [SerializeField] private int _health;
        [SerializeField] private GameObject _dropItemPrefab;
        [SerializeField] private float _dropForce = 3f;
        [SerializeField] private Image _lifeBar;
        [SerializeField] private CanvasGroup _lifeBarCG;

        private ResourcesSpawner _spawner;

        private float _noHitTime = 60f;
        private float _noHitTimer;
        private bool _hitted;
        private int _dropCounter;
        
        private int _currentHealth;
        private List<ItemScriptableObject> _dropItems => _scriptableObject.recollectableConfig.dropItems;
        private List<int> _dropRates => _scriptableObject.recollectableConfig.dropRates;

        protected virtual void Awake()
        {
            _currentHealth = _health;
        }

        protected virtual void Update()
        {
            CheckHitTimer();
        }

        private void DropItem(ItemObject item, Vector2 originPos)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 0f);
            while (x == 0 && y == 0)
            {
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 0f);
            }
            Vector2 dropDir = new Vector2(x, y).normalized;

            item.DropItem((dropDir * _dropForce + originPos));
            _dropCounter++;
        }

        private ItemScriptableObject GetRandomItem()
        {
            int objectIndex = 0;
            if (_dropItems.Count > 1)
            {
                int probability = Random.Range(0, 101);
                for (int i = 0; i < _dropRates.Count; i++)
                {
                    if (probability <= _dropRates[i])
                    {
                        objectIndex = i;
                    }
                }
            }
            return _dropItems[objectIndex];
        }

        private void DestroyRecollectable()
        {
            if(_spawner != null)
            {
                _spawner.ResourceObtained();
            }
            Destroy(gameObject);
        }
        
        public virtual void Recollect()
        {
            if (_dropCounter + _currentHealth == _health)
            {
                if (_dropItems.Count <= 0) return;

                Vector2 dropItemOriginPos = new Vector2(transform.position.x + (_dimensions.x / 2), transform.position.y);
                GameObject tempDroppingObject = Instantiate(_dropItemPrefab, dropItemOriginPos, Quaternion.identity);
                ItemObject tempDroppingItem = tempDroppingObject.GetComponent<ItemObject>();

                if (tempDroppingItem == null) return;
                tempDroppingItem.ObtainComponents();
                tempDroppingItem.SetItem(GetRandomItem());


                DropItem(tempDroppingItem, dropItemOriginPos);
            }
            
            _currentHealth--;
            HitReceived();
            UpdateLifeBar();
            StartCoroutine(HitAnimation());
        }

        private IEnumerator HitAnimation()
        {
            _hitShaderController.Hit(1);
            yield return new WaitForSeconds(0.1f);
            if (_currentHealth <= 0)
                DestroyRecollectable();
            _hitShaderController.Hit(0);
        }

        private void UpdateLifeBar()
        {
            if(_currentHealth != _health)
            {
                _lifeBarCG.alpha = 1;
            }
            if(_currentHealth <= 0)
            {
                _lifeBarCG.DOFade(0f, 0.95f);
            }
            float lifeAmount = _currentHealth / (float)_health;
            _lifeBar.fillAmount = lifeAmount;
        }

        private void HitReceived()
        {
            if (!_hitted)
            {
                _hitted = true;
            }
            _noHitTimer = _noHitTime;
        }

        private void CheckHitTimer()
        {
            if (_hitted)
            {
                if(_noHitTimer <= 0 && _currentHealth > 0)
                {
                    _lifeBarCG.alpha = 0;
                    _currentHealth = _health;
                    _hitted = false;
                }

                if(_noHitTimer > 0 && _currentHealth > 0)
                {
                    _noHitTimer -= Time.deltaTime;
                }
            }
        }

        public Vector2 GetDimensions()
        {
            return _dimensions;
        }

        public void SetSpawnerRef(ResourcesSpawner spawner)
        {
            _spawner = spawner;
        }
    }
}
