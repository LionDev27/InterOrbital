using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InterOrbital.Others
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _blackoutCanvasGroup;
        [SerializeField] private CanvasGroup _endGameCanvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _playPanel;

        private AsyncOperation _loadingScene;
        private float _loadingProgress;
        private float _fillAmount;
        private bool _loading;

        public CanvasGroup _loadingCanvasGroup;
        public LoadingBarsController loadingBarsController;

        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            EnableCanvasGroup(_blackoutCanvasGroup, true);
            EnableCanvasGroup(_loadingCanvasGroup, false);
            EnableCanvasGroup(_endGameCanvasGroup, false);
            EnableCanvasGroup(_backgroundCanvasGroup, false);
            _playPanel.SetActive(false);
            _blackoutCanvasGroup.DOFade(0f, 1f).OnComplete((() => EnableCanvasGroup(_blackoutCanvasGroup, false)));
        }

        private void Update()
        {
            if (_loading)
                UpdateLoadingBar();
        }

        private void UpdateLoadingBar()
        {
            _fillAmount = Mathf.MoveTowards(_fillAmount, _loadingProgress, Time.deltaTime);
            loadingBarsController.UpdateBarFills(1f, _fillAmount);
            if (_fillAmount >= 1f)
            {
                _loading = false;
                EnableCanvasGroup(_backgroundCanvasGroup, true);
                AllowSceneActivation();
            }
        }

        public void PlayGame()
        {
            Sequence playSequence = DOTween.Sequence();
            playSequence.Append(_blackoutCanvasGroup.DOFade(1f, 1f));
            playSequence.Append(_loadingCanvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
            {
                EnableCanvasGroup(_loadingCanvasGroup, false);
                EnableCanvasGroup(_backgroundCanvasGroup, false);
            }));
            playSequence.Append(_blackoutCanvasGroup.DOFade(0f, 2f));
            playSequence.Play();
        }

        public void BackMenu()
        {
            Sequence backSequence = DOTween.Sequence();
            backSequence.Append(_endGameCanvasGroup.DOFade(1f, 1f));
            backSequence.Append(_blackoutCanvasGroup.DOFade(1f, 1f).SetDelay(3f)
                .OnComplete(() => LoadScene("Main Screen")));
            _endGameCanvasGroup.alpha = 0f;
            backSequence.Play();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public async void LoadSceneWithLoading(string sceneName)
        {
            _fillAmount = 0f;
            _loading = true;
            _loadingScene = SceneManager.LoadSceneAsync(sceneName);
            _loadingScene.allowSceneActivation = false;

            do
            {
                await Task.Delay(100);
                _loadingProgress = _loadingScene.progress;
            } while (_loadingScene.progress < 0.9f);

            await Task.Delay(1000);

            _loadingProgress = 1f;
        }

        public void AllowSceneActivation()
        {
            if (_loadingScene == null) return;
            _loadingScene.allowSceneActivation = true;
            _loadingScene = null;
        }

        public void EnableCanvasGroup(CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }
}