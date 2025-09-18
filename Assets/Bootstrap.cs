using Game.Launcher;
using VContainer;
using VContainer.Unity;

public class Bootstrap : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        LauncherInstaller.InstallDefault(builder);
    }
}