using System;
using UnityEngine;

namespace InterOrbital.UI
{
    public class OrbitFlipper : MonoBehaviour
    {
        private const string OrbitalObjectTag = "UIOrbital";
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(OrbitalObjectTag))
                col.GetComponent<UIOrbit>().Flip();
        }
    }
}