using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.GameManager
{
    internal interface IBundleProvider
    {
        UniTask<IBundleHolder> LoadAsync(string bundleName, CancellationToken token);
    }
}