using UnityEngine;

namespace InterOrbital.Combat.Bullets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBulletController : MonoBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField] private float _speed;
        [Tooltip("Rango de la bala. Al superarlo, desaparecerá.")]
        [SerializeField] private float _range;
        private float _rangeMultiplier;
        private Vector2 _moveDir;
        private Rigidbody2D _rigidbody2D;
        private DamageDealer _damageDealer;

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _damageDealer = GetComponent<DamageDealer>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rigidbody2D.velocity = _speed * 100f * Time.deltaTime * _moveDir;
        }

        /// <summary>
        /// Metodo necesario para definir las variables del ataque del arma.
        /// </summary>
        /// <param name="damage">Daño que hace la bala.</param>
        /// <param name="rangeMultiplier">Multiplicador de rango de la bala. Si lo supera, desaparecerá.</param>
        /// <param name="attackerTag">Tag del objeto que ha lanzado la bala.</param>
        /// <param name="moveVector">Vector dirección de movimiento de la bala.</param>
        public void SetupBullet(int damage, float rangeMultiplier, string attackerTag, Vector2 moveVector)
        {
            _damageDealer.damage = damage;
            _rangeMultiplier = rangeMultiplier;
            _damageDealer.attackerTag = attackerTag;
            _moveDir = moveVector;
        }
    }
}
