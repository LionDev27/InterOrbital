using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InterOrbital.Player
{
    public class PlayerInputHandler : PlayerComponents
    {
        public enum InputType {Keyboard, Gamepad}
        
        public Vector2 MoveDirection { get; private set; }
        public Vector2 AimPosition { get; private set; }
        public Vector2 AimDirection { get; private set; }
        //Para los botones, ejecutaremos un Action que asignaremos en otro script.
        public Action OnAttack;
        public Action OnOpenInventory;
        public Action OnDashPerformed;
            
        //Haremos un metodo nuevo que se llame igual que el nuevo input introducido en los Input Settings.
        //A su vez, haremos una propiedad para obtener el valor del input en otro script.
        private void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        private void OnAimPosition(InputValue value)
        {
            AimPosition = value.Get<Vector2>();
        }

        private void OnAimDirection(InputValue value)
        {
            AimDirection = value.Get<Vector2>();
        }

        private void OnFire()
        {
            OnAttack();
        }
        
        private void OnInventory()
        {
            OnOpenInventory();
        }

        private void OnDash()
        {
            OnDashPerformed();
        }
        
        public void DeactivateControls()
        {
            PlayerInput.enabled = false;
            enabled = false;
            Rigidbody.velocity = Vector2.zero;
        }

        public InputType CurrentInput()
        {
            return PlayerInput.currentControlScheme == "Gamepad" ? InputType.Gamepad : InputType.Keyboard;
        }

        public void ChangeActionMap ()
        {
            string actionMap = PlayerInput.currentActionMap.name == "Player" ? "UI" : "Player";
            PlayerInput.SwitchCurrentActionMap(actionMap);
        }
    }
    
}
