using InterOrbital.Combat.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class BulletDD : DamageDealer
    {
        private bool _destroyAfterHit;
        private bool _maintainDmg;
        [SerializeField] private GameObject _bulletFinalGlow;
        private bool _canDamage = true;

        private void Awake()
        {
            _destroyAfterHit = true;
        }

        protected override void AttackDamageableTarget(Collider2D other, Damageable damageable)
        {
            if (!_canDamage) return;

            if(!_maintainDmg)
                _canDamage = false;

            base.AttackDamageableTarget(other, damageable);
            //Destruimos la bala después de atacar.
            if (_destroyAfterHit)
            {
                Instantiate(_bulletFinalGlow, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        public void DontDestroyAfterHit()
        {
            _destroyAfterHit = false;
        }

        public void MaintainDmg()
        {
            _maintainDmg = true;
        }

        // CON ESTO HACEMOS QUE SI CHOCA UNA BALA CON UNA ENEMIGA, SE DESTRUYA.
        // protected override void CheckCollision(Collider2D other)
        // {
        //     base.CheckCollision(other);
        //     //Si choca con otra bala.
        //     if (other.TryGetComponent(out DamageDealer damageDealer) && other.CompareTag("Bullet"))
        //     {
        //         BulletCollisionCheck(other, damageDealer);
        //     }
        // }
        //
        // private void BulletCollisionCheck(Collider2D other, DamageDealer damageDealer)
        // {
        //     //Comprobamos si el tag es el mismo. Si lo es, salimos del metodo. Si no, destruimos las dos balas.
        //     if (damageDealer.attackerTag == attackerTag) return;
        //     Destroy(other.gameObject);
        //     Destroy(gameObject);
        // }
    }
}
