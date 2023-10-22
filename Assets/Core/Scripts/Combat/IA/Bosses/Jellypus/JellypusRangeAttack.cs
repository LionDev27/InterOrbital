using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusRangeAttack : BossAttack
    {
        [SerializeField] private BossAgent _agent;
        private BulletInstantiator[] _instantiators;

        protected override void Awake()
        {
            base.Awake();
            _instantiators = GetComponentsInChildren<BulletInstantiator>();
        }

        protected override void OnEnable()
        {
            foreach (var instantiator in _instantiators)
                instantiator.target = _agent.Target;
            base.OnEnable();
        }
    }
}