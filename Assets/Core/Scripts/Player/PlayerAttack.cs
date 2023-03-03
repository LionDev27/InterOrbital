using InterOrbital.Combat;
using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAttack : PlayerComponents
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Weapon Upgrades")]
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackRangeMultiplier;
        [SerializeField] private float _attackCooldown;
        private float _timer;

        [HideInInspector] public bool canAttack = true;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnAttack += Attack;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        private void Attack()
        {
            if (!canAttack || !CooldownEnded()) return;
            _timer = _attackCooldown;
            //Creación de la bala [TODO: MODIFICAR A OBJECT POOLING]
            var tempBullet = Instantiate(_bulletPrefab, _attackPoint.position, Quaternion.identity);
            var bulletController = tempBullet.GetComponent<BaseBulletController>();
            var bulletMoveDir = _attackPoint.position - transform.position;
            bulletController.SetupBullet(_attackDamage, _attackRangeMultiplier, gameObject.tag, bulletMoveDir);
        }

        private bool CooldownEnded()
        {
            return _timer <= 0;
        }

        private void OnDestroy()
        {
            InputHandler.OnAttack -= Attack;
        }
    }
}