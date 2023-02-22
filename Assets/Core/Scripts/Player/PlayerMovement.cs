using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerMovement : PlayerComponents
    {
        [Range(0f, 10f)]
        [SerializeField] private float _moveSpeed;

        [HideInInspector]
        public bool canMove = true;

        private void FixedUpdate()
        {
            if (canMove)
            {
                Move();
            }
        }

        private void Move()
        {
            //Multiplicamos por 100 porque, al mover usando velocidad, tenemos que usar numeros muy grandes.
            Rigidbody.velocity = _moveSpeed * 100 * Time.deltaTime * InputHandler.MoveDirection;
        }
    }
}
