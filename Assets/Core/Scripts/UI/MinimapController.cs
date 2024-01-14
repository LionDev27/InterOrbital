using DG.Tweening;
using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.UI
{
    public class MinimapController : MonoBehaviour
    {
        [SerializeField] private Camera _minimapCamera;
        [SerializeField] private List<GameObject> _minimapSprites;
        private RectTransform _minimapFrameRect;
        private bool _minimapIsOpen;
        private bool _animationIsOn;

        private float _animationDuration = 0.5f;
        private float _orthographicOpenSize;
        private float _orthographicMinimizedSize;
        private float _scaleMinimapIcons;

        private void Awake()
        {
            _minimapFrameRect = GetComponent<RectTransform>();
            _orthographicMinimizedSize = _minimapCamera.orthographicSize;
        }

        private void Start()
        {
            GetMinimapCameraSizes();
            _scaleMinimapIcons = GridLogic.Instance.width / 100;
        }

        public void ToggleMinimap()
        {
            if (!_animationIsOn)
            {
                _animationIsOn = true;
                if (!_minimapIsOpen)
                {
                    AudioManager.Instance.PlaySFX("UIMenu");
                    DOTween.To(() => _minimapFrameRect.offsetMin, x => _minimapFrameRect.offsetMin = x, new Vector2(36, 36), _animationDuration).SetEase(Ease.Linear);
                    ChangeMinimapSpriteSize(false, _animationDuration, _scaleMinimapIcons);
                    _minimapCamera.GetComponent<CameraFollow>().OpenMinimap();
                    DOTween.To(() => _minimapCamera.orthographicSize, x => _minimapCamera.orthographicSize = x, _orthographicOpenSize, _animationDuration).SetEase(Ease.Linear);
                    Vector3 centerMapPos = new Vector3(GridLogic.Instance.width / 2, GridLogic.Instance.height / 2, _minimapCamera.transform.position.z);
                    _minimapCamera.transform.DOMove(centerMapPos, _animationDuration).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            _animationIsOn = false;
                        });

                    _minimapIsOpen = true;
                }
                else
                {
                    AudioManager.Instance.PlaySFX("UIMenuReverse");
                    DOTween.To(() => _minimapFrameRect.offsetMin, x => _minimapFrameRect.offsetMin = x, new Vector2(636, 636), _animationDuration).SetEase(Ease.Linear);
                    DOTween.To(() => _minimapCamera.orthographicSize, x => _minimapCamera.orthographicSize = x, _orthographicMinimizedSize, _animationDuration).SetEase(Ease.Linear);
                    ChangeMinimapSpriteSize(true, _animationDuration, _scaleMinimapIcons);
                    Vector3 playerPos = PlayerComponents.Instance.GetPlayerPosition();
                    float posX = Mathf.Clamp(playerPos.x, _orthographicMinimizedSize, GridLogic.Instance.width - _orthographicMinimizedSize);
                    float posY = Mathf.Clamp(playerPos.y, _orthographicMinimizedSize, GridLogic.Instance.width - _orthographicMinimizedSize);
                    Vector3 newPos = new Vector3(posX, posY, -_orthographicMinimizedSize);
                    _minimapCamera.transform.DOMove(new Vector3(newPos.x, newPos.y, _minimapCamera.transform.position.z), _animationDuration).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            _minimapCamera.GetComponent<CameraFollow>().CloseMinimap();
                            _minimapIsOpen = false;
                            _animationIsOn = false;
                        });

                }
            }
        }

        public void AddToMinimapSprites(GameObject minimapSprite)
        {
            _minimapSprites.Add(minimapSprite);
        }

        public void RemoveFromMinimapSprites(GameObject minimapSprite)
        {
            _minimapSprites.Remove(minimapSprite);
        }

        private void GetMinimapCameraSizes()
        {
            _orthographicOpenSize = GridLogic.Instance.width / 2;
        }

        private void ChangeMinimapSpriteSize(bool minimize, float duration, float scale)
        {
            if (minimize)
            {
                for (int i = 0; i < _minimapSprites.Count; i++)
                {
                    GameObject minimapSprite = _minimapSprites[i];
                    minimapSprite.transform.DOScale(Vector3.one, duration).OnComplete(() =>
                    {
                        if (minimapSprite.GetComponent<CircleCollider2D>() != null)
                        {
                            minimapSprite.GetComponent<CircleCollider2D>().radius *= scale;
                        }
                    });
                }
            }
            else
            {
                for (int i = 0; i < _minimapSprites.Count; i++)
                {
                    if (_minimapSprites[i].GetComponent<CircleCollider2D>() != null)
                    {
                        _minimapSprites[i].GetComponent<CircleCollider2D>().radius /= scale;
                    }
                    _minimapSprites[i].transform.DOScale(new Vector3(scale, scale, 1), duration);
                        
                }
            }
            
        }
    }
}
