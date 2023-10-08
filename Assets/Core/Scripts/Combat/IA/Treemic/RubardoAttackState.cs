using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class RubardoAttackState : TreemicAttackState
    {
        public override void Attack()
        {
            var tempBullet = Instantiate(_bulletPrefab, _bulletTransformSpawn.position, Quaternion.identity);
            var bulletController = tempBullet.GetComponent<BaseBulletController>();
            bulletController.SetupBullet(gameObject.tag, TargetDir(), transform.position);
        }
    }
}