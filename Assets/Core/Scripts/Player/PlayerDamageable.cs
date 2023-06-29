using System;
using System.Collections;
using InterOrbital.Combat;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField] private GameObject _hitSpriteObj;
        [SerializeField] private float _damageCameraShakeIntensity = 5f;
        [SerializeField] private float _invencibilityTime;
        [SerializeField] private float _loseHealthTimerDefaultValue;
        private float _loseHealthTimer;
        private float _invencibilityTimer;
        
        private PlayerComponents _playerComponents;

        private void Awake()
        {
            _playerComponents = PlayerComponents.Instance;
        }

        protected override void Start()
        {
            base.Start();
            if (_hitSpriteObj.activeInHierarchy)
                _hitSpriteObj.SetActive(false);
        }

        private void Update()
        {
            LoseHealthOverTime();
            
            if (!CanTakeDamage())
                RunInvencibilityTimer();
        }

        private void LoseHealthOverTime()
        {
            bool energyEmpty = _playerComponents.PlayerEnergy.EnergyEmpty;
            if (energyEmpty)
            {
                if (_loseHealthTimer > 0)
                {
                    _loseHealthTimer -= Time.deltaTime;
                }
                else
                {
                    _currentHealth = Mathf.Clamp(_currentHealth - 1, 0, _maxHealth);
                    UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
                    CameraShake.Instance.Shake(_damageCameraShakeIntensity / 2f, 0.5f);
                    ResetHealthTimer();
                    CheckHealth();
                }
            }
        }
        
        private void ResetHealthTimer()
        {
            _loseHealthTimer = _loseHealthTimerDefaultValue;
        }

        #region Invencibility

        private void SetInvencibilityState()
        {
            _invencibilityTimer = _invencibilityTime;
            StartCoroutine(nameof(HitAnimation));
        }
        
        private void RunInvencibilityTimer()
        {
            _invencibilityTimer -= Time.deltaTime;
        }

        private bool InvencibilityEnded()
        {
            return _invencibilityTimer <= 0;
        }
        
        private bool CanTakeDamage()
        {
            //Recibira daño si ha terminado su cooldown de invencibilidad y no está realizando un dash.
            return InvencibilityEnded() && !_playerComponents.PlayerDash.IsDashing();
        }

        #endregion
        
        public override void GetDamage(int damage)
        {
            if (CanTakeDamage())
            {
                Debug.Log("Recibiendo daño");
                base.GetDamage(damage);
                UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
                CameraShake.Instance.Shake(_damageCameraShakeIntensity, 0.5f);
                SetInvencibilityState();
            }
        }

        public override void RestoreHealth(int healthAmount)
        {
            base.RestoreHealth(healthAmount);
            UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
            ResetHealthTimer();
        }

        protected override void Death()
        {
            Debug.Log("Player Dead");
            //TODO: Desactivar colisiones y animacion de muerte.
            _playerComponents.InputHandler.DeactivateControls();
        }

        private IEnumerator HitAnimation()
        {
            while (!InvencibilityEnded())
            {
                _hitSpriteObj.SetActive(!_hitSpriteObj.activeInHierarchy);
                yield return new WaitForSeconds(0.2f);
            }
            _hitSpriteObj.SetActive(false);
        }
    }
}
