using System;
using UnityEditor.U2D.Path.GUIFramework;
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

        public int ScrollFastInventoryValue { get; private set; }
        //Para los botones, ejecutaremos un Action que asignaremos en otro script.
        public Action OnAttack;
        public Action OnOpenInventory;

        public Action OnOpenCraft;

        public Action OnDashPerformed;
        public Action OnInteractPerformed;


        private void Start()
        {
            ScrollFastInventoryValue = 1;
        }
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

        private void OnScrollY(InputValue value)
        {
            
            if (value.Get<float>() > 0)
            {
                ScrollFastInventoryValue += 1;
                if (ScrollFastInventoryValue > 5)
                    ScrollFastInventoryValue = 5;
            }
            else if(value.Get<float>() < 0)
            {
                ScrollFastInventoryValue -= 1;
                if (ScrollFastInventoryValue < 1)
                    ScrollFastInventoryValue = 1;
            }
           
        }
        private void OnSelectNumeric(InputValue value)
        {
            if (value.isPressed)
            {
                Debug.Log(value.Get<float>());
               ScrollFastInventoryValue = (int) value.Get<float>();
            }
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

        private void OnInteract()
        {
            OnInteractPerformed();
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

        private void OnCraft()
        {
            OnOpenCraft();
        }

    }
    
}
