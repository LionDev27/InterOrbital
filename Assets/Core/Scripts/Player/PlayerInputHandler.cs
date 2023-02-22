using UnityEngine;
using UnityEngine.InputSystem;

namespace InterOrbital.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 AimPosition { get; private set; }
        public bool Fire { get; private set; }

        private void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        private void OnAim(InputValue value)
        {
            AimPosition = value.Get<Vector2>();
        }

        private void OnFire(InputValue value)
        {
            Fire = value.isPressed;
        }
    }
}
