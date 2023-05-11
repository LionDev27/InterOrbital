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
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnDashPerformed += Dash;
        }

        private void Update()
        {
            _dashTimer -= Time.deltaTime;
            _dashInvulnerabilityTimer -= Time.deltaTime;
            if (_dashTimer <= 0)
            {
                PlayerMovement.canMove = true;
            }
        }

        private void Dash()
        {
            //TODO: animación del dash.
            if (IsDashing()) return;
            PlayerMovement.canMove = false;
            _dashTimer = _dashTime;
            _dashInvulnerabilityTimer = _dashTime + _dashInvulnerabilityExtraTime;
            //Si no se está moviendo, hará el dash a la dirección a la que apunta. Si se mueve, lo hará hacia la que se mueve.
            Vector3 direction = InputHandler.MoveDirection != Vector2.zero ? InputHandler.MoveDirection : PlayerAttack.attackPoint.position - transform.position;
            Rigidbody.AddForce(direction * _dashForce);
        }

        public bool IsDashing()
        {
            return _dashInvulnerabilityTimer > 0;
        }
    }
}
