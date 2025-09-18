using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Game.GameManager
{
    public interface IGameLauncher
    {
        UniTask LaunchAsync(IGameControllerRunner runner, CancellationToken token);
    }
}