using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytrinAlertState : EnemyStateBase
    {
        [SerializeField] private float _timeToIdle = 5f;
        private ElytrinAgent _currentAgent;
        private float _timer;

        public override void Setup(EnemyAgentBase agent)
        {
            _currentAgent = agent as ElytrinAgent;
        }

        public override void OnStateEnter()
        {
            //Navmesh
            Debug.Log("Estado de alerta");
        }

        public override void Execute()
        {
            if (!_currentAgent.IsDetectingPlayer())
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _currentAgent.ChangeState(_currentAgent.States[0]);
                }
            }
            else if (_timer < _timeToIdle)
            {
                _timer = _timeToIdle;
            }
        }
    }
}
