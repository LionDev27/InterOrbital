using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class ElytronAttackState : ElytrinAttackState
    {
        [SerializeField] private ParticleSystem _particles;

        protected override void SetDamageableDir()
        {
            base.SetDamageableDir();
            _particles.transform.position = _currentAgent.DamageDealer.position;
        }

        public void PlayAttackEffects()
        {
            _particles.Play();
            CameraShake.Instance.Shake(5f);
        }
    }
}