using UnityEngine;

namespace InterOrbital.Player
{
    [RequireComponent(typeof(PlayerInputHandler), typeof(Rigidbody2D))]
    public class PlayerComponents : MonoBehaviour
    {
        protected PlayerInputHandler InputHandler { get; private set; }
        protected Rigidbody2D Rigidbody { get; private set; }

        protected virtual void Awake()
        {
            InputHandler = GetComponent<PlayerInputHandler>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
