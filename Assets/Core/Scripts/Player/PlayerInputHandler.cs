using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InterOrbital.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 AimPosition { get; private set; }
        //Para los botones, ejecutaremos un Action que asignaremos en otro script.
        public Action OnAttack;

        //Haremos un metodo nuevo que se llame igual que el nuevo input introducido en los Input Settings.
        //A su vez, haremos una propiedad para obtener el valor del input en otro script.
        private void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        private void OnAim(InputValue value)
        {
            AimPosition = value.Get<Vector2>();
        }

        private void OnFire()
        {
            OnAttack();
        }
    }
}
