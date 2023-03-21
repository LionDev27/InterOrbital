using UnityEngine;

namespace InterOrbital.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        [HideInInspector] public int damage;
        //Tag del objeto que realiza el ataque, para evitar que se haga daño a sí mismo.
        [HideInInspector] public string attackerTag;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckCollision(other);
        }

        private void CheckCollision(Collider2D other)
        {
            if (other.CompareTag(attackerTag))
            {
                Debug.Log("No puede atacarse a si mismo.");
            }
            //Si choca con otra bala
            else if (other.TryGetComponent(out DamageDealer damageDealer))
            {
                BulletCollisionCheck(other, damageDealer);
            }
            else if (other.TryGetComponent(out Damageable damageable))
            {
                Debug.Log($"Atacando a {other.gameObject.name}");
                damageable.GetDamage(damage);
            }
        }

        private void BulletCollisionCheck(Collider2D other, DamageDealer damageDealer)
        {
            //Comprobamos si el tag es el mismo. Si lo es, salimos del metodo. Si no, destruimos las dos balas.
            if (damageDealer.attackerTag == attackerTag) return;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
