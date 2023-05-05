using InterOrbital.Combat;
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
        protected Animator Animator { get; private set; }
        protected SpriteRenderer PlayerSprite { get; private set; }
        protected PlayerInput PlayerInput { get; private set; }
        protected PlayerAttack PlayerAttack { get; private set; }
        protected PlayerMovement PlayerMovement { get; private set; }
        protected PlayerCraft PlayerCraft { get; private set; }


        public PlayerInputHandler InputHandler { get; private set; }
        public Inventory Inventory { get; private set; }
        public PlayerEnergy PlayerEnergy { get; private set; }
        public PlayerDash PlayerDash { get; private set; }
        public PlayerDamageable PlayerDamageable { get; private set; }

        public static PlayerComponents Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;

            InputHandler = GetComponent<PlayerInputHandler>();
            PlayerInput = GetComponent<PlayerInput>();
            PlayerAttack = GetComponent<PlayerAttack>();
            PlayerMovement = GetComponent<PlayerMovement>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponentInChildren<Animator>();
            PlayerSprite = GetComponentInChildren<SpriteRenderer>();
            Inventory = GetComponent<Inventory>();
            PlayerEnergy = GetComponent<PlayerEnergy>();
            PlayerDash = GetComponent<PlayerDash>();
            PlayerDamageable = GetComponent<PlayerDamageable>();
            PlayerCraft = GetComponent<PlayerCraft>();
        }
    }
}
