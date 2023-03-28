using System;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAim : PlayerComponents
    {
        [SerializeField] private Transform _gunSpriteT;
        private SpriteRenderer _gunSprite;
        private Camera _camera;
        private Vector2 _aimDir;
        
        public Transform cursorT;

        protected override void Awake()
        {
            base.Awake();
            _gunSprite = _gunSpriteT.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Aim();
            CheckInput();
        }

        private void Aim()
        {
            cursorT.position = _camera.ScreenToWorldPoint(InputHandler.AimPosition);
            if (_aimDir == Vector2.zero) return;
            PlayerAttack.attackPoint.localPosition = _aimDir;
            HandleSprites();
            //TODO: COMPROBAR EL RANGO DEL ATAQUE PARA PONER EL CURSOR CON MENOS OPACIDAD.
        }

        private void HandleSprites()
        {
            //Rotacion del sprite del jugador.
            if (cursorT.localPosition.x > 0)
                PlayerSprite.flipX = false;
            else if(cursorT.localPosition.x < 0)
                PlayerSprite.flipX = true;
            //Rotacion de la pistola.
            Quaternion gunRot = _gunSpriteT.rotation;
            Quaternion lookRot = Quaternion.LookRotation(_aimDir);
            _gunSpriteT.rotation = new Quaternion(gunRot.x, gunRot.y, lookRot.z, lookRot.w);
            _gunSprite.flipX = _aimDir.x < 0;
        }
        
        private void CheckInput()
        {
            switch (InputHandler.CurrentInput())
            {
                case PlayerInputHandler.InputType.Keyboard:
                    cursorT.gameObject.SetActive(true);
                    _aimDir = cursorT.position - transform.position;
                    _aimDir.Normalize();
                    break;
                case PlayerInputHandler.InputType.Gamepad:
                    cursorT.gameObject.SetActive(false);
                    _aimDir = InputHandler.AimDirection;
                    break;
            }
        }
    }
}
