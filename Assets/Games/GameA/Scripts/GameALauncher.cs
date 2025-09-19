using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;

namespace Games.GameA
{
    public class GameALauncher : IGameLauncher
    {
        private readonly IGameControllerRunner _controllerRunner;
        private readonly IGameSceneProvider _sceneProvider;

        public GameALauncher(IGameControllerRunner controllerRunner)
        {
            _controllerRunner = controllerRunner;
        }

        public UniTask LaunchAsync(CancellationToken token)
        {
            return _controllerRunner.ExecuteAndWaitResultAsync<GameASceneViewController>(token);
        }
    }
}