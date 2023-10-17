using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] private AnimationClip _attackAnimation;
        private BossAttacks _bossAttacks;

        protected virtual void Awake()
        {
            _bossAttacks = GetComponentInParent<BossAttacks>();
        }

        protected virtual void OnEnable()
        {
            Invoke(nameof(DeactivateAttack), _attackAnimation.length);
        }

        private void DeactivateAttack()
        {
            if (_bossAttacks != null)
                _bossAttacks.EndAttack();
            gameObject.SetActive(false);
        }
    }
}