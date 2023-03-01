using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerAttack : PlayerComponents
    {
        [SerializeField] private float _attackCooldown;
        private float _timer;
        
        [HideInInspector] public bool canAttack = true;

        protected override void Awake()
        {
            base.Awake();
            InputHandler.OnAttack += Attack;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        private void Attack()
        {
            if (!canAttack || !CooldownEnded()) return;
            _timer = _attackCooldown;
            Debug.Log("Attacking");
        }

        private bool CooldownEnded()
        {
            return _timer <= 0;
        }

        private void OnDestroy()
        {
            InputHandler.OnAttack -= Attack;
        }
    }
}
