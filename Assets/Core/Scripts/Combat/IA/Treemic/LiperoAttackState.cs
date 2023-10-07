using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class LiperoAttackState : TreemicAttackState
    {
        [SerializeField] private float _attackAngle;
        
        public override void Attack()
        {
            var currentAngle = 0f;

            for (int i = 0; i < 8; i++)
            {
                var tempBullet = Instantiate(_bulletPrefab, _bulletTransformSpawn.position, Quaternion.identity);
                var bulletController = tempBullet.GetComponent<BaseBulletController>();
                bulletController.SetupBullet(gameObject.tag, GetAngleDir(currentAngle).normalized, transform.position);
                currentAngle += _attackAngle;
            }
        }

        private Vector2 GetAngleDir(float angle)
        {
            if (angle == 0f)
                return TargetDir();
            return (Quaternion.AngleAxis(angle, Vector3.forward) * TargetDir());
        }
    }
}