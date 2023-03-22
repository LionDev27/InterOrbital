using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerMovement : PlayerComponents
    {
        [Range(0f, 10f)]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _timeForHelmetAnimation;
        private float _helmetAnimationTimer;

        [HideInInspector]
        public bool canMove = true;

        private void Update()
        {
            HandleAnimations();
        }

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

        private void HandleAnimations()
        {
            if (InputHandler.MoveDirection == Vector2.zero)
            {
                Animator.SetBool("PlayerRunning", false);
                _helmetAnimationTimer += Time.deltaTime;
                //Cuando lleva un tiempo parado se lanza la animaci�n del brillo en el casco
                if (_helmetAnimationTimer >= _timeForHelmetAnimation) 
                {
                    Animator.SetTrigger("HelmetShine");
                    _helmetAnimationTimer = 0;
                }
            }
            else
            {
                Animator.SetBool("PlayerRunning", true);
                _helmetAnimationTimer = 0;
                //Giramos el sprite seg�n la direcci�n de movimiento
                // PlayerSprite.flipX = InputHandler.MoveDirection.x < 0;
            }
        }
    }
}
