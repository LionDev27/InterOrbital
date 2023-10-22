using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class BossAgent : EnemyAgentBase
    {
        public Action ChangePhase;
        public BossDamageable Damageable => _damageable;
        
        [SerializeField] private List<BossPhase> _phases;
        [SerializeField] private float _changePhaseTime;
        [SerializeField] private Color _lastPhaseColor;
        private SpriteRenderer _spriteRenderer;
        private int _currentPhaseIndex;
        private BossDamageable _damageable;
        private bool _changingPhase;
        private bool _dying;

        protected override void Awake()
        {
            base.Awake();
            _damageable = GetComponent<BossDamageable>();
            _spriteRenderer = _hitShaderController.gameObject.GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            ChangeFillColor(_phases[_currentPhaseIndex]);
            base.Start();
        }

        public bool CanAttack()
        {
            return !CurrentAttacks().Attacking && !_changingPhase && !_dying;
        }

        public BossAttacks CurrentAttacks()
        {
            return _phases[_currentPhaseIndex].attacks;
        }

        public void DownPhase()
        {
            if (_currentPhaseIndex == 0) return;
            var currentPhase = _phases[_currentPhaseIndex];
            if (_damageable.CurrentHealth > currentPhase.healthToChange)
            {
                if (IsLastPhase())
                    ChangeSpriteColor(false);
                _currentPhaseIndex--;
                ChangeFillColor(_phases[_currentPhaseIndex]);
            }
        }

        public void UpPhase()
        {
            if (IsLastPhase()) return;
            var nextPhase = _phases[_currentPhaseIndex + 1];
            if (_damageable.CurrentHealth <= nextPhase.healthToChange)
            {
                StartCoroutine(ChangePhaseTimer());
                ChangePhase?.Invoke();
                CurrentAttacks().DeactivateAttacks();
                CurrentAttacks().EndAttack();
                _currentPhaseIndex++;
                if (IsLastPhase())
                    ChangeSpriteColor(true);
                ChangeFillColor(nextPhase);
            }
        }

        public override void Death()
        {
            CameraShake.Instance.Shake(10);
            _dying = true;
            CurrentAttacks().DeactivateAttacks();
            CurrentAttacks().EndAttack();
        }

        private bool IsLastPhase()
        {
            return _currentPhaseIndex == _phases.Count - 1;
        }

        private void ChangeFillColor(BossPhase phase)
        {
            BossInfoBar.OnChangeFillColor?.Invoke(phase.barFillColor);
        }

        private void ChangeSpriteColor(bool lastPhase)
        {
            var color = lastPhase ? _lastPhaseColor : Color.white;
            _spriteRenderer.DOColor(color, _changePhaseTime).SetEase(Ease.InOutCubic).Play();
            if (!lastPhase) return;
            transform.DOShakePosition(_changePhaseTime,0.5f, 20).SetEase(Ease.InOutCubic).Play();
        }

        private IEnumerator ChangePhaseTimer()
        {
            _changingPhase = true;
            yield return new WaitForSeconds(_changePhaseTime);
            _changingPhase = false;
        }
    }
}