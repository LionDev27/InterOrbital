using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAlertState : EnemyStateBase
    {
        [SerializeField] private AnimationClip _wakeUpAnimation;
        private ElytrinAgent _currentAgent;
        private bool _canRun;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            _canRun = false;
            _currentAgent.EnableNavigation(true);
            if (_currentAgent.Animator.GetBool("Idle"))
            {
                _currentAgent.Animator.SetBool("Idle", false);
                StartCoroutine(EnableRunTimer());
            }
            else
                _canRun = true;
            _currentAgent.Animator.SetBool("PlayerLost", false);
        }

        public override void Execute()
        {
            if (!_canRun) return;
            if (_currentAgent.IsDetectingPlayer())
            {
                if (Vector3.Distance(transform.position, _currentAgent.Target.position) <= _currentAgent.AttackRange)
                {
                    _currentAgent.ChangeState(_currentAgent.States[3]);
                    return;
                }
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
        
        private IEnumerator EnableRunTimer()
        {
            yield return new WaitForSeconds(0.2f);
            _currentAgent.Animator.SetBool("Running", true);
            yield return new WaitForSeconds(_wakeUpAnimation.length - 0.2f);
            _canRun = true;

        }
    }
}