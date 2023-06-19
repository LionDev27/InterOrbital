using System;
using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAttack : PlayerComponents
    {
        public Transform attackPoint;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _gunSpriteObj;

        [Header("Weapon Upgrades")]
        [SerializeField] private float _attackCooldown;
        private float _timer;

        [HideInInspector] public bool canAttack = true;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnAttack += Attack;
        }

        public void ChangeBullet(GameObject bullet) 
        {
            _bulletPrefab = bullet;
            if (!_bulletPrefab.CompareTag("EmptyBullet"))
            {
                _attackCooldown = bullet.GetComponent<BaseBulletController>().GetBulletAttackCooldown();
            }
        }

        private void Update()
        {
            _gunSpriteObj.SetActive(canAttack);
            _timer -= Time.deltaTime;
        }

        private void Attack()
        {
            if (!canAttack || !CooldownEnded()) return;
            _timer = _attackCooldown;
            //Creación de la bala [TODO: MODIFICAR A OBJECT POOLING]
            if (!_bulletPrefab.CompareTag("EmptyBullet")){
                var tempBullet = Instantiate(_bulletPrefab, attackPoint.position, Quaternion.identity);
                var bulletController = tempBullet.GetComponent<BaseBulletController>();
                var bulletMoveDir = attackPoint.position - transform.position;
                bulletController.SetupBullet(gameObject.tag, bulletMoveDir, transform.position);
                AttackEffects();
            }
        }
        
        private void AttackEffects()
        {
            CameraShake.Instance.Shake(2, 0.3f);
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
        }
    }
}