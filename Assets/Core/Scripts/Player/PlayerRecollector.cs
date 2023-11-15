using System.Collections;
using DG.Tweening;
using InterOrbital.Combat;
using UnityEngine;
using InterOrbital.Recollectables;
using TMPro;

namespace InterOrbital.Player
{
    public class PlayerRecollector : PlayerComponents
    {
        [SerializeField] private Animator _gunAnimator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _gunSprite;
        [SerializeField] private TextMeshProUGUI _usagesText;
        [SerializeField] private int _recollectionAttackDamage = 1;
        [SerializeField] private float _recollectionAttackCooldown = 2f;
        [SerializeField] private float _recollectionRange = 5f;
        [SerializeField] private float _recollectionCooldownInSeconds = 1f;
        [SerializeField] private float _recollectionWidth = 3f;

        private int _currentUsages;
        private int _currentTier;
        private bool _canAttack = true;
        private bool _transitionAnimationEnded;
        private float _timer;

        private void Start()
        {
            _usagesText.DOFade(0f, 0f);
        }

        private void Update()
        {
            if (_timer < _recollectionCooldownInSeconds)
                _timer += Time.deltaTime;
            
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (CanRecollect())
                    Recollect();
                if (_gunAnimator.GetBool("Recollecting") == false)
                {
                    _gunAnimator.SetBool("Recollecting", true);
                    _audioSource.Play();
                    _usagesText.DOFade(1f, 0.5f);
                    PlayerAttack.canAttack = false;
                }
            }
            else if (_gunAnimator.GetBool("Recollecting"))
            {
                _gunAnimator.SetBool("Recollecting", false);
                _audioSource.Stop();
                _usagesText.DOFade(0f, 0.5f);
                PlayerAttack.canAttack = true;
            }
        }

        private void Recollect()
        {
            Vector2 dir = PlayerAim.AimDir();
            Vector2 boxcastSize = new Vector2(0.05f, _recollectionWidth);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxcastSize, 0f, dir, _recollectionRange);
            
            if(hits.Length <= 0) return;

            foreach (var hit in hits)
            {
                Recollectable recollectable = hit.collider.GetComponent<Recollectable>();
                if (recollectable != null)
                {
                    recollectable.Recollect();
                    CheckUsages();
                    _timer = 0f;
                    return;
                }

                if (hit.collider.TryGetComponent(out Damageable damageable) && !hit.collider.CompareTag(tag) && _canAttack)
                {
                    damageable.GetDamage(_recollectionAttackDamage);
                    StartCoroutine(AttackCooldown());
                    CheckUsages();
                    _timer = 0f;
                    return;
                }
            }
        }

        private IEnumerator AttackCooldown()
        {
            _canAttack = false;
            _gunSprite.material.SetFloat("_AlphaValue", 0.25f);
            yield return new WaitForSeconds(_recollectionAttackCooldown);
            _gunSprite.material.SetFloat("_AlphaValue", 1f);
            _canAttack = true;
        }

        private bool CanRecollect()
        {
            return _timer >= _recollectionCooldownInSeconds && _transitionAnimationEnded;
        }

        private void CheckUsages()
        {
            if (_currentUsages < 0) return;
            _currentUsages--;
            _usagesText.SetText(_currentUsages.ToString());
            if (_currentUsages <= 0)
                RecollectorUpgrades.OnEndUpgrade?.Invoke();
        }

        public void SetTransitionStatus(bool value)
        {
            _transitionAnimationEnded = value;
        }

        public void ChangeTier(RecollectorTier tier, int tierIndex)
        {
            _gunAnimator.runtimeAnimatorController = tier.controller;
            _currentUsages = tier.usages;
            _usagesText.SetText(tierIndex > 0 ? _currentUsages.ToString() : "");
            _currentTier = tierIndex;
        }
    }
}
