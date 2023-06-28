using System;
using DG.Tweening;
using InterOrbital.Others;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InterOrbital.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private CanvasGroup _blackoutCanvasGroup;
        [SerializeField] private CanvasGroup _loadingCanvasGroup;

        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _playPanel;

        private LevelManager _levelManager;
        private float _fillAmount;
        private bool _loading;

        private void Start()
        {
            _levelManager = LevelManager.Instance;
            EnableCanvasGroup(_mainCanvasGroup, false);
            EnableCanvasGroup(_blackoutCanvasGroup, false);
            EnableCanvasGroup(_loadingCanvasGroup, false);
            _playPanel.SetActive(false);
            ShowMainMenu();
        }

        private void Update()
        {
            if (_loading)
                UpdateLoadingBar();
        }

        private void ShowMainMenu()
        {
            _mainCanvasGroup.DOFade(1f, 2f).OnComplete(() => EnableCanvasGroup(_mainCanvasGroup, true)).Play();
        }

        private void UpdateLoadingBar()
        {
            _fillAmount = Mathf.MoveTowards(_fillAmount, _levelManager.LoadingProgress, Time.deltaTime);
            _levelManager.loadingBarsController.UpdateBarFills(1f,_fillAmount);
            if (_fillAmount >= 1f)
            {
                _loading = false;
                _loadingPanel.SetActive(false);
                _playPanel.SetActive(true);
            }
        }
        
        private void EnableCanvasGroup(CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
        
        public void PlayGame()
        {
            Sequence playSequence = DOTween.Sequence();
            playSequence.Append(_loadingCanvasGroup.DOFade(0f, 0.5f).OnComplete(() => EnableCanvasGroup(_mainCanvasGroup, false)));
            playSequence.Join(_blackoutCanvasGroup.DOFade(1f, 1f).OnComplete(() =>
            {
                EnableCanvasGroup(_loadingCanvasGroup, false);
                _levelManager.AllowSceneActivation();
            }));
            playSequence.Append(_blackoutCanvasGroup.DOFade(0f, 2f));
            playSequence.Play();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ShowLoadingCanvas()
        {
            Sequence loadingSequence = DOTween.Sequence();
            loadingSequence.Append(_mainCanvasGroup.DOFade(0f, 0.5f).OnComplete(() => EnableCanvasGroup(_mainCanvasGroup, false)));
            loadingSequence.Append(_loadingCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                EnableCanvasGroup(_loadingCanvasGroup, true);
                _fillAmount = 0f;
                _levelManager.LoadSceneWithLoading("Game");
                _loading = true;
            }));
            loadingSequence.Play();
        }
    }

}