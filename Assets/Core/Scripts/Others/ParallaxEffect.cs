using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Others
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            _material.mainTextureOffset += (_speed * Time.deltaTime * Vector2.right);
        }
    }
}
