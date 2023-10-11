namespace InterOrbital.Combat.IA
{
    public class JellypusIdle : JellypusStateBase
    {
        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool("Idle", true);
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
