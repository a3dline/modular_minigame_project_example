using System;
using System.Threading;
using VContainer.Unity;

namespace Game.Launcher
{
    internal class LauncherEntryPoint : IStartable, IDisposable
    {
        private readonly CancellationTokenSource _globalCts;
        private readonly ILoggerService _logger;
        private readonly LauncherRootController _rootController;
        private bool _wasDisposed;

        internal LauncherEntryPoint(LauncherRootController rootController, ILoggerService logger)
        {
            _rootController = rootController;
            _logger = logger;
            _globalCts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (_wasDisposed)
            {
                return;
            }

            _globalCts.Cancel();
            _globalCts.Dispose();
            _wasDisposed = true;

            _logger.Log(LauncherLoggerTags.Launcher, "Launcher stopped");
        }

        public void Start()
        {
            ThrowIfDisposed();

            _rootController.LaunchTree(_globalCts.Token);
            _logger.Log(LauncherLoggerTags.Launcher, "Launcher started");
        }

        private void ThrowIfDisposed()
        {
            if (_wasDisposed)
            {
                throw new ObjectDisposedException(nameof(LauncherEntryPoint));
            }
        }
    }
}