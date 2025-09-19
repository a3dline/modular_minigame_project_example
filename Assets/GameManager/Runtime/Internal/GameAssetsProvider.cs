using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.GameManager
{
    internal class GameAssetsProvider : IGameAssetsProvider
    {
        private readonly IBundleHolder _holder;
        public GameAssetsProvider(IBundleHolder holder)
        {
            _holder = holder;
        }

        public UniTask<T> LoadAsync<T>(string assetName, CancellationToken cancellationToken) where T : Object
        {
            return _holder.LoadAssetAsync<T>(assetName, cancellationToken);
        }
    }
}