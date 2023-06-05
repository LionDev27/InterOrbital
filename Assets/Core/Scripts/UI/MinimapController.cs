using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterOrbital.UI
{
    public class MinimapController : MonoBehaviour
    {
        private RectTransform _minimapImageRect;
        private bool _minimapIsOpen;

        private void Awake()
        {
            _minimapImageRect = GetComponent<RectTransform>();
        }

        public void ToggleMinimap()
        {
            if (!_minimapIsOpen)
            {
                DOTween.To(() => _minimapImageRect.offsetMin, x => _minimapImageRect.offsetMin = x, new Vector2(36, 36), 0.5f);

                _minimapIsOpen = true;
            }
            else
            {
                DOTween.To(() => _minimapImageRect.offsetMin, x => _minimapImageRect.offsetMin = x, new Vector2(636, 636), 0.5f);


                _minimapIsOpen = false;
            }
        }

    }
}
