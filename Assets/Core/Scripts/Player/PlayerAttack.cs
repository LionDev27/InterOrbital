using System;
using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAttack : PlayerComponents
    {
        public Transform attackPoint;
        public Transform bulletsSpawnPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _attackRange;

        [Header("Weapon Upgrades")]
        [SerializeField] private int _attackDamage;
        [Range(1f, 20f)]
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
            var tempBullet = Instantiate(_bulletPrefab, bulletsSpawnPoint.position, Quaternion.identity);
            var bulletController = tempBullet.GetComponent<BaseBulletController>();
            var bulletMoveDir = attackPoint.position - transform.position;
            bulletController.SetupBullet(_attackDamage, _attackRange * _attackRangeMultiplier, gameObject.tag, bulletMoveDir, transform.position);
        }

        private bool CooldownEnded()
        {
            return _timer <= 0;
        }

        private void OnDestroy()
        {
            InputHandler.OnAttack -= Attack;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange * _attackRangeMultiplier);
        }
    }
}