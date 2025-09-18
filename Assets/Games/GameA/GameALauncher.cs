using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;

namespace Games.GameA
{
    public class GameALauncher : IGameLauncher
    {
        public UniTask LaunchAsync(IGameControllerRunner runner, CancellationToken token)
        {
            return runner.ExecuteAndWaitResultAsync<GameAController>(token);
        }
    }
}