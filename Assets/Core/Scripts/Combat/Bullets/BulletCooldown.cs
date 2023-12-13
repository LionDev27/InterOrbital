using UnityEngine;

namespace InterOrbital.Combat.Bullets
{
    public class BulletCooldown
    {
        public float Cooldown => _time;
        private float _time;
        private float _timer;

        public void Setup(float time)
        {
            _time = time;
            End();
        }
        
        public void Run()
        {
            if (Ended()) return;
            _timer -= Time.deltaTime;
        }

        public void Reset()
        {
            _timer = _time;
            Debug.Log("Resetted. Cooldown: " + _time);
        }

        public bool Ended()
        {
            return _timer <= 0;
        }
        
        private void End()
        {
            _timer = 0;
        }
    }
}