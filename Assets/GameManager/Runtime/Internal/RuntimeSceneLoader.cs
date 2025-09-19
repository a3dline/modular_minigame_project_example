using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    internal class RuntimeSceneLoader : ISceneLoader
    {
        public async UniTask<ISceneHolder> LoadAsync(string sceneName, CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
            return new SceneHolder(sceneName);
        }

        private class SceneHolder : ISceneHolder
        {
            public SceneHolder(string sceneName)
            {
                Scene = SceneManager.GetSceneByName(sceneName);
            }

            public Scene Scene { get; }

            public UniTask DisposeAsync()
            {
                return SceneManager.UnloadSceneAsync(Scene).ToUniTask();
            }
        }
    }
}