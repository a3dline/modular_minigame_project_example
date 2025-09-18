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
        private readonly IBundleProvider _bundleProvider;
        private readonly IObjectResolver _resolver;
        private readonly ISceneLoader _sceneLoader;
        private IControllerFactory _controllersFactory;

        public GameControllerRunner(IControllerFactory controllerFactory,
                                    IBundleProvider bundleProvider,
                                    ISceneLoader sceneLoader,
                                    IObjectResolver resolver)
            : base(controllerFactory)
        {
            _bundleProvider = bundleProvider;
            _sceneLoader = sceneLoader;
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
            using var _ = await _bundleProvider.LoadAsync(Args.BundleName, cancellationToken);
            await using var holder = await _sceneLoader.LoadAsync(Args.SceneName, cancellationToken);
            Scene = holder.Scene;

            var scope = _resolver.CreateScope(builder =>
            {
                builder.Register(Args.Manifest.LauncherType.Type, Lifetime.Transient);

                if (Args.Manifest.ScopeType != null)
                {
                    var installer = Activator.CreateInstance(Args.Manifest.ScopeType.Type) as IInstaller;
                    installer!.Install(builder);
                }
            });

            AddDisposable(scope);
            _controllersFactory = scope.Resolve<IControllerFactory>();
            var launcher = scope.Resolve(Args.Manifest.LauncherType.Type) as IGameLauncher;

            await launcher!.LaunchAsync(this, cancellationToken);

            Complete(default);
        }
    }
}