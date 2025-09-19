using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.GameManager
{
    internal class FakeAssetBundleHolder : IBundleHolder
    {
        public void Dispose()
        {
            
        }

        public UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken token) where T : Object
        {
            return new UniTask<T>(null);
        }
    }
}