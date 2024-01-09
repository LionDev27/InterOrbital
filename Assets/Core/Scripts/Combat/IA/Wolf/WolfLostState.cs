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
        private const string LostBlendSpeedAnim = "LostSpeed";

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
        }

        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool(LostBoolAnim, true);
            _currentAgent.EnableNavigation(true);
            _currentAgent.NavMeshAgent.speed = _speed;
            _currentAgent.Animator.SetFloat(LostBlendSpeedAnim, 0);
            StartCoroutine(MoveEveryRatePassed());
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (RunningTimer())
                _timer -= Time.deltaTime;
            if (!_currentAgent.IsDetectingPlayer()) return;
            _currentAgent.Animator.SetBool(LostBoolAnim, false);
            StopAllCoroutines();
            _currentAgent.ChangeState(_currentAgent.States[1]);
        }

        private IEnumerator MoveEveryRatePassed()
        {
            RunTimer();
            while (RunningTimer())
            {
                yield return new WaitForSeconds(_moveRate);
                if (_currentAgent.Animator.GetFloat(LostBlendSpeedAnim) <= 0)
                    _currentAgent.Animator.SetFloat(LostBlendSpeedAnim, 1);
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