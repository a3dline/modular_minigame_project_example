using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.GameManager
{
    internal interface ISceneLoader
    {
        UniTask<ISceneHolder> LoadAsync(string sceneName, CancellationToken token);
    }
}