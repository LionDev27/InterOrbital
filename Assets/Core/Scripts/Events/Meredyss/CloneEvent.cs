using System;
using InterOrbital.Combat.IA;
using InterOrbital.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InterOrbital.Events.Meredyss
{
    public class CloneEvent : EventBase
    {
        public static Action OnCloneDeath;
        
        [SerializeField] private ParticleSystem _rainEffect;
        [SerializeField] private ParticleSystem _splashEffect;
        [SerializeField] private float _raidusSpawn;
        [SerializeField] private GameObject _clonePrefab;
        private GameObject _currentClone;
        
        public override void StartEvent()
        {
            base.StartEvent();
            Rain();
            Spawn();
            AudioManager.Instance.ModifyMusicVolume(-10);
            AudioManager.Instance.PlayMusic("EventMusic1",true);
        }

        public override void EndEvent()
        {
            base.EndEvent();
            Invoke(nameof(DestroyClone), 0.5f);
            _rainEffect.Stop();
            _splashEffect.Stop();
            AudioManager.Instance.ModifyMusicVolume(10);
            AudioManager.Instance.StopAmbientSFX();
            AudioManager.Instance.PlayMusic("MainTheme", true);
        }

        private void OnEnable()
        {
            OnCloneDeath += EndEvent;
        }

        private void OnDisable()
        {
            OnCloneDeath -= EndEvent;
        }

        private void Spawn()
        {
            Vector2 playerPos = PlayerComponents.Instance.transform.position;
            _currentClone = Instantiate(_clonePrefab, playerPos + (RandomDir() * _raidusSpawn), Quaternion.identity);
        }
        
        private void Rain()
        {
            _rainEffect.Simulate(10, true, false);
            _rainEffect.Play();
            _splashEffect.Play();
            AudioManager.Instance.PlayAmbientSFX("Rain", true);
        }
        
        private Vector2 RandomDir()
        {
            var randomX = Random.Range(-1f, 1f);
            var randomY = Random.Range(-1f, 1f);

            var dir = new Vector2(randomX, randomY);
            dir.Normalize();
            return dir;
        }
        
        private void DestroyClone()
        {
            if (_currentClone == null) return;
            _currentClone.GetComponent<EnemyDamageable>().ExplosionDeath();
        }
    }
}