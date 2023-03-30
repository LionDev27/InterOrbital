using InterOrbital.Item;
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
                    
                    ItemObject item =collider.gameObject.GetComponent<ItemObject>();
                    if(!item.DropingItem)
                        item.playerT = transform;
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
