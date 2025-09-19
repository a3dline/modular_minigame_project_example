using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.GameManager
{
    public interface IGameLauncher
    {
        UniTask LaunchAsync(CancellationToken token);
    }
}