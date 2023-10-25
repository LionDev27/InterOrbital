using InterOrbital.Mission;

namespace InterOrbital.Combat.IA
{
    public class ElytrinDamageable : EnemyDamageable
    {
        private MissionCreator _missionCreator;

        protected override void Awake()
        {
            base.Awake();
            _missionCreator = FindObjectOfType<MissionCreator>();
        }

        protected override void Death()
        {
            _missionCreator.UpdateMission(1, null);
            base.Death();
        }
    }
}