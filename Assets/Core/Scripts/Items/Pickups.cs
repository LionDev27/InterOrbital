using System;
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

        private void Update()
        {
            PlayerReached();
        }

        private void FixedUpdate()
        {
            if (playerT)
            {
                Vector2 direction = (playerT.position - transform.position).normalized;
                _rigidbody.velocity = direction * 10f;
            }
        }

        private void PlayerReached()
        {
            if (playerT)
            {
                if (Vector3.Distance(playerT.position, transform.position) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}