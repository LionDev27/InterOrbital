using Cinemachine;
using InterOrbital.WorldSystem;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerMovement : PlayerComponents
    {
        [Range(0f, 10f)]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _timeForHelmetAnimation;
        private float _helmetAnimationTimer;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        private int selfLayerIndex;

        [HideInInspector]
        public bool canMove = true;

        protected override void Awake()
        {
            base.Awake();
            if (!followCamera)
            {
                followCamera = GameObject.Find("Followcam").GetComponent<CinemachineVirtualCamera>();
            }
        }

        private void Start()
        {
            selfLayerIndex = gameObject.layer;
            transform.position = new Vector3(GridLogic.Instance.width/2 + 0.5f, GridLogic.Instance.height/2 + 0.5f, 0f);
        }

        private void Update()
        {
            HandleAnimations();
            HandleMapBorders();
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

        private void HandleMapBorders()
        {
            float widthBorder = GridLogic.Instance.width;
            float heightBorder = GridLogic.Instance.height;
            Vector3 playerPos = transform.position;
            Vector3 cameraPos = followCamera.transform.position;

            if (playerPos.x >= widthBorder + 0.01f) 
            {
                transform.position = new Vector3(0, playerPos.y, 0);
                Vector3 newCameraPos = new Vector3(cameraPos.x - widthBorder, cameraPos.y, cameraPos.z);
                WarpFollowCamera(newCameraPos);
            }
            if (playerPos.y >= heightBorder + 0.01f)
            {
                transform.position = new Vector3(playerPos.x, 0, 0);
                Vector3 newCameraPos = new Vector3(cameraPos.x, cameraPos.y - heightBorder, cameraPos.z);
                WarpFollowCamera(newCameraPos);
            }
            if (playerPos.x <= -0.01f)
            {
                transform.position = new Vector3(widthBorder, playerPos.y, 0);
                Vector3 newCameraPos = new Vector3(cameraPos.x + widthBorder, cameraPos.y, cameraPos.z);
                WarpFollowCamera(newCameraPos);
            }
            if (playerPos.y <= -0.01f)
            {
                transform.position = new Vector3(playerPos.x, heightBorder, 0);
                Vector3 newCameraPos = new Vector3(cameraPos.x, cameraPos.y + heightBorder, cameraPos.z);
                WarpFollowCamera(newCameraPos);
            }
        }

        private void WarpFollowCamera(Vector3 newCameraPos)
        {
            followCamera.OnTargetObjectWarped(transform, newCameraPos - followCamera.transform.position);
        }
        
        public void EnableCollisions(bool value)
        {
            Physics2D.IgnoreLayerCollision(selfLayerIndex, 8, !value);
        }

        public void EnableMovement(bool value)
        {
            canMove = value;
        }
    }
}
