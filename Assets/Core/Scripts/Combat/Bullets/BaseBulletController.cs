using UnityEngine;

namespace InterOrbital.Combat.Bullets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBulletController : MonoBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField] private float _speed;
        [Range(0f, 20f)]
        [SerializeField] protected float _range;
        [Range(0f, 10f)]
        [SerializeField] protected float _cooldown;
        protected Vector2 _moveDir;
        private Vector2 _parentPos;
        private Rigidbody2D _rigidbody2D;
        protected DamageDealer _damageDealer;
        protected bool _canMove;

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _damageDealer = GetComponent<DamageDealer>();
            _canMove = true;
        }

        protected void Start()
        {
            Rotate();
        }

        protected void Update()
        {
            CheckDistanceToParent();
        }

        protected virtual void FixedUpdate()
        {
            if (_canMove)
            {
                Move();
            }
        }

        private void Move()
        {
            _rigidbody2D.velocity = _speed * 100f * Time.deltaTime * _moveDir;
        }

        protected void StopMove()
        {
            _canMove = false;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, _moveDir);
        }

        private void CheckDistanceToParent()
        {
            if (_range <= Vector2.Distance(transform.position, _parentPos))
                Destroy(gameObject);
        }

        public float GetBulletAttackCooldown()
        {
            return _cooldown;
        }

        /// <summary>
        /// Metodo necesario para definir las variables del ataque del arma.
        /// </summary>
        /// <param name="attackerTag">Tag del objeto que ha lanzado la bala.</param>
        /// <param name="moveVector">Vector dirección de movimiento de la bala.</param>
        /// <param name="parentPos">Posición del objeto que lanza la bala. Se usará para comprobar el rango.</param>
        public void SetupBullet(string attackerTag, Vector2 moveVector, Vector2 parentPos)
        {
            _damageDealer.attackerTag = attackerTag;
            _moveDir = moveVector;
            _parentPos = parentPos;
        }
    }
}
