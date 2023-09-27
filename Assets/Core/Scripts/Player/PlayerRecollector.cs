using InterOrbital.Combat;
using UnityEngine;
using InterOrbital.Recollectables;

namespace InterOrbital.Player
{
    public class PlayerRecollector : PlayerComponents
    {
        [SerializeField] private Animator _gunAnimator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _recollectionAttackDamage = 1;
        [SerializeField] private float _recollectionRange = 5f;
        [SerializeField] private float _recollectionCooldownInSeconds = 1f;
        [SerializeField] private float _recollectionWidth = 3f;

        private bool _transitionAnimationEnded;
        private float _timer;

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
                }
            }
            else if (_gunAnimator.GetBool("Recollecting"))
            {
                _gunAnimator.SetBool("Recollecting", false);
                _audioSource.Stop();
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
                    _timer = 0f;
                    return;
                }

                if (hit.collider.TryGetComponent(out Damageable damageable) && !hit.collider.CompareTag(tag))
                {
                    damageable.GetDamage(_recollectionAttackDamage);
                    _timer = 0f;
                    return;
                }
            }
        }

        private bool CanRecollect()
        {
            return _timer >= _recollectionCooldownInSeconds && _transitionAnimationEnded;
        }

        public void SetTransitionStatus(bool value)
        {
            _transitionAnimationEnded = value;
        }
    }
}
