using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;

namespace Game.GameManager
{
    internal class EditorBundleProvider : IBundleProvider
    {
        private readonly ILoggerService _logger;

        public EditorBundleProvider(ILoggerService logger)
        {
            _logger = logger;
        }

        public async UniTask<IBundleHolder> LoadAsync(string bundleName, CancellationToken token)
        {
            _logger.Log("EditorBundleProvider", $"Simulating loading bundle '{bundleName}' in editor for 1 sec.");
            await UniTask.Delay(1000, cancellationToken: token);
            return new BundleHolder(bundleName, _logger);
        }

        private class BundleHolder : IBundleHolder
        {
            private readonly string _bundleName;
            private readonly ILoggerService _logger;

            public BundleHolder(string bundleName, ILoggerService logger)
            {
                _bundleName = bundleName;
                _logger = logger;
            }

            public void Dispose()
            {
                _logger.Log("EditorBundleProvider", $"Unloading bundle '{_bundleName}' in editor.");
            }
        }
    }
}