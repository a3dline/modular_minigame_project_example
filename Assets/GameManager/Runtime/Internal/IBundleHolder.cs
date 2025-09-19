using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.GameManager
{
    internal interface IBundleHolder : IDisposable
    {
        public UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken token) where T : UnityEngine.Object;
    }
}