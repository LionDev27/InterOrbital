using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InterOrbital.Others
{
    public class LevelManager : MonoBehaviour
    {
        private AsyncOperation _loadingScene;
        private float _loadingProgress;

        public LoadingBarsController loadingBarsController;
        public float LoadingProgress => _loadingProgress;
        
        public static LevelManager Instance;
        
        private void Awake()
        {
            if (Instance)
                Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public async void LoadSceneWithLoading(string sceneName)
        {
            _loadingScene = SceneManager.LoadSceneAsync(sceneName);
            _loadingScene.allowSceneActivation = false;

            do
            {
                await Task.Delay(100);
                _loadingProgress = _loadingScene.progress;
            } while (_loadingScene.progress < 0.9f);

            await Task.Delay(1000);

            _loadingProgress = _loadingScene.progress;
        }

        public void AllowSceneActivation()
        {
            if (_loadingScene == null) return;
            _loadingScene.allowSceneActivation = true;
            _loadingScene = null;
        }
    }
}
