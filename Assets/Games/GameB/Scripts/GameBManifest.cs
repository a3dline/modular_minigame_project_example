using Game.GameManager;

namespace Games.GameB
{
    public class GameBManifest : IGameManifest
    {
        public ITypeReference LauncherType => new GameLauncherTypeReference<GameBLauncher>();
        public ITypeReference ScopeType => null;
    }
}