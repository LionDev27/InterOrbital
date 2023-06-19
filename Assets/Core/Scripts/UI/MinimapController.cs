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
        private RectTransform _minimapFrameRect;
        private bool _minimapIsOpen;

        private float _animationDuration = 0.5f;
        private float _orthographicOpenSize;
        private float _orthographicMinimizedSize;

        private void Awake()
        {
            _minimapFrameRect = GetComponent<RectTransform>();
            _orthographicMinimizedSize = _minimapCamera.orthographicSize;
        }

        private void Start()
        {
            GetMinimapCameraSizes();
        }

        public void ToggleMinimap()
        {
            if (!_minimapIsOpen)
            {
                DOTween.To(() => _minimapFrameRect.offsetMin, x => _minimapFrameRect.offsetMin = x, new Vector2(36, 36), _animationDuration).SetEase(Ease.Linear);
                _minimapCamera.GetComponent<CameraFollow>().OpenMinimap();
                DOTween.To(() => _minimapCamera.orthographicSize, x => _minimapCamera.orthographicSize = x, _orthographicOpenSize, _animationDuration).SetEase(Ease.Linear);
                Vector3 centerMapPos = new Vector3(GridLogic.Instance.width / 2, GridLogic.Instance.height / 2, _minimapCamera.transform.position.z);
                _minimapCamera.transform.DOMove(centerMapPos, _animationDuration).SetEase(Ease.Linear);

                _minimapIsOpen = true;
            }
            else
            {
                DOTween.To(() => _minimapFrameRect.offsetMin, x => _minimapFrameRect.offsetMin = x, new Vector2(636, 636), _animationDuration).SetEase(Ease.Linear);
                DOTween.To(() => _minimapCamera.orthographicSize, x => _minimapCamera.orthographicSize = x, _orthographicMinimizedSize, _animationDuration).SetEase(Ease.Linear);
                Vector3 playerPos = PlayerComponents.Instance.GetPlayerPosition();
                float posX = Mathf.Clamp(playerPos.x, _orthographicMinimizedSize, GridLogic.Instance.width - _orthographicMinimizedSize);
                float posY = Mathf.Clamp(playerPos.y, _orthographicMinimizedSize, GridLogic.Instance.width - _orthographicMinimizedSize);
                Vector3 newPos = new Vector3(posX, posY, -_orthographicMinimizedSize);
                _minimapCamera.transform.DOMove(new Vector3(newPos.x, newPos.y, _minimapCamera.transform.position.z), _animationDuration).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _minimapCamera.GetComponent<CameraFollow>().CloseMinimap();
                        _minimapIsOpen = false;
                    });

            }
        }

        private void GetMinimapCameraSizes()
        {
            _orthographicOpenSize = GridLogic.Instance.width / 2;
        }

    }
}
