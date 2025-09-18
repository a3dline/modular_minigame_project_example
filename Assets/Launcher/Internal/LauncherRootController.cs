using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using GameManager.Public;
using Playtika.Controllers;

namespace Game.Launcher
{
    internal class LauncherRootController : RootController
    {
        private readonly ILoggerService _logger;
        public LauncherRootController(IControllerFactory controllerFactory, ILoggerService logger) : base(controllerFactory)
        {
            _logger = logger;
        }

        protected override void OnStart()
        {
            base.OnStart();
            FlowAsync(CancellationToken).Forget();
        }

        private async UniTaskVoid FlowAsync(CancellationToken token)
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))
            {
                try
                {
                    await ExecuteAndWaitResultAsync<GameManagerController>(token);
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                    _logger.LogError(LauncherLoggerTags.Launcher, "Launcher caught exception. Restarting...");
                    await UniTask.Delay(500, cancellationToken: token);
                }
            }
        }
    }
}