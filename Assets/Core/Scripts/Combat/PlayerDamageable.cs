using InterOrbital.Player;
using UnityEngine;

namespace InterOrbital.Combat
{
    public class PlayerDamageable : Damageable
    {
        private PlayerComponents _playerComponents;

        private void Awake()
        {
            _playerComponents = GetComponent<PlayerComponents>();
        }

        protected override void Death()
        {
            Debug.Log("Player Dead");
            _playerComponents.InputHandler.DeactivateControls();
        }
    }
}
