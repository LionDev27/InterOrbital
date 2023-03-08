using UnityEngine;

namespace InterOrbital.Items
{
    public class Pickups : MonoBehaviour
    {
        public Transform playerT;

        private Rigidbody2D _rigidbody;
        private float _minSpeed = 5f;
        private float _maxSpeed = 30f;
        private float accelerationTime = 0.5f;
        private float distanceToBeCollected = 0.5f;
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
            MoveToPlayer();
        }

        private void MoveToPlayer()
        {
            if (playerT)
            {
                Vector2 direction = (playerT.position - transform.position).normalized;
                float newVelocity = Mathf.Max(_minSpeed,_rigidbody.velocity.magnitude);
    
                if (_rigidbody.velocity.magnitude < _maxSpeed)   // Si la velocidad actual es menor que la velocidad máxima
                {
                    float t = Time.fixedDeltaTime / accelerationTime;  // Tiempo para acelerar
                    newVelocity  = Mathf.Lerp(newVelocity, _maxSpeed, t);  // Lerp de la velocidad actual a la velocidad máxima

                }
                Debug.Log(newVelocity);
                _rigidbody.velocity = direction * newVelocity;   // Establecer la velocidad del Rigidbody2D
            }
            
            
        }

        private void PlayerReached()
        {
            if (playerT)
            {
                if (Vector3.Distance(playerT.position, transform.position) < distanceToBeCollected)
                {
                    //TO DO Add event to update UI
                    Destroy(gameObject);
                }
            }
        }
    }
}