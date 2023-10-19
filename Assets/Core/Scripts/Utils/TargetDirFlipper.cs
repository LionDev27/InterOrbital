using UnityEngine;

namespace InterOrbital.Utils
{
    public class TargetDirFlipper : MonoBehaviour
    {
        public Transform target;

        public void Flip()
        {
            if (target == null) return;
            var dir = target.position - transform.position;
            var currentScale = transform.localScale;
            currentScale.x = dir.x switch
            {
                > 0f => 1,
                < 0f => -1,
                _ => currentScale.x
            };
            transform.localScale = currentScale;
        }
    }
}