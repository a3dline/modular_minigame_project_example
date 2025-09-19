using System;
using System.Collections.Generic;
using System.Threading;
using AUniTaskSemaphore;
using Cysharp.Threading.Tasks;
using Game.Common;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Game.GameManager
{
    internal class RuntimeBundleProvider : IBundleProvider, IDisposable
    {
        private readonly ILoggerService _logger;
        private readonly UniTaskSemaphore _semaphore = new(1);
        private readonly List<AssetBundle> _loadedBundles = new();

        public RuntimeBundleProvider(ILoggerService logger)
        {
            _logger = logger;
        }

        public async UniTask<IBundleHolder> LoadAsync(string bundleName, CancellationToken token)
        {
            using var _ = await _semaphore.Acquire(token);
            var bundlePath = $"{Application.streamingAssetsPath}/{bundleName}";
            var request = UnityWebRequestAssetBundle.GetAssetBundle(bundlePath);
            await request.SendWebRequest().WithCancellation(token);
            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to load AssetBundle from {bundlePath}: {request.error}");
            }

            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            _loadedBundles.Add(bundle);
            _logger.Log("RuntimeBundleProvider", $"Loaded bundle from '{bundlePath}'.");
            return new BundleHolder(bundle, _logger);
        }

        public void Dispose()
        {
            foreach (var bundle in _loadedBundles)
            {
                if (bundle == null)
                {
                    continue;
                }

                _logger.Log("RuntimeBundleProvider", $"Unloading bundle '{bundle.name}' during provider disposal.");
                bundle.Unload(true);
            }
        }

        private class BundleHolder : IBundleHolder
        {
            private readonly AssetBundle _bundle;
            private readonly ILoggerService _logger;

            public BundleHolder(AssetBundle bundle, ILoggerService logger)
            {
                _bundle = bundle;
                _logger = logger;
            }

            public void Dispose()
            {
                _logger.Log("RuntimeBundleProvider", $"Unloading bundle '{_bundle.name}'.");
                _bundle.Unload(true);
            }

            public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken token) where T : Object
            {
                return (T)await _bundle.LoadAssetAsync<T>(assetName).ToUniTask(cancellationToken: token);
            }
        }
    }
}