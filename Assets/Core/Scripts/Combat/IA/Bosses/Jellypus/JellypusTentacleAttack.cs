using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusTentacleAttack : BossAttack
    {
        public BossAgent Agent => _agent;
        
        [SerializeField] private BossAgent _agent;
        private JellypusTentaclePositioner _positioner;

        protected override void Awake()
        {
            base.Awake();
            _positioner = GetComponentInChildren<JellypusTentaclePositioner>();
        }

        protected override void OnEnable()
        {
            _positioner.target = _agent.Target;
            _positioner.anim = _attackAnimation;
            float timer = (_positioner.waitPerTentacle * (_positioner.TentaclesCount - 1)) + _attackAnimation.length;
            Invoke(nameof(DeactivateAttack), timer);
        }
    }
}