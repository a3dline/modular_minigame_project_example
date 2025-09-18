using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    internal class EditorSceneLoader : ISceneLoader
    {
        public async UniTask<ISceneHolder> LoadAsync(string sceneName, CancellationToken token)
        {
            var scenePath = AssetDatabase
                            .FindAssets("t:Scene")
                            .Select(AssetDatabase.GUIDToAssetPath)
                            .First(x => Path.GetFileNameWithoutExtension(x) == sceneName);
            var parameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None);
            await EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath, parameters)
                                    .ToUniTask(cancellationToken: token);

            return new Holder(SceneManager.GetSceneByName(sceneName));
        }

        private class Holder : ISceneHolder
        {
            public Holder(Scene scene)
            {
                Scene = scene;
            }

            public Scene Scene { get; }

            public UniTask DisposeAsync()
            {
                return SceneManager.UnloadSceneAsync(Scene).ToUniTask();
            }
        }
    }
}