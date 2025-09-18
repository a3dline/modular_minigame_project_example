using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;
using UnityEngine.SceneManagement;

namespace Games.GameA
{
    public class GameALauncher : IGameLauncher
    {
        public UniTask LaunchAsync(IGameControllerRunner runner, CancellationToken token)
        {
            return runner.ExecuteAndWaitResultAsync<GameASceneViewController, Scene>(runner.Scene, token);
        }
    }
}