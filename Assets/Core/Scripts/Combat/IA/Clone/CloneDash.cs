using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneDash : EnemyStateBase
    {
        [SerializeField] private float _dashForce = 800f;
        [Tooltip("Tiempo en segundos que durará realizando el dash.")]
        [Range(0f, 0.5f)]
        [SerializeField] private float _dashTime = 0.3f;
        private float _dashTimer;
        private float _dashAnimationSpeed;

        private CloneAgent _agent;
        private Rigidbody2D _rigidbody;

        public override void Setup(EnemyAgentBase agent)
        {
            _agent = agent as CloneAgent;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            var dashAnimationDuration = 0f;
            foreach (var animationClip in _agent.Animator.runtimeAnimatorController.animationClips)
            {
                if (animationClip.name == "PlayerDash")
                    dashAnimationDuration = animationClip.length;
            }

            _dashAnimationSpeed = dashAnimationDuration / _dashTime;
        }
        
        public override void OnStateEnter()
        {
            _agent.EnableNavigation(false);
            _rigidbody.WakeUp();
            Dash();
        }

        public override void Execute()
        {
            switch (_dashTimer)
            {
                case <= 0:
                    EndDash();
                    break;
                case > 0:
                    _dashTimer -= Time.deltaTime;
                    break;
            }
        }

        private void Dash()
        {
            AudioManager.Instance.PlaySFX("Dash");
            _agent.ShowGun(false);
            _dashTimer = _dashTime;
            _rigidbody.AddForce(RandomDir() * _dashForce);
            SetAnimation();
        }

        private void EndDash()
        {
            _agent.Animator.speed = 1;
            _agent.ShowGun(true);
            _rigidbody.Sleep();
            _agent.EnableNavigation(true);
            _agent.ChangeState(1);
        }

        private Vector2 RandomDir()
        {
            var randomX = Random.Range(-1f, 1f);
            var randomY = Random.Range(-1f, 1f);

            var dir = new Vector2(randomX, randomY);
            dir.Normalize();
            return dir;
        }

        private void SetAnimation()
        {
            _agent.Animator.SetTrigger("PlayerDash");
            _agent.Animator.speed = _dashAnimationSpeed;
        }
    }
}