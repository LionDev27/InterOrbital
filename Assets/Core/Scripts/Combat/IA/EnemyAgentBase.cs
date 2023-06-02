using System;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] private List<EnemyStateBase> _states;
        private EnemyStateBase _currentState;

        protected virtual void Awake()
        {
            foreach (var state in _states)
            {
                state.Setup(this);
            }
        }

        protected virtual void Update()
        {
            _currentState.Execute();
        }

        public virtual void ChangeState(EnemyStateBase newState)
        {
            _currentState = newState;
            _currentState.OnStateEnter();
        }
    }
}