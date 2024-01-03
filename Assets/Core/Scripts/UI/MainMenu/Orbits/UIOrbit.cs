using System;
using UnityEngine;

namespace InterOrbital.UI
{
    public class UIOrbit : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _speed;
        private Transform _myTransform;

        private void Start()
        {
            _myTransform = transform;
        }

        private void Update()
        {
            RotateAroundPivot();
        }

        private void RotateAroundPivot()
        {
            _myTransform.RotateAround(_pivot.position, new Vector3(0, 0, 1), -_speed * Time.deltaTime);
        }

        public void Flip()
        {
            var currentPos = _myTransform.position;
            currentPos = new Vector3(-currentPos.x, currentPos.y, currentPos.z);
            _myTransform.position = currentPos;

            var currentRot = _myTransform.rotation;
            currentRot = new Quaternion(currentRot.x, currentRot.y, -currentRot.z, currentRot.w);
            _myTransform.rotation = currentRot;
        }
    }
}
