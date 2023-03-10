using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAim : PlayerComponents
    {
        private Camera _camera;
        private Vector2 _aimDir;
        
        public Transform cursorT;

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
            //TODO: COMPROBAR EL RANGO DEL ATAQUE PARA PONER EL CURSOR CON MENOS OPACIDAD.
        }
        
        private void CheckInput()
        {
            if (PlayerInput.currentControlScheme == "Gamepad")
            {
                cursorT.gameObject.SetActive(false);
                _aimDir = InputHandler.AimDirection;
            }
            else
            {
                cursorT.gameObject.SetActive(true);
                _aimDir = cursorT.position - transform.position;
                _aimDir.Normalize();
            }
        }
    }
}
