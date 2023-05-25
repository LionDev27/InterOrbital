using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerDash : PlayerComponents
    {
        [SerializeField] private float _dashForce = 800f;
        [Tooltip("Tiempo en segundos que durará realizando el dash.")]
        [Range(0f, 0.5f)]
        [SerializeField] private float _dashTime = 0.3f;
        [Tooltip("Tiempo en segundos de invencibilidad mientras realiza el dash. Se sumará al tiempo del dash.")]
        [Range(0f, 0.5f)]
        [SerializeField] private float _dashInvulnerabilityExtraTime;
        private float _dashTimer;
        private float _dashInvulnerabilityTimer;
        private float _dashTotalTime => _dashTime + _dashInvulnerabilityExtraTime;
        private float _dashAnimationSpeed;
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnDashPerformed += Dash;
        }

        private void Start()
        {
            float dashAnimationDuration = 0f;
            
            foreach (var animationClip in Animator.runtimeAnimatorController.animationClips)
            {
                if (animationClip.name == "PlayerDash")
                {
                    dashAnimationDuration = animationClip.length;
                }
            }

            _dashAnimationSpeed = dashAnimationDuration / _dashTotalTime;
        }

        private void Update()
        {
            _dashTimer -= Time.deltaTime;
            _dashInvulnerabilityTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
                PlayerMovement.canMove = true;
            if (_dashInvulnerabilityTimer <= 0)
                Animator.speed = 1;
            if (IsDashing() && InputHandler.MoveDirection != Vector2.zero)
                HandleSprites();
        }

        private void Dash()
        {
            if (IsDashing()) return;
            PlayerMovement.canMove = false;
            _dashTimer = _dashTime;
            _dashInvulnerabilityTimer = _dashTotalTime;
            //Si no se está moviendo, hará el dash a la dirección a la que apunta. Si se mueve, lo hará hacia la que se mueve.
            Vector3 direction = InputHandler.MoveDirection != Vector2.zero ? InputHandler.MoveDirection : PlayerAttack.attackPoint.position - transform.position;
            Rigidbody.AddForce(direction * _dashForce);
            SetAnimation();
        }

        private void SetAnimation()
        {
            Animator.SetTrigger("PlayerDash");
            Animator.speed = _dashAnimationSpeed;
        }

        private void HandleSprites()
        {
            //Rotacion del sprite del jugador.
            if (InputHandler.MoveDirection.x >= 0)
                PlayerSprite.flipX = false;
            else
                PlayerSprite.flipX = true;
        }

        public bool IsDashing()
        {
            return _dashInvulnerabilityTimer > 0;
        }
    }
}
