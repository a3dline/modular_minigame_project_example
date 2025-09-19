using VContainer;
using VContainer.Unity;

namespace Games.GameB
{
    public class GameBScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<GameBPopupLaunchController>(Lifetime.Transient);
        }
    }
}