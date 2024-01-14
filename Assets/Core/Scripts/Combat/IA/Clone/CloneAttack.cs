using System.Collections.Generic;
using InterOrbital.Combat.Bullets;
using InterOrbital.Item;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneAttack : MonoBehaviour
    {
        [SerializeField] private List<ItemBulletScriptableObject> _bullets;
        [SerializeField] private float _shootRate;
        private CloneAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<CloneAgent>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Attack), _shootRate, _shootRate);
        }

        public void Attack()
        {
            if (!_agent.CanAttack()) return;
            var randomIndex = Random.Range(0, _bullets.Count - 1);
            var currentBullet = _bullets[randomIndex].bulletPrefab;
            var tempBullet = Instantiate(currentBullet, _agent.AttackPoint.position, Quaternion.identity);
            var bulletController = tempBullet.GetComponent<BaseBulletController>();
            var bulletMoveDir = _agent.AttackPoint.position - transform.position;
            bulletController.SetupBullet(gameObject.tag, bulletMoveDir, transform.position);
            AudioManager.Instance.PlaySFX(_bullets[randomIndex].shotSFX);
        }
    }
}