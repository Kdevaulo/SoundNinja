using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kdevaulo.SoundNinja
{
    [AddComponentMenu(nameof(SoundNinja) + "/" + nameof(SceneManagerService))]
    public sealed class SceneManagerService : MonoBehaviour
    {
        [SerializeField] private SceneName _startupScene;

        [SerializeField] private SceneData[] _sceneDataCollection;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private int _sceneForUnloadIndex;

        private void Start()
        {
            SwitchScene(_startupScene, LoadSceneMode.Additive, false);
        }

        public void SwitchScene(SceneName targetScene, LoadSceneMode loadSceneMode, bool unloadActiveScene = true)
        {
            _sceneForUnloadIndex = SceneManager.GetActiveScene().buildIndex;

            var targetSceneBuildIndex = GetSceneIndexByName(targetScene);

            var loadingOperation = SceneManager.LoadSceneAsync(targetSceneBuildIndex, loadSceneMode)
                .WithCancellation(_cts.Token);

            HandleSceneLoadingAsync(loadingOperation, targetSceneBuildIndex, unloadActiveScene).Forget();
        }

        private int GetSceneIndexByName(SceneName sceneName)
        {
            var sceneData = _sceneDataCollection.First(x => x.SceneName == sceneName);

            return sceneData.BuildIndex;
        }

        private async UniTask HandleSceneLoadingAsync(UniTask loadingOperation, int targetSceneIndex,
            bool unloadActiveScene)
        {
            await loadingOperation;

            SetSceneActive(targetSceneIndex);

            if (unloadActiveScene)
            {
                SceneManager.UnloadSceneAsync(_sceneForUnloadIndex).WithCancellation(_cts.Token).Forget();
            }
        }

        private void SetSceneActive(int sceneBuildIndex)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneBuildIndex));
        }
    }
}