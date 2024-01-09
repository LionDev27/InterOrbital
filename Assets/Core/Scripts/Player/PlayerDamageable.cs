using System;
using System.Collections;
using InterOrbital.Combat;
using InterOrbital.Others;
using InterOrbital.Spaceship;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField] private ParticleSystem _deathParticles;
        [SerializeField] private HitShaderController _hitShaderController;
        [SerializeField] private float _damageCameraShakeIntensity = 5f;
        [SerializeField] private float _invencibilityTime;
        [SerializeField] private float _loseHealthTimerDefaultValue;
        [SerializeField] private GameObject _dmgPopup;
        [SerializeField] private GameObject _deathBag;
        private float _loseHealthTimer;
        private float _invencibilityTimer;
        private bool _godMode;
        
        private PlayerComponents _playerComponents;

        private void Awake()
        {
            _playerComponents = PlayerComponents.Instance;
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (_godMode && Application.isEditor) return;
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
                    GetDamage(1);
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
#if UNITY_EDITOR
            if (_godMode) return false;
#endif
            //Recibira daño si ha terminado su cooldown de invencibilidad y no está realizando un dash.
            return InvencibilityEnded() && !_playerComponents.PlayerDash.IsDashing();
        }

        #endregion
        
        public override void GetDamage(int damage)
        {
            if (CanTakeDamage())
            {
                base.GetDamage(damage);
                GameObject dmgPopup = Instantiate(_dmgPopup,transform.position,Quaternion.identity);
                dmgPopup.GetComponent<NumberPopup>().Setup(damage);
                UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
                CameraShake.Instance.Shake(_damageCameraShakeIntensity);
                AudioManager.Instance.PlaySFX("LoseLife");
                StartCoroutine(SlowTimeEffect.Instance.Play(0.2f));
                SetInvencibilityState();
            }
        }

        public override void RestoreHealth(int healthAmount)
        {
            base.RestoreHealth(healthAmount);
            UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
            ResetHealthTimer();
        }

        public override void ResetHealth()
        {
            base.ResetHealth();
            UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
            ResetHealthTimer();
        }

        public override void UpgradeHealth(int healthAmount)
        {
            base.UpgradeHealth(healthAmount);
            _currentHealth = _maxHealth;
            UIManager.Instance.UpgradeLifeUI();
            UIManager.Instance.UpdateLifeUI(_maxHealth, _currentHealth);
        }

        protected override void Death()
        {
            if (!PlayerComponents.Instance.Inventory.isHide)
            {
                UIManager.Instance.OpenInventory(false);
            }
            _playerComponents.InputHandler.DeactivateControls();
            _canTakeDamage = false;
            StartCoroutine(DeathSequence());
        }
        
        private IEnumerator DeathSequence()
        {
            Vector3 deathPos = transform.position;
            PlayerComponents.Instance.DeathAnimation();
            Instantiate(_deathParticles, deathPos, _deathParticles.transform.rotation).Play();
            yield return new WaitForSeconds(_invencibilityTime);
            LevelManager.Instance.GameBlackout(true,1.5f);
            yield return new WaitForSeconds(2f);
            Instantiate(_deathBag, deathPos, transform.rotation);
            transform.position = SpaceshipComponents.Instance.transform.position;
            SpaceshipComponents.Instance.Animator.SetTrigger("StartAnim");
            yield return new WaitForSeconds(1f);
            LevelManager.Instance.GameBlackout(false, 2f);
            ResetHealth();
            _playerComponents.InputHandler.ActivateControls();
            _canTakeDamage = true;
        }

        private IEnumerator HitAnimation()
        {
            while (!InvencibilityEnded())
            {
                _hitShaderController.Hit(!_hitShaderController.HitValue());
                yield return new WaitForSeconds(0.2f);
            }
            _hitShaderController.Hit(0);
        }

        public void ToggleGodMode()
        {
            if (Application.isEditor)
                _godMode = !_godMode;
        }
    }
}
