using System;
using InterOrbital.Combat.Bullets;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAttack : PlayerComponents
    {
        public Transform attackPoint;
        [SerializeField] private GameObject _gunSpriteObj;
        private GameObject _bulletPrefab;
        private AudioClip _bulletSFX;

        [Header("Weapon Upgrades")]
        [SerializeField] private float _attackCooldown;
        private float _timer;

        [HideInInspector] public bool canAttack = true;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnAttack += Attack;
        }

        public void ChangeBullet(GameObject bullet,AudioClip bulletSFX) 
        {
            _bulletPrefab = bullet;
            _bulletSFX = bulletSFX;
            if (HasBullet())
                _attackCooldown = bullet.GetComponent<BaseBulletController>().GetBulletAttackCooldown();
        }

        private void Update()
        {
            if (InputHandler != null && InputHandler.enabled && InputHandler.CurrentActionMap() == "Player")
                CursorController.Instance.SetAlpha(!HasBullet());
            _gunSpriteObj.SetActive(canAttack);
            _timer -= Time.deltaTime;
        }

        private void Attack()
        {
            if (!canAttack || !CooldownEnded()) return;
            _timer = _attackCooldown;
            //Creación de la bala [TODO: MODIFICAR A OBJECT POOLING]
            if (HasBullet()){ 
                var tempBullet = Instantiate(_bulletPrefab, attackPoint.position, Quaternion.identity);
                var bulletController = tempBullet.GetComponent<BaseBulletController>();
                var bulletMoveDir = attackPoint.position - transform.position;
                bulletController.SetupBullet(gameObject.tag, bulletMoveDir, transform.position);
                AttackEffects();
                AudioManager.Instance.PlaySFX(_bulletSFX);
                BulletSelector.Instance.SubstractBullet();
            }
            else
            {
                AudioManager.Instance.PlaySFX(_bulletSFX);
            }
        }
        
        private void AttackEffects()
        {
            CameraShake.Instance.Shake(0.5f);
        }
        
        private bool CooldownEnded()
        {
            return _timer <= 0;
        }

        private bool HasBullet()
        {
            return !_bulletPrefab.CompareTag("EmptyBullet");
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