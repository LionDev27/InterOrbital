using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerItemsCollector : MonoBehaviour
    {
        public float attractionRange = 5f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRange);
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.CompareTag("Pickup")) {
                    Vector2 direction = (transform.position - collider.transform.position).normalized;
                    collider.attachedRigidbody.AddForce(direction * 10f);
                }
            }
        }
    }
}
