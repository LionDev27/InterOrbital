using UnityEngine;

namespace InterOrbital.Combat
{
    public class EnemyDamageable : Damageable
    {
        protected override void Death()
        {
            Destroy(gameObject);
        }
    }
}
