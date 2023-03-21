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

        //TODO: TIEMPO DE INVENCIBILIDAD DESPUES DE RECIBIR DAÑO
        public override void GetDamage(int damage)
        {
            base.GetDamage(damage);
        }

        protected override void Death()
        {
            Debug.Log("Player Dead");
            _playerComponents.InputHandler.DeactivateControls();
        }
    }
}
