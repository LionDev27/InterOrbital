using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusCloseAttack : BossAttack
    {
        [SerializeField] private BossAgent _agent;
        private JellypusTentacleRotator _rotator;

        protected override void Awake()
        {
            base.Awake();
            _rotator = GetComponentInChildren<JellypusTentacleRotator>();
        }

        protected override void OnEnable()
        {
            _rotator.target = _agent.Target;
            base.OnEnable();
            AudioManager.Instance.PlaySFX("BossTentacleAttack");
        }
    }
}