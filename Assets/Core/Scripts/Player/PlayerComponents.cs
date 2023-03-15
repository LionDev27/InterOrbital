using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Player
{
    /// <summary>
    /// Obtiene todos los componentes necesarios que necesitara el jugador, para llamarlos de una sola vez y tenerlos todos bien organizados.
    /// </summary>
    [RequireComponent(typeof(PlayerInputHandler), typeof(Rigidbody2D))]
    public class PlayerComponents : MonoBehaviour
    {
        protected PlayerInputHandler InputHandler { get; private set; }
        protected Rigidbody2D Rigidbody { get; private set; }

        public Inventory Inventory { get; private set; }

        public static PlayerComponents Instance = null;

       

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;
           

            InputHandler = GetComponent<PlayerInputHandler>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Inventory = GetComponent<Inventory>();
          

        }
    }
}
