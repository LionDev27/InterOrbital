using InterOrbital.Combat.Spawner;
using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Others;
using UnityEngine;
using UnityEngine.AI;

namespace InterOrbital.Combat.IA
{
    public class EnemyAgentBase : MonoBehaviour
    {
        [SerializeField] protected List<EnemyStateBase> _states;
        [SerializeField] private HitShaderController _hitShaderController;
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
            if (TryGetComponent(out NavMeshAgent agent))
                _navMeshAgent = agent;
        }

        protected virtual void Start()
        {
            if (_navMeshAgent == null) return;
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
        }

        protected virtual void Update()
        {
            _hitTimer -= Time.deltaTime;
            if (HitAnimationPlaying()) return;
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
            if (_navMeshAgent == null) return false;
            return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                   (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
        }

        public void EnableNavigation(bool value)
        {
            if (_navMeshAgent == null) return;
            _navMeshAgent.velocity = value ? Vector3.one : Vector3.zero;
            _navMeshAgent.isStopped = !value;
        }

        public void HitEnemy()
        {
            _animator.SetBool("Hit", true);
            _hitTimer = _hitAnimationTime;
            StartCoroutine(HitAnimation());
            if (_navMeshAgent == null) return;
            if (_navMeshAgent.isStopped) return;
            EnableNavigation(false);
        }

        private bool HitAnimationPlaying()
        {
            return _hitTimer > 0;
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

        private IEnumerator HitAnimation()
        {
            while (HitAnimationPlaying())
            {
                _hitShaderController.Hit(!_hitShaderController.HitValue());
                yield return new WaitForSeconds(0.15f);
            }
            _hitShaderController.Hit(0);
        }
    }
}