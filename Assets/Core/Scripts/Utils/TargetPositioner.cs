using UnityEngine;

namespace InterOrbital.Utils
{
    public class TargetPositioner : MonoBehaviour
    {
        public Transform target;

        public virtual void Move()
        {
            if (target == null) return;
            transform.position = target.position;
        }
    }
}