using System;
using System.Collections.Generic;
using InterOrbital.Combat.Bullets;
using InterOrbital.Item;
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
        private List<BulletCooldown> _cooldowns = new();
        private bool _changingCooldowns;
        private int _currentCooldownIndex;

        [HideInInspector] public bool canAttack = true;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnAttack += Attack;
        }

        private void Update()
        {
            if (InputHandler != null && InputHandler.enabled && InputHandler.CurrentActionMap() == "Player")
                CursorController.Instance.SetAlpha(!HasBullet());
            _gunSpriteObj.SetActive(canAttack);
            if (_changingCooldowns) return;
            CheckCooldowns();
        }

        private void CheckCooldowns()
        {
            foreach (var cd in _cooldowns)
                cd.Run();
        }

        public void SetupCooldowns(List<ItemBulletScriptableObject> bulletsData)
        {
            _changingCooldowns = true;
            _cooldowns.Clear();
            for (int i = 0; i < bulletsData.Count; i++)
                _cooldowns.Add(new BulletCooldown());
            _changingCooldowns = false;
        }
        
        public void ChangeBullet(GameObject bullet, AudioClip bulletSFX, int index) 
        {
            _bulletPrefab = bullet;
            _bulletSFX = bulletSFX;
            if (HasBullet())
            {
                _currentCooldownIndex = index;
                var newCooldown = bullet.GetComponent<BaseBulletController>().GetBulletAttackCooldown();
                //Si el cooldown que estamos poniendo es distinto, setupeamos.
                if (Math.Abs(newCooldown - _cooldowns[index].Cooldown) > 0.005f)
                    _cooldowns[index].Setup(newCooldown);
            }
        }

        private void Attack()
        {
            if (!canAttack || !CooldownEnded()) return;
            _cooldowns[_currentCooldownIndex].Reset();
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
                AudioManager.Instance.PlaySFX(_bulletSFX);
        }
        
        private void AttackEffects()
        {
            CameraShake.Instance.Shake(0.5f);
        }
        
        private bool CooldownEnded()
        {
            return _cooldowns[_currentCooldownIndex].Ended();
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