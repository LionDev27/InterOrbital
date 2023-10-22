using UnityEngine;

namespace InterOrbital.Utils
{
    public class TargetRotator : MonoBehaviour
    {
        public Transform target;
        protected Vector3 _lookRotation = new();
        
        public virtual void Rotate()
        {
            if (target == null) return;
            Vector2 targetDir = (target.position - transform.position).normalized;
            var rot = Quaternion.LookRotation(Vector3.forward, targetDir);
            _lookRotation = rot.eulerAngles;
            transform.rotation = Quaternion.Euler(_lookRotation);
        }
    }
}