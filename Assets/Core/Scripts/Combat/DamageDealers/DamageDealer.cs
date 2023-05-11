using System;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        public int damage;
        //Tag del objeto que realiza el ataque, para evitar que se haga daño a sí mismo.
        [HideInInspector] public string attackerTag;

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckCollision(other);
        }

        protected virtual void CheckCollision(Collider2D other)
        {
            //Si este DamageDealer tiene asignado un attackerTag, lo comprueba. Si no, comprueba su propio tag.
            if ((attackerTag != "" && other.CompareTag(attackerTag)) || other.CompareTag(gameObject.tag))
            {
                Debug.Log("No puede atacarse a si mismo ni a otro objeto con el mismo tag.");
            }
            else if (other.CompareTag("Player") && other is BoxCollider2D)
            {
                Debug.Log("No puede atacarse al BoxCollider del jugador.");
            }
            else if (other.TryGetComponent(out Damageable damageable))
            {
                AttackDamageableTarget(other, damageable);
            }
        }

        protected virtual void AttackDamageableTarget(Collider2D other ,Damageable damageable)
        {
            Debug.Log($"Atacando a {other.gameObject.name}");
            damageable.GetDamage(damage);
        }
    }
}
