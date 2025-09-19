using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;
using UnityEngine;

namespace Games.GameB
{
    public class GameBLauncher : IGameLauncher
    {
        private readonly IGameControllerRunner _controllerRunner;
        private readonly IGameSceneProvider _sceneProvider;

        public GameBLauncher(IGameSceneProvider sceneProvider,
                             IGameControllerRunner controllerRunner)
        {
            _sceneProvider = sceneProvider;
            _controllerRunner = controllerRunner;
        }

        public UniTask LaunchAsync(CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();
            var view = _sceneProvider.Scene
                                     .GetRootGameObjects()
                                     .Select(x => x.GetComponent<SceneBView>())
                                     .FirstOrDefault(x => x != null);

            if (view == null)
            {
                throw new Exception("SceneBView not found in the scene");
            }

            view.OnCloseRequested += () => tcs.TrySetResult();
            view.OnExceptionRequested += () => tcs.TrySetException(new Exception("GameB Raise test exception"));
            view.OnDisplayPopupRequested += () => DisplayPopupFlow(view.transform, token).Forget();

            return tcs.Task;
        }

        private async UniTaskVoid DisplayPopupFlow(Transform popupRoot, CancellationToken token)
        {
            await _controllerRunner.ExecuteAndWaitResultAsync<GameBPopupLaunchController, Transform>(popupRoot, token);
        }
    }
}