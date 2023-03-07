using UnityEngine;

namespace Core.Scripts.Items
{
    public class Pickups : MonoBehaviour
    {
        public Transform playerT;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (playerT)
            {
                Vector2 direction = (playerT.position - transform.position).normalized;
                _rigidbody.velocity = direction * 10f;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                //Añadir aqui los eventos de actualizar UI/Inventario
                Destroy(gameObject);
            }
        }
    }
}