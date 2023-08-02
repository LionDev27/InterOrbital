using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class DarkBall : EnemyAgentBase
    {
        private SpriteFlipper _spriteFlipper;

        private Transform _target;
        private bool _isChasingPlayer = true;
        private Vector3 _previousPlayerPosition;

        public float initialSpeed = 3f;
        public float maxSpeed = 7f; 
        public float acceleration = 2f;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;


        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
            _target = FindObjectOfType<Player.PlayerAim>().GetComponent<Transform>();
            
        }

        protected override void Start()
        {
            base.Start();
            NavMeshAgent.speed = initialSpeed;
        }

        protected override void Update()
        {
            if (_target)
            {
                Vector3 playerDirection = _target.position - _previousPlayerPosition;
                Vector3 enemyDirection = transform.position - _previousPlayerPosition;

                if (Vector3.Angle(enemyDirection, playerDirection) > 120f)
                {
                    _isChasingPlayer = false;
                }
                else
                {
                    _isChasingPlayer = true;
                }

                if (_isChasingPlayer)
                {
                    float desiredSpeed = Mathf.Lerp(initialSpeed, maxSpeed, NavMeshAgent.velocity.magnitude / maxSpeed);
                    NavMeshAgent.speed = Mathf.MoveTowards(NavMeshAgent.speed, desiredSpeed, acceleration * Time.deltaTime);
                }
                
                NavMeshAgent.SetDestination(_target.position);

                _previousPlayerPosition = _target.position;
            }

        }

    }
}

  
