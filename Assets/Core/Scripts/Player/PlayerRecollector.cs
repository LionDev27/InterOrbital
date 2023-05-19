using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace InterOrbital.Player
{
    public class PlayerRecollector : PlayerComponents
    {
        [SerializeField] private float _recollectionRange;
        [SerializeField] private LayerMask _recollectionLayer;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Recollect();
            }
        }

        private void Recollect()
        {
            Vector2 dir = PlayerAttack.attackPoint.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _recollectionRange, _recollectionLayer);
            if (hit)
            {
                Debug.Log("Recollecting");
            }
            else
            {
                Debug.Log("Nothing to recollect");
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, _recollectionRange);
            }
        }
    }
}
