namespace InterOrbital.Combat.IA
{
    public class TreemicAgent : EnemyAgentBase
    {
        public override void HitEnemy()
        {
            base.HitEnemy();
            ChangeState(_states[0]);
        }
    }
}
