using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;

namespace InterOrbital.Others
{
    [RequireComponent(typeof(Collider2D))]
    public class SpriteMaskController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private SpriteRenderer _gunSpriteRenderer;
        [SerializeField] private SpriteMask _spriteMask;
        [SerializeField] private OrderInLayerController _olControllerPlayer;
        [SerializeField] private OrderInLayerController _olControllerGun;
        private Collider2D _spriteMaskCollider;
        private List<SpriteRenderer> _otherRenderers;

        public bool checking = false;

        private void Awake()
        {
            _otherRenderers = new List<SpriteRenderer>();
            _spriteMaskCollider = GetComponent<Collider2D>();
            _spriteMaskCollider.isTrigger = true;
        }

        void Update()
        {
            if (_otherRenderers.Count > 0)
            {
                _olControllerPlayer.SetCanChange(false);
                _olControllerGun.SetCanChange(false);
                foreach (var renderer in _otherRenderers)
                {
                    if (_playerSpriteRenderer.transform.position.y > renderer.transform.position.y)
                    {
                        _playerSpriteRenderer.sortingOrder = renderer.sortingOrder - 1;
                        _gunSpriteRenderer.sortingOrder = renderer.sortingOrder - 1 ;
                        renderer.ChangueSpriteAlphaColor(0.4f);
                    }
                    else
                    {
                        renderer.ChangueSpriteAlphaColor(1f);
                    }
                }
                // foreach(SpriteRenderer renderer in _otherRenderers)
                // {
                //     if(_playerSpriteRenderer.sortingLayerName == renderer.sortingLayerName
                //         && _playerSpriteRenderer.sortingOrder <= renderer.sortingOrder
                //         && _playerSpriteRenderer.transform.position.y > renderer.transform.position.y)
                //     {
                //         //_spriteMask.enabled = true;
                //         //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                //         renderer.ChangueSpriteAlphaColor(0.4f);
                //         return;
                //     }
                //     else
                //     {
                //         renderer.ChangueSpriteAlphaColor(1f);
                //         //_spriteMask.enabled = false;
                //         //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                //     }
                // }
            }
            else
            {
                _olControllerPlayer.SetCanChange(true);
                _olControllerGun.SetCanChange(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && collision.CompareTag("ColliderMask"))
            {
                _otherRenderers.Add(spriteRenderer);
                //checking = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            SpriteRenderer spriteRenderer = collision.GetComponentInChildren<SpriteRenderer>();
            if (_otherRenderers.Contains(spriteRenderer))
            {
                spriteRenderer.ChangueSpriteAlphaColor(1f);
                _otherRenderers.Remove(spriteRenderer);
            }
            // if(spriteRenderer != null && collision.CompareTag("StaticObject"))
            // {
            //     //checking = false;
            //     //spriteRenderer.ChangueSpriteAlphaColor(1f);
            //     //_spriteMask.enabled = false;
            //     //_playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None; 
            // }
        }
    }
}
