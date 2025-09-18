using GameManager.Public;
using Playtika.Controllers;
using VContainer;
using VContainer.Unity;

namespace Game.Launcher
{
    public static class LauncherInstaller
    {
        public static void InstallDefault(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LauncherEntryPoint>();
            builder.Register<IControllerFactory, VContainerControllerFactory>(Lifetime.Scoped);
            builder.Register<ILoggerService, UnityLoggerService>(Lifetime.Singleton);
            
            builder.Register<LauncherRootController>(Lifetime.Transient);
            builder.Register<GameManagerController>(Lifetime.Transient);
        }
    }
}