using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] protected List<EnemyStateBase> _states;
        protected EnemyStateBase _currentState;
        protected Animator _animator;
        protected NavMeshAgent _navMeshAgent;

        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public List<EnemyStateBase> States => _states;

        protected virtual void Awake()
        {
            if(_states.Count <= 0) return;
            foreach (var state in _states)
            {
                state.Setup(this);
            }
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
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

        public bool ArrivedDestination()
        {
            return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                   (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
        }
    }
}