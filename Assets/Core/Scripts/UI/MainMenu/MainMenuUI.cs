using DG.Tweening;
using InterOrbital.Others;
using UnityEngine;

namespace InterOrbital.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _mainCanvasGroup;
        private CanvasGroup _activeCanvas = new();

        private LevelManager _levelManager;

        private void Start()
        {
            _levelManager = LevelManager.Instance;
            _levelManager.EnableCanvasGroup(_mainCanvasGroup, false);
            _activeCanvas = _mainCanvasGroup;
            ShowMainMenu();
        }

        public void ShowMenu(CanvasGroup canvas)
        {
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(_activeCanvas.DOFade(0f, 0.5f).OnComplete(() => _levelManager.EnableCanvasGroup(_activeCanvas, false)));
            showSequence.Append(canvas.DOFade(1f, 0.5f).OnComplete(() => _levelManager.EnableCanvasGroup(canvas, true)));
            _activeCanvas = canvas;
        }
        
        private void ShowMainMenu()
        {
            _mainCanvasGroup.DOFade(1f, 2f).OnComplete(() => _levelManager.EnableCanvasGroup(_mainCanvasGroup, true)).Play();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ShowLoadingCanvas()
        {
            Sequence loadingSequence = DOTween.Sequence();
            loadingSequence.Append(_mainCanvasGroup.DOFade(0f, 0.5f).OnComplete(() => _levelManager.EnableCanvasGroup(_mainCanvasGroup, false)));
            loadingSequence.Append(_levelManager._loadingCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                _levelManager.EnableCanvasGroup(_levelManager._loadingCanvasGroup, true);
                _levelManager.LoadSceneWithLoading("Game");
            }));
            loadingSequence.Play();
        }
    }
}