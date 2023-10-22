using InterOrbital.Combat.Bullets;
using UnityEngine;

namespace InterOrbital.Utils
{
    public class BulletInstantiator : MonoBehaviour
    {
        public Transform target;
        public Vector2 direction;
        [SerializeField] private BaseBulletController _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;

        public void Instantiate()
        {
            Vector2 dir = target == null ? direction : (target.position - _bulletSpawnPoint.position).normalized;
            BaseBulletController bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            bullet.SetupBullet(tag, dir, _bulletSpawnPoint.position);
        }
    }
}