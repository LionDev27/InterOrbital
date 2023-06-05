using System;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] protected List<EnemyStateBase> _states;
        protected EnemyStateBase _currentState;
        protected Animator _animator;

        public List<EnemyStateBase> States => _states;

        protected virtual void Awake()
        {
            if(_states.Count <= 0) return;
            foreach (var state in _states)
            {
                state.Setup(this);
            }
            _animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Update()
        {
            if (_currentState)
            {
                _currentState.Execute();
            }
        }

        public virtual void ChangeState(EnemyStateBase newState)
        {
            _currentState = newState;
            _currentState.OnStateEnter();
        }
    }
}