namespace InterOrbital.Combat.IA
{
    public class JellypusIdle : JellypusStateBase
    {
        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool("Idle", true);
            _currentAgent.Damageable.DeactivateBoss();
        }

        public override void Execute()
        {
            if (_currentAgent.IsDetectingPlayer())
            {
                _currentAgent.ChangeState(_currentAgent.States[1]);
            }
        }
    }
}
