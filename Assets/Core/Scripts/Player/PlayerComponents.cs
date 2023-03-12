using UnityEngine;
using UnityEngine.InputSystem;

namespace InterOrbital.Player
{
    /// <summary>
    /// Obtiene todos los componentes necesarios que necesitara el jugador, para llamarlos de una sola vez y tenerlos todos bien organizados.
    /// </summary>
    [RequireComponent(typeof(PlayerInputHandler), typeof(Rigidbody2D))]
    public class PlayerComponents : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody { get; private set; }
        protected PlayerInput PlayerInput { get; private set; }
        protected PlayerAttack PlayerAttack { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }

        protected virtual void Awake()
        {
            InputHandler = GetComponent<PlayerInputHandler>();
            PlayerInput = GetComponent<PlayerInput>();
            PlayerAttack = GetComponent<PlayerAttack>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
