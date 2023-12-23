using DG.Tweening;
using System.Collections;
using UnityEngine;
using InterOrbital.Player;
using InterOrbital.Utils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.Scripting;

namespace InterOrbital.Item
{
    [Serializable]
    public class ItemObject : MonoBehaviour
    {
        public bool DropingItem { get; private set; } = true;
        [HideInInspector] public ItemScriptableObject itemSo;
        public Transform playerT;
        public int amount;

        private float _minSpeed = 5f;
        private float _maxSpeed = 30f;
        private float accelerationTime = 0.5f;
        private float distanceToBeCollected = 0.5f;

        [SerializeField] private float _timeToDespawn;
        private float _currentTimeToDespawn;
        private bool _blink;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
       
        private Sequence _sequenceIdleItem;


     public ItemObject() { }

        public ItemObject(bool dropingItem, ItemScriptableObject itemSo, Transform playerT, int amount, float minSpeed, float maxSpeed, float accelerationTime, float distanceToBeCollected, float timeToDespawn, 
            float currentTimeToDespawn, bool blink, Rigidbody2D rigidbody, SpriteRenderer spriteRenderer, Animator animator, Sequence sequenceIdleItem)
        {
            DropingItem = dropingItem;
            this.itemSo = itemSo;
            this.playerT = playerT;
            this.amount = amount;
            this._minSpeed = minSpeed;
            this._maxSpeed = maxSpeed;
            this.accelerationTime = accelerationTime;
            this.distanceToBeCollected = distanceToBeCollected;
            this._timeToDespawn = timeToDespawn;
            this._currentTimeToDespawn = currentTimeToDespawn;
            this._blink = blink;
            this._rigidbody = rigidbody;
            this._spriteRenderer = spriteRenderer;
            this._animator = animator;
            this._sequenceIdleItem = sequenceIdleItem;
        }

        public ItemObject Clone()
        {
            return new ItemObject(this.DropingItem, this.itemSo, this.playerT, this.amount, this._minSpeed, this._maxSpeed, this.accelerationTime, this.distanceToBeCollected, this._timeToDespawn,
                 this._currentTimeToDespawn, this._blink, this._rigidbody, this._spriteRenderer, this._animator, this._sequenceIdleItem);
        }

        private void Start()
        {
            StartCoroutine(Despawn());
        }
        private void Update()
        { 
             PlayerReached();
        }

        private void FixedUpdate()
        {
             MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            if (playerT)
            {
                StopCoroutine(Despawn());
                _blink = false;
                if(_spriteRenderer.color.a != 1f)
                {
                    _spriteRenderer.ChangueSpriteAlphaColor(1f);
                }
                _sequenceIdleItem.Kill();
                Vector2 direction = (playerT.position - transform.position).normalized;
                float newVelocity = Mathf.Max(_minSpeed,_rigidbody.velocity.magnitude);
    
                if (_rigidbody.velocity.magnitude < _maxSpeed)   // Si la velocidad actual es menor que la velocidad máxima
                {
                    float t = Time.fixedDeltaTime / accelerationTime;  // Tiempo para acelerar
                    newVelocity  = Mathf.Lerp(newVelocity, _maxSpeed, t);  // Lerp de la velocidad actual a la velocidad máxima

                }
                _rigidbody.velocity = direction * newVelocity;   // Establecer la velocidad del Rigidbody2D
            }
            
        }

        private IEnumerator Despawn()
        {
            _currentTimeToDespawn = _timeToDespawn;
            while (_currentTimeToDespawn >= 0)
            {
                _currentTimeToDespawn -= Time.deltaTime;
                if(_currentTimeToDespawn <= _timeToDespawn/4 && !_blink)
                {
                    _blink = true;
                    StartCoroutine(Blink());
                }
                yield return null;
            }

            _sequenceIdleItem.Kill();
            Destroy(gameObject);
        }

        private IEnumerator Blink()
        {
            while (_blink)
            {
                float tiempoEspera = 1 / _timeToDespawn * _currentTimeToDespawn;
                _spriteRenderer.ChangueSpriteAlphaColor(_spriteRenderer.color.a == 1 ? 0.3f : 1f);
                yield return new WaitForSeconds(tiempoEspera);
            }
            _spriteRenderer.ChangueSpriteAlphaColor(1f);
        }


        private void PlayerReached()
        {
            if (playerT)
            {
                if (Vector3.Distance(playerT.position, transform.position) < distanceToBeCollected)
                {
                    if (!PlayerComponents.Instance.Inventory.IsInventoryFull())
                    {
                        PlayerComponents.Instance.Inventory.AddItem(this);
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void SetItem(ItemScriptableObject item)
        {
            itemSo = item;
            _spriteRenderer.sprite = itemSo.itemSprite;
            if (item.animator != null)
                _animator.runtimeAnimatorController = item.animator;
            else
                _animator.enabled = false;
        }

        public void DropItem(Vector3 directionDrop)
        {
            transform.DOMove(directionDrop, 0.5f).SetEase(Ease.OutQuint).Play().OnComplete(() =>
            {
                DropingItem = false;
                _sequenceIdleItem = DOTween.Sequence();
                _sequenceIdleItem.Append(transform.DOMoveY(transform.position.y + 0.2f, 1).SetEase(Ease.InOutSine).SetLoops(int.MaxValue, LoopType.Yoyo)).Play();
            });
        }

        public void ObtainComponents()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
    }
}