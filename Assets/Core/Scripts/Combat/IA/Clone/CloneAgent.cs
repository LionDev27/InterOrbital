using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class CloneAgent : EnemyAgentBase
    {
        [SerializeField] private float _minDashTime;
        [SerializeField] private float _maxDashTime;
        private float _currentTime;
        private float _timer;
    
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
                //Dash
                Dash();
                SetTimer();
            }
            else
                _timer -= Time.deltaTime;
        }

        private void Dash()
        {
            Debug.Log("Dashing");
        }

        private bool TimerEnded()
        {
            return _timer <= 0f;
        }
        
        private void SetTimer()
        {
            _currentTime = GetRandomTime();
            _timer = _currentTime;
            Debug.Log(_currentTime);
        }
        
        private float GetRandomTime()
        {
            return Random.Range(_minDashTime, _maxDashTime);
        }
    }
}
