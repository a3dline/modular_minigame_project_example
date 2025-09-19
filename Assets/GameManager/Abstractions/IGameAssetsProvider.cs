using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.GameManager
{
    public interface IGameAssetsProvider
    {
        UniTask<T> LoadAsync<T>(string assetName, CancellationToken cancellationToken) where T : UnityEngine.Object;
    }
}