using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfLostState : EnemyStateBase
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lostTime;
        [SerializeField] private float _moveRate;
        private WolfAgent _currentAgent;
        private float _timer;
        
        private const string LostBoolAnim = "Lost";

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool(LostBoolAnim, true);
            _currentAgent.EnableNavigation(true);
            _currentAgent.NavMeshAgent.speed = _speed;
            StartCoroutine(MoveEveryRatePassed());
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (RunningTimer())
                _timer -= Time.deltaTime;
            if (!_currentAgent.IsDetectingPlayer()) return;
            StopAllCoroutines();
            _currentAgent.ChangeState(_currentAgent.States[1]);
        }

        private IEnumerator MoveEveryRatePassed()
        {
            RunTimer();
            while (RunningTimer())
            {
                yield return new WaitForSeconds(_moveRate);
                _currentAgent.NavMeshAgent.SetDestination(_currentAgent.Target.position);
            }
            _currentAgent.Animator.SetBool(LostBoolAnim, false);
            _currentAgent.ChangeState(_currentAgent.States[0]);
        }

        private void RunTimer()
        {
            _timer = _lostTime;
        }

        private bool RunningTimer()
        {
            return _timer > 0;
        }
    }
}