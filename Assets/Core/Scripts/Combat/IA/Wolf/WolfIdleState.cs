using System.Collections;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class WolfIdleState : EnemyStateBase
    {
        [SerializeField] private float _waitTime;
        [SerializeField] private float _speed;
        [SerializeField] private float _minMoveRadius, _maxMoveRadius;
        private WolfAgent _currentAgent;
        private bool _moving;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as WolfAgent;
        }

        public override void OnStateEnter()
        {
            StartCoroutine(MoveWait());
            _currentAgent.EnableNavigation(true);
            _currentAgent.NavMeshAgent.SetDestination(transform.position);
            _currentAgent.NavMeshAgent.speed = _speed;
        }

        public override void Execute()
        {
            _currentAgent.FlipSprite();
            if (_currentAgent.IsDetectingPlayer())
                _currentAgent.ChangeState(_currentAgent.States[1]);
            else if (_currentAgent.ArrivedDestination() && _moving)
            {
                _moving = false;
                StartCoroutine(MoveWait());
            }
        }

        private IEnumerator MoveWait()
        {
            yield return new WaitForSeconds(_waitTime);
            Move();
        }

        private void Move()
        {
            _moving = true;
            _currentAgent.NavMeshAgent.SetDestination(GetNewRandomPos());
        }

        private Vector2 GetNewRandomPos()
        {
            var currentPos = transform.position;
            return new Vector2(GetRandomCoordinate() + currentPos.x, GetRandomCoordinate() + currentPos.y);
        }

        private float GetRandomCoordinate()
        {
            var negativeRandomNum = GetRandomNum(-_maxMoveRadius, -_minMoveRadius);
            var randomNum = GetRandomNum(_minMoveRadius, _maxMoveRadius);
            return Random.Range(0, 100f) > 50 ? negativeRandomNum : randomNum;
        }

        private float GetRandomNum(float min, float max)
        {
            return Random.Range(min, max);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _minMoveRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxMoveRadius);
        }
    }
}