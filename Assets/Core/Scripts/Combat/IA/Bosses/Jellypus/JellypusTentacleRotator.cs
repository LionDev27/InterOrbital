using System.Collections.Generic;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusTentacleRotator : TargetRotator
    {
        [SerializeField] private List<Transform> _tentacles;

        private void OnEnable()
        {
            Rotate();
        }

        public override void Rotate()
        {
            base.Rotate();
            foreach (var tentacle in _tentacles)
                tentacle.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -_lookRotation.z));
        }
    }
}