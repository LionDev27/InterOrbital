using System;
using UnityEngine;

namespace InterOrbital.Utils
{
    public class SpriteMaskChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _rendererToApplyMask;
        [SerializeField] private bool updateSprite;
        private SpriteMask _spriteMask;

        private void Awake()
        {
            _spriteMask = GetComponent<SpriteMask>();
        }

        private void Start()
        {
            if (!updateSprite)
                _spriteMask.sprite = _rendererToApplyMask.sprite;
        }

        private void Update()
        {
            if (updateSprite)
            {
                ChangeSpriteMask();
                FlipSpriteMask();
            }
        }

        private void ChangeSpriteMask()
        {
            if (_rendererToApplyMask.sprite != _spriteMask.sprite)
                _spriteMask.sprite = _rendererToApplyMask.sprite;
        }

        private void FlipSpriteMask()
        {
            Vector2 rotation = transform.rotation.eulerAngles;
            rotation.y = _rendererToApplyMask.flipX ? 180f : 0f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
