using DG.Tweening;
using System.Collections;
using UnityEngine;
using InterOrbital.Player;


namespace InterOrbital.Item
{
    public class ItemObject : MonoBehaviour
    {
        public Transform playerT;

        private Rigidbody2D _rigidbody;
        private float _minSpeed = 5f;
        private float _maxSpeed = 30f;
        private float accelerationTime = 0.5f;
        private float distanceToBeCollected = 0.5f;
        private SpriteRenderer _spriteRenderer;
       
        private Sequence _sequenceIdleItem;

        public bool DropingItem { get; private set; } = true;
       [HideInInspector] public ItemScriptableObject itemSo;

        public int amount;

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
                _sequenceIdleItem.Kill();
                Vector2 direction = (playerT.position - transform.position).normalized;
                float newVelocity = Mathf.Max(_minSpeed,_rigidbody.velocity.magnitude);
    
                if (_rigidbody.velocity.magnitude < _maxSpeed)   // Si la velocidad actual es menor que la velocidad máxima
                {
                    float t = Time.fixedDeltaTime / accelerationTime;  // Tiempo para acelerar
                    newVelocity  = Mathf.Lerp(newVelocity, _maxSpeed, t);  // Lerp de la velocidad actual a la velocidad máxima

                }
                //Debug.Log(newVelocity);
                _rigidbody.velocity = direction * newVelocity;   // Establecer la velocidad del Rigidbody2D
            }
            
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
        }

        public void DropItem(Vector3 directionDrop)
        {
            transform.DOMove(directionDrop, 0.5f).SetEase(Ease.OutQuint).Play().OnComplete(() =>
            {
                DropingItem = false;
                _sequenceIdleItem = DOTween.Sequence();
                _sequenceIdleItem.Append(transform.DOMoveY(transform.position.y + 0.1f, 1).SetEase(Ease.InOutSine).SetLoops(int.MaxValue, LoopType.Yoyo)).Play();
            });
        }

        public void ObtainComponents()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }



        
    }
}