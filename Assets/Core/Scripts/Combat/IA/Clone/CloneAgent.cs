using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneAgent : EnemyAgentBase
    {
        public Transform AttackPoint => _aim.AttackPoint;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;
        public float NormalDistance => _normalDirDistance;
        public float RandomDistance => _randomDirDistance;

        [SerializeField] private float _normalDirDistance;
        [SerializeField] private float _randomDirDistance;
        [SerializeField] private float _distanceToShoot;
        [SerializeField] private float _minDashTime;
        [SerializeField] private float _maxDashTime;
        private SpriteFlipper _spriteFlipper;
        private CloneAim _aim;
        private float _currentTime;
        private float _timer;
        private bool _canAttack;

        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
            _aim = GetComponent<CloneAim>();
        }

        protected override void Start()
        {
            base.Start();
            _target = PlayerComponents.Instance.transform;
            SetTimer();
        }

        protected override void Update()
        {
            base.Update();
            if (TimerEnded())
            {
                ChangeState(3);
                SetTimer();
            }
            else
                _timer -= Time.deltaTime;
        }

        private bool TimerEnded()
        {
            return _timer <= 0f;
        }

        private void SetTimer()
        {
            _currentTime = GetRandomTime();
            _timer = _currentTime;
        }

        private float GetRandomTime()
        {
            return Random.Range(_minDashTime, _maxDashTime);
        }

        public bool CanAttack()
        {
            return _currentState != _states[3] && _currentState != _states[0] &&
                   Vector2.Distance(_target.position, transform.position) <= _distanceToShoot;
        }

        public void ShowGun(bool show)
        {
            _aim.ShowGun(show);
        }

        public Vector2 AimDir()
        {
            return (_target.position - transform.position).normalized;
        }

        public void FlipSprite()
        {
            if (Vector2.Distance(_target.position, transform.position) < 0.5f) return;
            switch (Mathf.Sign(_target.position.x - transform.position.x))
            {
                case > 0:
                    SpriteFlipper.FlipX(0);
                    break;
                case < 0:
                    SpriteFlipper.FlipX(1);
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _randomDirDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _normalDirDistance);
        }
    }
}