using System;
using DG.Tweening;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAim : PlayerComponents
    {
        [SerializeField] private Transform _gunSpriteT;
        private SpriteRenderer _gunSprite;
        private Camera _camera;
        private Vector2 _aimDir;
        private float _aimOffset;
        private float _gunSpriteOffset;

        protected override void Awake()
        {
            base.Awake();
            _gunSprite = _gunSpriteT.GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _camera = Camera.main;
            _aimOffset = PlayerAttack.attackPoint.localPosition.x;
            _gunSpriteOffset = _gunSpriteT.transform.localPosition.x;
        }

        private void Update()
        {
            Aim();
            CheckInput();
        }

        private void Aim()
        {
            if (_aimDir == Vector2.zero) return;
            PlayerAttack.attackPoint.localPosition = _aimDir * _aimOffset;
            HandleSprites();
            //TODO: COMPROBAR EL RANGO DEL ATAQUE PARA PONER EL CURSOR CON MENOS OPACIDAD.
        }

        private void HandleSprites()
        {
            //Rotacion de la pistola.
            _gunSpriteT.transform.localPosition = _aimDir * _gunSpriteOffset;
            var lookAtPos = PlayerAttack.attackPoint.localPosition;
            if (_aimDir.x > 0f)
            {
                _gunSprite.flipX = false;
                _gunSpriteT.right = lookAtPos - _gunSpriteT.localPosition;
            }
            else if (_aimDir.x < 0f)
            {
                _gunSprite.flipX = true;
                _gunSpriteT.right = -lookAtPos - _gunSpriteT.localPosition;
            }
            //Rotacion del sprite del jugador.
            if (PlayerDash.IsDashing()) return;
            if (_aimDir.x > 0)
                PlayerSprite.flipX = false;
            else
                PlayerSprite.flipX = true;
        }

        private void CheckInput()
        {
            var cursorPos = _camera.ScreenToWorldPoint(InputHandler.AimPosition);
            _aimDir = cursorPos - transform.position;
            _aimDir.Normalize();
        }

        public Vector2 AimDir()
        {
            return (PlayerAttack.attackPoint.position - transform.position).normalized;
        }

        public void ShowGun(bool show)
        {
            if (show)
            {
                _gunSpriteT.gameObject.SetActive(true);
                _gunSpriteT.transform.DOScale(Vector2.one, 0.1f).Play();
            }
            else
            {
                _gunSpriteT.transform.DOScale(Vector2.zero, 0.1f).
                    OnComplete(() => _gunSpriteT.gameObject.SetActive(false)).Play();
            }
        }
    }
}