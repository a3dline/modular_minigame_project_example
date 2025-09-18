using VContainer;
using VContainer.Unity;

namespace Games.GameA
{
    public class GameAScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<GameASceneViewController>(Lifetime.Transient);
        }
    }
}