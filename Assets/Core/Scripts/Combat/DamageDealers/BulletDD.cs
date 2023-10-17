using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class BulletDD : DamageDealer
    {
        protected override void AttackDamageableTarget(Collider2D other, Damageable damageable)
        {
            Debug.Log("Attacking. tag: " + attackerTag);
            base.AttackDamageableTarget(other, damageable);
            //Destruimos la bala después de atacar.
            Destroy(gameObject);
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
