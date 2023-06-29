using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace InterOrbital.Utils
{
    public class SpriteMaskChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _rendererToApplyMask;
        [SerializeField] private bool _updateSprite;
        [SerializeField] private bool _changePosition;
        private SpriteMask _spriteMask;

        private void Awake()
        {
            _spriteMask = GetComponent<SpriteMask>();
        }

        private void Start()
        {
            if (!_updateSprite)
                _spriteMask.sprite = _rendererToApplyMask.sprite;
            if (_changePosition)
                transform.position = _rendererToApplyMask.transform.position;
        }

        private void Update()
        {
            if (_updateSprite)
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
