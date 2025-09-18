using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;

namespace Games.GameB
{
    public class GameBLauncher : IGameLauncher
    {
        public UniTask LaunchAsync(IGameControllerRunner runner, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();
            var view = runner.Scene.GetRootGameObjects()
                             .Select(x => x.GetComponent<SceneBView>())
                             .FirstOrDefault(x => x != null);

            if (view == null)
            {
                throw new Exception("SceneBView not found in the scene");
            }

            view.OnCloseRequested += () => tcs.TrySetResult();
            view.OnExceptionRequested += () => tcs.TrySetException(new Exception("GameB Raise test exception"));

            return tcs.Task;
        }
    }
}