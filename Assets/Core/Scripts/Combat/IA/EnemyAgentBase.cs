using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] protected List<EnemyStateBase> _states;
        private EnemyStateBase _currentState;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private float _speed;

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
            _speed = _navMeshAgent.speed;
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

        public void EnableNavigation(bool value)
        {
            _navMeshAgent.velocity = value ? Vector3.one : Vector3.zero;
            _navMeshAgent.isStopped = !value;
        }
    }
}