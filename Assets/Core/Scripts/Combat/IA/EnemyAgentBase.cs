using InterOrbital.Combat.Spawner;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] protected List<EnemyStateBase> _states;
        [SerializeField] private float _hitAnimationTime;
        protected EnemyStateBase _currentState;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private float _hitTimer;
        private EnemySpawner _enemySpawner;


        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public List<EnemyStateBase> States => _states;

        protected virtual void Awake()
        {
            if (_states.Count <= 0) return;
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
            _hitTimer -= Time.deltaTime;
            if (_hitTimer > 0) return;
            if (_animator.GetBool("Hit"))
            {
                _animator.SetBool("Hit", false);
                EnableNavigation(true);
            }
            if (_currentState)
                _currentState.Execute();
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

        public void HitEnemy()
        {
            _animator.SetBool("Hit", true);
            _hitTimer = _hitAnimationTime;
            if (_navMeshAgent.isStopped) return;
            EnableNavigation(false);
        }

        public void SetEnemySpawner(EnemySpawner spawner)
        {
            if(spawner != null && _enemySpawner == null)
            {
                _enemySpawner = spawner;
            }
        }

        public void Death()
        {
            _enemySpawner.EnemyDead();
        }
    }
}