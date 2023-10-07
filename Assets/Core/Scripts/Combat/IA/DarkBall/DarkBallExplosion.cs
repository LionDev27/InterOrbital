using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class DarkBallExplosion : DamageDealer
    {
        private DarkBall _darkBall;
        private bool _isDead = false;
        public float radiusExplosion;

        private void Awake()
        {
            _darkBall = GetComponentInParent<DarkBall>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !_isDead)
            {
                _darkBall.DeathDarkBall();
            }
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusExplosion);
        }

        public void Explode()
        {
            Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radiusExplosion);
            foreach (Collider2D c in objs)
            {
                CheckCollision(c);
            }
        }
    }
}

