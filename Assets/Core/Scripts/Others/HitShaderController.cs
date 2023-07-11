using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Others
{
    public class HitShaderController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Hit(bool value)
        {
            _spriteRenderer.material.SetInt("_Hit", value ? 1 : 0);
        }

        public void Hit(int value)
        {
            if (value > 1)
                value = 1;
            else if (value < 0)
                value = 0;
            
            _spriteRenderer.material.SetInt("_Hit", value);
        }

        public bool HitValue()
        {
            return _spriteRenderer.material.GetInt("_Hit") != 0;
        }
    }
}
