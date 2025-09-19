using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.GameManager.Editor;
using UnityEditor;
using UnityEngine;

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
            _logger.Log("EditorBundleProvider", $"Simulating loading bundle '{bundleName}' in editor for 0.1 sec.");
            await UniTask.Delay(100, cancellationToken: token);
            return new BundleHolder(bundleName, _logger);
        }

        private class BundleHolder : IBundleHolder
        {
            private readonly string _bundleName;
            private readonly ILoggerService _logger;
            private readonly List<Object> _assets;

            public BundleHolder(string bundleName, ILoggerService logger)
            {
                _bundleName = bundleName;
                _logger = logger;
                
                //game_a.scene.bundle
                var bundleGameName = bundleName.Split('.')[0];

                var foldersEnumerator = AssetDatabase.FindAssets("t:DefaultAsset").Select(AssetDatabase.GUIDToAssetPath);
                foreach (var folder in foldersEnumerator)
                {
                    if (GameMetadataStorage.IsGameEnabled(folder))
                    {
                        var gameName = GameMetadataStorage.GetGameName(folder);
                        var expectedBundleName = gameName.Replace(' ', '_').ToLowerInvariant();
                        if (bundleGameName == expectedBundleName)
                        {
                            _assets = GameMetadataStorage.GetAllAssets(folder);
                            break;
                        }
                    }
                }
            }

            public void Dispose()
            {
                _logger.Log("EditorBundleProvider", $"Unloading bundle '{_bundleName}' in editor.");
            }

            public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken token) where T : Object
            {
                var asset = _assets.FirstOrDefault(x => x.name == assetName);
                await UniTask.Delay(100, cancellationToken: token);
                return asset as T;
            }
        }
    }
}