using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Playtika.Controllers;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Game.GameManager
{
    internal class GameControllerRunner : ControllerWithResultBase<GameData, EmptyControllerResult>, IGameControllerRunner
    {
        private readonly GameAssetsProviderFactory _assetsProviderFactory;
        private readonly IBundleProvider _bundleProvider;
        private readonly IObjectResolver _resolver;
        private readonly ISceneLoader _sceneLoader;
        private readonly GameSceneProviderFactory _sceneProviderFactory;
        private IControllerFactory _controllersFactory;

        public GameControllerRunner(IControllerFactory controllerFactory,
                                    IBundleProvider bundleProvider,
                                    ISceneLoader sceneLoader,
                                    GameAssetsProviderFactory assetsProviderFactory,
                                    GameSceneProviderFactory sceneProviderFactory,
                                    IObjectResolver resolver)
            : base(controllerFactory)
        {
            _bundleProvider = bundleProvider;
            _sceneLoader = sceneLoader;
            _assetsProviderFactory = assetsProviderFactory;
            _sceneProviderFactory = sceneProviderFactory;
            _resolver = resolver;
        }

        public Scene Scene { get; private set; }

        public new UniTask<TResult> ExecuteAndWaitResultAsync<T, TArg, TResult>(TArg arg, CancellationToken token)
            where T : class, IControllerWithResult<TResult>, IController<TArg>
        {
            return base.ExecuteAndWaitResultAsync<T, TArg, TResult>(arg, _controllersFactory, token);
        }

        public new UniTask ExecuteAndWaitResultAsync<T, TArg>(TArg arg, CancellationToken token)
            where T : class, IControllerWithResult<EmptyControllerResult>, IController<TArg>
        {
            return base.ExecuteAndWaitResultAsync<T, TArg, EmptyControllerResult>(arg, _controllersFactory, token);
        }

        public new UniTask<TResult> ExecuteAndWaitResultAsync<T, TResult>(CancellationToken token)
            where T : class, IControllerWithResult<TResult>, IController<EmptyControllerArg>
        {
            return base.ExecuteAndWaitResultAsync<T, TResult>(_controllersFactory, token);
        }

        public new UniTask ExecuteAndWaitResultAsync<T>(CancellationToken token) where T : class, IControllerWithResult<EmptyControllerResult>, IController<EmptyControllerArg>
        {
            return base.ExecuteAndWaitResultAsync<T>(_controllersFactory, token);
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var t1 = _bundleProvider.LoadAsync(Args.SceneBundleName, cancellationToken);
            var t2 = string.IsNullOrEmpty(Args.AssetsBundleName) ? new UniTask<IBundleHolder>(new FakeAssetBundleHolder()) : _bundleProvider.LoadAsync(Args.AssetsBundleName, cancellationToken);

            var holders = await UniTask.WhenAll(t1, t2);
            using var _ = holders.Item1;
            using var assetsBundleHolder = holders.Item2;
            cancellationToken.ThrowIfCancellationRequested();

            await using var sceneHolder = await _sceneLoader.LoadAsync(Args.SceneName, cancellationToken);
            Scene = sceneHolder.Scene;

            var manifestType = Type.GetType(Args.ManifestType);
            if (manifestType is null)
            {
                throw new NullReferenceException($"Cannot find game manifest type {Args.ManifestType}");
            }

            var manifest = (IGameManifest)Activator.CreateInstance(manifestType);

            var assetsProvider = _assetsProviderFactory.Create(assetsBundleHolder);
            var sceneProvider = _sceneProviderFactory.Create(Scene);
            var scope = _resolver.CreateScope(builder =>
            {
                builder.Register(manifest.LauncherType.Type, Lifetime.Transient);
                builder.RegisterInstance(assetsProvider);
                builder.RegisterInstance<IGameControllerRunner>(this);
                builder.RegisterInstance(sceneProvider);

                if (manifest.ScopeType != null)
                {
                    var installer = Activator.CreateInstance(manifest.ScopeType.Type) as IInstaller;
                    installer!.Install(builder);
                }
            });

            AddDisposable(scope);
            _controllersFactory = scope.Resolve<IControllerFactory>();
            var launcher = scope.Resolve(manifest.LauncherType.Type) as IGameLauncher;

            await launcher!.LaunchAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            Complete(default);
        }
    }
}