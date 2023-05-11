using UnityEngine;

namespace InterOrbital.Combat.Bullets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBulletController : MonoBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField] private float _speed;
        private Vector2 _moveDir;
        private Vector2 _parentPos;
        private float _range;
        private Rigidbody2D _rigidbody2D;
        private DamageDealer _damageDealer;

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _damageDealer = GetComponent<DamageDealer>();
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
            Move();
        }

        private void Move()
        {
            _rigidbody2D.velocity = _speed * 100f * Time.deltaTime * _moveDir;
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

        /// <summary>
        /// Metodo necesario para definir las variables del ataque del arma.
        /// </summary>
        /// <param name="damage">Daño que hace la bala.</param>
        /// <param name="bulletRange">Rango de la bala. Si lo supera, desaparecerá.</param>
        /// <param name="attackerTag">Tag del objeto que ha lanzado la bala.</param>
        /// <param name="moveVector">Vector dirección de movimiento de la bala.</param>
        /// <param name="parentPos">Posición del objeto que lanza la bala. Se usará para comprobar el rango.</param>
        public void SetupBullet(int damage, float bulletRange, string attackerTag, Vector2 moveVector, Vector2 parentPos)
        {
            _damageDealer.damage = damage;
            _range = bulletRange;
            _damageDealer.attackerTag = attackerTag;
            _moveDir = moveVector;
            _parentPos = parentPos;
        }
    }
}
