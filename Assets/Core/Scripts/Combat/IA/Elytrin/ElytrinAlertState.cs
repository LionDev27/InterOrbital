using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAlertState : EnemyStateBase
    {
        private ElytrinAgent _currentAgent;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.EnableNavigation(true);
            _currentAgent.Animator.SetBool("Idle", false);
            _currentAgent.Animator.SetBool("PlayerLost", false);
        }

        public override void Execute()
        {
            if (GetCurrentClipName() != "ElytrinRun") return;
            if (_currentAgent.Animator.GetBool("Running") == false) _currentAgent.Animator.SetBool("Running", true);
            if (_currentAgent.IsDetectingPlayer())
            {
                if (Vector3.Distance(transform.position, _currentAgent.Target.position) <= _currentAgent.AttackRange)
                    _currentAgent.ChangeState(_currentAgent.States[3]);
                if (_currentAgent.NavMeshAgent.destination != _currentAgent.Target.position)
                    _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);

                if (Mathf.Sign(_currentAgent.Target.transform.position.x - transform.position.x) > 0)
                {
                    _currentAgent.SpriteFlipper.FlipX(1);
                }
                else if (Mathf.Sign(_currentAgent.Target.transform.position.x - transform.position.x) < 0)
                {
                    _currentAgent.SpriteFlipper.FlipX(0);
                }
            }
            else if (_currentAgent.ArrivedDestination())
                _currentAgent.ChangeState(_currentAgent.States[2]);
        }

        private string GetCurrentClipName()
        {
            return _currentAgent.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }
    }
}