using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfAlertState : EnemyStateBase
    {
        [SerializeField] private float _speed;
        private WolfAgent _currentAgent;
        
        private const string PlayerDetectedBoolAnim = "PlayerDetected";

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.EnableNavigation(true);
            _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
            _currentAgent.NavMeshAgent.speed = _speed;
            _currentAgent.Animator.SetBool(PlayerDetectedBoolAnim, true);
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (_currentAgent.IsDetectingPlayer())
            {
                if (_currentAgent.IsTargetInAttackRange() && _currentAgent.CanAttack)
                {
                    _currentAgent.Animator.SetBool(PlayerDetectedBoolAnim, false);
                    _currentAgent.ChangeState(_currentAgent.States[3]);
                    return;
                }
                MoveToTarget();
            }
            else if (_currentAgent.ArrivedDestination())
            {
                _currentAgent.Animator.SetBool(PlayerDetectedBoolAnim, false);
                _currentAgent.ChangeState(_currentAgent.States[2]);
            }
        }

        private void MoveToTarget()
        {
            if (_currentAgent.NavMeshAgent.destination != _currentAgent.Target.position)
                _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
        }
    }
}