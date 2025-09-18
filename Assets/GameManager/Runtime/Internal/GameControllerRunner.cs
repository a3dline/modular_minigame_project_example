using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Playtika.Controllers;
using VContainer;
using VContainer.Unity;

namespace Game.GameManager
{
    internal class GameControllerRunner : ControllerWithResultBase<IGameManifest, EmptyControllerResult>, IGameControllerRunner
    {
        private readonly IObjectResolver _resolver;
        private IControllerFactory _controllersFactory;

        public GameControllerRunner(IControllerFactory controllerFactory,
                                    IObjectResolver resolver)
            : base(controllerFactory)
        {
            _resolver = resolver;
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var installer = Activator.CreateInstance(Args.ScopeType.Type) as IInstaller;
            var scope = _resolver.CreateScope(builder =>
            {
                builder.Register(Args.LauncherType.Type, Lifetime.Transient);
                installer!.Install(builder);
            });
            
            AddDisposable(scope);
            _controllersFactory = scope.Resolve<IControllerFactory>();
            var launcher = scope.Resolve(Args.LauncherType.Type) as IGameLauncher;
            
            await launcher!.LaunchAsync(this, cancellationToken);
            Complete(default);
        }
        
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
    }
}