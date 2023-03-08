using InterOrbital.Items;
using UnityEngine;

namespace InterOrbital.Player
{
    public class PlayerItemsCollector : MonoBehaviour
    {
        public float attractionRange = 5f;
        
        // Update is called once per frame
        void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRange);
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.CompareTag("Pickup"))
                {
                    collider.gameObject.GetComponent<Pickups>().playerT = transform;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attractionRange);
        }
    }
}
