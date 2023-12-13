using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.Combat.Bullets
{
    public class BulletCooldown
    {
        public float Cooldown => _time;
        private Image _image;
        private float _time;
        private float _timer;

        public void SetImage(Image image)
        {
            _image = image;
        }
        
        public void Setup(float time)
        {
            _time = time;
            End();
        }
        
        public void Run()
        {
            if (Ended()) return;
            _timer -= Time.deltaTime;
            _image.fillAmount = (_time - _timer) / _time;
        }

        public void Reset()
        {
            _timer = _time;
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